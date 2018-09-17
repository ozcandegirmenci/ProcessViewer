using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace ProcessViewer
{
    /// <summary>
    /// About form
    /// </summary>
    internal partial class FormAbout : Form
	{
        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormAbout()
		{
			InitializeComponent();

			lblName.Text = string.Format(CultureInfo.InvariantCulture, lblName.Text, Properties.Resources.ProgramName);
			Text = string.Format(CultureInfo.InvariantCulture, Text, Properties.Resources.ProgramName);
			AssemblyName name = typeof(FormAbout).Assembly.GetName();
			lblVersion.Text = string.Format(CultureInfo.InvariantCulture, lblVersion.Text, name.Version.ToString());
		}

        #endregion

        #region Private Methods

        private void lnkGotoWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Common.GotoHomepage();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

        #endregion
    }
}
