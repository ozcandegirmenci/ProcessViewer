using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace ProcessViewer
{
    /// <summary>
    /// Tooltip form for form selection
    /// </summary>
    internal partial class FormSelectedTooltip : Form
	{
        #region Types

        /// <summary>
        /// Base class for defining a related window
        /// </summary>
        private abstract class RelatedWindow
        {
            #region Members

            protected string Text = string.Empty;
            protected internal IntPtr HWnd;
            protected int Indent;

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class with the provided values
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="indent"></param>
            public RelatedWindow(IntPtr hWnd, int indent)
            {
                HWnd = hWnd;
                Indent = indent;

                int textLength = NativeMethods.GetWindowTextLength(hWnd);
                if (SystemInformation.DbcsEnabled)
                    textLength = (textLength * 2) + 1;
                textLength++;

                StringBuilder text = new StringBuilder(textLength);
                NativeMethods.GetWindowText(hWnd, text, text.MaxCapacity);
                Text = text.ToString();
                if (string.IsNullOrEmpty(Text))
                    Text = string.Format(CultureInfo.InvariantCulture,
                        "{0:X8}-{1}", HWnd.ToInt32(), Properties.Resources.No_Text);
                else
                    Text = string.Format(CultureInfo.InvariantCulture,
                        "{0:X8}-{1}", HWnd.ToInt32(), Text);
            }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Draws item
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void Draw(DrawItemEventArgs e)
            {
                e.DrawBackground();
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    e.DrawFocusRectangle();

                Rectangle bounds = e.Bounds;
                int x = Indent * 5;
                int y = bounds.Top - (bounds.Height / 2);
                Point upper = new Point(x, y);
                Point bottom = new Point(x, y + bounds.Height);
                e.Graphics.DrawLine(GdiHelper.DashedPen, upper, bottom);
                Point right = new Point(x + 10, bottom.Y);
                e.Graphics.DrawLine(GdiHelper.DashedPen, bottom, right);

                bounds.X = right.X + 1;
                bounds.Width -= (right.X - bounds.X) - 3;

                DrawText(e, bounds);
            }

            /// <summary>
            /// Draws item text
            /// </summary>
            /// <param name="e"></param>
            /// <param name="bounds"></param>
            protected abstract void DrawText(DrawItemEventArgs e, Rectangle bounds);

            #endregion
        }

        /// <summary>
        /// Represents a parent related window
        /// </summary>
        private class ParentWindow : RelatedWindow
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class with the provided values
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="indent"></param>
            public ParentWindow(IntPtr hWnd, int indent)
                : base(hWnd, indent)
            { }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Draws item text
            /// </summary>
            /// <param name="e"></param>
            /// <param name="bounds"></param>
            protected override void DrawText(DrawItemEventArgs e, Rectangle bounds)
            {
                e.Graphics.DrawString(Text, Instance.Font, Brushes.Navy, bounds);
            }

            #endregion
        }

        /// <summary>
        /// Represents current window
        /// </summary>
        private class CurrentWindow : ParentWindow
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="indent"></param>
            public CurrentWindow(IntPtr hWnd, int indent)
                : base(hWnd, indent)
            { }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Draws item text
            /// </summary>
            /// <param name="e"></param>
            /// <param name="bounds"></param>
            protected override void DrawText(DrawItemEventArgs e, Rectangle bounds)
            {
                e.Graphics.DrawString(Text, GdiHelper.BoldFont, Brushes.Red, bounds);
            }

            #endregion
        }

        /// <summary>
        /// Represents a child related window
        /// </summary>
        private class ChildWindow : ParentWindow
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="indent"></param>
            public ChildWindow(IntPtr hWnd, int indent)
                : base(hWnd, indent)
            { }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Draws item text
            /// </summary>
            /// <param name="e"></param>
            /// <param name="bounds"></param>
            protected override void DrawText(DrawItemEventArgs e, Rectangle bounds)
            {
                e.Graphics.DrawString(Text, Instance.Font, Brushes.Gray, bounds);
            }

            #endregion
        }

        #endregion

        #region Members

        private static FormSelectedTooltip Instance;

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormSelectedTooltip()
		{
			Instance = this;
			InitializeComponent();

			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.FixedHeight | ControlStyles.FixedWidth, true);
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize information for the given window
        /// </summary>
        /// <param name="hWnd"></param>
        internal void SetInfo(IntPtr hWnd)
		{
			try
			{
				lblHandle.Text = string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", hWnd.ToInt32());
				IntPtr hWndParent = NativeMethods.GetParent(hWnd);
				lblParent.Text = string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", hWndParent.ToInt32());

				StringBuilder text = new StringBuilder(256);
				NativeMethods.GetClassName(hWnd, text, 256);
				lblClass.Text = text.ToString().Trim();

				int textLength = NativeMethods.GetWindowTextLength(hWnd);
				if (SystemInformation.DbcsEnabled)
					textLength = (textLength * 2) + 1;
				textLength++;

				text = new StringBuilder(textLength);
				NativeMethods.GetWindowText(hWnd, text, text.Capacity);
				lblText.Text = text.ToString();

				int pid = 0;
				int tid = NativeMethods.GetWindowThreadProcessId(hWnd, out pid);
				lblThreadProcess.Text = tid.ToString(CultureInfo.InvariantCulture) + " / " + pid.ToString(CultureInfo.InvariantCulture);


				IntPtr wnd = IntPtr.Zero;
				List<IntPtr> parents = new List<IntPtr>();
				wnd = NativeMethods.GetParent(hWnd);
				while (wnd != IntPtr.Zero)
				{
					parents.Add(wnd);
					wnd = NativeMethods.GetParent(wnd);
				}

				lvwParentList.Items.Clear();
				int indent = 1;
				for (int i = parents.Count - 1; i > -1; i--)
				{
					lvwParentList.Items.Add(new ParentWindow(parents[i], indent));
					indent++;
				}
				lvwParentList.Items.Add(new CurrentWindow(hWnd, indent));
				indent++;

				FormMain.WindowsEnumerator enumerator = new FormMain.WindowsEnumerator();
				enumerator.Begin(hWnd);

				if (enumerator.Windows.ContainsKey(pid))
				{
					List<FormMain.WindowInfo> childs = enumerator.Windows[pid];
					for (int i = 0; i < childs.Count; i++)
					{
						if (childs[i].ParentHandle == hWnd)
						{
							lvwParentList.Items.Add(new ChildWindow(childs[i].Handle, indent));
						}
					}
				}

				int height = lvwParentList.Items.Count * (lvwParentList.ItemHeight + 1) + 2;
				if (height == 0)
					height = 20;
				lvwParentList.Height = height;
				Height = lvwParentList.Bottom + 22;
			}
			catch { }
		}

        /// <summary>
        /// Chekcs that is given window exists in the list or not?
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
		internal int ContainsHandle(IntPtr hWnd)
		{
			if (Handle == hWnd)
				return 1;

			if (lvwParentList.Handle == hWnd 
				&& ((Control.ModifierKeys & Keys.Control) == Keys.Control))
			{
				int index = lvwParentList.IndexFromPoint(lvwParentList.PointToClient(Cursor.Position));
				
				if (index > -1 && index < lvwParentList.Items.Count)
				{
					RelatedWindow cont = lvwParentList.Items[index] as RelatedWindow;
					return cont.HWnd.ToInt32();
				}
				return 1;
			}

			for (int i = 0; i < Controls.Count; i++)
			{
				if (Controls[i].Handle == hWnd)
					return 1;
			}
			return 0;
		}

        /// <summary>
        /// Shows form
        /// </summary>
        public new void Show()
        {
            NativeMethods.SetWindowPos(Handle, IntPtr.Zero, Location.X, Location.Y,
                Bounds.Width, Bounds.Height,
                NativeMethods.FlagsSetWindowPos.SWP_NOACTIVATE);
            NativeMethods.ShowWindow(Handle, 4);
        }

        #endregion

        #region Protected Methods

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int)NativeMethods.Msgs.WM_MOUSEACTIVATE)
				m.Result = new IntPtr(3);
			else
				base.WndProc(ref m);
		}

        #endregion

        #region Private Methods

        private void lvwParentList_DrawItem(object sender, DrawItemEventArgs e)
		{
			RelatedWindow window = lvwParentList.Items[e.Index] as RelatedWindow;
			if (window != null)
			{
				window.Draw(e);
			}
		}

        #endregion
    }
}
