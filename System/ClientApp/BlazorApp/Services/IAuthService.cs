using System.Security.Claims;
using BlazorApp.Entities;
namespace BlazorApp.Services;
using BlazorApp.Entities;

public interface IAuthService
{
    public Task LoginAsync(string username, string password);
    public Task LogoutAsync();
    public Task RegisterAsync(string userName, string password, string passwordRepeat, bool isTeacher);
    public Task<ClaimsPrincipal> GetAuthAsync();
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
    public Task ChangePasswordAsync(string username, string currentPassword, string newPassword);
}