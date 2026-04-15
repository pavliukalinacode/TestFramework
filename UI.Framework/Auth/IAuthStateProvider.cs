namespace UI.Framework.Auth
{
    public interface IAuthStateProvider
    {
        Task<string?> GetStorageStatePathAsync(string? profile = null);
        Task SaveStorageStatePathAsync(string profile, string storageStatePath);
    }
}
