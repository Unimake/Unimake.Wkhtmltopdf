using System;
using System.IO;

namespace Unimake.Wkhtmltopdf
{
    /// <summary>
    /// Conversor para HTML em PDF
    /// </summary>
    public sealed class HtmlToPdfConverter
    {
        #region Private Fields

        private ConvertOptions _convertOptions;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Cria uma instância do conversor
        /// </summary>
        public HtmlToPdfConverter() => _convertOptions = new ConvertOptions();

        /// <summary>
        /// Cria uma instância do conversor e atribui as opções de conversão
        /// </summary>
        /// <param name="convertOptions">Opções de conversão</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="convertOptions"/> for nulo</exception>
        public HtmlToPdfConverter(ConvertOptions convertOptions) => _convertOptions = convertOptions ?? throw new ArgumentNullException(nameof(convertOptions));

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Converte o html em uma string pdf no padrão base64
        /// </summary>
        /// <param name="html">html para conversão</param>
        /// <returns></returns>
        public string GetPDFAsBase64(string html) => Convert.ToBase64String(GetPDFAsByteArray(html));

        /// <summary>
        /// Converte o html em uma cadeia de bytes pdf
        /// </summary>
        /// <param name="html">html para conversão</param>
        /// <returns></returns>
        public byte[] GetPDFAsByteArray(string html) => WkhtmlDriver.Convert(_convertOptions.WkhtmltoxPath, _convertOptions.GetConvertOptions(), html);

        /// <summary>
        /// Faz a leitura do html pelo arquivo e retorna uma string em base64
        /// </summary>
        /// <param name="path">Caminho válido do arquivo</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Se o arquivo <paramref name="path"/> não existir</exception>
        public string GetPDFBase64FromHtmlFile(string path)
        {
            var fi = new FileInfo(path);

            if(!fi.Exists)
            {
                throw new FileNotFoundException($"Could not find file '{path}'.");
            }

            var html = File.ReadAllText(fi.FullName);
            return GetPDFAsBase64(html);
        }

        /// <summary>
        /// Faz a leitura do html pelo arquivo e retorna os bytes
        /// </summary>
        /// <param name="path">Caminho válido do arquivo</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Se o arquivo <paramref name="path"/> não existir</exception>
        public byte[] GetPDFBytesFromHtmlFile(string path)
        {
            var fi = new FileInfo(path);

            if(!fi.Exists)
            {
                throw new FileNotFoundException($"Could not find file '{path}'.");
            }

            var html = File.ReadAllText(fi.FullName);
            return GetPDFAsByteArray(html);
        }

        /// <summary>
        /// Salva o html em formato pdf no arquivo informado em <paramref name="path"/>
        /// </summary>
        /// <param name="path">Caminho do arquivo.</param>
        /// <param name="html">Html para conversão</param>
        /// <param name="deleteIfExists">Se verdadeiro, exclui o arquivo, caso já exista</param>
        public void SaveToFile(string path, string html, bool deleteIfExists = true)
        {
            var bytes = GetPDFAsByteArray(html);

            if(deleteIfExists)
            {
                var fi = new FileInfo(path);

                if(fi.Exists)
                {
                    fi.Delete();
                }
            }

            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Modifica as opções de conversão
        /// <para>Se desejar limpar as opções use <see cref="ConvertOptions.Default"/></para>
        /// </summary>
        /// <param name="convertOptions">Opções de conversão</param>
        /// <exception cref="ArgumentNullException">lançada se as opções forem nulas. Se desejar limpar as opções use <see cref="ConvertOptions.Default"/></exception>
        public HtmlToPdfConverter SetConvertOptions(ConvertOptions convertOptions)
        {
            _convertOptions = convertOptions ?? throw new ArgumentNullException(nameof(convertOptions));
            return this;
        }

        #endregion Public Methods
    }
}