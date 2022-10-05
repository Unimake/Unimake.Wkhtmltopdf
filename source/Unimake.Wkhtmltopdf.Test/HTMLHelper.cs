namespace Unimake.Wkhtmltopdf.Test
{
    internal static class HTMLHelper
    {
        #region Public Methods

        public static string SaveHtmlToFile()
        {
            var fi = new FileInfo("test.html");
            File.WriteAllText(fi.FullName, ToHtml());
            return fi.FullName;
        }

        public static string ToHtml()
        {
            var client = new HttpClient();
            var response = client.GetAsync("https://raw.githubusercontent.com/Unimake/Unimake.Wkhtmltopdf/main/LICENSE").GetAwaiter().GetResult();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        #endregion Public Methods
    }
}