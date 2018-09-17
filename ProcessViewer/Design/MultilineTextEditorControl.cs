using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ProcessViewer.Design
{
    /// <summary>
    /// Multiline text editor control, which will be used to edit multiline strings
    /// </summary>
	internal partial class MultilineTextEditorControl : UserControl, IDropDownEditControl
    {
        #region Members

        private string _Value;
        private bool _CancelFlag;
        private IWindowsFormsEditorService _Service;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the result value
        /// </summary>
        public object Value
        {
            get
            {
                if (_CancelFlag)
                    return _Value;
                return txtText.Text;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public MultilineTextEditorControl()
		{
			InitializeComponent();
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize edit operation for the given text value
        /// </summary>
        /// <param name="service"></param>
        /// <param name="value"></param>
        public void Initialize(IWindowsFormsEditorService service, object value)
		{
			_Service = service;
			_Value = value.ToString();
			txtText.Text = _Value;
		}

        /// <summary>
        /// Resets edit operation and puts control in a clean state
        /// </summary>
		public void Reset()
		{
			_Value = null;
			_Service = null;
		}

        #endregion

        #region Protected Methods

        protected override bool ProcessDialogKey(Keys keyData)
		{
			if (((keyData & Keys.KeyCode) == Keys.Return)
				&& ((keyData & (Keys.Alt | Keys.Control)) == Keys.None))
			{
				_Service.CloseDropDown();
				return true;
			}
			if (((keyData & Keys.KeyCode) == Keys.Escape)
				&& ((keyData & (Keys.Alt | Keys.Control)) == Keys.None))
			{
				_CancelFlag = true;
				_Service.CloseDropDown();
				return true;
			}

			return base.ProcessDialogKey(keyData);
		}

        #endregion

        #region Private Methods

        private void btnRefresh_Click(object sender, EventArgs e)
		{
			txtText.Text = _Value;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			_CancelFlag = true;
			_Service.CloseDropDown();
		}

		private void btnOkey_Click(object sender, EventArgs e)
		{
			_Service.CloseDropDown();
		}

        #endregion
    }
}
