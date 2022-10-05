using System.Diagnostics;

namespace Unimake.Wkhtmltopdf.Test
{
    public class ConvertToPDFTest
    {
        #region Public Methods

        [Fact]
        public void AnotherWkhtmltopdfLibFolder()
        {
            var html = HTMLHelper.ToHtml();
            var options = new ConvertOptions
            {
                WkhtmltoxPath = "libs\\MyLibFolder"
            };

            var bytes = new HtmlToPdfConverter(options).GetPDFAsByteArray(html);
            Assert.NotNull(bytes);

            var fi = new FileInfo("test.pdf");

            if(fi.Exists)
            {
                fi.Delete();
            }

            File.WriteAllBytes(fi.FullName, bytes);

            //Open pdf file
            _ = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fi.FullName
            });
        }

        [Fact]
        public void GetPDFAsBase64()
        {
            var html = HTMLHelper.ToHtml();
            var pdf = new HtmlToPdfConverter().GetPDFAsBase64(html);
            Assert.NotNull(pdf);

            //save to file
            var bytes = Convert.FromBase64String(pdf);
            Assert.NotNull(bytes);

            var fi = new FileInfo("test.pdf");

            if(fi.Exists)
            {
                fi.Delete();
            }

            File.WriteAllBytes(fi.FullName, bytes);

            //Open pdf file
            _ = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fi.FullName
            });
        }

        [Fact]
        public void GetPDFAsByteArray()
        {
            var html = HTMLHelper.ToHtml();
            var bytes = new HtmlToPdfConverter().GetPDFAsByteArray(html);
            Assert.NotNull(bytes);

            var fi = new FileInfo("test.pdf");

            if(fi.Exists)
            {
                fi.Delete();
            }

            File.WriteAllBytes(fi.FullName, bytes);

            //Open pdf file
            _ = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fi.FullName
            });
        }

        [Fact]
        public void LoadHtmlFromFile()
        {
            var bytes = new HtmlToPdfConverter().GetPDFBytesFromHtmlFile(HTMLHelper.SaveHtmlToFile());
            Assert.NotNull(bytes);

            var fi = new FileInfo("test.pdf");

            if(fi.Exists)
            {
                fi.Delete();
            }

            File.WriteAllBytes(fi.FullName, bytes);

            //Open pdf file
            _ = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fi.FullName
            });
        }

        [Fact]
        public void SaveToFile()
        {
            var html = HTMLHelper.ToHtml();
            var fi = new FileInfo("test.pdf");

            new HtmlToPdfConverter().SaveToFile(fi.FullName, html);

            //Open pdf file
            _ = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fi.FullName
            });
        }

        #endregion Public Methods
    }
}