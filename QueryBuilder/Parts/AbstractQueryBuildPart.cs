namespace QueryBuilder.Parts
{
    internal abstract class AbstractQueryBuildPart
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IgnoreUriEscape { get; set; }
    }
}