namespace TruYum.Api.Dtos
{
    public record RegisterDto(string Username, string Password, string Role);
    public record LoginDto(string Username, string Password);
    public record TokenResponse(string Token);
}
