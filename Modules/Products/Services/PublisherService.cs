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
    public class PublisherService
    {
        private readonly PublisherRepository _publisherRepository;

        public PublisherService(PublisherRepository publisherRepository) { _publisherRepository = publisherRepository; }

        public async Task<PublisherResponse> CreatePublisher(CreatePublisherRequest request)
        {
            var check = await _publisherRepository.CheckPublisherByCodeAsync(request.Code);
            if (check)
            {
                throw new AppException(PublisherErrorCode.PublisherExisted);
            }
            var publisher = new Publisher
            {
                Code = request.Code,
                Name = request.Name,
                Address = request.Address,
                Status = PublisherStatusConstants.DEFAULT

            };
            var newPublisher = await _publisherRepository.CreatePublisherAsync(publisher);
            return ToPublisherResponse(newPublisher);
        }

        public async Task<PaginationResponse<PublisherResponse>> GetListPublisher(PaginationRequest request)
        {
       
            List<PublisherResponse> publishers;
            int totalCount;

      
            if (request.All != null && request.All == true)
            {
           
                var publisherEntities = await _publisherRepository.GetAllAsync();

                publishers = publisherEntities.Select(p => ToPublisherResponse(p)).ToList();
                totalCount = publishers.Count;

       
                return new PaginationResponse<PublisherResponse>(
                    publishers,
                    totalCount,
                    pageNumber: 1,
                    pageSize: totalCount > 0 ? totalCount : 1
                );
            }
            else
            {
            
                var (publisherEntities, total) = await _publisherRepository.GetPagePublisherAsync(request.PageNumber, request.PageSize);

                publishers = publisherEntities.Select(p => ToPublisherResponse(p)).ToList();
                totalCount = total;

        
                return new PaginationResponse<PublisherResponse>(
                    publishers,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );
            }
        }


        public async Task<PublisherResponse> UpdatePublisher(UpdatePublisherRequest request, string id)
        {
            var publisher = await _publisherRepository.GetPublisherById(id);
            if (publisher == null) throw new AppException(PublisherErrorCode.PublisherNotExisted);

            if (publisher.Code != request.Code)
            {
                var checkCode = await _publisherRepository.CheckPublisherByCodeAsync(request.Code);
                if (checkCode) throw new AppException(PublisherErrorCode.PublisherExisted);
            }
            publisher.Code = request.Code;

            publisher.Name = request.Name;
            publisher.Status = request.Status;
            publisher.Address = request.Address;
            var update = await _publisherRepository.UpdatePublisherAsync(publisher);
            
            return ToPublisherResponse(update);

        }

        public async Task DeletePublisher(string id)
        {
            var publisher = await _publisherRepository.GetPublisherById(id);
            if (publisher == null) throw new AppException(PublisherErrorCode.PublisherNotExisted);

            publisher.IsDeleted = true;
            await _publisherRepository.UpdatePublisherAsync(publisher);
        }

        public async Task<PublisherResponse> RestorePublisher(string id)
        {
            var publisher = await _publisherRepository.GetPublisherById(id);
            if (publisher == null) throw new AppException(PublisherErrorCode.PublisherNotExisted);

            publisher.IsDeleted = false;
            var restoreAuthor = await _publisherRepository.RestorePublisherAsync(publisher);
            return ToPublisherResponse(restoreAuthor);
        }

        public async Task<PublisherResponse> FindPublisherById(string id)
        {
            var publisher = await _publisherRepository.GetPublisherById(id);
            if (publisher == null) throw new AppException(PublisherErrorCode.PublisherNotExisted);

            return ToPublisherResponse( publisher);
        }

        public async Task<PaginationResponse<PublisherResponse>> FilterPublisher(FIlterPublisherRequest request)
        {
            var pubisherEntities = await _publisherRepository.FilterPublisherAsync(request);
            var publishers = pubisherEntities.Select(au => ToPublisherResponse(au)).ToList();
            return new PaginationResponse<PublisherResponse>(publishers, publishers.Count, request.PageNumber, request.PageSize);
        }

        public async Task<List<SuggestionsResponse>> GetListSuggestions(FIlterPublisherRequest request)
        {
            return await _publisherRepository.GetSuggestionsAsync(request);
        }
      
        private PublisherResponse ToPublisherResponse(Publisher publisher)
        {
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(publisher.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(publisher.UpdatedAt, vietnamTimeZone);
            return new PublisherResponse
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Code = publisher.Code,
                Status = publisher.Status,
                IsDeleted = publisher.IsDeleted,
                Address = publisher.Address,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
            };
        }
    }
}
