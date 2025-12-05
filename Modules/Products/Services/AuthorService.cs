using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.ErrorCode;
using StoreManagement.API.Modules.Products.Repository;
using System.ComponentModel;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Products.Services
{
    public class AuthorService
    {
        private AuthorRepository _authorRepository;
        public AuthorService(AuthorRepository authorRepository) { 
          _authorRepository = authorRepository;
        }

        public async Task<AuthorResponse> CreateAuthor(CreateAuthorRequest request)
        {
            var check = await _authorRepository.CheckAuthorByCode(request.Code);

            if(check)
            {
                throw new AppException(AuthorErrorCode.AuthorExisted);

            }
            var author = new Author
            {
                Name = request.Name,
                Code = request.Code,
                Status =AuthorStatusConstants.DEFAULT
            };
            var newAuthor = await _authorRepository.CreateAuthorAsync(author);
            return ToAuthorResponse(newAuthor);
        }

        public async Task<PaginationResponse<AuthorResponse>> GetListAuthors(PaginationRequest request)
        {

            if(request.All!=null && request.All==true)
            {
                var authorEntities = await _authorRepository.GetAllAsync();
                var list = authorEntities.Select(a => ToAuthorResponse(a)).ToList();
                var  totalCount = list.Count; 

              
                return new PaginationResponse<AuthorResponse>(
                    list,
                    totalCount,
                    pageNumber: 1,
                    pageSize: totalCount > 0 ? totalCount : 1
                );

            }
            var (authorEntitties, total) = await _authorRepository.GetPageAuthorAsync(request.PageNumber, request.PageSize);
            var authors = authorEntitties.Select(a => ToAuthorResponse(a)).ToList();
            return new PaginationResponse<AuthorResponse>(authors, total, request.PageNumber, request.PageSize);
        }


        public async Task<AuthorResponse> UpdateAuthor(UpdateAuthorRequest request,string authorId)
        {
             var author = await _authorRepository.GetAuthorByIdAsync(authorId);
            if (author== null) throw new AppException(AuthorErrorCode.AuthorNotExisted);
            if(author.Code!=request.Code)
            {
                var checkCode = await _authorRepository.CheckAuthorByCode(request.Code);
                if(checkCode) throw new AppException(AuthorErrorCode.AuthorExisted);
            }
            author.Code = request.Code;

            author.Name = request.Name;
            author.Status = request.Status;
            var updateAuthor = await _authorRepository.UpdateAuthorAsync(author);
            return ToAuthorResponse(updateAuthor);

        }

        public async Task DeleteAuthor(string id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author == null) throw new AppException(AuthorErrorCode.AuthorNotExisted);
            author.IsDeleted = true;
            await _authorRepository.UpdateAuthorAsync(author);
        }

        public async Task<AuthorResponse> RestoreAuthor(string id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author == null) throw new AppException(AuthorErrorCode.AuthorNotExisted);
            author.IsDeleted = false;
            var restoreAuthor =   await _authorRepository.UpdateAuthorAsync(author);
            return ToAuthorResponse(restoreAuthor);
        }

        public async Task<AuthorResponse> FindAuthorById(string id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author == null) throw new AppException(AuthorErrorCode.AuthorNotExisted);
            return ToAuthorResponse(author);
        }

        public async Task<PaginationResponse<AuthorResponse>> FilterAuthor(FiltertAuthorRequest request)
        {
            var authorEntities = await _authorRepository.FilterAuthorAsync(request);
            var authors = authorEntities.Select(au => ToAuthorResponse(au)).ToList();
            return new PaginationResponse<AuthorResponse>(authors, authors.Count, request.PageNumber, request.PageSize);
        }

        public async Task<List<SuggestionsResponse>> GetListSuggestions(FiltertAuthorRequest request)
        {
            return await _authorRepository.GetSuggestionsAsync(request);
        }

        private AuthorResponse ToAuthorResponse(Author author)
        {
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(author.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(author.UpdatedAt, vietnamTimeZone);
            return new AuthorResponse
            {
                Id = author.Id,
                Name = author.Name,
                Code = author.Code,
                Status = author.Status,
                IsDeleted = author.IsDeleted,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
            };
        }

    }
}
