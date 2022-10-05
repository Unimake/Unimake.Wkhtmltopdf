using System.Globalization;
using System.Linq;
using System.Text;

namespace Unimake.Wkhtmltopdf.Options
{
    /// <summary>
    /// Configure the pdf margins
    /// </summary>
    public class Margins
    {
        #region Public Properties

        /// <summary>
        /// Page bottom margin in mm.
        /// </summary>
        [OptionFlag("-B")] public int? Bottom { get; set; }

        /// <summary>
        /// Page left margin in mm.
        /// </summary>
        [OptionFlag("-L")] public int? Left { get; set; }

        /// <summary>
        /// Page right margin in mm.
        /// </summary>
        [OptionFlag("-R")] public int? Right { get; set; }

        /// <summary>
        /// Page top margin in mm.
        /// </summary>
        [OptionFlag("-T")] public int? Top { get; set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Instantiate <see cref="Margins"/>
        /// </summary>
        public Margins()
        {
        }

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        /// <param name="top">Page top margin in mm.</param>
        /// <param name="right">Page right margin in mm.</param>
        /// <param name="bottom">Page bottom margin in mm.</param>
        /// <param name="left">Page left margin in mm.</param>
        public Margins(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Convert the <see cref="Margins"/> to a valid formatted string options pattern 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            var properties = GetType().GetProperties();

            foreach(var pi in properties)
            {
                if(!(pi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() is OptionFlag of))
                {
                    continue;
                }

                var value = pi.GetValue(this);

                if(value != null)
                {
                    _ = result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
                }
            }

            return result.ToString().Trim();
        }

        #endregion Public Methods
    }
}