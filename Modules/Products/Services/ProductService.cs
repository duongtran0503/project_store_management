using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.ErrorCode;
using StoreManagement.API.Modules.Products.Repository;
using System.ComponentModel;

namespace StoreManagement.API.Modules.Products.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        public ProductService(ProductRepository productRepository,
            CategoryRepository categoryRepository) { 
         _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<BookResponse> CreateProduct(CreateBookRequest request)
        {
            var category = await _categoryRepository.GetCategoryById(request.CategoryId);
            if(category==null)
            {
                throw new AppException(CategoryErrorCode.CategoryNotExisted);
            }

            var checkBook = await _productRepository.CheckBookByISBN(request.Isbn);
            if(checkBook)
            {
                throw new AppException(BookErrorCode.BookExisted);
            }
            
            var product = await _productRepository.CreateBookAsync(new Common.Entities.Book
            {
                Title = request.Title,
                Author = request.Author,
                Publisher = request.Publisher,
                Isbn = request.Isbn,
                Image = request.Image ?? ProductConstants.PRODUCT_DEFAULT_IMAGE,
                CategoryId = request.CategoryId,
                RetailPrice = request.RetailPrice,
                StockQuantity = request.StockQuantity,
                IsAvailable = request.IsAvailable,
                Category = category
            });
            return ToBookResposne(product, category);
        }

        public async Task<List<BookResponse>> GetBooks()
        {
            var books = await _productRepository.GetBooksAsync();
            List < BookResponse > result = books.Select(b => ToBookResposne(b, b.Category)).ToList();
            return result;

        }

        public async Task<PaginationResponse<BookResponse>> GetBooksAsync(PaginationRequest request)
        {
     
            var (bookEntities, totalCount) = await _productRepository.GetPagedBooksAsync(
                request.PageNumber,
                request.PageSize
            );

          
            var bookResponses = bookEntities.Select(bookEntity =>ToBookResposne(bookEntity,bookEntity.Category)).ToList();

         
            return new PaginationResponse<BookResponse>(
                bookResponses,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }

        private BookResponse ToBookResposne(Book product ,Category category)
        {
            return new BookResponse
            {
                Id = product.Id,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                Title = product.Title,
                Author = product.Author,
                Publisher = product.Publisher,
                Isbn = product.Isbn,
                Image = product.Image,
                CategoryId = product.CategoryId,
                RetailPrice = product.RetailPrice,
                StockQuantity = product.StockQuantity,
                IsAvailable = product.IsAvailable,
                CategoryName = category.CategoryName
            };
        }

     
    }
}
