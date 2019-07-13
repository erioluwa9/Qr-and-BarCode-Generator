using C1.C1Pdf;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QR_Generator
{
    public partial class MainWindow : Window
    {
        readonly ConvertImage ConvertImage = new ConvertImage();
        readonly BitmapCreator BitmapCreator = new BitmapCreator();
        readonly SavingImageToFile SavingImageToFile = new SavingImageToFile();
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBarCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBarCode.Text != "")
                {
                    Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                    var Image = barcode.Draw(txtBarCode.Text, 150);
                    PictureBox.Source = ConvertImage.GetImageStream(Image);
                    txtDisplay.Text = txtBarCode.Text;
                    txtBarCode.Text = null;
                }
                else
                {
                    MessageBox.Show("Input Box cannot be empty.", "Alert");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("UnExpected Error! Please review input", "Alert");
            }
        }

        private void BtnQRCode_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                /************** --- Method 1---*******************/

                //QRCodeGenerator qr = new QRCodeGenerator();
                //QRCodeData data = qr.CreateQrCode(txtQRCode.Text, QRCodeGenerator.ECCLevel.Q);
                //QRCode code = new QRCode(data);
                //PictureBox.Source = GetImageStream(code.GetGraphic(50));


                /************** --- Method 2---*******************/
                if (txtQRCode.Text != "")
                {
                    Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                    var qrImage = qrcode.Draw(txtQRCode.Text, 100);
                    PictureBox.Source = ConvertImage.GetImageStream(qrImage);
                    txtDisplay.Text = txtQRCode.Text;
                    txtQRCode.Text = null;
                }
                else
                {
                    MessageBox.Show("Input Box cannot be empty.", "Alert");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("UnExpected Error! Please review input", "Alert");
            }

        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png|PDF|*.pdf";
            fileDialog.Title = "Save an Image File";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != "")
            {
                /* Saves the Image in the appropriate ImageFormat based upon the
                 File type selected in the dialog box. */
                switch (fileDialog.FilterIndex)
                {
                    case 1:
                        var jpeg = new JpegBitmapEncoder();
                        SavingImageToFile.SaveUsingEncoder(PictureBox, fileDialog.FileName, jpeg);
                        break;

                    case 2:
                        var bmp = new BmpBitmapEncoder();
                        SavingImageToFile.SaveUsingEncoder(PictureBox, fileDialog.FileName, bmp);
                        break;

                    case 3:
                        var gif = new GifBitmapEncoder();
                        SavingImageToFile.SaveUsingEncoder(PictureBox, fileDialog.FileName, gif);
                        break;

                    case 4:
                        var png = new PngBitmapEncoder();
                        SavingImageToFile.SaveUsingEncoder(PictureBox, fileDialog.FileName, png);
                        break;

                    case 5:
                        // create document  
                        try 
                        {
                            var pdf = new C1PdfDocument(PaperKind.Letter);
                            pdf.Clear();

                            var img = new WriteableBitmap(BitmapCreator.CreateBitmap(PictureBox));
                            var image = BitmapCreator.BitmapFromWriteableBitmap(img);

                            pdf.DrawImage(image, pdf.PageRectangle, ContentAlignment.MiddleCenter, default);

                            // save document  
                            using (var file = File.OpenWrite(fileDialog.FileName))
                                pdf.Save(file);

                            MessageBox.Show("Saved successfully!");
                        }
                        catch
                        {
                            MessageBox.Show("UnExpected Error! Please review operation", "Alert");
                        }
                        break;
                }
            }
        }
        
        private void TopBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
