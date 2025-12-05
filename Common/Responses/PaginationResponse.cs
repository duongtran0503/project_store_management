namespace StoreManagement.API.Common.Responses
{
    public class PaginationResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalPages { get; set; }
        public int TotalProduct { get; set; }
        public List<T> Items { get; set; } = new List<T>();



        public PaginationResponse(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalProduct = count;
            Items = items;
        }
        public PaginationResponse(List<T> items, int count, int pageNumber, int pageSize, int totalPage)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalProduct = count;
            Items = items;
            TotalPages = totalPage;
        }


    }
}
