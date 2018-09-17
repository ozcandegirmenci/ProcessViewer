using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using ProcessViewer.Design;

namespace ProcessViewer
{
    /// <summary>
    /// Form for editing a windows message parameters
    /// </summary>
    internal partial class FormEditMessage : Form
	{
        #region Types

        /// <summary>
        /// Wrapper class for message editing
        /// </summary>
        private class EditMessage
        {
            #region Members

            private NativeMethods.Msgs _Msg = NativeMethods.Msgs.WM_NULL;
            private FormMain.MessageParam _WParam;
            private FormMain.MessageParam _LParam;
            private NativeMethods.Msgs _ModifiedMsg = NativeMethods.Msgs.WM_NULL;
            private bool _Ignore;
            private readonly int[] mParams;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the editing messages msg parameter
            /// </summary>
            [SRDescription("Des_EditMessage_Msg")]
            [SRCategory("Cat_Behaviour")]
            public NativeMethods.Msgs Msg
            {
                get { return _Msg; }
            }

            /// <summary>
            /// Gets or sets the modified msg param
            /// </summary>
            [SRDescription("Des_EditMessage_ModifiedMsg")]
            [SRCategory("Cat_Operation")]
            [TypeConverter(typeof(FormMain.MsgTypeConverter))]
            public NativeMethods.Msgs ModifiedMsg
            {
                get { return _ModifiedMsg; }
                set { _ModifiedMsg = value; }
            }

            /// <summary>
            /// Gets or sets the wparam
            /// </summary>
            [SRCategory("Cat_Parameter")]
            [SRDescription("Des_EditMessage_WParam")]
            [DefaultValue(0)]
            public FormMain.MessageParam WParam
            {
                get { return _WParam; }
                set { _WParam = value; }
            }

            /// <summary>
            /// Gets or sets the lparam
            /// </summary>
            [SRCategory("Cat_Parameter")]
            [SRDescription("Des_EditMessage_LParam")]
            [DefaultValue(0)]
            public FormMain.MessageParam LParam
            {
                get { return _LParam; }
                set { _LParam = value; }
            }

            /// <summary>
            /// Gets or sets that is this message will be ignored or not?
            /// </summary>
            [SRDescription("Des_EditMessage_Ignore")]
            [SRCategory("Cat_Operation")]
            [DefaultValue(false)]
            public bool Ignore
            {
                get { return _Ignore; }
                set { _Ignore = value; }
            }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="msg"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            public EditMessage(NativeMethods.Msgs msg, IntPtr wParam, IntPtr lParam)
            {
                _Msg = msg;
                _ModifiedMsg = msg;
                mParams = new int[] { wParam.ToInt32(), lParam.ToInt32() };
                _WParam = new FormMain.MessageParam(mParams[0]);
                _LParam = new FormMain.MessageParam(mParams[1]);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Resets edit operation
            /// </summary>
            public void Reset()
            {
                ModifiedMsg = Msg;
                WParam = new FormMain.MessageParam(mParams[0]);
                LParam = new FormMain.MessageParam(mParams[1]);
                Ignore = false;
            }

            #endregion
        }

        #endregion

        #region Members

        private EditMessage _EditMessage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the WParam of the editing message
        /// </summary>
        public IntPtr WParam
        {
            get { return new IntPtr(_EditMessage.WParam.Value); }
        }

        /// <summary>
        /// Gets the LParam of the editing message
        /// </summary>
        public IntPtr LParam
        {
            get { return new IntPtr(_EditMessage.LParam.Value); }
        }

        /// <summary>
        /// Gets that will message be ignored or not?
        /// </summary>
        public bool Ignore
        {
            get { return _EditMessage.Ignore; }
        }

        /// <summary>
        /// Gets the msg of the editing message
        /// </summary>
        public NativeMethods.Msgs ModifiedMsg
        {
            get { return _EditMessage.ModifiedMsg; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormEditMessage()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Initialize a new instance of this class with the provided values
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
		public FormEditMessage(IntPtr hWnd, NativeMethods.Msgs msg, IntPtr wParam, IntPtr lParam)
			: this()
		{
			lblHeader.Text = string.Format(CultureInfo.InvariantCulture, lblHeader.Text, hWnd.ToInt32());
			_EditMessage = new EditMessage(msg, wParam, lParam);
			propertyGrid.SelectedObject = _EditMessage;
			propertyGrid.ExpandAllGridItems();
		}

        #endregion

        #region Private Methods

        /// <summary>
        /// Accept changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}

        /// <summary>
        /// Resets changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnReset_Click(object sender, EventArgs e)
		{
			_EditMessage.Reset();
			propertyGrid.Refresh();
		}

        /// <summary>
        /// Ignore message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnIgnore_Click(object sender, EventArgs e)
		{
			_EditMessage.Ignore = true;
			btnOkey.PerformClick();
		}

        #endregion
	}
}
