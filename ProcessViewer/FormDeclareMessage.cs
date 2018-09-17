using System;
using System.Globalization;
using System.Windows.Forms;

namespace ProcessViewer
{
    /// <summary>
    /// Provides declaring a new windows messaging
    /// </summary>
    internal partial class FormDeclareMessage : Form
	{
        #region Properties

        /// <summary>
        /// Gets the declared message id
        /// </summary>
        public int Msg
        {
            get
            {
                if (string.IsNullOrEmpty(txtMsg.Text))
                    return 0;
                return Convert.ToInt32(txtMsg.Text.Trim(), CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormDeclareMessage()
		{
			InitializeComponent();
		}

        #endregion

        #region Private Methods 

        private void txtMsg_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
				e.Handled = true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

        #endregion
    }
}
