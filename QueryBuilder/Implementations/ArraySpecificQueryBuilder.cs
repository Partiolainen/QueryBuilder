using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QueryBuilder.Attributes;
using QueryBuilder.Extensions;
using QueryBuilder.Parts;

namespace QueryBuilder.Implementations
{
    public class ArraySpecificQueryBuilder : IQueryBuilder
    {
        public string BuildQueryParametres(object request)
        {
            var parts = BuildParts(request);
            return BuildQuerStringParametres(parts.Cast<QueryStringBuildPart>());
        }

        public string BuildQuery(object request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var type = request.GetType();
            var queryRequestAttr = type.GetTypeInfo().GetCustomAttribute<QueryRequestAttribute>();
            string path = "";

            if (queryRequestAttr != null)
                path = queryRequestAttr.Path;

            var parts = BuildParts(request);

            path = BuildQueryPath(path, parts);
            var parametres = BuildQuerStringParametres(parts);

            var hasParamatres = parametres.IsEmpty() ? "" : "?";
            return $"{path}{hasParamatres}{parametres}";
        }

        private static string BuildQueryPath(string path, IEnumerable<AbstractQueryBuildPart> parts)
        {
            path = parts.Where(o => o is QueryBuildPart).Aggregate(path, (current, part) => current.Replace($"{{{part.Name}}}", part.Value));
            return path;
        }

        private string BuildQuerStringParametres(IEnumerable<AbstractQueryBuildPart> parts)
        {
            return string.Join("&", parts.Where(o => o is QueryStringBuildPart).Select(x => string.Concat(Uri.EscapeDataString(x.Name), "=", x.IgnoreUriEscape ? x.Value : Uri.EscapeDataString(x.Value))));
        }

        private AbstractQueryBuildPart BuildPart(PropertyInfo property, object value)
        {
            var queryPartAttribute = property.GetCustomAttribute<AbstractQueryPartAttribute>();
            if (queryPartAttribute != null)
            {
                if (queryPartAttribute.IsRequired && value == null)
                    throw new ArgumentNullException(property.Name);
                if (value == null)
                    return null;

                var name = queryPartAttribute.Name;
                if (string.IsNullOrEmpty(name))
                    name = property.Name;

                if (queryPartAttribute is QueryStringPartAttribute)
                    return new QueryStringBuildPart()
                    {
                        Name = name,
                        Value = value.ToString(),
                        IgnoreUriEscape = queryPartAttribute.IgnoreUriEscape
                    };
                else
                    return new QueryBuildPart
                    {
                        Name = name,
                        Value = value.ToString(),
                        IgnoreUriEscape = queryPartAttribute.IgnoreUriEscape
                    };
            }

            if (value != null)
                return new QueryStringBuildPart
                {
                    Name = property.Name,
                    Value = value.ToString()
                };

            return null;
        }

        private AbstractQueryBuildPart[] BuildParts(object request)
        {
            var type = request.GetType();
            var properties = type.GetRuntimeProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetCustomAttribute<IgnoreQueryPartAttribute>() == null).ToList();
            var parts = new List<AbstractQueryBuildPart>();

            foreach (var property in properties)
            {
                var typeInfo = property.PropertyType.GetTypeInfo();

                if (typeInfo.IsArray)
                {
                    var value = property.GetValue(request);
                    var enumerable = value as IEnumerable;
                    if (enumerable != null)
                        foreach (var item in enumerable)
                        {
                            parts.Add(BuildPart(property, item));
                        }
                }
                else if (IsPrimitve(property))
                {
                    var value = property.GetValue(request);
                    parts.Add(BuildPart(property, value));
                }
            }

            return parts.Where(x => x != null).ToArray();
        }

        private bool IsPrimitve(PropertyInfo property)
        {
            var typeInfo = property.PropertyType.GetTypeInfo();

            return typeInfo.IsPrimitive || property.PropertyType == typeof(string) ||
                   property.PropertyType == typeof(decimal) ||
                   (typeInfo.IsGenericType && typeInfo.GenericTypeArguments.Length == 1 &&
                    typeInfo.GenericTypeArguments[0].GetTypeInfo().IsPrimitive);
        }
    }
}