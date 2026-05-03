using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.Interfaces;

public interface IBaseManager<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAll();
    Task<List<TEntity>> GetAll(SearchObject<TEntity> search);
    Task<TEntity?> GetById(Guid id);
    Task<ResponseHandler> Save(TEntity entity);
    Task<ResponseHandler> Validate(TEntity entity);
    Task<bool> Exists(SearchObject<TEntity> search);
    
}