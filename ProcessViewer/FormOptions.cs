using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.IO;

namespace ProcessViewer
{
	/// <summary>
	/// Options window of the Process Viewer
	/// </summary>
	internal partial class FormOptions : Form
	{
        #region Types
        
        /// <summary>
        /// Custom type descriptor class for settings
        /// </summary>
        private class SettingsTypeDescriptor : CustomTypeDescriptor
        {
            #region Members

            private static PropertyDescriptorCollection _Properties;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the property descriptors list
            /// </summary>
            private static PropertyDescriptorCollection PropertyDescriptors
            {
                get
                {
                    if (_Properties == null)
                    {
                        List<SettingsPropertyDescriptor> ps = new List<SettingsPropertyDescriptor>();
                        foreach (SettingsProperty item in Properties.Settings.Default.Properties)
                        {
                            ps.Add(new SettingsPropertyDescriptor(item.Name));
                        }
                        _Properties = new PropertyDescriptorCollection(ps.ToArray());
                    }
                    return _Properties;
                }
            }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public SettingsTypeDescriptor()
            {

            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns the default property descriptor
            /// </summary>
            /// <returns></returns>
            public override PropertyDescriptor GetDefaultProperty()
            {
                return PropertyDescriptors["AutoHighlight"];
            }

            /// <summary>
            /// Returns the property owner
            /// </summary>
            /// <param name="pd"></param>
            /// <returns></returns>
            public override object GetPropertyOwner(PropertyDescriptor pd)
            {
                return Properties.Settings.Default;
            }

            /// <summary>
            /// Returns the all properties
            /// </summary>
            /// <returns></returns>
            public override PropertyDescriptorCollection GetProperties()
            {
                return PropertyDescriptors;
            }

            /// <summary>
            /// Returns the properties according to the given attributes
            /// </summary>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                return PropertyDescriptors;
            }

            #endregion
        }

        /// <summary>
        /// Provides a custom property descriptor for Settings
        /// </summary>
        private class SettingsPropertyDescriptor : PropertyDescriptor
        {
            #region Properties

            /// <summary>
            /// Gets the component type
            /// </summary>
            public override Type ComponentType
            {
                get { return typeof(Properties.Settings); }
            }

            /// <summary>
            /// Gets that is this property read only or not?
            /// </summary>
            public override bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// Gets the value type of the property
            /// </summary>
            public override Type PropertyType
            {
                get { return Properties.Settings.Default.Properties[base.Name].PropertyType; }
            }


            /// <summary>
            /// Gets the category string of the property
            /// </summary>
            public override string Category
            {
                get
                {
                    try
                    {
                        return Properties.Resources.ResourceManager.GetString(
                            string.Format(CultureInfo.InvariantCulture, "Set_Cat_{0}",
                            base.Name), Properties.Settings.Default.ProgramLanguage);
                    }
                    catch (MissingManifestResourceException)
                    {
                        return base.DisplayName;
                    }
                }
            }

            /// <summary>
            /// Gets the description of the property
            /// </summary>
            public override string Description
            {
                get
                {
                    try
                    {
                        return Properties.Resources.ResourceManager.GetString(
                            string.Format(CultureInfo.InvariantCulture, "Set_Desc_{0}",
                            base.Name), Properties.Settings.Default.ProgramLanguage);
                    }
                    catch (MissingManifestResourceException)
                    {
                        return base.DisplayName;
                    }
                }
            }

            /// <summary>
            /// Gets the display name of the property
            /// </summary>
            public override string DisplayName
            {
                get
                {
                    try
                    {
                        return Properties.Resources.ResourceManager.GetString(
                            string.Format(CultureInfo.InvariantCulture, "Set_DispName_{0}",
                            base.Name), Properties.Settings.Default.ProgramLanguage);
                    }
                    catch (MissingManifestResourceException)
                    {
                        return base.DisplayName;
                    }
                }
            }

            /// <summary>
            /// Gets <see cref="TypeConverter"/> for the property
            /// </summary>
            public override TypeConverter Converter
            {
                get
                {
                    if (base.Name == "ProgramLanguage")
                        return SupportedLanguagesConverter.Instance;
                    return base.Converter;
                }
            }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class with the provided name
            /// </summary>
            /// <param name="name"></param>
            public SettingsPropertyDescriptor(string name)
                : base(name, null)
            {

            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns that can reset value of the property
            /// </summary>
            /// <param name="component"></param>
            /// <returns></returns>
            public override bool CanResetValue(object component)
            {
                return true;
            }

            /// <summary>
            /// Returns the value of the property with the given component
            /// </summary>
            /// <param name="component"></param>
            /// <returns></returns>
            public override object GetValue(object component)
            {
                return Properties.Settings.Default[base.Name];
            }

            /// <summary>
            /// Resets components property
            /// </summary>
            /// <param name="component"></param>
            public override void ResetValue(object component)
            {
                SetValue(component, Properties.Settings.Default.Properties[base.Name].DefaultValue);
            }

            /// <summary>
            /// Set value of the property with in the given component
            /// </summary>
            /// <param name="component"></param>
            /// <param name="value"></param>
            public override void SetValue(object component, object value)
            {
                Properties.Settings.Default[base.Name] = value;
            }

            /// <summary>
            /// Returns that is value of the property changed
            /// </summary>
            /// <param name="component"></param>
            /// <returns></returns>
            public override bool ShouldSerializeValue(object component)
            {
                return !object.Equals(Properties.Settings.Default.Properties[base.Name].DefaultValue,
                    Properties.Settings.Default.PropertyValues[base.Name].SerializedValue);
            }

            #endregion
        }

        /// <summary>
        /// Type converter for the supported languages
        /// </summary>
        internal class SupportedLanguagesConverter : CultureInfoConverter
        {
            #region Properties

            /// <summary>
            /// Gets the singleton instance of this class
            /// </summary>
            public static SupportedLanguagesConverter Instance { get; } = new SupportedLanguagesConverter();

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public SupportedLanguagesConverter()
            {

            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns that is standart values are exclusive
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }
            
            /// <summary>
            /// Returns the standart values
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>();
                supportedCultures.Add(CultureInfo.InvariantCulture);
                supportedCultures.Add(new CultureInfo(1033));
                string[] dirs = Directory.GetDirectories(Application.StartupPath);
                for (int i = 0; i < dirs.Length; i++)
                {
                    string fileName = string.Format(CultureInfo.InvariantCulture,
                                                    @"{0}\Process Viewer.resources.dll",
                                                         dirs[i]);
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            string cultureName = GetLanguageName(dirs[i]);
                            supportedCultures.Add(new CultureInfo(cultureName, false));
                        }
                        catch { }
                    }
                }
                return new StandardValuesCollection(supportedCultures);
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Returns the language name from its satallite assembly path
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            private static string GetLanguageName(string path)
            {
                while (path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                int index = path.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase);
                return path.Substring(index + 1, path.Length - 1 - index);
            }

            #endregion
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormOptions()
		{
			InitializeComponent();

			propertyGrid.SelectedObject = new SettingsTypeDescriptor();
		}

        #endregion

        #region Private Methods

        private void btnSaveAndClose_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Save();
			Close();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnDefault_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Reset();
			propertyGrid.Refresh();
		}

        #endregion
    }
}
