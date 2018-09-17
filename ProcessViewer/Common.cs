using System.Diagnostics;

namespace ProcessViewer
{
    /// <summary>
    /// Provides common fields and methods
    /// </summary>
	internal static class Common
	{
        #region Members

        public const string Url = "http://www.ozcandegirmenci.com";
		public const string PVUrl = "http://www.ozcandegirmenci.com/process-viewer/";

        #endregion

        #region Public Methods

        /// <summary>
        /// Goto given url
        /// </summary>
        /// <param name="url"></param>
        public static void GotoUrl(string url)
		{
			try
			{
				Process.Start(url);
			}
			catch { }
		}

        /// <summary>
        /// Goto home page
        /// </summary>
		public static void GotoHomepage()
		{
			GotoUrl(Url);
		}

        /// <summary>
        /// Goto PV's home page
        /// </summary>
		public static void GotoPvsHome()
		{
			GotoUrl(PVUrl);
		}

        #endregion
    }
}
