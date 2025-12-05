using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.Services;

namespace StoreManagement.API.Modules.Products.Controllers
{
    [ApiController]
    [Route("/api/authors")]
    public class AuthorController : ControllerBase
    {
        private AuthorService _authorService;
        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _authorService.CreateAuthor(request);
            return Ok(ApiResponse<AuthorResponse>.Ok(res));
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] PaginationRequest request)
        {
            var result = await _authorService.GetListAuthors(request);


            return Ok(ApiResponse<PaginationResponse<AuthorResponse>>.Ok(result));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorRequest request, [FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _authorService.UpdateAuthor(request, id);
            return Ok(ApiResponse<AuthorResponse>.Ok(res));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuthor([FromRoute] string id)
        {
            await _authorService.DeleteAuthor(id);
            return Ok(ApiResponse.Ok());
        }

        [HttpPatch("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> RestoreAuthor([FromRoute] string id)
        {
            var res = await _authorService.RestoreAuthor(id);
            return Ok(ApiResponse<AuthorResponse>.Ok(res));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> FindAuthorByid([FromRoute] string id)
        {
            var res = await _authorService.FindAuthorById(id);
            return Ok(ApiResponse<AuthorResponse>.Ok(res));
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterAuthor([FromQuery] FiltertAuthorRequest request)
        {
            var res = await _authorService.FilterAuthor(request);
            return Ok(ApiResponse<PaginationResponse<AuthorResponse>>.Ok(res));
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetListSuggestion([FromQuery] FiltertAuthorRequest request)
        {
            var res = await _authorService.GetListSuggestions(request);
            return Ok(ApiResponse<List<SuggestionsResponse>>.Ok(res));
        }
    }
}
