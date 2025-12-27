namespace SelfLearningApiProject.Models.Responses
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }  // actual records
        public int TotalRecords { get; set; }     // total count without pagination
        public int TotalPages { get; set; }       // total pages
        public int PageNumber { get; set; }       // current page
        public int PageSize { get; set; }         // current page size
    }
}
