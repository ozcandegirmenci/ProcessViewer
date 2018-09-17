using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace ProcessViewer
{
    /// <summary>
    /// Manage breakpoints form
    /// </summary>
    internal partial class FormBreakpoints : Form
	{
        #region Types

        /// <summary>
        /// Helper class for message filtering operations
        /// </summary>
        private class FilterTextBoxHelper
        {
            #region Members

            private FormBreakpoints _Form = null;
            private bool _Suspend = false;

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class with the given values
            /// </summary>
            /// <param name="form"></param>
            public FilterTextBoxHelper(FormBreakpoints form)
            {
                _Form = form;
                form.txtFilter.GotFocus += new EventHandler(txtFilter_GotFocus);
                form.txtFilter.LostFocus += new EventHandler(txtFilter_LostFocus);
                form.txtFilter.TextChanged += new EventHandler(txtFilter_TextChanged);
                form.txtFilter.Text = Properties.Resources.Filter_Key;
                form.txtFilter.KeyDown += new KeyEventHandler(txtFilter_KeyDown);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Filters messages
            /// </summary>
            public void Filter()
            {
                if (_Suspend)
                    return;

                _Form.FillList();
            }

            #endregion

            #region Private Methods

            private void txtFilter_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (_Form.lvwMessages.Items.Count > 0)
                    {
                        _Form.lvwMessages.SelectedIndices.Clear();
                        _Form.lvwMessages.SelectedIndices.Add(0);
                        _Form.lvwMessages.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (_Form.lvwMessages.Items.Count > 0)
                    {
                        _Form.lvwMessages.SelectedIndices.Clear();
                        _Form.lvwMessages.SelectedIndices.Add(_Form.lvwMessages.Items.Count - 1);
                        _Form.lvwMessages.Focus();
                    }
                }
            }

            private void txtFilter_TextChanged(object sender, EventArgs e)
            {
                if (_Form.txtFilter.Text.Equals(Properties.Resources.Filter_Key))
                    _Form.txtFilter.ForeColor = Color.FromKnownColor(KnownColor.Gray);
                else
                    _Form.txtFilter.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                Filter();
            }

            private void txtFilter_LostFocus(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(_Form.txtFilter.Text))
                {
                    _Suspend = true;
                    _Form.txtFilter.Text = Properties.Resources.Filter_Key;
                    _Suspend = false;
                }
            }

            private void txtFilter_GotFocus(object sender, EventArgs e)
            {
                if (_Form.txtFilter.Text.Equals(Properties.Resources.Filter_Key))
                {
                    _Suspend = true;
                    _Form.txtFilter.Text = string.Empty;
                    _Suspend = false;
                }
            }

            #endregion
        }

        #endregion

        #region Members

        private static NativeMethods.Msgs[] mouseMessages = new NativeMethods.Msgs[] { 
				NativeMethods.Msgs.WM_LBUTTONDBLCLK,
				NativeMethods.Msgs.WM_LBUTTONDOWN,
				NativeMethods.Msgs.WM_LBUTTONUP,
				NativeMethods.Msgs.WM_MBUTTONDBLCLK,
				NativeMethods.Msgs.WM_MBUTTONDOWN,
				NativeMethods.Msgs.WM_MBUTTONUP,
				NativeMethods.Msgs.WM_RBUTTONDBLCLK,
				NativeMethods.Msgs.WM_RBUTTONDOWN,
				NativeMethods.Msgs.WM_RBUTTONUP,
				NativeMethods.Msgs.WM_MOUSEACTIVATE,
				NativeMethods.Msgs.WM_MOUSEHOVER,
				NativeMethods.Msgs.WM_MOUSELEAVE,
				NativeMethods.Msgs.WM_MOUSEMOVE,
				NativeMethods.Msgs.WM_MOUSEWHEEL
			};

        private static NativeMethods.Msgs[] keyboardMessages = new NativeMethods.Msgs[]{
				NativeMethods.Msgs.WM_CHAR,
				NativeMethods.Msgs.WM_CHARTOITEM,
				NativeMethods.Msgs.WM_DEADCHAR,
				NativeMethods.Msgs.WM_IME_KEYDOWN,
				NativeMethods.Msgs.WM_IME_KEYLAST, 
				NativeMethods.Msgs.WM_IME_KEYUP,
				NativeMethods.Msgs.WM_KEYDOWN,
				NativeMethods.Msgs.WM_KEYLAST,
				NativeMethods.Msgs.WM_KEYUP,
				NativeMethods.Msgs.WM_SYSCHAR,
				NativeMethods.Msgs.WM_SYSDEADCHAR,
				NativeMethods.Msgs.WM_SYSKEYDOWN,
				NativeMethods.Msgs.WM_SYSKEYUP
			};

        private static NativeMethods.Msgs[] ncMessages = new NativeMethods.Msgs[] {
				NativeMethods.Msgs.WM_NCACTIVATE,
				NativeMethods.Msgs.WM_NCCALCSIZE,
				NativeMethods.Msgs.WM_NCCREATE,
				NativeMethods.Msgs.WM_NCDESTROY,
				NativeMethods.Msgs.WM_NCHITTEST, 
				NativeMethods.Msgs.WM_NCLBUTTONDBLCLK,
				NativeMethods.Msgs.WM_NCLBUTTONDOWN,
				NativeMethods.Msgs.WM_NCLBUTTONUP,
				NativeMethods.Msgs.WM_NCMBUTTONDBLCLK,
				NativeMethods.Msgs.WM_NCMBUTTONDOWN,
				NativeMethods.Msgs.WM_NCMBUTTONUP,
				NativeMethods.Msgs.WM_NCMOUSEMOVE,
				NativeMethods.Msgs.WM_NCPAINT,
				NativeMethods.Msgs.WM_NCRBUTTONDBLCLK,
				NativeMethods.Msgs.WM_NCRBUTTONDOWN,
				NativeMethods.Msgs.WM_NCRBUTTONUP
			};

        private Dictionary<NativeMethods.Msgs, FormMain.MessageBreakpoint> _Watchs;
        private bool suspendSelection;
        private FilterTextBoxHelper textBoxHelper;
        private Font boltFont;

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormBreakpoints()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Initialize a new instance of this class with the provided values
        /// </summary>
        /// <param name="watchs"></param>
		public FormBreakpoints(Dictionary<NativeMethods.Msgs, FormMain.MessageBreakpoint> watchs)
			: this()
		{
			_Watchs = watchs;
			textBoxHelper = new FilterTextBoxHelper(this);
			boltFont = new Font(Font, FontStyle.Bold);
			FillList();
		}

        #endregion

        #region Private Methods

        private void FillList()
		{
			// first finish edit opeation
			FinishEdit();

			lvwMessages.Items.Clear();
			Array values = Enum.GetValues(typeof(NativeMethods.Msgs));

			Array.Sort(values);

			string filter = txtFilter.Text;
			if (filter.Equals(Properties.Resources.Filter_Key))
				filter = string.Empty;
			filter = filter.ToUpper(CultureInfo.InvariantCulture);

			bool onlyWatched = chkOnlyWactheds.Checked;
			CompareInfo info = CompareInfo.GetCompareInfo(1033);
			
			for (int i = 0; i < values.Length; i++)
			{
				NativeMethods.Msgs msg = (NativeMethods.Msgs)values.GetValue(i);
				if ((onlyWatched && !_Watchs.ContainsKey(msg)) 
					|| info.IndexOf(msg.ToString().ToUpper(CultureInfo.InvariantCulture), filter) == -1)
					continue;
				lvwMessages.Items.Add(msg);
			}

			foreach (KeyValuePair<NativeMethods.Msgs, FormMain.MessageBreakpoint> watch in _Watchs)
			{
				if (Array.IndexOf(values, watch.Key) == -1)
				{
					lvwMessages.Items.Add(watch.Key);
				}
			}
		}

		private void lvwMessages_SelectedValueChanged(object sender, EventArgs e)
		{
			if (suspendSelection)
				return;

			FinishEdit();
			if (lvwMessages.SelectedItems.Count == 0)
			{
				propertyGrid.SelectedObjects = null;
			}
			else
			{
				List<FormMain.MessageBreakpoint> watchs = new List<FormMain.MessageBreakpoint>();
				for (int i = 0; i < lvwMessages.SelectedItems.Count; i++)
				{
					NativeMethods.Msgs msg = (NativeMethods.Msgs)lvwMessages.SelectedItems[i];
					if (_Watchs.ContainsKey(msg))
						watchs.Add(_Watchs[msg]);
					else
					{
						FormMain.MessageBreakpoint mwatch = new FormMain.MessageBreakpoint();
						mwatch.Msg = msg;
						mwatch.ModifiedMsg = msg;
						mwatch.Action = FormMain.BreakpointAction.None;
						_Watchs.Add(msg, mwatch);
						watchs.Add(mwatch);
					}
				}
				propertyGrid.SelectedObjects = watchs.ToArray();
			}
		}

        private void FinishEdit()
		{
			if (propertyGrid.SelectedObjects == null || propertyGrid.SelectedObjects.Length == 0)
				return;

			foreach (FormMain.MessageBreakpoint watch in propertyGrid.SelectedObjects)
			{
				if (watch.Action == FormMain.BreakpointAction.None)
				{
					if (_Watchs.ContainsKey(watch.Msg))
						_Watchs.Remove(watch.Msg);
				}
			}
			propertyGrid.SelectedObjects = null;
		}

		private void btnMouseMessages_Click(object sender, EventArgs e)
		{
			suspendSelection = true;
			lvwMessages.SelectedItems.Clear();
			for (int i = 0; i < mouseMessages.Length; i++)
			{
				lvwMessages.SelectedItems.Add(mouseMessages[i]);
			}
			suspendSelection = false;
			lvwMessages_SelectedValueChanged(lvwMessages, EventArgs.Empty);
		}

		private void btnKeyboardMessages_Click(object sender, EventArgs e)
		{
			suspendSelection = true;
			lvwMessages.SelectedItems.Clear();
			for (int i = 0; i < keyboardMessages.Length; i++)
			{
				lvwMessages.SelectedItems.Add(keyboardMessages[i]);
			}
			suspendSelection = false;
			lvwMessages_SelectedValueChanged(lvwMessages, EventArgs.Empty);
		}

		private void btnNonClientMessages_Click(object sender, EventArgs e)
		{
			suspendSelection = true;
			lvwMessages.SelectedItems.Clear();
			for (int i = 0; i < ncMessages.Length; i++)
			{
				lvwMessages.SelectedItems.Add(ncMessages[i]);
			}
			suspendSelection = false;
			lvwMessages_SelectedValueChanged(lvwMessages, EventArgs.Empty);
		}

		private void btnAllMessages_Click(object sender, EventArgs e)
		{
			suspendSelection = true;
			lvwMessages.SelectedItems.Clear();
			for (int i = 0; i < lvwMessages.Items.Count; i++)
			{
				lvwMessages.SelectedItems.Add(lvwMessages.Items[i]);
			}
			suspendSelection = false;
			lvwMessages_SelectedValueChanged(lvwMessages, EventArgs.Empty);
		}

		private void btnAddCustomMessages_Click(object sender, EventArgs e)
		{
			FormDeclareMessage form = new FormDeclareMessage();
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				int msg = form.Msg;
				if (msg != 0)
				{
					if (!lvwMessages.Items.Contains(msg))
					{
						lvwMessages.Items.Add(msg);
					}
				}
			}
			form.Dispose();
		}

		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			if (_Watchs.Count == 0)
				return;
			
			if (MessageBox.Show(this, Properties.Resources.Breakpoints_Delete_Confirmation, Properties.Resources.Confirm,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;
			_Watchs.Clear();
			FillList();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (_Watchs.Count == 0)
				return;
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.Filter = Properties.Resources.BreakpointsFile;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					FileStream stream = null;
					BinaryWriter writer = null;

					lvwMessages.SelectedItems.Clear();
					try
					{
						stream = new FileStream(dialog.FileName, FileMode.Create);
						writer = new BinaryWriter(stream);
						writer.Write(_Watchs.Count);
						foreach (KeyValuePair<NativeMethods.Msgs, FormMain.MessageBreakpoint> watch in _Watchs)
						{
							writer.Write((int)watch.Key);
							writer.Write((int)watch.Value.Action);
							writer.Write((int)watch.Value.ModifiedMsg);
							writer.Write(watch.Value.WParam.Value);
							writer.Write(watch.Value.LParam.Value);
							writer.Write((int)watch.Value.Modifications);
						}
						writer.Flush();
						stream.Flush();
					}
					finally 
					{ 
						if (stream != null)
							stream.Close();
						if (writer != null)
							writer.Close();
					}
				}
			}
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				dialog.Filter = Properties.Resources.BreakpointsFile;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					FileStream stream = null;
					BinaryReader reader = null;
					lvwMessages.SelectedItems.Clear();
					try {
						stream = File.Open(dialog.FileName, FileMode.Open);
						reader = new BinaryReader(stream);

						int count = reader.ReadInt32();
						_Watchs.Clear();
						for (int i = 0; i < count; i++)
						{
							FormMain.MessageBreakpoint watch = new FormMain.MessageBreakpoint();
							watch.Msg = (NativeMethods.Msgs)reader.ReadInt32();
							watch.Action = (FormMain.BreakpointAction)reader.ReadInt32();
							watch.ModifiedMsg = (NativeMethods.Msgs)reader.ReadInt32();
							watch.WParam = new FormMain.MessageParam(reader.ReadInt32());
							watch.LParam = new FormMain.MessageParam(reader.ReadInt32());
							watch.Modifications = (FormMain.ModifiyingSections)reader.ReadInt32();
							_Watchs.Add(watch.Msg, watch);
						}

						lvwMessages.Invalidate();
					}
					finally {
						if (stream != null)
							stream.Close();
						if (reader != null)
							reader.Close();
					}
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			lvwMessages.SelectedItems.Clear();
			Close();
		}

		private void chkOnlyWactheds_CheckedChanged(object sender, EventArgs e)
		{
			FillList();
		}

		private void lvwMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			NativeMethods.Msgs msg = (NativeMethods.Msgs)lvwMessages.Items[e.Index];
			Brush br = SystemBrushes.WindowText;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				br = SystemBrushes.HighlightText;
			if (_Watchs.ContainsKey(msg)
				&& _Watchs[msg].Action != FormMain.BreakpointAction.None)
			{
				e.Graphics.DrawString(msg.ToString(), boltFont, br, e.Bounds);
			}
			else
			{
				e.Graphics.DrawString(msg.ToString(), lvwMessages.Font, br, e.Bounds);
			}
		}

		private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if (e.ChangedItem.PropertyDescriptor.Name == "Action"
				&& e.ChangedItem.Value.Equals(FormMain.BreakpointAction.None)
				|| e.OldValue.Equals(FormMain.BreakpointAction.None))
			{
				if (lvwMessages.SelectedIndices.Count == 0)
					return;

				if (lvwMessages.SelectedIndices.Count == 1)
				{
					lvwMessages.Invalidate(lvwMessages.GetItemRectangle(lvwMessages.SelectedIndices[0]));
				}
				else
				{
					Region rgn = null;
					GraphicsPath path = null;
					try
					{
						path = new GraphicsPath();
						for (int i = 0; i < lvwMessages.SelectedIndices.Count; i++)
						{
							path.AddRectangle(lvwMessages.GetItemRectangle(lvwMessages.SelectedIndices[i]));
						}
						rgn = new Region(path);
						lvwMessages.Invalidate(rgn);
					}
					finally
					{
						if (path != null)
							path.Dispose();
						if (rgn != null)
							rgn.Dispose();
					}
				}
			}
		}

        #endregion
    }
}
