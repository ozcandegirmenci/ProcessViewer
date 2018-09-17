using System.Windows.Forms.Design;

namespace ProcessViewer.Design
{
    /// <summary>
    /// PRovides base functionality for a DropDown UI Editor Edit Control
    /// </summary>
    internal interface IDropDownEditControl
    {
        #region Properties

        /// <summary>
        /// Gets the result value
        /// </summary>
        object Value { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize edit operation for the given text value
        /// </summary>
        /// <param name="service"></param>
        /// <param name="value"></param>
        void Initialize(IWindowsFormsEditorService service, object value);

        /// <summary>
        /// Resets edit operation and puts control in a clean state
        /// </summary>
        void Reset();

        #endregion
    }
}
