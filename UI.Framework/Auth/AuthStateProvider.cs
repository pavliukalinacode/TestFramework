namespace UI.Framework.Auth
{
    public sealed class AuthStateProvider : IAuthStateProvider
    {
        private readonly Dictionary<string, string> storageStatePaths = new(StringComparer.OrdinalIgnoreCase);

        public Task<string?> GetStorageStatePathAsync(string? profile = null)
        {
            if (string.IsNullOrWhiteSpace(profile))
            {
                return Task.FromResult<string?>(null);
            }

            storageStatePaths.TryGetValue(profile, out var path);
            return Task.FromResult<string?>(path);
        }

        public Task SaveStorageStatePathAsync(string profile, string storageStatePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(profile);
            ArgumentException.ThrowIfNullOrWhiteSpace(storageStatePath);

            storageStatePaths[profile] = storageStatePath;
            return Task.CompletedTask;
        }
    }
}