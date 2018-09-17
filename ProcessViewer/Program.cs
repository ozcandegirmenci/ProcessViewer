using System;
using System.Windows.Forms;
using System.Threading;

namespace ProcessViewer
{
    /// <summary>
    /// Program main entry point
    /// </summary>
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Control.CheckForIllegalCrossThreadCalls = false;
			SetThreadCulture();
			Application.Run(new FormMain());
		}

        /// <summary>
        /// Sets the thread culture for multilanguage
        /// </summary>
        /// <returns></returns>
		internal static bool SetThreadCulture()
		{
			if (Thread.CurrentThread.CurrentUICulture.LCID == Properties.Settings.Default.ProgramLanguage.LCID)
				return false;
			Thread.CurrentThread.CurrentUICulture = Properties.Settings.Default.ProgramLanguage;
			return true;
		}
	}
}
