using System;
using System.ComponentModel;

namespace ProcessViewer.Design
{
    /// <summary>
    /// Multilingual <see cref="CategoryAttribute"/> attribute which provides localizable Category text
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class with the given category
        /// </summary>
        /// <param name="category">Category value</param>
        public SRCategoryAttribute(string category) : base(category)
		{ 
			
		}

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the localized string value for the category
        /// </summary>
        /// <param name="value">Category value</param>
        /// <returns>Returns the localized category value</returns>
        protected override string GetLocalizedString(string value)
		{
			return Properties.Resources.ResourceManager.GetString(value);
		}

        #endregion
    }
}
