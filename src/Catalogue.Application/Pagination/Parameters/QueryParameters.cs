namespace Catalogue.Application.Pagination.Parameters;

public class QueryParameters
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }

    public QueryParameters(int pageSize, int pageNumber)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    public static bool TryParse(string value, IFormatProvider formatProvider, out QueryParameters? result)
    {
        result = null;

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        string[] queryParts = value.Split('&');
        if (queryParts.Length != 2)
        {
            return false;
        }

        if (!int.TryParse(queryParts[0], out int pageSize) || !int.TryParse(queryParts[1], out int pageNumber))
        {
            return false;
        }

        result = new QueryParameters(pageSize, pageNumber);

        return true;
    }
}
