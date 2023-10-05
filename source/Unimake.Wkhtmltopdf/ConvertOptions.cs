using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Unimake.Wkhtmltopdf.Enumerations;
using Unimake.Wkhtmltopdf.Options;

namespace Unimake.Wkhtmltopdf
{
    /// <summary>
    /// Conversion options
    /// </summary>
    public sealed class ConvertOptions
    {
        #region Private Fields

        private string _wkhtmltoxFullPath;

        #endregion Private Fields

        #region Private Methods

        private string GetConvertBaseOptions()
        {
            var result = new StringBuilder();
            var properties = GetType().GetProperties();

            foreach(var pi in properties)
            {
                if(!(pi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() is OptionFlag of))
                {
                    continue;
                }

                var value = pi.GetValue(this, null);

                if(value == null)
                {
                    continue;
                }

                if(pi.PropertyType == typeof(Dictionary<string, string>))
                {
                    var dictionary = (Dictionary<string, string>)value;
                    foreach(var d in dictionary)
                    {
                        _ = result.AppendFormat(" {0} \"{1}\" \"{2}\"", of.Name, d.Key, d.Value);
                    }
                }
                else if(pi.PropertyType == typeof(bool))
                {
                    if((bool)value)
                    {
                        _ = result.AppendFormat(CultureInfo.InvariantCulture, " {0}", of.Name);
                    }
                }
                else
                {
                    _ = result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
                }
            }

            return result.ToString().Trim();
        }

        #endregion Private Methods

        #region Public Fields

        /// <summary>
        /// Default conversion options
        /// </summary>
        public static readonly ConvertOptions Default = new ConvertOptions();

        #endregion Public Fields

        #region Public Properties

        /// <summary>
        /// Number of copies to print into the PDF file.
        /// </summary>
        [OptionFlag("--copies")]
        public int? Copies { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated with forms.
        /// </summary>
        [OptionFlag("--enable-forms")]
        public bool EnableForms { get; set; }

        /// <summary>
        /// Path to footer HTML file.
        /// </summary>
        [OptionFlag("--footer-html")]
        public string FooterHtml { get; set; }

        /// <summary>
        /// Sets the footer spacing.
        /// </summary>
        [OptionFlag("--footer-spacing")]
        public int? FooterSpacing { get; set; }

        /// <summary>
        /// Path to header HTML file.
        /// </summary>
        [OptionFlag("--header-html")]
        public string HeaderHtml { get; set; }

        /// <summary>
        /// Sets the header spacing.
        /// </summary>
        [OptionFlag("--header-spacing")]
        public int? HeaderSpacing { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated in grayscale.
        /// </summary>
        [OptionFlag("-g")]
        public bool IsGrayScale { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated in lower quality.
        /// </summary>
        [OptionFlag("-l")]
        public bool IsLowQuality { get; set; }

        /// <summary>
        /// Sets the page height in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageWidth"/> has to be also specified.</remarks>
        [OptionFlag("--page-height")]
        public double? PageHeight { get; set; }

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        public Margins PageMargins { get; set; }

        /// <summary>
        /// Sets the page orientation.
        /// </summary>
        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        /// <summary>
        /// Sets the page size.
        /// </summary>
        [OptionFlag("-s")]
        public Size? PageSize { get; set; }

        /// <summary>
        /// Sets the page width in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageHeight"/> has to be also specified.</remarks>
        [OptionFlag("--page-width")]
        public double? PageWidth { get; set; }

        /// <summary>
        /// Sets the variables to replace in the header and footer html
        /// </summary>
        /// <remarks>Replaces [name] with value in header and footer (repeatable).</remarks>
        [OptionFlag("--replace")]
        public Dictionary<string, string> Replacements { get; set; }

        /// <summary>
        /// Define de executable filename to wkhtmltopdf. Default is: "wkhtmltopdf.exe"
        /// </summary>
        public string WkhtmltoxExecutableFilename { get; set; } = "wkhtmltopdf.exe";

        /// <summary>
        /// This is the full executable path of wkhtmltopdf. The default is the combination of <see cref="WkhtmltoxPath"/> / <see cref="WkhtmltoxSubDirectory"/> / <see cref="WkhtmltoxExecutableFilename"/>
        /// </summary>
        public string WkhtmltoxFullPath
        {
            get => string.IsNullOrWhiteSpace(_wkhtmltoxFullPath) ? Path.Combine(WkhtmltoxPath, WkhtmltoxSubDirectory, WkhtmltoxExecutableFilename) : _wkhtmltoxFullPath;
            set => _wkhtmltoxFullPath = value;
        }

        /// <summary>
        /// Relative path to the directory containing wkhtmltopdf executable. Default is "wkhtmltox".
        /// </summary>
        public string WkhtmltoxPath { get; set; } = "wkhtmltox";

        /// <summary>
        /// Relative path to the directory <see cref="WkhtmltoxPath"/>. Default is "win-x64"
        /// </summary>
        public string WkhtmltoxSubDirectory { get; set; } = "win-x64";

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        ///
        /// </summary>
        public ConvertOptions() => PageMargins = new Margins();

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Get convert options as string
        /// </summary>
        /// <returns></returns>
        public string GetConvertOptions()
        {
            var result = new StringBuilder();

            if(PageMargins != null)
            {
                _ = result.Append(PageMargins.ToString());
            }

            _ = result.Append(' ');
            _ = result.Append(GetConvertBaseOptions());

            return result.ToString().Trim();
        }

        #endregion Public Methods
    }
}