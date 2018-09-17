using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using ProcessViewer.Design;

namespace ProcessViewer
{
    /// <summary>
    /// Main application window
    /// </summary>
    internal partial class FormMain : Form
	{
        #region Types
        
        /// <summary>
        /// TreeNode for the Processes
        /// </summary>
        private sealed class ProcessNode : WindowContainerNode
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="process"></param>
            public ProcessNode(Process process)
            {
                base.Tag = process;
                if (process != null)
                {
                    var text = new StringBuilder();
                    text.Append(process.Id);
                    text.Append(" - ");
                    text.Append(process.ProcessName);

                    Text = text.ToString();
                    try
                    {
                        base.ImageIndex = base.SelectedImageIndex =
                            FormMain.Instance._IconHelper.GetImageIndex(process.MainModule.FileName);
                    }
                    catch { }
                    base.Nodes.Add(new ModulesNode());
                }
                else
                {
                    Text = Properties.Resources.Err_UnknownProcess;
                }
            }

            #endregion
        }

        /// <summary>
        /// TreeNode for Modules
        /// </summary>
        private sealed class ModulesNode : BaseNode
        {
            #region Types

            /// <summary>
            /// PRovides comparing functionality for Modules
            /// </summary>
            private class ModuleComparer : IComparer
            {
                #region Public Methods

                /// <summary>
                /// Compares given modules
                /// </summary>
                /// <param name="x"></param>
                /// <param name="y"></param>
                /// <returns></returns>
                public int Compare(object x, object y)
                {
                    string m1Name = ((ProcessModule)x).ModuleName;
                    string m2Name = ((ProcessModule)y).ModuleName;
                    return string.Compare(m1Name, m2Name, StringComparison.OrdinalIgnoreCase);
                }

