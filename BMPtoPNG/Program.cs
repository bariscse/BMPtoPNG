using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;







namespace BMPtoPNG
{
    internal class Program
    {
        static void ConvertBmpToPngWithScaling(string bmpFilePath, float scale)
        {
            string pngFilePath = System.IO.Path.ChangeExtension(bmpFilePath, ".png");

            using (Bitmap bmp = new Bitmap(bmpFilePath))
            {
                // Calculate the scaled dimensions
                int newWidth = (int)(bmp.Width * scale);
                int newHeight = (int)(bmp.Height * scale);

                // Use the ResizeImage method to create a new scaled bitmap
                using (Bitmap newBmp = ResizeImage(bmp, newWidth, newHeight))
                {
                    // Save the PNG file to the specified path
                    newBmp.Save(pngFilePath, ImageFormat.Png);
                }
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: BmpToPngConverter <bmpFilePath> [scale]");
                return;
            }

            string bmpFilePath = args[0];
            float scale = 1.0f;

            if (args.Length >= 2)
            {
                scale = float.Parse(args[1]);
            }

            ConvertBmpToPngWithScaling(bmpFilePath, scale);
        }
    }
}

