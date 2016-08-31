using System;

namespace QueryBuilder.Attributes
{
    /// <summary>
    /// Part of query in url. 
    /// For example /api/users/{userId}/
    /// where {userId} is our query part
    /// </summary>
    public class QueryPartAttribute : AbstractQueryPartAttribute
    {
        public QueryPartAttribute(string name, bool isRequired = false)
        {
            Name = name;
            IsRequired = isRequired;
        }
    }
}