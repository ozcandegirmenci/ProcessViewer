using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace ProcessViewer.Design
{
    /// <summary>
    /// UI Type editor for enum values which has Flag attribute
    /// </summary>
    /// <remarks>This editor allows multiple selection for enum values</remarks>
	internal class FlagsEditor : DropDownEditorBase<FlagsEditorControl>
	{
        #region Members
        
        private static readonly Type[] _SupportedUnderlyingTypes = new Type[] {
                                                                                typeof(byte), typeof(sbyte), typeof(short),
                                                                                typeof(ushort), typeof(int), typeof(uint),
                                                                                typeof(long), typeof(ulong)
                                                                              };

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Edits the user value by using the editor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
            if (!(value is Enum))
            {
                throw new NotSupportedException(Properties.Resources.Err_ValueIsNotEnumOrDoesNotHaveFlagsAttribute);
            }
            
			var enumType = value.GetType();
			var attributes = enumType.GetCustomAttributes(typeof(FlagsAttribute), true);
            if (attributes.Length == 0)
            {
                throw new NotSupportedException(Properties.Resources.Err_ValueIsNotEnumOrDoesNotHaveFlagsAttribute);
            }
            
			var underlyingType = Enum.GetUnderlyingType(value.GetType());
            if (!_SupportedUnderlyingTypes.Any(t=> t == underlyingType))
            {
                return value;
            }

            value = base.EditValue(context, provider, value);

			return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
		}

        #endregion
    }
}
