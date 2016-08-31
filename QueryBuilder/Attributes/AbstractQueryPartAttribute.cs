using System;

namespace QueryBuilder.Attributes
{
    public abstract class AbstractQueryPartAttribute : Attribute
    {
        /// <summary>
        /// Name of part
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is it required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Some "very_smart" servers can't convert from UriEscape.
        /// Ñ - crutch
        /// </summary>
        public bool IgnoreUriEscape { get; set; }
    }
}