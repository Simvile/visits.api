using System.Linq.Expressions;

namespace visits.models.Core;

public class SearchObject<TEntity>
{
    private readonly Dictionary<string, object> _fields = new();

    public SearchObject()
    {
        foreach (var prop in typeof(TEntity).GetProperties())
        {
            var fieldType = typeof(SearchField<>).MakeGenericType(prop.PropertyType);
            _fields[prop.Name] = Activator.CreateInstance(fieldType)!;
        }
    }

    public SearchField<TValue> Field<TValue>(Expression<Func<TEntity, TValue>> selector)
    {
        var memberName = ((MemberExpression)selector.Body).Member.Name;

        if (_fields.TryGetValue(memberName, out var field))
            return (SearchField<TValue>)field;

        throw new ArgumentException($"Property '{memberName}' not found on {typeof(TEntity).Name}");
    }
    
    public IEnumerable<(string PropertyName, SearchField Field)> GetSetFields()
    {
        foreach (var (name, field) in _fields)
        {
            var searchField = (SearchField)field;
            if (searchField.IsSet)
                yield return (name, searchField);
        }
    }
}

public enum SearchType
{
    Equals,
    NotEquals,
    Contains,
    StartsWith,
    EndsWith,
    GreaterThan,
    LessThan
}

public abstract class SearchField
{
    public object? Value { get; protected set; }
    public SearchType SearchType { get; protected set; }
    public bool IsSet { get; protected set; }
}

public class SearchField<TValue> : SearchField
{
    public new TValue? Value => (TValue?)base.Value;

    public void SetValue(TValue value, SearchType searchType)
    {
        base.Value = value;
        SearchType = searchType;
        IsSet = true;
    }
}