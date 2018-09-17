using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Globalization;

namespace ProcessViewer.Design
{
    /// <summary>
    /// Flags editor control, which will be used to edit Flags enum values
    /// </summary>
	internal class FlagsEditorControl : UserControl, IDropDownEditControl
	{
        #region Types

        /// <summary>
        /// Enum value container, which provides access to string representation and value combination of an enum value
        /// </summary>
        private class EnumValueContainer
        {
            #region Members

            private readonly Type _Type;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the value of the contained enum
            /// </summary>
            public object Value { get; } = 0;

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="type"></param>
            /// <param name="value"></param>
            public EnumValueContainer(Type type, object value)
            {
                _Type = type;
                Value = value;
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns the string representation of the contained enum value
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Enum.GetName(_Type, Value);
            }

            #endregion
        }

        #endregion

        #region Members

        private CheckedListBox lvwItems;
        private ToolStrip mnAction;
        private ToolStripButton btnReset;
        private ToolStripButton btnCancel;
        private ToolStripButton btnOk;

        private IWindowsFormsEditorService _Service = null;
        private object _Value;
        private long _LeftOver;
        private bool _CancelFlag;

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
                {
                    return _Value;
                }

                var result = 0L;
                for (int i = 0; i < lvwItems.CheckedItems.Count; i++)
                {
                    var container = lvwItems.CheckedItems[i] as EnumValueContainer;
                    if (container != null)
                    {
                        result |= Convert.ToInt64(container.Value, CultureInfo.InvariantCulture);
                    }
                }
                return result | _LeftOver;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FlagsEditorControl()
		{
			SuspendLayout();

			lvwItems = new CheckedListBox();
            lvwItems.Dock = DockStyle.Fill;
			lvwItems.FormattingEnabled = true;
			lvwItems.IntegralHeight = false;
			lvwItems.BorderStyle = BorderStyle.None;
			Controls.Add(this.lvwItems);

			mnAction = new ToolStrip();
			mnAction.Dock = DockStyle.Bottom;
			mnAction.GripStyle = ToolStripGripStyle.Hidden;
			Controls.Add(mnAction);

			btnReset = new ToolStripButton();
			btnReset.Image = Properties.Resources.refresh_16;
			btnReset.Click += new EventHandler(btnReset_Click);
			btnReset.Text = Properties.Resources.Button_ResetValue;
			btnReset.DisplayStyle = ToolStripItemDisplayStyle.Image;
			mnAction.Items.Add(btnReset);

			btnCancel = new ToolStripButton();
			btnCancel.Image = Properties.Resources.cancel_16;
			btnCancel.Click += new EventHandler(btnCancel_Click);
			btnCancel.Text = Properties.Resources.Button_Cancel;
			btnCancel.DisplayStyle = ToolStripItemDisplayStyle.Image;
			mnAction.Items.Add(btnCancel);

			btnOk = new ToolStripButton();
			btnOk.Image = Properties.Resources.confirm_16;
			btnOk.Click += new EventHandler(btnOk_Click);
			btnOk.Text = Properties.Resources.Button_Okey;
			btnOk.DisplayStyle = ToolStripItemDisplayStyle.Image;
			mnAction.Items.Add(btnOk);
			
			Font = new System.Drawing.Font("Tahoma", 8.25F);

			ResumeLayout(false);
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize edit operation for the given enum value
        /// </summary>
        /// <param name="service"></param>
        /// <param name="value"></param>
        public void Initialize(IWindowsFormsEditorService service, object value)
        {
            _Service = service;
            lvwItems.Items.Clear();

            var enumType = value.GetType();
            var enumValues = Enum.GetValues(enumType);
            Array.Sort(enumValues);
            var currentValue = Convert.ToInt64(value, CultureInfo.InvariantCulture);

            for (int i = 0; i < enumValues.Length; i++)
            {
                var enumValue = Convert.ToInt64(enumValues.GetValue(i), CultureInfo.InvariantCulture);
                var isChecked = false;
                if (enumValue == 0L)
                {
                    isChecked = (currentValue == 0L);
                }
                else
                {
                    isChecked = ((currentValue & enumValue) == enumValue);
                    if (isChecked)
                    {
                        currentValue &= ~enumValue;
                    }
                }

                lvwItems.Items.Add(new EnumValueContainer(enumType, enumValue), isChecked);
            }

            _LeftOver = currentValue;
            _Value = value;
        }

        /// <summary>
        /// Resets edit operation
        /// </summary>
        public void Reset()
        {
            _CancelFlag = false;
            _Service = null;
            _Value = 0;
            _LeftOver = 0;
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

        private void btnOk_Click(object sender, EventArgs e)
		{
			_Service.CloseDropDown();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			Initialize(_Service, _Value);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			_CancelFlag = true;
			_Service.CloseDropDown();
		}

        #endregion
    }
}
