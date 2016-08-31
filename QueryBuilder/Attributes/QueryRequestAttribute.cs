using System;

namespace QueryBuilder.Attributes
{
    public class QueryRequestAttribute : Attribute
    {

        public string Path { get; set; }

        public QueryRequestAttribute(string path)
        {
            Path = path;
        }
    }
}