                #endregion
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the Tag of the node
            /// </summary>
            public new object Tag
            {
                get
                {
                    if (base.Parent == null)
                        return null;

                    Process process = base.Parent.Tag as Process;
                    if (process == null)
                        return null;
                    return process.Modules;
                }
                set { base.Tag = value; }
            }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public ModulesNode()
            {
                base.Text = Properties.Resources.Modules;
                base.Nodes.Add(new TreeNode());
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Occurs before the node expand
            /// </summary>
            internal override void OnBeforeExpand()
            {
                if (base.Nodes.Count != 1)
                    return;
                Instance.Cursor = Cursors.WaitCursor;
                base.Nodes.Clear();

                Process p = base.Parent.Tag as Process;
                ProcessModuleCollection modules = p.Modules;
                ProcessModule[] moduleArray = new ProcessModule[modules.Count];
                modules.CopyTo(moduleArray, 0);
                Array.Sort(moduleArray, new ModuleComparer());

                ProcessModule mainModule = p.MainModule;
                string mainName = mainModule.ModuleName;
                base.Nodes.Add(new ModuleNode(mainModule));
                base.Nodes[0].NodeFont = GdiHelper.UnderlineFont;
                for (int i = 0; i < moduleArray.Length; i++)
                {
                    if (moduleArray[i].ModuleName.Equals(mainName))
                        continue;
                    base.Nodes.Add(new ModuleNode(moduleArray[i]));
                }
                Instance.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// TreeNode for a single module
        /// </summary>
        private sealed class ModuleNode : BaseNode
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class with the provided module
            /// </summary>
            /// <param name="module"></param>
            public ModuleNode(ProcessModule module)
            {
                base.Tag = module;
                base.Text = string.Format(CultureInfo.InvariantCulture, "{0} [0x{1:X8}]", module.ModuleName, module.BaseAddress.ToInt32());

                int index = FormMain.Instance._IconHelper.GetImageIndex(module.FileName);
                if (index == -1)
                    index = 3;
                base.ImageIndex = base.SelectedImageIndex = index;
            }

            #endregion
        }

        /// <summary>
        /// TreeNode for the Windows and Controls
        /// </summary>
        private sealed class WindowNode : WindowContainerNode
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class with the provided window info
            /// </summary>
            /// <param name="info"></param>
            public WindowNode(WindowInfo info)
            {
                Tag = info;
                if (info != null)
                {
                    StringBuilder text = new StringBuilder();
                    text.AppendFormat("0x{0:x8}", info.Handle.ToInt32());
                    text.Append("    " + GetFormattedText(info.Text));
                    text.Append("    " + info.ClassName);
                    Text = text.ToString();
                    base.ImageIndex = base.SelectedImageIndex = 1;
                }
                else
                {
                    Text = Properties.Resources.Err_UnknownWindow;
                }
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Custom Draw of window nodes
            /// </summary>
            /// <param name="e"></param>
            internal override void OnDrawNode(DrawTreeNodeEventArgs e)
            {
                if (!base.IsVisible)
                    return;

                Font font = GetFont();
                e.DrawDefault = false;
                bool isSelected = ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                    && ((e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused);

                WindowInfo info = Tag as WindowInfo;
                string str = "0x" + info.Handle.ToInt32().ToString("x8", CultureInfo.InvariantCulture);
                SizeF sf = e.Graphics.MeasureString(str, font);

                Rectangle rct = new Rectangle(e.Bounds.X, e.Bounds.Y, (int)sf.Width + 2, e.Bounds.Height);
                e.Graphics.DrawString(str, font, isSelected ? Brushes.Cyan : Brushes.Red, rct, GdiHelper.CenterFormat);

                string text = GetFormattedText(info.Text);
                sf = e.Graphics.MeasureString(text, TreeView.Font);
                rct = new Rectangle(rct.Right, e.Bounds.Y, (int)sf.Width + 2, e.Bounds.Height);
                e.Graphics.DrawString(text, font, isSelected ? Brushes.White : Brushes.Navy, rct, GdiHelper.CenterFormat);

                rct = new Rectangle(rct.Right, rct.Top, e.Bounds.Right - rct.Right, e.Bounds.Height);
                e.Graphics.DrawString(info.ClassName, font, isSelected ? Brushes.LightGray : Brushes.Gray, rct, GdiHelper.CenterFormat);
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Returns the font that will be used for this tree node
            /// </summary>
            /// <returns></returns>
            private Font GetFont()
            {
                if (base.NodeFont == null)
                    return TreeView.Font;
                return base.NodeFont;
            }

            /// <summary>
            /// Returns the text
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            private static string GetFormattedText(string text)
            {
                if (text.Length > 60)
                    return text.Substring(0, 57) + "...";
                return text;
            }

            #endregion
        }

        /// <summary>
        /// Helper class for EnumWindows. Enumerates the given window's child windows.
        /// </summary>
        internal class WindowsEnumerator
        {
            #region Properties

            /// <summary>
            /// Gets the list of child windows
            /// </summary>
            public Dictionary<int, List<WindowInfo>> Windows { get; private set; } = new Dictionary<int, List<WindowInfo>>();

            /// <summary>
            /// Gets the handle of the root window
            /// </summary>
            public IntPtr RootWindow { get; private set; } = IntPtr.Zero;

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public WindowsEnumerator()
            {
                
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Begins enumeration for the Desktop window
            /// </summary>
            public void Begin()
            {
                RootWindow = NativeMethods.GetDesktopWindow();

                Begin(RootWindow);
            }

            /// <summary>
            /// Begins enumeration for the given parent window
            /// </summary>
            /// <param name="hWndParent"></param>
            public void Begin(IntPtr hWndParent)
            {
                Windows.Clear();

                NativeMethods.CallbackEnum enumChildCallback = new NativeMethods.CallbackEnum(EnumChildCallBack);
                NativeMethods.EnumChildWindows(hWndParent, enumChildCallback, 0);
            }

            #endregion

            #region Private Methods

            private int EnumChildCallBack(IntPtr hWnd, int lParam)
            {
                WindowInfo info = new WindowInfo(hWnd);

                if (Windows.ContainsKey(info.ProcessId))
                {
                    Windows[info.ProcessId].Add(info);
                }
                else
                {
                    List<WindowInfo> controls = new List<WindowInfo>
                    {
                        info
                    };
                    Windows.Add(info.ProcessId, controls);
                }
                return 1;
            }

            #endregion
        }

        /// <summary>
        /// Our site class which we are using this just for Designer Verbs support in the PropertyGrid.
        /// PropertyGrid needs IMenuCommandService or IDesignerHost to support DesignerVerbs
        /// </summary>
        private class WindowsSite : ISite, IMenuCommandService
        {
            #region Members

            private WindowInfo _Component;
            private IContainer _Container;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the component
            /// </summary>
            public IComponent Component
            {
                get { return _Component; }
            }

            /// <summary>
            /// Gets the container
            /// </summary>
            public IContainer Container
            {
                get
                {
                    if (_Container == null)
                        _Container = new Container();
                    return _Container;
                }
            }

            /// <summary>
            /// Gets that is design mode or not?
            /// </summary>
            public bool DesignMode
            {
                get { return false; }
            }

            /// <summary>
            /// Gets the name of the site
            /// </summary>
            public string Name
            {
                get
                {
                    return _Component.Handle.ToString();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Gets the collection of Designer Verbs
            /// </summary>
            public DesignerVerbCollection Verbs { get; private set; }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="info"></param>
            /// <param name="container"></param>
            public WindowsSite(WindowInfo info, IContainer container)
            {
                _Component = info;
                _Container = container;
                Verbs = new DesignerVerbCollection();

                AddVerb(new DesignerVerb(Properties.Resources.Cmd_ShowWindow, new EventHandler(ShowWindow)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_HideWindow, new EventHandler(HideWindow)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_EnableWindow, new EventHandler(EnableWindow)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_DisableWindow, new EventHandler(DisableWindow)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_Highlight, new EventHandler(Highlight)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_ListenMessages, new EventHandler(ListenMessages)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_StopListening, new EventHandler(StopMessages)));
                AddVerb(new DesignerVerb(Properties.Resources.Cmd_Invalidate, new EventHandler(Invalidate)));
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns the service
            /// </summary>
            /// <param name="serviceType"></param>
            /// <returns></returns>
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IMenuCommandService))
                    return this;
                return null;
            }

            /// <summary>
            /// Adds a designer verb to the collection
            /// </summary>
            /// <param name="verb"></param>
            public void AddVerb(DesignerVerb verb)
            {
                Verbs.Add(verb);
            }

            /// <summary>
            /// Remove designer verb from the collection
            /// </summary>
            /// <param name="verb"></param>
            public void RemoveVerb(DesignerVerb verb)
            {
                Verbs.Remove(verb);
            }

            #region " Not Need To Implement "
            public void AddCommand(MenuCommand command)
            {
                throw new NotImplementedException();
            }

            public MenuCommand FindCommand(CommandID commandID)
            {
                throw new NotImplementedException();
            }

            public bool GlobalInvoke(CommandID commandID)
            {
                throw new NotImplementedException();
            }

            public void RemoveCommand(MenuCommand command)
            {
                throw new NotImplementedException();
            }

            public void ShowContextMenu(CommandID menuID, int x, int y)
            {
                throw new NotImplementedException();
            }
            #endregion

            #endregion

            #region Private Methods

            private void Invalidate(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                _Component.Invalidate();
            }

            private void ListenMessages(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                FormMain.Instance._SubClass.Begin(_Component);
            }

            private void StopMessages(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                FormMain.Instance._SubClass.End();
            }

            private void ShowWindow(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                _Component.Visible = true;
            }

            private void HideWindow(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                _Component.Visible = false;
            }

            private void EnableWindow(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                _Component.Enabled = true;
            }

            private void DisableWindow(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;
                _Component.Enabled = false;
            }

            private void Highlight(object sender, EventArgs e)
            {
                if (_Component == null)
                    return;

                FormMain.Instance.Highlight();
            }

            #endregion
        }

        /// <summary>
        /// Provides get and setting some information about a Window or Control
        /// </summary>
        internal class WindowInfo : Component
        {
            #region Members
            
            private string _Text;

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the Site object
            /// </summary>
            [Browsable(false)]
            public override ISite Site
            {
                get
                {
                    if (Properties.Settings.Default.ShowVerbs)
                    {
                        return new WindowsSite(this, FormMain.Instance._Container);
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    base.Site = value;
                }
            }

            /// <summary>
            /// Gets or sets the text of the window
            /// </summary>
            [DefaultValue("")]
            [SRDescription("Des_WindowInfo_Text")]
            [SRCategory("Cat_Appearance")]
            [Editor(typeof(Design.MultilineTextEditor), typeof(UITypeEditor))]
            public string Text
            {
                get
                {
                    if (_Text == null)
                    {
                        int textLength = NativeMethods.GetWindowTextLength(Handle);
                        if (SystemInformation.DbcsEnabled)
                            textLength = (textLength * 2) + 1;
                        textLength++;

                        StringBuilder text = new StringBuilder(textLength);
                        //try
                        //{
                        Marshal.ThrowExceptionForHR(NativeMethods.GetWindowText(Handle, text, text.Capacity));
                        //}
                        //catch (IndexOutOfRangeException)
                        //{
                        //    // this is just for protecting from a bug (ToolStripContainer throws indexoutofrange when trying to get its window text)
                        //    text = new StringBuilder(255);
                        //    Marshal.ThrowExceptionForHR(NativeMethods.GetWindowText(Handle, text, text.Capacity));
                        //}
                        _Text = text.ToString();
                    }
                    return _Text;
                }
                set
                {
                    if (Text == value)
                        return;

                    Marshal.ThrowExceptionForHR(NativeMethods.SetWindowText(Handle, value));
                    _Text = value;
                }
            }

            /// <summary>
            /// Gets or sets the visibility of the window
            /// </summary>
            [DefaultValue(true)]
            [SRCategory("Cat_Appearance")]
            [SRDescription("Des_WindowInfo_Visible")]
            public bool Visible
            {
                get
                {
                    return NativeMethods.IsWindowVisible(Handle);
                }
                set
                {
                    if (Visible == value)
                        return;

                    if (value)
                        Marshal.ThrowExceptionForHR(NativeMethods.ShowWindow(Handle, NativeMethods.SW_SHOWNORMAL));
                    else
                        Marshal.ThrowExceptionForHR(NativeMethods.ShowWindow(Handle, NativeMethods.SW_HIDE));
                }
            }

            /// <summary>
            /// Gets or sets that is window enabled or not?
            /// </summary>
            [DefaultValue(true)]
            [SRCategory("Cat_Appearance")]
            [SRDescription("Des_WindowInfo_Enabled")]
            public bool Enabled
            {
                get
                {
                    return !((Styles & NativeMethods.WindowStyles.WS_DISABLED) == NativeMethods.WindowStyles.WS_DISABLED);
                }
                set
                {
                    if (value)
                    {
                        Styles &= ~NativeMethods.WindowStyles.WS_DISABLED;
                    }
                    else
                    {
                        Styles |= NativeMethods.WindowStyles.WS_DISABLED;
                    }

                    Invalidate();
                }
            }

            /// <summary>
            /// Gets or sets the window styles
            /// </summary>
            [DefaultValue(0)]
            [SRCategory("Cat_Appearance")]
            [SRDescription("Des_WindowInfo_Styles")]
            [Editor(typeof(Design.FlagsEditor), typeof(UITypeEditor))]
            public NativeMethods.WindowStyles Styles
            {
                get
                {
                    return (NativeMethods.WindowStyles)NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_STYLE);
                }
                set
                {
                    if (Styles == value)
                        return;

                    NativeMethods.SetWindowLong(Handle, NativeMethods.GWL_STYLE, Convert.ToUInt32(value, CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Gets or sets the window ex styles
            /// </summary>
            [DefaultValue(0)]
            [SRCategory("Cat_Appearance")]
            [SRDescription("Des_WindowInfo_ExStyles")]
            [Editor(typeof(Design.FlagsEditor), typeof(UITypeEditor))]
            public NativeMethods.WindowExStyles ExStyles
            {
                get
                {
                    return (NativeMethods.WindowExStyles)NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_EXSTYLE);
                }
                set
                {
                    if (ExStyles == value)
                        return;

                    NativeMethods.SetWindowLong(Handle, NativeMethods.GWL_EXSTYLE, Convert.ToUInt32(value, CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Gets or sets the Bounds of the window
            /// </summary>
            [SRCategory("Cat_Layout")]
            [SRDescription("Des_WindowInfo_Bounds")]
            public Rectangle Bounds
            {
                get
                {
                    NativeMethods.RECT rect = new NativeMethods.RECT();
                    NativeMethods.GetWindowRect(Handle, ref rect);

                    // TODO: Not completed, remove this part
                    // Because of DPI changes we must resolve this issue
                    var dc = NativeMethods.GetDC(Handle);
                    var dpi = NativeMethods.GetDeviceCaps(dc, 88);

                    float m = Convert.ToSingle(dpi) / 96f;
                    return new Rectangle(Convert.ToInt32(rect.left * m), Convert.ToInt32(rect.top * m), Convert.ToInt32(rect.Width * m), Convert.ToInt32(rect.Height * m));
                }
                set
                {
                    if (Bounds == value)
                        return;

                    NativeMethods.SetWindowPos(Handle, ParentHandle,
                        value.X, value.Y, value.Width, value.Height, NativeMethods.FlagsSetWindowPos.SWP_NOACTIVATE);
                }
            }

            /// <summary>
            /// Gets or sets the location of the window
            /// </summary>
            [SRCategory("Cat_Layout")]
            [SRDescription("Des_WindowInfo_Location")]
            public Point Location
            {
                get { return Bounds.Location; }
                set
                {
                    if (Bounds.Location == value)
                        return;
                    Rectangle rct = Bounds;
                    Bounds = new Rectangle(value, rct.Size);
                }
            }

            /// <summary>
            /// Gets or sets the Size of the window
            /// </summary>
            [SRCategory("Cat_Layout")]
            [SRDescription("Des_WindowInfo_Size")]
            public Size Size
            {
                get { return Bounds.Size; }
                set
                {
                    if (Bounds.Size == value)
                        return;
                    Rectangle rct = Bounds;
                    Bounds = new Rectangle(rct.Location, value);
                }
            }

            /// <summary>
            /// Gets or sets that is window capture mouse and keyboard or not at the moment
            /// </summary>
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_WindowInfo_Capture")]
            [DefaultValue(false)]
            public bool Capture
            {
                get
                {
                    return (NativeMethods.GetCapture() == Handle);
                }
                set
                {
                    if (Capture == value)
                        return;
                    if (value)
                    {
                        NativeMethods.SetCapture(Handle);
                    }
                    else
                    {
                        NativeMethods.ReleaseCapture();
                    }
                }
            }

            /// <summary>
            /// Gets the class name of the window
            /// </summary>
            [ReadOnly(true)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_WindowInfo_ClassName")]
            public string ClassName { get; private set; }
            
            /// <summary>
            /// Gets the process id of the window
            /// </summary>
            [ReadOnly(true)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_WindowInfo_ProcessId")]
            public int ProcessId { get; private set; }
            
            /// <summary>
            /// Gets the thread id of the window
            /// </summary>
            [ReadOnly(true)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_WindowInfo_ThreadId")]
            public int ThreadId { get; private set; }
            
            /// <summary>
            /// Gets the handle of the window
            /// </summary>
            [ReadOnly(true)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_WindowInfo_Handle")]
            [TypeConverter(typeof(HandleConverter))]
            public IntPtr Handle { get; private set; }
            
            /// <summary>
            /// Gets the parent handle of the window
            /// </summary>
            [ReadOnly(true)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_WindowInfo_ParentHandle")]
            [TypeConverter(typeof(HandleConverter))]
            public IntPtr ParentHandle { get; private set; }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="hWnd"></param>
            public WindowInfo(IntPtr hWnd)
            {
                Handle = hWnd;

                ParentHandle = NativeMethods.GetParent(hWnd);

                StringBuilder text = new StringBuilder(256);
                int hr = NativeMethods.GetClassName(hWnd, text, 256);
                Marshal.ThrowExceptionForHR(hr);
                ClassName = text.ToString().Trim();

                var processId = 0;
                ThreadId = NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
                ProcessId = processId;
                Marshal.ThrowExceptionForHR(ThreadId);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Invalidates the control
            /// </summary>
            public void Invalidate()
            {
                NativeMethods.SendMessage(Handle,
                    (int)NativeMethods.Msgs.WM_PAINT,
                    IntPtr.Zero, IntPtr.Zero);
            }

            #endregion
        }

        /// <summary>
        /// <see cref="System.ComponentModel.TypeConverter"/> for the Handle values.
        /// Before converting to string, this converter first converts Handle to <see cref="System.Int32"/> and than shows as 
        /// Hex values.
        /// </summary>
        internal class HandleConverter : TypeConverter
        {
            #region Public Methods

            /// <summary>
            /// Returns that can convert to given type
            /// </summary>
            /// <param name="context"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            /// <summary>
            /// Returns the converted value to the target type
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", ((IntPtr)value).ToInt32());
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            #endregion
        }

        /// <summary>
        /// Breakpoint information
        /// </summary>
        [TypeConverter(typeof(MessageBreakpointTypeConverter))]
        internal class MessageBreakpoint
        {
            #region Properties

            /// <summary>
            /// Gets or sets the window message
            /// </summary>
            [ReadOnly(true)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_MessageBreakpoint_Msg")]
            public NativeMethods.Msgs Msg { get; set; } = NativeMethods.Msgs.WM_NULL;

            /// <summary>
            /// Gets or sets the action
            /// </summary>
            [DefaultValue(BreakpointAction.None)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_MessageBreakpoint_Action")]
            [RefreshProperties(RefreshProperties.All)]
            public BreakpointAction Action { get; set; } = BreakpointAction.None;

            /// <summary>
            /// Gets or sets the wparam
            /// </summary>
            [DefaultValue(0)]
            [SRCategory("Cat_Data")]
            [SRDescription("Des_MessageBreakpoint_WParam")]
            public MessageParam WParam { get; set; }

            /// <summary>
            /// Gets or sets the lparam
            /// </summary>
            [DefaultValue(0)]
            [SRCategory("Cat_Data")]
            [SRDescription("Des_MessageBreakpoint_LParam")]
            public MessageParam LParam { get; set; }

            /// <summary>
            /// Gets or sets the modified message
            /// </summary>
            [DefaultValue(NativeMethods.Msgs.WM_NULL)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_MessageBreakpoint_ModifiedMsg")]
            [TypeConverter(typeof(MsgTypeConverter))]
            public NativeMethods.Msgs ModifiedMsg { get; set; } = NativeMethods.Msgs.WM_NULL;

            /// <summary>
            /// Gets or sets the type of the modifications in the message
            /// </summary>
            [Editor(typeof(Design.FlagsEditor), typeof(UITypeEditor))]
            [DefaultValue(ModifiyingSections.WParam | ModifiyingSections.Message | ModifiyingSections.LParam)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_MessageBreakpoint_Modifications")]
            [RefreshProperties(RefreshProperties.All)]
            public ModifiyingSections Modifications { get; set; } = ModifiyingSections.LParam
                                                | ModifiyingSections.Message
                                                | ModifiyingSections.WParam;

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public MessageBreakpoint()
            { }

            #endregion
        }

        /// <summary>
        /// Indicates the sections of a Windows Message
        /// </summary>
        [Flags]
        internal enum ModifiyingSections
        {
            Message = 0x01,
            WParam = 0x02,
            LParam = 0x04
        }

        /// <summary>
        /// TypeConverter for the <see cref="MessageBreakpoint"/> class.
        /// The main goal of this class is hiding <see cref="MessageBrealPoint.WParam"/> and
        /// <see cref="MessageBrealPoint.LParam"/> properties if the <see cref="MessageBrealPoint.Action"/>
        /// is not <see cref="BreakpointAction.AutoChangeParameters"/>.
        ///  
        /// Because only for this kind of action we need WParam and LParam properties.
        /// </summary>
        private class MessageBreakpointTypeConverter : TypeConverter
        {
            #region Public Methods

            /// <summary>
            /// Returns that properties supported or not by this TypeConverter?
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            /// <summary>
            /// Returns the list of properties
            /// </summary>
            /// <param name="context"></param>
            /// <param name="value"></param>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                PropertyDescriptorCollection ps = TypeDescriptor.GetProperties(value, attributes);

                MessageBreakpoint watch = value as MessageBreakpoint;
                if (watch.Action != BreakpointAction.AutoChangeParameters)
                {
                    // if action is not AutoChangeParameters then
                    // Hide WParam and LParam properties from the PropertyGrid
                    int count = 0;
                    PropertyDescriptor[] filtereds = new PropertyDescriptor[ps.Count - 4];
                    for (int i = 0; i < ps.Count; i++)
                    {
                        if (ps[i].Name == "WParam" || ps[i].Name == "LParam"
                            || ps[i].Name == "ModifiedMsg" || ps[i].Name == "Modifications")
                            continue;

                        filtereds[count] = ps[i];
                        count++;
                    }
                    return new PropertyDescriptorCollection(filtereds);
                }
                else
                {
                    List<PropertyDescriptor> ds = new List<PropertyDescriptor>();
                    for (int i = 0; i < ps.Count; i++)
                    {
                        if (ps[i].Name == "WParam")
                        {
                            if ((watch.Modifications & ModifiyingSections.WParam)
                                != ModifiyingSections.WParam)
                                continue;
                        }
                        else if (ps[i].Name == "LParam")
                        {
                            if ((watch.Modifications & ModifiyingSections.LParam)
                                    != ModifiyingSections.LParam)
                                continue;
                        }
                        else if (ps[i].Name == "ModifiedMsg")
                        {
                            if ((watch.Modifications & ModifiyingSections.Message)
                                    != ModifiyingSections.Message)
                                continue;
                        }

                        ds.Add(ps[i]);
                    }
                    return new PropertyDescriptorCollection(ds.ToArray());
                }
            }

            #endregion
        }

        /// <summary>
        /// Type Converter for the <see cref="NativeMethods.Msgs"/> enum, because default
        /// enum converter does not allow int editing and also does not allow values which are not a member of 
        /// this enum. So we try to allow int edit and set a value which is not a member of <see cref="NativeMethods.Msgs"/> enum.
        /// </summary>
        internal class MsgTypeConverter : EnumConverter
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public MsgTypeConverter()
                : base(typeof(NativeMethods.Msgs))
            { }

            #endregion

            #region Public Methods

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(int))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(int))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(int))
                    return Convert.ToInt32(value, CultureInfo.InvariantCulture);
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                Type valType = value.GetType();
                if (valType == typeof(int))
                    return Enum.ToObject(typeof(NativeMethods.Msgs), Convert.ToInt32(value, CultureInfo.InvariantCulture));
                return base.ConvertFrom(context, culture, value);
            }

            public override bool IsValid(ITypeDescriptorContext context, object value)
            {
                if (value.GetType() == typeof(int))
                    return true;
                return base.IsValid(context, value);
            }

            /// <summary>
            /// We are allowing new values which are not a member of <see cref="NativeMethods.Msgs"/> enum.
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            #endregion
        }

        /// <summary>
        /// Class for the Message parameters.
        /// Many of the Windows Messages uses Hi-Order and Lo-Order of the WParam and LParam
        /// parameters for different purposes..
        ///  
        /// This class allows us to edit these parameters' Hi and Lo orders separately.
        /// Also it allows editing original paramter value
        /// </summary>
        [TypeConverter(typeof(MessageParamConverter))]
        internal struct MessageParam
        {
            #region Properties

            /// <summary>
            /// Gets or sets the low word or der value
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            [DefaultValue(0)]
            [SRCategory("Cat_Detail")]
            [SRDescription("Des_MessageParam_LoWord")]
            public int LoWord
            {
                get { return NativeMethods.LoWord(Value); }
                set
                {
                    Value = (value & 0xffff) | HiWord << 0x10;
                }
            }

            /// <summary>
            /// Gets or sets the High word of the value
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            [DefaultValue(0)]
            [SRCategory("Cat_Detail")]
            [SRDescription("Des_MessageParam_HiWord")]
            public int HiWord
            {
                get { return NativeMethods.HiWord(Value); }
                set
                {
                    Value = LoWord | (value & 0xffff) << 0x10;
                }
            }

            /// <summary>
            /// Gets or sets the value
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            [DefaultValue(0)]
            [SRCategory("Cat_Behaviour")]
            [SRDescription("Des_MessageParam_Value")]
            [Browsable(false)]
            public int Value { get; private set; }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            /// <param name="value"></param>
            public MessageParam(int value)
            {
                Value = value;
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns the string representation fo this class
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Value.ToString(CultureInfo.InvariantCulture);
            }

            #endregion
        }

        /// <summary>
        /// TypeConverter for the <see cref="MessageParam"/> class.
        /// </summary>
        internal class MessageParamConverter : ExpandableObjectConverter
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public MessageParamConverter()
            { }

            #endregion

            #region Public Methods

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string)
                    || sourceType == typeof(int)
                    || sourceType == typeof(IntPtr))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string)
                    || destinationType == typeof(int)
                    || destinationType == typeof(IntPtr))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                MessageParam param = (MessageParam)value;
                if (destinationType == typeof(string))
                    return param.ToString();
                else if (destinationType == typeof(int))
                    return param.Value;
                else if (destinationType == typeof(IntPtr))
                    return new IntPtr(param.Value);
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                Type type = value.GetType();
                if (type == typeof(string)
                    || type == typeof(int) || type == typeof(IntPtr))
                    return new MessageParam(Convert.ToInt32(value, CultureInfo.InvariantCulture));

                return base.ConvertFrom(context, culture, value);
            }

            #endregion
        }

        /// <summary>
        /// Represents actions for a breakpoint
        /// </summary>
        internal enum BreakpointAction
        {
            /// <summary>
            ///  Do nothing
            /// </summary>
            None = 0,
            /// <summary>
            ///  Ignore message
            /// </summary>
            IgnoreMessage,
            /// <summary>
            ///  Edit message parameters manually
            /// </summary>
            ManuelEditParameters,
            /// <summary>
            ///  Automatically set message parameters to predefined values
            /// </summary>
            AutoChangeParameters
        }

        /// <summary>
        /// A control for listening messages from ProcessViewer.Hooks.dll (Our Hook dll)
        /// </summary>
        /// <remarks>
        /// This class is really very important. Because it allows us to transport informations between 
        /// Hooked application and our application.
        ///  
        /// Whenever a message comes to our Hooked Window it redirects this message to this control
        /// by using WM_COPYDATA message.
        ///  
        /// Also Hook dll sends messages to this control about Parameters changing and result of the message
        /// </remarks>
        private class ListenerControl : Control
        {
            #region Members

            private static int HM_MESSAGE_RESULT;
            //private static int HM_PARAMETERS_CHANGED;
            #endregion

            #region Initialization
            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public ListenerControl()
            {
                HM_MESSAGE_RESULT = NativeMethods.RegisterWindowMessage("ProcessViewer_MessageResult");
                //HM_PARAMETERS_CHANGED = NativeMethods.RegisterWindowMessage("ProcessViewer_ParametersChanged");
                Parent = Instance;
            }

            #endregion

            #region Protected Methods

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == (int)NativeMethods.Msgs.WM_COPYDATA)
                {
                    // message was sended by ProcessViewer.Hooks.dll from the Hooked application when
                    // a new message comes to that window

                    // LPARAM of this message contains a COPYDATASTRUCT which has HOOK_MSG struct in it
                    NativeMethods.COPYDATASTRUCT cdata =
                        (NativeMethods.COPYDATASTRUCT)m.GetLParam(typeof(NativeMethods.COPYDATASTRUCT));
                    // This is the information of the message which is sended to the hooked window
                    NativeMethods.HOOK_MSG msg = (NativeMethods.HOOK_MSG)Marshal.PtrToStructure(cdata.lpData,
                        typeof(NativeMethods.HOOK_MSG));

                    // process message and set its result (0 ignore, 1 do nothing, other values replace parameters)
                    m.Result = Instance._SubClass.ProcessMessage(m.WParam, ref msg);
                    Marshal.DestroyStructure(m.LParam, typeof(NativeMethods.COPYDATASTRUCT));
                }
                #region DELETED
                //else if (m.Msg == HM_PARAMETERS_CHANGED)
                //{
                //    // this message is also sended by hooked window
                //    // to inform us parameters are changed by us
                //    StringBuilder text = new StringBuilder();
                //    text.Append(Properties.Resources.Hook_Parameters_Changed);
                //    text.Append(" wParam->" + m.WParam.ToInt32());
                //    text.Append(", lParam->" + m.LParam.ToInt32());
                //    text.AppendLine();

                //    MainForm.txtMessages.AppendText(text.ToString());
                //}
                #endregion
                else if (m.Msg == HM_MESSAGE_RESULT
                    && Properties.Settings.Default.HandleMessageResults)
                {
                    // this message sended by hooked window to give information about the result of a message
                    Instance._SubClass.ProcessMessageResult(m.WParam.ToInt32(), m.LParam);
                }
                else
                {
                    base.WndProc(ref m);
                }
            }

            #endregion
        }

        /// <summary>
        /// Manages SubClassing operations (Start, Stop and Processing messages)
        /// </summary>
        private class SubClassManager : IDisposable
        {
            #region Members

            private Dictionary<NativeMethods.Msgs, string> msgValues;
            private ListenerControl listener;
            private bool _Enabled = true;
            private bool _ShowMessages = true;
            private WindowInfo _Window;

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets that is subclassing enabled or not?
            /// </summary>
            public bool Enabled
            {
                get { return _Enabled; }
                set
                {
                    if (_Enabled == value)
                        return;
                    _Enabled = value;
                    if (value)
                    {
                        FormMain.Instance.sbarWatcher.Text = Properties.Resources.Breakpoints_On;
                        FormMain.Instance.btnWatcherOff.Checked = false;
                        FormMain.Instance.btnWatcherOn.Checked = true;
                    }
                    else
                    {
                        FormMain.Instance.sbarWatcher.Text = Properties.Resources.Breakpoints_Off;
                        FormMain.Instance.btnWatcherOff.Checked = true;
                        FormMain.Instance.btnWatcherOn.Checked = false;
                    }
                    Application.DoEvents();
                }
            }

            /// <summary>
            /// Gets or sets that will messages will be show in the logs
            /// </summary>
            public bool ShowMessages
            {
                get { return _ShowMessages; }
                set
                {
                    if (_ShowMessages == value)
                        return;
                    _ShowMessages = value;
                    Instance.btnShowMessages.Checked = value;
                    Instance.showMessagesLogToolStripMenuItem.Checked = value;
                    if (!value)
                        Instance.txtMessages.Text = string.Empty;
                }
            }

            /// <summary>
            /// Gets the list of breakpoints
            /// </summary>
            public Dictionary<NativeMethods.Msgs, FormMain.MessageBreakpoint> Breakpoints { get; private set; }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public SubClassManager()
            {
                Type enumType = typeof(NativeMethods.Msgs);
                Array msgArray = Enum.GetValues(enumType);
                msgValues = new Dictionary<NativeMethods.Msgs, string>(msgArray.Length);
                for (int i = 0; i < msgArray.Length; i++)
                {
                    NativeMethods.Msgs value = (NativeMethods.Msgs)msgArray.GetValue(i);
                    string name = Enum.GetName(enumType, value);

                    if (msgValues.ContainsKey(value))
                        msgValues[value] = name;
                    else
                        msgValues.Add(value, name);
                }
                // create Listener control which we will use to interact with the hooked window
                listener = new ListenerControl();

                Breakpoints = new Dictionary<NativeMethods.Msgs, MessageBreakpoint>();
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Process a message result
            /// </summary>
            /// <param name="message"></param>
            /// <param name="result"></param>
            internal void ProcessMessageResult(int message, IntPtr result)
            {
                if (ShowMessages)
                {
                    // write result of a message
                    StringBuilder text = new StringBuilder();
                    text.Append(Properties.Resources.Hook_Result);
                    NativeMethods.Msgs msg = (NativeMethods.Msgs)message;
                    if (msgValues.ContainsKey(msg))
                        text.Append(msgValues[msg]);
                    else
                        text.Append(msg);
                    text.Append(" -> " + result.ToInt32());
                    text.AppendLine();

                    Instance.txtMessages.AppendText(text.ToString());
                }
            }

            /// <summary>
            ///  Process message which belongs to the hooked window
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="hookedMessage"></param>
            /// <returns>
            ///		This return value will be sended to the Hooked window by our Listener Control
            ///		
            ///		Return value might be;
            ///		0 : which means ignore this message (Hooked window will ignore this message)
            ///		1 : Do nothing (Hooked window will do its original action for this message)
            ///		<Other Values> : We are going to change the wParam and lParam for this message
            ///		here the return value indicates an adress in the target process memory which contains
            ///		the new values of the wParam and lParam
            ///		By passing this address to the Hooked window, we make it read the new values
            ///		from this address and replace original wParam and lParam with the new values
            ///		and process message according to this new parameters
            /// </returns>
            internal IntPtr ProcessMessage(IntPtr hWnd, ref NativeMethods.HOOK_MSG hookedMessage)
            {
                lock (this)
                {
                    IntPtr result = IntPtr.Zero;
                    NativeMethods.Msgs msg = (NativeMethods.Msgs)hookedMessage.msg;
                    if (Enabled && Breakpoints.ContainsKey(msg))
                    {
                        // if Breakpoints enabled and if we have a breakpoint for the
                        // given message then do our action
                        MessageBreakpoint watch = Breakpoints[msg];
                        switch (watch.Action)
                        {
                            case BreakpointAction.IgnoreMessage:
                                result = new IntPtr(1);
                                break;
                            case BreakpointAction.ManuelEditParameters:
                                FormEditMessage em = new FormEditMessage(hWnd, msg, hookedMessage.wParam, hookedMessage.lParam);
                                em.ShowDialog(FormMain.Instance);
                                if (!em.Ignore)
                                    result = WriteToTargetProcessMemory(em.WParam, em.LParam, em.ModifiedMsg);
                                else
                                    result = new IntPtr(1);
                                break;
                            case BreakpointAction.AutoChangeParameters:
                                NativeMethods.Msgs modifiedMsg = (NativeMethods.Msgs)hookedMessage.msg;
                                IntPtr wParam = hookedMessage.wParam;
                                IntPtr lParam = hookedMessage.lParam;

                                if ((watch.Modifications & ModifiyingSections.Message) == ModifiyingSections.Message
                                    && watch.ModifiedMsg != NativeMethods.Msgs.WM_NULL)
                                    modifiedMsg = watch.ModifiedMsg;

                                if ((watch.Modifications & ModifiyingSections.WParam) == ModifiyingSections.WParam)
                                    wParam = new IntPtr(watch.WParam.Value);

                                if ((watch.Modifications & ModifiyingSections.LParam) == ModifiyingSections.LParam)
                                    lParam = new IntPtr(watch.LParam.Value);


                                result = WriteToTargetProcessMemory(wParam, lParam, modifiedMsg);
                                break;
                        }
                    }

                    if (ShowMessages)
                    {
                        StringBuilder text = new StringBuilder();
                        text.AppendFormat("0x{0:X8}", hWnd.ToInt32());
                        text.Append("  ");
                        if (msgValues.ContainsKey(msg))
                            text.Append(msgValues[msg]);
                        else
                            text.Append(msg);

                        text.Append("  ");
                        text.AppendFormat("wParam:{0}", hookedMessage.wParam);
                        text.AppendFormat(" lParam:{0}", hookedMessage.lParam);
                        if (result == (IntPtr)1)
                            text.Append(Properties.Resources.Hook_Ignored);
                        text.AppendLine();

                        FormMain.Instance.txtMessages.AppendText(text.ToString());
                    }

                    return result;
                }
            }

            /// <summary>
            ///  Begin Hook operation
            /// </summary>
            /// <param name="window"></param>
            public void Begin(WindowInfo window)
            {
                // first try to end (if exist any hook)
                End();

                // do not allow hook on PV's log messages window (might cause problems)
                if (window.Handle == Instance.txtMessages.Handle)
                    return;

                _Window = window;
                // Call StartHook method from our ProcessViewer.Hooks.dll
                int hhk = NativeMethods.StartHook(window.Handle, listener.Handle);
                if (hhk == 0)
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                FormMain.Instance.pnlMessages.Visible = true;
                Instance.SetVerbsHeight();
            }

            /// <summary>
            ///  Ends hook operation
            /// </summary>
            public void End()
            {
                // Call EndHook method from our ProcessViewer.Hooks.dll
                NativeMethods.EndHook();
                if (Properties.Settings.Default.ResetBreakpointsOnChange)
                    Clear();

                FormMain.Instance.pnlMessages.Visible = false;
                FormMain.Instance.txtMessages.Text = string.Empty;
                _Window = null;
                Instance.SetVerbsHeight();
            }

            /// <summary>
            ///  Clears all breakpoints
            /// </summary>
            public void Clear()
            {
                Breakpoints.Clear();
            }

            /// <summary>
            ///  Show Edit Breakpoints dialog
            /// </summary>
            public void ShowDialog()
            {
                bool flag = Enabled;
                Enabled = false;

                using (FormBreakpoints form = new FormBreakpoints(Breakpoints))
                {
                    form.ShowDialog(FormMain.Instance);
                }
                Enabled = flag;
            }

            public void Dispose()
            {
                End();
                if (listener != null)
                    listener.Dispose();
                GC.SuppressFinalize(this);
            }

            ~SubClassManager()
            {
                Dispose();
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// This is also a very important method for us..
            /// This method is invoke when we are going to change the parameters of the message
            /// By one way or another we have to pass these new parameters to the hooked window
            /// which is generally belongs to a different process.
            ///  
            /// To resolve this problem. We are going to write our new values to the memory of that process
            /// which ProcessViewer.Hooks.dll will read these values from there...
            /// </summary>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            private IntPtr WriteToTargetProcessMemory(IntPtr wParam, IntPtr lParam, NativeMethods.Msgs modifiedMsg)
            {
                // Open hooked window's process
                IntPtr hProcess = NativeMethods.OpenProcess(NativeMethods.PROCESS_VM_OPERATION
                                                            | NativeMethods.PROCESS_VM_READ
                                                            | NativeMethods.PROCESS_VM_WRITE,
                                                    false, _Window.ProcessId);
                // allocate memory from target proces's memory block
                // 12 bytes : 4 modified msg + 4 wParam + 4 lParam
                IntPtr memAddress = NativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero,
                    12, NativeMethods.MEM_COMMIT,
                    NativeMethods.PAGE_READWRITE);
                if (memAddress == IntPtr.Zero)
                    return IntPtr.Zero;

                int written = 0;
                // write our new parameter values to the tarhet process memory
                bool hr = NativeMethods.WriteProcessMemory(hProcess, memAddress,
                                            new int[] {(int)modifiedMsg, wParam.ToInt32(),
                                                        lParam.ToInt32()},
                                            12, out written);
                // close handle
                NativeMethods.CloseHandle(hProcess);

                if (!hr)
                    return IntPtr.Zero;

                if (Properties.Settings.Default.ShowChangedParameterValues)
                {
                    StringBuilder text = new StringBuilder();
                    text.Append(Properties.Resources.Hook_Parameters_Changed);
                    text.Append("MSG: ");
                    text.Append(modifiedMsg.ToString());
                    text.Append(", WParam: ");
                    text.Append(wParam.ToInt32());
                    text.Append(", LParam: ");
                    text.Append(lParam.ToInt32());
                    text.AppendLine();

                    Instance.txtMessages.AppendText(text.ToString());
                }

                return memAddress;
            }

            #endregion
        }

        /// <summary>
        /// Base class for all types of nodes in the treeview
        /// </summary>
        private abstract class BaseNode : TreeNode
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public BaseNode()
            { }

            #endregion

            #region Public Methods

            /// <summary>
            ///  Invokes before this node is expand
            /// </summary>
            internal virtual void OnBeforeExpand()
            {

            }

            internal virtual void OnDrawNode(DrawTreeNodeEventArgs e)
            {
                e.DrawDefault = true;
            }

            #endregion
        }

        /// <summary>
        /// Base class for the Process and Window tree nodes.
        /// </summary>
        private abstract class WindowContainerNode : BaseNode
        {
            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public WindowContainerNode() { }

            #endregion

            #region Public Methods

            /// <summary>
            /// Enumerates the child window of this window (actually windowinfo holded by this treenode)
            /// </summary>
            /// <param name="enumerator"></param>
            internal override void OnBeforeExpand()
            {
                FormMain.Instance.statusBarMessage.Text = Properties.Resources.Loading;

                WindowsEnumerator enumerator = FormMain.Instance._Enumerator;
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[i].Nodes.Count > 0)
                        continue;

                    WindowInfo info = Nodes[i].Tag as WindowInfo;
                    if (info != null)
                    {
                        if (enumerator.Windows.ContainsKey(info.ProcessId))
                        {
                            List<WindowInfo> wi = enumerator.Windows[info.ProcessId];
                            for (int j = 0; j < wi.Count; j++)
                            {
                                if (wi[j].ParentHandle == info.Handle)
                                {
                                    WindowNode node = new WindowNode(wi[j]);
                                    Nodes[i].Nodes.Add(node);
                                }
                            }
                        }
                    }

                    if (Nodes[i].Nodes.Count > 0)
                        Nodes[i].ImageIndex = Nodes[i].SelectedImageIndex = 2;
                }

                FormMain.Instance.statusBarMessage.Text = Properties.Resources.Ready;
            }

            #endregion
        }

        /// <summary>
        /// Finds the control which currently mouse overs it
        /// </summary>
        internal class WindowFinder : IDisposable
        {
            #region Members

            private IntPtr _Window = IntPtr.Zero;
            private Cursor _TargetCursor;
            private bool _IsOperating;
            private bool _DeadLockFlag;
            private bool _SuspendTooltipPosSetting;
            private Rectangle _ScreenBounds;
            internal FormSelectedTooltip _Tooltip;

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets Window Handle which mouse is currently on it
            /// </summary>
            public IntPtr Window
            {
                get { return _Window; }
                set
                {
                    if (_Window == value)
                        return;
                    Instance._Highlighter.Target = Rectangle.Empty;
                    _Window = value;

                    if (value != IntPtr.Zero)
                    {
                        if (Properties.Settings.Default.EnableFindWindowActivation)
                            FindWindow(value);

                        NativeMethods.RECT target = new NativeMethods.RECT();

                        NativeMethods.GetWindowRect(_Window, ref target);

                        Rectangle rct = Rectangle.Empty;
                        if (target.Width > 0 && target.Height > 0)
                        {
                            rct = new Rectangle(target.left, target.top,
                                                target.Width, target.Height);
                        }
                        _Tooltip.SetInfo(value);
                        if (!_SuspendTooltipPosSetting)
                        {
                            Point pt = new Point(target.left + 10, target.top + 10);
                            if (pt.X + _Tooltip.Width > _ScreenBounds.Right)
                            {
                                pt.X = _ScreenBounds.Right - _Tooltip.Width;
                            }
                            if (pt.Y + _Tooltip.Height > _ScreenBounds.Bottom)
                            {
                                pt.Y = rct.Top - _Tooltip.Height + 2;
                            }
                            _Tooltip.Location = pt;
                        }
                        if (!_Tooltip.Visible && Properties.Settings.Default.ShowWindowTooltipDuringDrag)
                            _Tooltip.Show();
                        Instance._Highlighter.Target = rct;
                        _SuspendTooltipPosSetting = false;
                    }
                    else
                    {
                        if (Properties.Settings.Default.EnableFindWindowActivation)
                            Instance.trvResult.SelectedNode = null;
                        Instance._Highlighter.Target = Rectangle.Empty;
                        _Tooltip.Visible = false;
                    }
                }
            }

            /// <summary>
            /// Gets the Visible area of the tooltip
            /// </summary>
            public Rectangle TooltipArea
            {
                get
                {
                    if (_Tooltip.Visible)
                    {
                        return _Tooltip.Bounds;
                    }
                    return Rectangle.Empty;
                }
            }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public WindowFinder()
            {
                // our Cursor which indicates the Find Window is active now
                using (MemoryStream str = new MemoryStream(Properties.Resources.FndWnd))
                {
                    _TargetCursor = new Cursor(str);
                }

                _Tooltip = new FormSelectedTooltip();
                SetTooltipOpacity();
                _ScreenBounds = Screen.PrimaryScreen.Bounds;

                Instance.btnFindWindow.MouseDown += new MouseEventHandler(btnFindWindow_MouseDown);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Sets opacity of the form
            /// </summary>
            internal void SetTooltipOpacity()
            {
                int op = Properties.Settings.Default.TooltipOpacity;
                if (op < 10)
                    op = 10;
                if (op > 100)
                    op = 100;

                _Tooltip.Opacity = (op / 100d);
            }

            /// <summary>
            /// Release all unmanaged resources
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Release all unmanaged resources
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (_TargetCursor != null)
                    _TargetCursor.Dispose();
                if (_Tooltip != null)
                    _Tooltip.Dispose();
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Finds the TreeNode for the given Window Handle
            /// </summary>
            /// <param name="hWnd"></param>
            private void FindWindow(IntPtr hWnd)
            {
                if (hWnd == IntPtr.Zero)
                    return;

                Instance._TreeUpdater.BeginUpdate();

                int pId = 0;
                NativeMethods.GetWindowThreadProcessId(hWnd, out pId);
                TreeNode selected = null;
                for (int i = 0; i < Instance.trvResult.Nodes.Count; i++)
                {
                    Process p = Instance.trvResult.Nodes[i].Tag as Process;
                    if (p != null && p.Id == pId)
                    {
                        WindowContainerNode bnode = Instance.trvResult.Nodes[i] as WindowContainerNode;
                        if (bnode != null)
                            bnode.OnBeforeExpand();
                        selected = FindWindow(
                            Instance.trvResult.Nodes[i].Nodes, hWnd);
                        break;
                    }
                }

                if (selected == null && !_IsOperating)
                {
                    if ((Properties.Settings.Default.AutoRefreshIfWindowNotFound && !_DeadLockFlag)
                        || (MessageBox.Show(Properties.Resources.Window_Not_Found_Msg,
                            Properties.Resources.Window_Not_Found_Title,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        Instance.RefreshList();
                        _DeadLockFlag = true;
                        FindWindow(hWnd);
                        _DeadLockFlag = false;
                    }
                }
                else
                {
                    Instance._Highlighter.Suspend();
                    Instance.trvResult.SelectedNode = selected;
                    Instance._Highlighter.Resume();
                }

                Instance._TreeUpdater.EndUpdate();
            }

            /// <summary>
            /// Finds window with in the given nodes list
            /// </summary>
            /// <param name="nodes"></param>
            /// <param name="hWnd"></param>
            /// <returns></returns>
            private TreeNode FindWindow(TreeNodeCollection nodes, IntPtr hWnd)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    WindowContainerNode bnode = nodes[i] as WindowContainerNode;
                    if (bnode != null)
                    {
                        bnode.OnBeforeExpand();
                        WindowInfo info = bnode.Tag as WindowInfo;
                        if (info != null && info.Handle == hWnd)
                        {
                            return nodes[i];
                        }
                        else
                        {
                            TreeNode sub = FindWindow(nodes[i].Nodes, hWnd);
                            if (sub != null)
                                return sub;
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// When mouse downs to the btnFindWindow button then we enter a while look which we process 
            /// WM_MOUSEMOVE by ourselves to find the window which mouse on it
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnFindWindow_MouseDown(object sender, MouseEventArgs e)
            {
                Instance.Cursor = _TargetCursor;
                Instance._Highlighter.SelectionMode = true;

                NativeMethods.MSG msg = new NativeMethods.MSG();
                _IsOperating = true;
                while (_IsOperating)
                {
                    int msgResult = NativeMethods.GetMessageA(out msg, IntPtr.Zero, 0, 0);
                    if (msgResult == -1 || msgResult == 0)
                        break;

                    switch ((NativeMethods.Msgs)msg.message)
                    {
                        case NativeMethods.Msgs.WM_KEYDOWN:
                            // if user presses Escape button cancel operation
                            int kCode = msg.wParam.ToInt32();
                            if (kCode == NativeMethods.VK_ESCAPE)
                            {
                                Window = IntPtr.Zero;
                                _IsOperating = false;
                            }
                            else if (kCode == NativeMethods.VK_CONTROL)
                            {
                                // if CTRL key is down then try to select control which we 
                                // are currently on it in the Parents & Childs List
                                IntPtr currentOver = FindOverWindow();
                                if (currentOver != IntPtr.Zero)
                                    Window = currentOver;
                                continue;
                            }
                            break;
                        case NativeMethods.Msgs.WM_MOUSEMOVE:
                            IntPtr ptr = FindOverWindow();
                            if (ptr != IntPtr.Zero)
                                Window = ptr;
                            continue;
                        case NativeMethods.Msgs.WM_LBUTTONUP:
                            // finish operation
                            _IsOperating = false;
                            break;
                    }

                    // translate and dispatch messages to their owners
                    NativeMethods.TranslateMessage(out msg);
                    NativeMethods.DispatchMessageA(ref msg);
                }
                if (Window != IntPtr.Zero)
                    FindWindow(Window);
                Window = IntPtr.Zero;
                Instance.Cursor = Cursors.Default;
                Instance._Highlighter.SelectionMode = false;
            }

            /// <summary>
            /// Find over window
            /// </summary>
            /// <returns></returns>
            private IntPtr FindOverWindow()
            {
                NativeMethods.POINT pt;
                pt.x = Control.MousePosition.X;
                pt.y = Control.MousePosition.Y;
                IntPtr ptr = NativeMethods.WindowFromPoint(pt);

                if (!Properties.Settings.Default.AllowDragTooltipSelectable)
                {
                    int wResult = _Tooltip.ContainsHandle(ptr);
                    if (wResult == 1)
                    {
                        if (Properties.Settings.Default.DisableTooltipOpacityOnMouseOver)
                            _Tooltip.Opacity = 1d;
                        return IntPtr.Zero;
                    }
                    else if (wResult != 0)
                    {
                        _SuspendTooltipPosSetting = true;
                        ptr = new IntPtr(wResult);
                    }
                    else if (Properties.Settings.Default.DisableTooltipOpacityOnMouseOver)
                    {
                        SetTooltipOpacity();
                    }
                }
                return ptr;
            }

            #endregion
        }

        /// <summary>
        /// Helper class for highlighting operations
        /// At first; we were using ControlPaint's DrawReversibleFrame method to
        /// highlight the given area but now we are using our custom draw frame method.
        /// Because ControlPaint's method draw whole rectangle area (even it intersects with our ProcessViewer's
        /// main window). To reduce this flickings minumum and make available HideIntersection option
        /// we wrote our own draw frame method and used it.
        /// </summary>
        private class HighlightingManager : IDisposable
        {
            #region Members

            private Rectangle _Target = Rectangle.Empty;
            private bool _SelectionMode = false;
            private int _Step = 0;
            private Timer _Timer = null;
            private bool _IsSuspended = false;
            internal int MaxCount = 4;

            #endregion

            #region Properties
            
            /// <summary>
            /// Gets or sets the highlighting frame
            /// </summary>
            public Rectangle Target
            {
                set
                {
                    if (_Target == value || _IsSuspended)
                    {
                        return;
                    }

                    _Timer.Enabled = false;

                    if (!_IsSuspended)
                    {
                        if (SelectionMode)
                        {
                            if (_Target != Rectangle.Empty)
                                GdiHelper.DrawRegion(_Target);
                            // ControlPaint.DrawReversibleFrame(_Target, Color.Black, FrameStyle.Dashed);
                        }
                        else
                        {
                            if (_Step != MaxCount
                                && _Target != Rectangle.Empty
                                && ((_Step % 2) != 0))
                            {
                                //ControlPaint.DrawReversibleFrame(_Target, Color.Black, FrameStyle.Thick);
                                GdiHelper.DrawRegion(_Target);
                            }
                        }
                    }
                    _Target = value;
                    if (!_IsSuspended)
                    {
                        if (_Target != Rectangle.Empty)
                        {
                            if (SelectionMode)
                            {
                                //ControlPaint.DrawReversibleFrame(_Target, Color.Black, FrameStyle.Dashed);
                                GdiHelper.DrawRegion(_Target);
                            }
                            else
                            {
                                //ControlPaint.DrawReversibleFrame(_Target, Color.Black, FrameStyle.Thick);
                                GdiHelper.DrawRegion(_Target);
                                _Step = 1;
                                _Timer.Enabled = true;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Gets or sets that is it in selection mode ? Which means 
            /// highlighting operations invoked by the Window Finder button
            /// which we dont need to animate highlighting operation
            /// </summary>
            public bool SelectionMode
            {
                get { return _SelectionMode; }
                set
                {
                    if (_SelectionMode == value)
                        return;
                    _SelectionMode = value;
                    Target = Rectangle.Empty;
                }
            }

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public HighlightingManager()
            {
                _Timer = new Timer();
                _Timer.Interval = 300;
                _Timer.Tick += new EventHandler(timer_Tick);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Suspend highlighting
            /// </summary>
            public void Suspend()
            {
                _IsSuspended = true;
            }

            /// <summary>
            /// Resume highlighting
            /// </summary>
            public void Resume()
            {
                _IsSuspended = false;
            }

            /// <summary>
            /// Calculates max count
            /// </summary>
            internal void CalculateMaxCount()
            {
                MaxCount = Properties.Settings.Default.HighlightingSteps;
                if (MaxCount < 0)
                    MaxCount = 2;
                if ((MaxCount % 2) != 0)
                    MaxCount++;
            }

            /// <summary>
            /// Release all unmanaged resources
            /// </summary>
            public void Dispose()
            {
                if (_Timer != null)
                    _Timer.Dispose();
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Private Methods

            private void timer_Tick(object sender, EventArgs e)
            {
                if (_Target != Rectangle.Empty)
                {
                    //ControlPaint.DrawReversibleFrame(_Target, Color.Black, FrameStyle.Thick);
                    GdiHelper.DrawRegion(_Target);
                }
                else
                {
                    _Timer.Enabled = false;
                }

                _Step++;
                if (_Step >= MaxCount)
                {
                    _Timer.Enabled = false;
                }
            }

            #endregion
        }

        /// <summary>
        /// Provides accessing file icons
        /// </summary>
        private class IconHasher : IDisposable
        {
            #region Members

            private readonly Dictionary<string, int> _Hash;
            internal ImageList Images;

            #endregion

            #region Initialization

            /// <summary>
            /// Initialize a new instance of this class
            /// </summary>
            public IconHasher()
            {
                _Hash = new Dictionary<string, int>();
                Images = new ImageList();
                Images.ImageSize = new Size(16, 16);
                //Bitmap bmp = new Bitmap(16, 16);
                //Images.Images.Add(bmp);
                Images.Images.Add(Properties.Resources.software_16x16);
                Images.Images.Add(Properties.Resources.window);
                Images.Images.Add(Properties.Resources.windows);
                GetImageIndex(".dll");
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Returns the index of the image in the image list which is associated with the given file type
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            public int GetImageIndex(string filePath)
            {
                string extension = Path.GetExtension(filePath).ToUpperInvariant();
                int iconIndex = 0;
                if (extension != ".EXE")
                {
                    if (_Hash.ContainsKey(extension))
                        return _Hash[extension];

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false))
                    {
                        if (key == null)
                            filePath = null;
                        else
                        {
                            string fileKey = key.GetValue(null).ToString();
                            fileKey = fileKey + @"\DefaultIcon";
                            using (RegistryKey fkey = Registry.ClassesRoot.OpenSubKey(fileKey, false))
                            {
                                if (fkey == null)
                                    filePath = null;
                                else
                                {
                                    string iconFile = fkey.GetValue(null).ToString();
                                    int index = iconFile.LastIndexOf(",", StringComparison.OrdinalIgnoreCase);
                                    iconIndex = Convert.ToInt32(iconFile.Substring(index + 1, iconFile.Length - 1 - index), CultureInfo.InvariantCulture);
                                    filePath = iconFile.Substring(0, index);
                                }
                            }
                        }
                    }
                }

                if (filePath == null)
                    return -1;

                if (_Hash.ContainsKey(filePath))
                    return _Hash[filePath];

                IntPtr hIconBig = IntPtr.Zero;
                IntPtr hIconSmall = IntPtr.Zero;
                NativeMethods.ExtractIconEx(filePath, iconIndex, ref hIconBig, ref hIconSmall, 1);
                int iIndex = -1;
                if (hIconSmall != IntPtr.Zero)
                {
                    Icon icon = Icon.FromHandle(hIconSmall);
                    iIndex = Images.Images.Count;
                    Images.Images.Add(icon);

                    if (extension == ".EXE")
                        _Hash.Add(filePath, iIndex);
                    else
                        _Hash.Add(extension, iIndex);
                }
                return iIndex;
            }

            /// <summary>
            /// Release all unmanaged resources
            /// </summary>
            public void Dispose()
            {
                Dispose(true);

                GC.SuppressFinalize(this);
            }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Release all unmanaged resources
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Images.Dispose();
                    _Hash.Clear();
                }
            }

            #endregion
        }

        /// <summary>
        /// We dont want to call BeginUpdate and EndUpdate unnecassarily on TreeView
        /// This class helps us to call only one BeginUpdate and EndUpdate on TreeView
        /// </summary>
        private class TreeUpdateHelper
        {
            #region Members

            private int _UpdateFlag = 0;

            #endregion

            #region Public Methods

            /// <summary>
            /// Begins update operation of the tree view
            /// </summary>
            public void BeginUpdate()
            {
                _UpdateFlag++;
                if (_UpdateFlag == 1)
                    FormMain.Instance.trvResult.BeginUpdate();
            }

            /// <summary>
            /// Ends update operation of the tree view
            /// </summary>
            public void EndUpdate()
            {
                _UpdateFlag--;
                if (_UpdateFlag < 0)
                    _UpdateFlag = 0;
                if (_UpdateFlag == 0)
                    FormMain.Instance.trvResult.EndUpdate();
            }

            #endregion
        }


        #endregion

        #region Members

        public static FormMain Instance;
		
		private bool _IsOperating;
		private bool _IsVerbsHeightSetted;
		private WindowsEnumerator _Enumerator;
		private HighlightingManager _Highlighter;
		private TreeNode _SelectedNode;
		private IContainer _Container;
		private int _ProcessId = 0;
		private SubClassManager _SubClass;
		private IconHasher _IconHelper;
		private TreeUpdateHelper _TreeUpdater;
		internal WindowFinder Finder;

        #endregion

        #region Properties

        /// <summary>
        /// Gets that is selected widnow belongs PV
        /// </summary>
        internal bool IsSelectedMine
        {
            get
            {
                ProcessNode pnode = SelectedNode as ProcessNode;
                if (pnode != null)
                {
                    Process p = pnode.Tag as Process;
                    if (p.Id == _ProcessId)
                        return true;
                }
                else
                {
                    WindowNode wnode = SelectedNode as WindowNode;
                    if (wnode != null)
                    {
                        WindowInfo info = wnode.Tag as WindowInfo;
                        if (info.ProcessId == _ProcessId)
                            return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets that is mouse over PV
        /// </summary>
        internal bool IsOverMine
        {
            get
            {
                if (Finder.Window == IntPtr.Zero)
                    return false;
                int pid = 0;
                NativeMethods.GetWindowThreadProcessId(Finder.Window, out pid);
                return (pid == _ProcessId);
            }
        }

        /// <summary>
        /// Gets the selection mode
        /// </summary>
        internal bool IsSelectionMode
        {
            get { return _Highlighter.SelectionMode; }
        }

        /// <summary>
        /// Gets or sets the selected tree node
        /// </summary>
        private TreeNode SelectedNode
        {
            get { return _SelectedNode; }
            set
            {
                if (_SelectedNode == value)
                    return;
                _Highlighter.Target = Rectangle.Empty;

                _SelectedNode = value;
                if (value == null)
                {
                    propertyGrid.SelectedObject = null;
                    for (int i = 0; i < tlbWindow.Items.Count; i++)
                    {
                        tlbWindow.Items[i].Enabled = false;
                    }
                    btnKill.Enabled = false;
                }
                else
                {
                    propertyGrid.SelectedObject = value.Tag;
                    bool enableActions = (value.Tag is WindowInfo);
                    for (int i = 0; i < tlbWindow.Items.Count; i++)
                    {
                        tlbWindow.Items[i].Enabled = enableActions;
                    }
                    btnKill.Enabled = (value.Tag is Process);
                }
                killProcessToolStripMenuItem.Enabled = btnKill.Enabled;

                if (Properties.Settings.Default.AutoHighlight)
                    Highlight();

                if (value is WindowNode)
                {
                    if (!_IsVerbsHeightSetted)
                    {
                        SetVerbsHeight();
                        _IsVerbsHeightSetted = true;
                    }
                }
                else
                {
                    _IsVerbsHeightSetted = false;
                }
            }
        }

        /// <summary>
        /// Gets the selected window info
        /// </summary>
        private WindowInfo SelectedWindow
        {
            get
            {
                WindowNode node = trvResult.SelectedNode as WindowNode;
                if (node != null)
                    return node.Tag as WindowInfo;
                return null;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public FormMain()
		{
			Instance = this;
            _Container = new Container();
			InitializeComponent();

			Text = string.Format(CultureInfo.InvariantCulture, Text, Properties.Resources.ProgramName);

			// create our helper object instances
			_SubClass = new SubClassManager();
			_Enumerator = new WindowsEnumerator();
			_Highlighter = new HighlightingManager();
			Finder = new WindowFinder();
			_IconHelper = new IconHasher();
			_TreeUpdater = new TreeUpdateHelper();

			trvResult.ImageList = _IconHelper.Images;
			//trvResult.StateImageList = IconHelper.Images;
			_ProcessId = Process.GetCurrentProcess().Id;
			TopMost = Properties.Settings.Default.TopMost;
			if (TopMost)
				btnTopMost.Text = Properties.Resources.Window_TopMost;
			else
				btnTopMost.Text = Properties.Resources.Window_Normal;
			Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);

			// we will load current processes after our program enters Idle mode
			// this will let the system create our controls before the our 'windows tree' create
			Application.Idle += new EventHandler(Application_Idle);
		}

        #endregion

        #region Public Methods

        /// <summary>
        ///  Highlights Selected Window's region
        /// </summary>
        internal void Highlight()
        {
            WindowNode node = SelectedNode as WindowNode;
            if (node == null)
                return;

            WindowInfo info = node.Tag as WindowInfo;
            if (info == null || info.Handle == IntPtr.Zero)
                return;

            NativeMethods.RECT target = new NativeMethods.RECT();

            NativeMethods.GetWindowRect(info.Handle, ref target);

            Rectangle rct = new Rectangle();
            if (target.Width > 0 && target.Height > 0)
            {
                rct = new Rectangle(target.left, target.top,
                                    target.Width, target.Height);
            }

            _Highlighter.Target = Rectangle.Empty;
            _Highlighter.Target = rct;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// On application Idle load the processes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Idle(object sender, EventArgs e)
		{
			Application.Idle -= new EventHandler(Application_Idle);
			RefreshList();
		}

		/// <summary>
		///  Reads the current process and finds their TopLevel forms
		/// </summary>
		private void RefreshList()
		{
			if (_IsOperating)
				return;
			_IsOperating = true;
			try
			{
				statusBarMessage.Text = Properties.Resources.Loading;
				Application.DoEvents();
				Cursor = Cursors.WaitCursor;
				_TreeUpdater.BeginUpdate();
				trvResult.Nodes.Clear();

				_Enumerator.Begin();

				foreach (KeyValuePair<int, List<WindowInfo>> program in _Enumerator.Windows)
				{
					Process p = Process.GetProcessById(program.Key);
					ProcessNode process = new ProcessNode(p);

					for (int i = 0; i < program.Value.Count; i++)
					{
						WindowInfo info = program.Value[i];
						if (info.ParentHandle.ToInt32() != 0)
							continue;
						
						if (Properties.Settings.Default.IgnoreUnVisibleWindows
							&& !info.Visible)
							continue;

						WindowNode node = new WindowNode(info);
						if (p !=  null && info.Handle == p.MainWindowHandle)
							node.NodeFont = GdiHelper.UnderlineFont;
						process.Nodes.Add(node);
					}

					trvResult.Nodes.Add(process);
				}
				statusBarMessage.Text = Properties.Resources.Ready;
				Application.DoEvents();
			}
			finally
			{
				Cursor = Cursors.Default;
				_IsOperating = false;
				_TreeUpdater.EndUpdate();
			}
		}

		/// <summary>
		///  PropertyGrid sets Verbs areas height to the [[number of verb] * [Font.Height]], which assumes that
		///  every verb will be in a line.
		///  But this is wrong because all the verbs does not have their own lines. In a line there can be more than one verb.
		///  So here in this method we are going to fix Verbs areas height in the propertygrid
		/// </summary>
		private void SetVerbsHeight()
		{
			try
			{
				Type pgType = typeof(PropertyGrid);
				FieldInfo hotCommandsField = pgType.GetField("hotcommands", BindingFlags.Instance
					| BindingFlags.NonPublic | BindingFlags.GetField);
				if (hotCommandsField == null)
					return;
				object hotCommands = hotCommandsField.GetValue(propertyGrid);
				if (hotCommands == null)
					return;

				PropertyInfo heightProperty = hotCommands.GetType().GetProperty("Height", BindingFlags.Instance | BindingFlags.Public);
				heightProperty.SetValue(hotCommands, Properties.Settings.Default.VerbsHeight, null);

				MethodInfo layMet = pgType.GetMethod("OnLayoutInternal", BindingFlags.NonPublic
											| BindingFlags.Instance | BindingFlags.InvokeMethod);
				if (layMet == null)
					return;

				layMet.Invoke(propertyGrid, new object[] { true });
			}
			catch { 
				// we are using too many internal and private members here...
				// this means these members might change in the future versions of property grid
				// Therefore we dont want to get any exception from here..
				// it is not so important for us to set verbs height for this project :)
			}
		}

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "TopMost")
			{
				TopMost = Properties.Settings.Default.TopMost;
				if (TopMost)
					btnTopMost.Text = Properties.Resources.Window_TopMost;
				else
					btnTopMost.Text = Properties.Resources.Window_Normal;
			}
			else if (e.PropertyName == "HighlightingSteps")
			{
				_Highlighter.CalculateMaxCount();
			}
			else if (e.PropertyName == "ShowDescriptions")
			{
				propertyGrid.HelpVisible = Properties.Settings.Default.ShowDescriptions;
			}
			else if (e.PropertyName == "ShowVerbs")
			{
				propertyGrid.CommandsVisibleIfAvailable = Properties.Settings.Default.ShowVerbs;
				SetVerbsHeight();
			}
			else if (e.PropertyName == "VerbsHeight")
			{
				SetVerbsHeight();
			}
			else if (e.PropertyName == "ProgramLanguage")
			{
				if (Program.SetThreadCulture())
				{
					ComponentResourceManager manager = new ComponentResourceManager(GetType());
					manager.ApplyResources(this, "$this");

					FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
					foreach (FieldInfo field in fields)
					{
						object value = field.GetValue(this);
						if (value is IComponent)
						{
							manager.ApplyResources(value, field.Name);
						}
					}

					Text = string.Format(CultureInfo.InvariantCulture, Text, Properties.Resources.ProgramName);

					propertyGrid.Refresh();
					SetVerbsHeight();
				}
			}
			else if (e.PropertyName == "TooltipOpacity")
			{
				Finder.SetTooltipOpacity();
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
            using (FormOptions form = new FormOptions())
            {
                form.ShowDialog(this);
            }
		}

		private void aboutProcessViewerToolStripMenuItem_Click(object sender, EventArgs e)
		{
            using (FormAbout form = new FormAbout())
            {
                form.ShowDialog(this);
            }
		}

		private void gotoOzcansWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Common.GotoHomepage();
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RefreshList();
		}

		private void highlightSelectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Highlight();
		}

		private void trvResult_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			_TreeUpdater.BeginUpdate();
			BaseNode node = e.Node as BaseNode;
			if (node != null)
				node.OnBeforeExpand();
			_TreeUpdater.EndUpdate();
		}

		private void trvResult_AfterSelect(object sender, TreeViewEventArgs e)
		{
			SelectedNode = e.Node as BaseNode;
		}

		private void btnShowButton_Click(object sender, EventArgs e)
		{
			WindowInfo info = SelectedWindow;
			if (info == null)
				return;
			info.Visible = true;
		}

		private void btnHideWindow_Click(object sender, EventArgs e)
		{
			WindowInfo info = SelectedWindow;
			if (info == null)
				return;
			info.Visible = false;
		}

		private void btnEnableWindow_Click(object sender, EventArgs e)
		{
			WindowInfo info = SelectedWindow;
			if (info == null)
				return;
			info.Enabled = true;
		}

		private void btnDisableWindow_Click(object sender, EventArgs e)
		{
			WindowInfo info = SelectedWindow;
			if (info == null)
				return;
			info.Enabled = false;
		}

		private void btnListenMessages_Click(object sender, EventArgs e)
		{
			WindowInfo info = SelectedWindow;
			if (info == null)
				return;
			_SubClass.Begin(info);
		}

		private void btnWatcherOn_Click(object sender, EventArgs e)
		{
			_SubClass.Enabled = true;
		}

		private void btnWatcherOff_Click(object sender, EventArgs e)
		{
			_SubClass.Enabled = false;
		}

		private void btnClearMessages_Click(object sender, EventArgs e)
		{
			txtMessages.Text = string.Empty;
		}

		private void btnKill_Click(object sender, EventArgs e)
		{
			ProcessNode node = SelectedNode as ProcessNode;
			if (node == null)
				return;

			Process process = node.Tag as Process;
			if (process == null)
				return;

			process.Kill();

			trvResult.Nodes.Remove(node);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_Highlighter.Target = Rectangle.Empty;
			Close();
		}

		private void trvResult_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			BaseNode node = e.Node as BaseNode;
			if (node != null)
				node.OnDrawNode(e);
			else
				e.DrawDefault = true;
		}

		private void lblMessages_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.X > lblMessages.Width - 22)
				_SubClass.End();
		}

		private void btnEditWatchs_Click(object sender, EventArgs e)
		{
			_SubClass.ShowDialog();
		}

		private void btnCopyToClipboard_Click(object sender, EventArgs e)
		{
			Clipboard.SetData(DataFormats.Text, txtMessages.Text);
		}

		private void treeContext_Opening(object sender, CancelEventArgs e)
		{
			if (SelectedWindow == null)
			{
				ProcessNode node = SelectedNode as ProcessNode;
				if (node == null)
					e.Cancel = true;
				else
				{
					for (int i = 0; i < treeContext.Items.Count - 1; i++)
					{
						treeContext.Items[i].Visible = false;
					}
					treeContext.Items[treeContext.Items.Count - 1].Visible = true;
				}
			}
			else
			{
				for (int i = 0; i < treeContext.Items.Count - 1; i++)
				{
					treeContext.Items[i].Visible = true;
				}
				treeContext.Items[treeContext.Items.Count - 1].Visible = false;
			}
		}

		private void trvResult_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode node = trvResult.GetNodeAt(e.X, e.Y);
				trvResult.SelectedNode = node;
			}
		}

		private void propertyGridToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			showDescriptionsToolStripMenuItem.Checked = Properties.Settings.Default.ShowDescriptions;
			showVerbsToolStripMenuItem1.Checked = Properties.Settings.Default.ShowVerbs;
		}

		private void showDescriptionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.ShowDescriptions = !Properties.Settings.Default.ShowDescriptions;
			Properties.Settings.Default.Save();
		}

		private void showVerbsToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.ShowVerbs = !Properties.Settings.Default.ShowVerbs;
			Properties.Settings.Default.Save();
		}

		private void sbarWatcher_Click(object sender, EventArgs e)
		{
			_SubClass.Enabled = !_SubClass.Enabled;
		}

		private void btnShowMessages_Click(object sender, EventArgs e)
		{
			_SubClass.ShowMessages = !_SubClass.ShowMessages;
		}

		private void btnTopMost_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.TopMost = !Properties.Settings.Default.TopMost;
		}

		private void breakpointsManagerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_SubClass.ShowDialog();
		}

		private void gotoProcessViewerPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Common.GotoPvsHome();
		}

		private void englishToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.ProgramLanguage.LCID == 1033)
				return;

			Properties.Settings.Default.ProgramLanguage = new CultureInfo(1033);
		}

		private void turkishToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.ProgramLanguage.LCID == 1055)
				return;

			Properties.Settings.Default.ProgramLanguage = new CultureInfo(1055);
		}

		private void languageToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.ProgramLanguage.LCID == 1055)
			{
				turkishToolStripMenuItem.Checked = true;
				englishToolStripMenuItem.Checked = false;
			}
			else
			{
				turkishToolStripMenuItem.Checked = false;
				englishToolStripMenuItem.Checked = true;
			}
		}

        #endregion
	}
}
