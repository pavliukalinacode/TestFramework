using System.Threading.Tasks;

namespace UI.Apps.SauceDemo.Auth
{
    public interface ISauceDemoAuthProvider
    {
        Task EnsureAuthStateAsync(SauceDemoUserType userType);
    }
}