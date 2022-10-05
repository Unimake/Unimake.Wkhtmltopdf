using System;

namespace Unimake.Wkhtmltopdf.Options
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class OptionFlag : Attribute
    {
        #region Public Properties

        public string Name { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public OptionFlag(string name) => Name = name;

        #endregion Public Constructors
    }
}