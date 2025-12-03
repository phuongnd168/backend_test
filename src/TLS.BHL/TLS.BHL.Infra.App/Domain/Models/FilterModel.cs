namespace TLS.BHL.Web.AdminApi.Models
{
    public class FilterModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? SortField { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SortOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Page { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int? PerPage { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public int? First { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Dictionary<string, string?>>? Filters { get; set; }
    }
}
