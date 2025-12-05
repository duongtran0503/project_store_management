namespace StoreManagement.API.Modules.Products.Dtos.Response
{
    public class CategoryWithDetail
    {
        public CategoryResponse Category { get; set; } = default!;
        public int TotalBooks { get; set; } = 0;

    }
}
