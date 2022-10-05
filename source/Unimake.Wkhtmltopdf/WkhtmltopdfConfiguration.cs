using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace Unimake.Wkhtmltopdf
{
    internal static class WkhtmltopdfConfiguration
    {
        #region Public Properties

        /// <summary>
        /// Relative path to the directory containing wkhtmltopdf executable. Default is "wkhtmltox". Download at https://wkhtmltopdf.org/downloads.html
        /// </summary>
        public static string WkhtmltoxPath { get; set; } = "wkhtmltox";

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Setup Wkhtmltopdf library
        /// </summary>
        /// <param name="services">The IServiceCollection object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf. Default is "Wkhtmltox". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static IServiceCollection AddWkhtmltopdf(this IServiceCollection services, string wkhtmltopdfRelativePath = "Wkhtmltox")
        {
            WkhtmltoxPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);

            if(!Directory.Exists(WkhtmltoxPath))
            {
                throw new Exception("Folder containing wkhtmltopdf not found, searched for " + WkhtmltoxPath);
            }

            services.TryAddTransient<HtmlToPdfConverter, HtmlToPdfConverter>();

            return services;
        }

        #endregion Public Methods
    }
}