using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Unimake.Wkhtmltopdf
{
    internal class WkhtmlDriver
    {
        #region Private Constructors

        private WkhtmlDriver()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        /// <summary>
        /// Encode all special chars
        /// </summary>
        /// <param name="text">Html text</param>
        /// <returns>Html with special chars encoded</returns>
        private static string SpecialCharsEncode(string text)
        {
            var chars = text.ToCharArray();
            var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach(var c in chars)
            {
                var value = System.Convert.ToInt32(c);
                _ = value > 127 ? result.AppendFormat("&#{0};", value) : result.Append(c);
            }

            return result.ToString();
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Converts given URL or HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmlFullPath">Path to wkthmltopdf\wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public static byte[] Convert(string wkhtmlFullPath, string switches, string html)
        {
            var fi = new FileInfo(wkhtmlFullPath);

            if(!fi.Exists)
            {
                throw new Exception("wkhtmltopdf not found, searched for " + fi.FullName);
            }

            // switches:
            //     "-q"  - silent output, only errors - no progress messages
            //     " -"  - switch output to stdout
            //     "- -" - switch input to stdin and output to stdout
            switches = "-q " + switches + " -";

            // generate PDF from given HTML string, not from URL
            if(!string.IsNullOrEmpty(html))
            {
                switches += " -";
                html = SpecialCharsEncode(html);
            }

            using(var proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo
                {
                    FileName = fi.FullName,
                    Arguments = switches,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    WorkingDirectory = fi.DirectoryName,
                    CreateNoWindow = true
                };

                _ = proc.Start();

                // generate PDF from given HTML string, not from URL
                if(!string.IsNullOrEmpty(html))
                {
                    using(var sIn = proc.StandardInput)
                    {
                        sIn.WriteLine(html);
                    }
                }

                using(var ms = new MemoryStream())
                {
                    using(var sOut = proc.StandardOutput.BaseStream)
                    {
                        var buffer = new byte[4096];
                        int read;

                        while((read = sOut.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                    }

                    var error = proc.StandardError.ReadToEnd();

                    if(proc.ExitCode != 0)
                    {
                        throw new Exception(error);
                    }

                    proc.WaitForExit();

                    return ms.ToArray();
                }
            }
        }

        #endregion Public Methods
    }
}