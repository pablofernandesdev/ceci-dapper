namespace CeciDapper.Domain.DTO.Commons
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a generic query filter with pagination and search functionality.
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// Initializes a new instance of the QueryFilter class with default pagination settings.
        /// </summary>
        public QueryFilter()
        {
            Page = 1;
            PerPage = 10;
        }

        /// <summary>
        /// Gets or sets the search keyword to filter the query results.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the page number for pagination (1-based index).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page for pagination.
        /// </summary>
        public int PerPage { get; set; }
    }
}