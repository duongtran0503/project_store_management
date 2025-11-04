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

        public async Task<PaginationResponse<BookResponse>> FilterProducts(FilterProductRequest request)
        {
            var (bookEntities, totalCount) = await _productRepository.GetFilteredBooksAsync(request);
            var bookResponses = bookEntities.Select(bookEntity => ToBookResposne(bookEntity, bookEntity.Category)).ToList();

            return new PaginationResponse<BookResponse>(
               bookResponses,
               totalCount,
               request.PageNumber,
               request.PageSize
           );

        
        }

        public async Task<BookResponse> UpdateBook(UpdateBookRequest request,string id)
        {
           var book = await _productRepository.GetBookByIdAsync(id);
           if(book==null)
            {
                throw new AppException(BookErrorCode.BookNotExisted);
            }
            var category = await _categoryRepository.GetCategoryById(request.CategoryId);
            if (category == null)
            {
                throw new AppException(CategoryErrorCode.CategoryNotExisted);
            }
            book.Title = request.Title;
            book.Author = request.Author;
            book.Publisher = request.Publisher;
            book.Isbn = request.Isbn;
            book.CategoryId = request.CategoryId;
            book.RetailPrice = request.RetailPrice;
            book.Image = request.Image ?? ProductConstants.PRODUCT_DEFAULT_IMAGE;
            book.IsAvailable = request.IsAvailable;
            var updatedBook = await _productRepository.UpdateBookAsync(book);
            return ToBookResposne(updatedBook, category);

        } 

        public async Task DeleteProduct(string id)
        {
            var product = await _productRepository.GetBookByIdAsync(id);
            if (product ==null)
            {
                throw new AppException(BookErrorCode.BookNotExisted);
            }
            product.IsAvailable = false;
            await _productRepository.UpdateBookAsync(product);
        }

        public async Task<BookResponse> GetBookById(string id)
        {
            var book = await _productRepository.GetBookByIdAsync(id);
            if (book == null) throw new AppException(BookErrorCode.BookNotExisted);
            return ToBookResposne(book,book.Category);
        }

        public async Task<PaginationResponse<BookResponse>> GetBookDeleted(PaginationRequest request)
        {
            var (books, totalCount) = await _productRepository.GetPagedBooksDeletedAsync(request.PageNumber,request.PageSize);
            var bookResponses = books.Select(bookEntity => ToBookResposne(bookEntity, bookEntity.Category)).ToList();

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
