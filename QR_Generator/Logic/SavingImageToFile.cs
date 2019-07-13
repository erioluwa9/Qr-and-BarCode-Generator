using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QR_Generator
{
    public class SavingImageToFile
    {
        public void SaveUsingEncoder(FrameworkElement element, string fileName, BitmapEncoder encoder)
        {
            try
            {
                var rect = new Rect(element.RenderSize);
                var visual = new DrawingVisual();

                using (var dc = visual.RenderOpen())
                {
                    dc.DrawRectangle(new VisualBrush(element), null, rect);
                }

                var bitmap = new RenderTargetBitmap(
                    (int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
                bitmap.Render(visual);

                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var file = File.OpenWrite(fileName))
                {
                    encoder.Save(file);
                }
                MessageBox.Show("Saved successfully!");
            }
            catch
            {
                MessageBox.Show("UnExpected Error! Please review operation", "Alert");
            }
        }

    }
}
