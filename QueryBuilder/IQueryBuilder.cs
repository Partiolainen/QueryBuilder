namespace QueryBuilder
{
    public interface IQueryBuilder
    {
        string BuildQueryParametres(object request);

        string BuildQuery(object request);
    }
}
