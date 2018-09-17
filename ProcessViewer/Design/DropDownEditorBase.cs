using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ProcessViewer.Design
{
    /// <summary>
    /// Provides base functionality for drop down custom ui editor
    /// </summary>
    internal class DropDownEditorBase<TControl> : UITypeEditor, IDisposable
        where TControl: Control, IDropDownEditControl, new()
    {
        #region Members

        private TControl _EditorControl;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns editor edit style
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Edits the given value by using the editor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (service != null)
                {
                    var editorControl = GetEditorControl();

                    editorControl.Initialize(service, value);
                    service.DropDownControl(_EditorControl);
                    value = editorControl.Value;
                    editorControl.Reset();
                }
            }

            return value;
        }

        /// <summary>
        /// Clean up any resources that are being used
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the editor control
        /// </summary>
        /// <returns></returns>
        protected virtual TControl GetEditorControl()
        {
            if (_EditorControl == null)
            {
                _EditorControl = new TControl();
            }
            return _EditorControl;
        }

        /// <summary>
        /// Clean up any resources that are being used
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_EditorControl != null)
                {
                    _EditorControl.Dispose();
                }
            }
        }

        #endregion
    }
}
