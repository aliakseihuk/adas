using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Adas.Ui.Wpf
{
    public static class BitmapExtensions
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            IntPtr pointer = bitmap.GetHbitmap();
            BitmapSource bitmapSource;
            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(pointer, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(pointer);
            }
            return bitmapSource;
        }
    }
}