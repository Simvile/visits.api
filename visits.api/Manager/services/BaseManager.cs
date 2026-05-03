using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using visits.api.Data;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.services;

public abstract class BaseManager<TEntity>(AppDbContext dbContext) : IBaseManager<TEntity> where TEntity : class
{
    #region GetAll
    /// <summary>
    /// 
    /// </summary>
    /// <returns>List Of Objects of Type TEntity</returns>
    public virtual async Task<List<TEntity>> GetAll()
    {
        return await dbContext.Set<TEntity>().ToListAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="search"></param>
    /// <returns>List Of Objects Of Type TEntity</returns>
    public virtual async Task<List<TEntity>> GetAll(SearchObject<TEntity> search)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();

        foreach (var (propertyName, field) in search.GetSetFields())
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, propertyName);
            var value = Expression.Constant(field.Value, property.Type);

            Expression? condition = field.SearchType switch
            {
                SearchType.Equals      => Expression.Equal(property, value),
                SearchType.NotEquals   => Expression.NotEqual(property, value),
                SearchType.GreaterThan => Expression.GreaterThan(property, value),
                SearchType.LessThan    => Expression.LessThan(property, value),
                SearchType.Contains    => Expression.Call(property,
                    typeof(string).GetMethod("Contains", [typeof(string)])!,
                    value),
                SearchType.StartsWith  => Expression.Call(property,
                    typeof(string).GetMethod("StartsWith", [typeof(string)])!,
                    value),
                SearchType.EndsWith    => Expression.Call(property,
                    typeof(string).GetMethod("EndsWith", [typeof(string)])!,
                    value),
                _ => null
            };

            if (condition is null) continue;

            var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
            query = query.Where(lambda);
        }

        return await query.ToListAsync();
    }
    #endregion
    
    #region GetById

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Object Of Type T</returns>
    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await dbContext.Set<TEntity>().FindAsync(id);
    }
    #endregion

    #region Save
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>ResponseHandler</returns>
    public virtual async Task<ResponseHandler> Save(TEntity entity)
    {
        var response = new ResponseHandler();
        try
        {
            var entry = dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
                dbContext.Set<TEntity>().Update(entity);

            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            response.AddMessage($"An error occurred: {ex.Message}");
        }
        return response;
    }
    #endregion
    
    #region Validate
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>ResponseHandler</returns>
    public abstract Task<ResponseHandler> Validate(TEntity entity);
    #endregion

    #region Exists
    /// <summary>
    /// 
    /// </summary>
    /// <param name="search"></param>
    /// <returns>Boolean</returns>
    public virtual async Task<bool> Exists(SearchObject<TEntity> search)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();

        foreach (var (propertyName, field) in search.GetSetFields())
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, propertyName);
            var value = Expression.Constant(field.Value, property.Type);

            Expression? condition = field.SearchType switch
            {
                SearchType.Equals    => Expression.Equal(property, value),
                SearchType.NotEquals => Expression.NotEqual(property, value),
                SearchType.GreaterThan => Expression.GreaterThan(property, value),
                SearchType.LessThan  => Expression.LessThan(property, value),
                SearchType.Contains  => Expression.Call(property,
                    typeof(string).GetMethod("Contains", [typeof(string)])!,
                    value),
                SearchType.StartsWith => Expression.Call(property,
                    typeof(string).GetMethod("StartsWith", [typeof(string)])!,
                    value),
                SearchType.EndsWith  => Expression.Call(property,
                    typeof(string).GetMethod("EndsWith", [typeof(string)])!,
                    value),
                _ => null
            };

            if (condition is null) continue;

            var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
            query = query.Where(lambda);
        }

        return await query.AnyAsync();
    }
    #endregion
}