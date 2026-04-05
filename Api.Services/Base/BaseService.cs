using Microsoft.Extensions.Configuration;

namespace Api.Services.Base
{
    /// <summary>
    /// Abstract base class for services
    /// </summary>
    public abstract class BaseService(IConfiguration config)
    {
        protected readonly IConfiguration config = config;
    }
}