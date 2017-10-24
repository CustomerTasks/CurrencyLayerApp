namespace CurrencyLayerApp.Resources.Strings
{
    public static class MainLogMessages
    {
        public static readonly string NotAvailableInternetMessage =
            "Internet isn`t available. Please, check connection";

        public static readonly string ConnectedMessage = "Connected to CurrentLayerServer";
        public static readonly string StartupMessage = "Application isn`t prepared for working";
        public static readonly string NotFoundFile = "File isn't found";
        public static readonly string ApiKeyRequire = "API key is required! 1-3 tabs aren't available without API key";
        public static readonly string EmptyHistory = $"Historical data is empty. {NotAvailableInternetMessage}";
    }
}
