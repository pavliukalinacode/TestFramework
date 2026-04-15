namespace UI.Apps.SauceDemo.Auth
{
    public sealed class SauceDemoUser
    {
        public SauceDemoUserType Type { get; init; }
        public string? ProfileName { get; init; }
        public string? UserName { get; init; }
        public string? Password { get; init; }
        public bool SupportsSavedAuthState { get; init; }
    }
}