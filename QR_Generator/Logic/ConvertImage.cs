using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QR_Generator
{
    public class ConvertImage
    {
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        public System.Windows.Controls.Image ConvertDrawingImageToWPFImage(Image imagePassed)
        {

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            //convert System.Drawing.Image to WPF image
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(imagePassed);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap =
                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = WpfBitmap;
            img.Width = 500;
            img.Height = 600;
            img.Stretch = System.Windows.Media.Stretch.Fill;
            return img;

        }

        //convert System.Drawing.Image to WPF image
        public BitmapSource GetImageStream(Image myImage)
        {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }
    }
}
