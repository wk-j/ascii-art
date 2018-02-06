using System;
using System.Drawing;
using System.Text;

namespace AsciiArt {
    class Art {
        private string[] _AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", "&nbsp;" };
        public string ConvertToAscii(Bitmap image) {
            Boolean toggle = false;
            StringBuilder sb = new StringBuilder();
            for (int h = 0; h < image.Height; h++) {
                for (int w = 0; w < image.Width; w++) {
                    Color pixelColor = image.GetPixel(w, h);
                    //Average out the RGB components to find the Gray Color
                    int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    Color grayColor = Color.FromArgb(red, green, blue);
                    //Use the toggle flag to minimize height-wise stretch
                    if (!toggle) {
                        int index = (grayColor.R * 10) / 255;
                        sb.Append(_AsciiChars[index]);
                    }
                }

                if (!toggle) {
                    sb.Append("<BR>");
                    toggle = true;
                } else {
                    toggle = false;
                }
            }
            return sb.ToString();
        }

        public Bitmap GetResizedImage(Bitmap inputBitmap, int asciiWidth) {
            int asciiHeight = 0;
            //Calculate the new Height of the image from its width
            asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);
            //Create a new Bitmap and define its resolution
            Bitmap result = new Bitmap(asciiWidth, asciiHeight);
            Graphics g = Graphics.FromImage((Image)result);
            //The interpolation mode produces high quality images
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
            g.Dispose();
            return result;
        }
    }

    class Program {
        static void Main(string[] args) {
            var image = "/Users/wk/Source/AsciiArt/images/Jw.png";
            var art = new Art();
            var small = art.GetResizedImage(Bitmap.FromFile(image) as Bitmap, 200);
            var ascii = art.ConvertToAscii(small);
            Console.WriteLine(ascii);
        }
    }
}
