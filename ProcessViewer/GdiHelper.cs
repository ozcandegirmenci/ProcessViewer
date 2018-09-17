using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ProcessViewer
{
    /// <summary>
    /// Contains some static GDI operations
    /// </summary>
    internal static class GdiHelper
    {
        #region Members

        private static IntPtr _HighlightBrush = IntPtr.Zero;
        private static Pen _DashedPen;
        private static Font _BoldFont;
        private static Font _UnderlineFont;
        private static StringFormat _CenterFormat;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the brush that iwll be used for highlighting
        /// </summary>
        public static IntPtr HighlightBrush
        {
            get
            {
                if (_HighlightBrush == IntPtr.Zero)
                {
                    _HighlightBrush = CreateHighlightBrushBrush();
                }

                return _HighlightBrush;
            }
        }

        /// <summary>
        /// Gets dotted dahs style pen object
        /// </summary>
        public static Pen DashedPen
        {
            get
            {
                if (_DashedPen == null)
                {
                    _DashedPen = new Pen(Color.DarkGray)
                    {
                        DashStyle = DashStyle.Dot
                    };
                }
                return _DashedPen;
            }
        }

        /// <summary>
        /// Gets bold font object
        /// </summary>
        public static Font BoldFont
        {
            get
            {
                if (_BoldFont == null)
                {
                    _BoldFont = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point);
                }

                return _BoldFont;
            }
        }

        /// <summary>
        /// Gets unlined font object
        /// </summary>
        public static Font UnderlineFont
        {
            get
            {
                if (_UnderlineFont == null)
                {
                    _UnderlineFont = new Font("Tahoma", 8.25f, FontStyle.Underline, GraphicsUnit.Point);
                }

                return _UnderlineFont;
            }
        }

        /// <summary>
        /// Gets a string format object which is centered line alignment
        /// </summary>
        public static StringFormat CenterFormat
        {
            get {
                if (_CenterFormat == null)
                {
                    StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    _CenterFormat = format;
                }
                return _CenterFormat;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a border region for the given rectangle area
        /// </summary>
        /// <param name="rect">The area</param>
        /// <param name="thickness">Thickness of the border</param>
        /// <returns></returns>
        public static Region CreateRectangleRegion(Rectangle rect, int thickness)
        {
            var region = new Region(rect);

            if ((thickness <= 0) || (rect.Width <= 2 * thickness)
                || (rect.Height <= 2 * thickness))
            {
                return region;
            }

            rect.X += thickness;
            rect.Y += thickness;
            rect.Width -= 2 * thickness;
            rect.Height -= 2 * thickness;

            region.Xor(rect);

            return region;
        }

        /// <summary>
        /// Draws a reversible frame to the given rectangle's region
        /// </summary>
        /// <param name="rct"></param>
        public static void DrawRegion(Rectangle rct)
        {
            using (var rgn = CreateRectangleRegion(rct, 2))
            {
                DrawRegion(rgn);
            }
        }

        /// <summary>
        /// Draws a reversible frame to the given region
        /// </summary>
        /// <param name="region"></param>
        public static void DrawRegion(Region region)
        {
            if (region == null)
            {
                return;
            }

            var needExlude = false;
            if (Properties.Settings.Default.HideIntersection)
            {
                if (FormMain.Instance.IsSelectionMode)
                {
                    needExlude = !FormMain.Instance.IsOverMine;
                }
                else
                {
                    needExlude = !FormMain.Instance.IsSelectedMine;
                }
            }
            
            if (FormMain.Instance.Finder._Tooltip.Visible)
            {
                region.Exclude(FormMain.Instance.Finder._Tooltip.Bounds);
            }
            
            if (needExlude)
            {
                var rct = new NativeMethods.RECT();
                NativeMethods.GetWindowRect(FormMain.Instance.Handle, ref rct);

                var rect = new Rectangle(rct.left, rct.top, rct.Width, rct.Height);
                region.Exclude(rect);
            }

            var hDc = NativeMethods.GetDC(IntPtr.Zero);
            NativeMethods.SelectClipRgn(hDc, region.GetHrgn(Graphics.FromHdc(hDc)));

            var myBox = new NativeMethods.RECT();
            NativeMethods.GetClipBox(hDc, ref myBox);

            var brHandler = HighlightBrush;
            var oldHandle = NativeMethods.SelectObject(hDc, brHandler);

            NativeMethods.PatBlt(hDc,
                myBox.left,
                myBox.top,
                myBox.right - myBox.left,
                myBox.bottom - myBox.top,
                (uint)NativeMethods.RasterOperations.PATINVERT);

            NativeMethods.SelectObject(hDc, oldHandle);
            NativeMethods.SelectClipRgn(hDc, IntPtr.Zero);
            NativeMethods.ReleaseDC(IntPtr.Zero, hDc);
        }

        /// <summary>
        /// Create brush which will be used for drawing frames
        /// </summary>
        /// <returns></returns>
        static IntPtr CreateHighlightBrushBrush()
        {
            Bitmap bitmap = new Bitmap(8, 8, PixelFormat.Format32bppArgb);
            byte[] lights = new byte[] { 255, 255, 255, 255 };
            byte[] darks = new byte[] { 255, 0, 0, 0 };

            bool useLights = true;

            BitmapData data = null;
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                IntPtr scan = data.Scan0;
                unsafe
                {
                    byte* p = (byte*)(void*)scan;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            if (useLights)
                            {
                                p[0] = lights[0]; p[1] = lights[1]; p[2] = lights[2]; p[3] = lights[3];
                                p += 4;
                            }
                            else
                            {
                                p[0] = darks[0]; p[1] = darks[1]; p[2] = darks[2]; p[3] = darks[3];
                                p += 4;
                            }

                            useLights = !useLights;
                        }
                    }
                }

                bitmap.UnlockBits(data);
            }
            catch
            {
                if (data != null)
                    bitmap.UnlockBits(data);
            }

            IntPtr hBitmap = bitmap.GetHbitmap();

            NativeMethods.LOGBRUSH brush = new NativeMethods.LOGBRUSH();
            brush.lbStyle = (uint)NativeMethods.BrushStyles.BS_HATCHED;
            brush.lbHatch = (uint)hBitmap;

            return NativeMethods.CreateBrushIndirect(ref brush);
        }

        #endregion
    }
}
