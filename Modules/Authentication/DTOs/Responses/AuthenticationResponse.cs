namespace StoreManagement.API.Modules.Authentication.DTOs.Responses
{
    public class AuthenticationResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
