using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.ErrorCode;
using StoreManagement.API.Modules.Products.Repository;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Products.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly PublisherRepository _publisherRepository;
        public ProductService(ProductRepository productRepository,
            CategoryRepository categoryRepository,
            AuthorRepository authorRepository,
            PublisherRepository publisherRepository) { 
         _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
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

            var author = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);
            if(author==null)
            {
                throw new AppException(AuthorErrorCode.AuthorNotExisted);
            }
            var publisher = await _publisherRepository.GetPublisherById(request.PublisherId);
            if(publisher==null)
            {
                throw new AppException(PublisherErrorCode.PublisherNotExisted);
            }
            string image = string.IsNullOrWhiteSpace(request.Image) ?ProductConstants.PRODUCT_DEFAULT_IMAGE : request.Image;
            var product = await _productRepository.CreateBookAsync(new Common.Entities.Book
            {
                Title = request.Title,
                AuthorId = author.Id,
                Isbn = request.Isbn,
                PublisherId = publisher.Id,
                Image = image,
                CategoryId = request.CategoryId,
                RetailPrice = request.RetailPrice,
                Status = request.Status,
                
            });
            product.Author = author;
            product.Publisher = publisher;
            product.Category = category;
            return ToBookResposne(product, category);
        }


        public async Task<List<BookResponse>> CreateProducts(List<CreateBookRequest> requests)
        {
            if (requests == null || !requests.Any()) return new List<BookResponse>();

            var categoryIds = requests.Select(r => r.CategoryId).Distinct().ToList();
            var authorIds = requests.Select(r => r.AuthorId).Distinct().ToList();
            var publisherIds = requests.Select(r => r.PublisherId).Distinct().ToList();
            var isbns = requests.Select(r => r.Isbn).Distinct().ToList();

            var categories = (await _categoryRepository.GetCategoriesByIds(categoryIds)).ToDictionary(c => c.Id);
            var authors = (await _authorRepository.GetAuthorsByIds(authorIds)).ToDictionary(a => a.Id);
            var publishers = (await _publisherRepository.GetPublishersByIds(publisherIds)).ToDictionary(p => p.Id);

            var existingIsbns = await _productRepository.GetExistingIsbns(isbns);

            var booksToCreate = new List<Book>();

            foreach (var request in requests)
            {
                if (!categories.ContainsKey(request.CategoryId))
                    throw new AppException(CategoryErrorCode.CategoryNotExisted);

                if (!authors.ContainsKey(request.AuthorId))
                    throw new AppException(AuthorErrorCode.AuthorNotExisted);

                if (!publishers.ContainsKey(request.PublisherId))
                    throw new AppException(PublisherErrorCode.PublisherNotExisted);

                if (existingIsbns.Contains(request.Isbn))
                    throw new AppException(BookErrorCode.BookExisted, $"ISBN {request.Isbn} already exists.");

                string image = string.IsNullOrWhiteSpace(request.Image)
                    ? ProductConstants.PRODUCT_DEFAULT_IMAGE
                    : request.Image;

                var book = new Book
                {
                    Title = request.Title,
                    AuthorId = request.AuthorId,
                    Isbn = request.Isbn,
                    PublisherId = request.PublisherId,
                    Image = image,
                    CategoryId = request.CategoryId,
                    RetailPrice = request.RetailPrice,
                    Status = request.Status,
                };

                booksToCreate.Add(book);
            }

            var createdBooks = await _productRepository.CreateBooksAsync(booksToCreate);

            return createdBooks.Select(b => {
                b.Author = authors[b.AuthorId];
                b.Publisher = publishers[b.PublisherId];
                b.Category = categories[b.CategoryId];
                return ToBookResposne(b, b.Category);
            }).ToList();
        }


        public async Task<PaginationResponse<BookResponse>> GetBooksAsync(PaginationRequest request)
        {
    
            var bookResponses = await _productRepository.GetProductListWithVouchersAsync(request.PageNumber,
                request.PageSize);
            var totalCount =bookResponses.Count;
           
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

            var author = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new AppException(AuthorErrorCode.AuthorNotExisted);
            }
            var publisher = await _publisherRepository.GetPublisherById(request.PublisherId);
            if (publisher == null)
            {
                throw new AppException(PublisherErrorCode.PublisherNotExisted);
            }
            string image = string.IsNullOrWhiteSpace(request.Image) ? ProductConstants.PRODUCT_DEFAULT_IMAGE : request.Image;
            book.Title = request.Title;
            book.AuthorId=author.Id;
            book.PublisherId = publisher.Id;
            book.Isbn = request.Isbn;
            book.CategoryId = request.CategoryId;
            book.RetailPrice = request.RetailPrice;
            book.Image = image;
            book.Status = request.Status;
            var updatedBook = await _productRepository.UpdateBookAsync(book);
            return ToBookResposne(updatedBook, category);

        } 

        public async Task<DeletedResponse> DeleteProduct(string id)
        {
            var product = await _productRepository.GetBookByIdAsync(id);
            if (product ==null)
            {
                throw new AppException(BookErrorCode.BookNotExisted);
            }
            product.IsDeleted = true;
           var res =    await _productRepository.UpdateBookAsync(product);
            return new DeletedResponse { Name = res.Title };
        }
        public async Task<BookResponse> RestoreProduct(string id)
        {
            var product = await _productRepository.GetBookByIdAsync(id);
            if (product == null)
            {
                throw new AppException(BookErrorCode.BookNotExisted);
            }
            product.IsDeleted = false;
          var res =   await _productRepository.UpdateBookAsync(product);
            return ToBookResposne(res, res.Category);
        }

        public async Task<BookResponse> GetBookById(string id)
        {
            var book = await _productRepository.GetProductByIdWithVouchersAsync(id);
            if (book == null) throw new AppException(BookErrorCode.BookNotExisted);
           return book;
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



        public async Task<List<SuggestionsResponse>> GetSuggestions(FilterProductRequest request)
        {
            return await _productRepository.GetSuggestionsAsync(request);
        }
        private BookResponse ToBookResposne(Book product ,Category category)
        {

            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(product.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(product.UpdatedAt, vietnamTimeZone);
            return new BookResponse
            {
                Id = product.Id,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                Title = product.Title,
                Author = product.Author.Name,
                AuthorId = product.AuthorId,
                Publisher = product.Publisher.Name,
                PublisherId = product.PublisherId,
                Isbn = product.Isbn
,                   Status = product.Status,                
                Image = product.Image,
                CategoryId = product.CategoryId,
                RetailPrice = product.RetailPrice,
                StockCanBeSold = product.Inventory != null ? product.Inventory.StockCanBeSold : 0,
                CategoryName = category.CategoryName
            };
        } 

     
    }
}
