namespace Configuration.Config
{
    /// <summary>
    /// Defines constant keys used for accessing configuration values.
    /// Centralizes configuration key management.
    /// </summary>
    public static class ConfigKeys
    {
        // Common
        public const string BaseUrl = "BaseUrl";

        public const string Version = "Version";
        public const string Timeout = "Timeout";

        // Logger
        public const string LoggerType = "LoggerType";

        // Playwright
        public const string BrowserName = "BrowserName";

        public const string Headless = "Headless";
        public const string SlowMoMs = "SlowMoMs";
        public const string ExpectTimeoutMs = "ExpectTimeoutMs";
        public const string DefaultSeconds = "DefaultSeconds";

        // Sections
        public const string GlobalParametersSection = "GlobalParameters";

        public const string LoggerSection = "Logger";
        public const string PlaywrightSection = "Playwright";
        public const string ScenariosSection = "Scenarios";

        //Pet
        public const string PetTests = "PetTests";

        public const string PostPet = "PostPet";

        //Internet
        public const string InternetTests = "InternetTests";
    }
}