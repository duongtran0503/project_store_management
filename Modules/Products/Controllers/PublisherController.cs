using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.Services;

namespace StoreManagement.API.Modules.Products.Controllers
{
    [ApiController]
    [Route("/api/publishers")]
    public class PublisherController:ControllerBase
    {
        private PublisherService _publisherService;

        public PublisherController(PublisherService publisherService) { 
         _publisherService = publisherService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAuthor([FromBody] CreatePublisherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _publisherService.CreatePublisher(request);
            return Ok(ApiResponse<PublisherResponse>.Ok(res));
        }


        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] PaginationRequest request)
        {
            var result = await _publisherService.GetListPublisher(request);


            return Ok(ApiResponse<PaginationResponse<PublisherResponse>>.Ok(result));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePublisher([FromBody] UpdatePublisherRequest request, [FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _publisherService.UpdatePublisher(request, id);
            return Ok(ApiResponse<PublisherResponse>.Ok(res));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePublisher([FromRoute] string id)
        {
            await  _publisherService.DeletePublisher(id);
            return Ok(ApiResponse.Ok());
        }

        [HttpPatch("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> RestorePublisher([FromRoute] string id)
        {
            var res = await _publisherService.RestorePublisher(id);
            return Ok(ApiResponse<PublisherResponse>.Ok(res));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> FindPublisherByid([FromRoute] string id)
        {
            var res = await _publisherService.FindPublisherById(id);
            return Ok(ApiResponse<PublisherResponse>.Ok(res));
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterAuthor([FromQuery] FIlterPublisherRequest request)
        {
            var res = await _publisherService.FilterPublisher(request);
            return Ok(ApiResponse<PaginationResponse<PublisherResponse>>.Ok(res));
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetListSuggestion([FromQuery] FIlterPublisherRequest request)
        {
            var res = await _publisherService.GetListSuggestions(request);
            return Ok(ApiResponse<List<SuggestionsResponse>>.Ok(res));
        }
    }
}
