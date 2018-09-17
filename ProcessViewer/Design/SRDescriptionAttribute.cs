using System;
using System.ComponentModel;

namespace ProcessViewer.Design
{
    /// <summary>
    /// Multilingual <see cref="DescriptionAttribute"/> attribute which provides localizable Description text
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
        #region Members

        private bool _Localized = false;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the localized description string
        /// </summary>
        public override string Description
        {
            get
            {
                if (!_Localized)
                {
                    _Localized = true;
                    base.DescriptionValue = Properties.Resources.ResourceManager.GetString(base.Description);
                }
                return base.Description;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class with the given description
        /// </summary>
        /// <param name="description">Description value</param>
        public SRDescriptionAttribute(string description)
			: base(description)
		{ 
		
		}

        #endregion
    }
}
