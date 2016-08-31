namespace QueryBuilder.Attributes
{
    /// <summary>
    /// Part of query STRING in url.
    /// For example /api/users?{id=666}
    /// where {id=666} will be our part of query string
    /// </summary>
    public class QueryStringPartAttribute : AbstractQueryPartAttribute
    {
        public QueryStringPartAttribute(string name, bool isRequired = false)
        {
            Name = name;
            IsRequired = isRequired;
        }
    }
}