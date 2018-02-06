using System;
using System.Drawing;
using System.Text;

namespace AsciiArt {
    class Art {
        string[] AsciiChars { get; } = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };
        public string ConvertToAscii(Bitmap image) {
            var toggle = false;
            var sb = new StringBuilder();
            for (int h = 0; h < image.Height; h++) {
                for (int w = 0; w < image.Width; w++) {
                    var pixelColor = image.GetPixel(w, h);
                    //Average out the RGB components to find the Gray Color
                    var red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    var green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    var blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    var grayColor = Color.FromArgb(red, green, blue);
                    //Use the toggle flag to minimize height-wise stretch
                    if (!toggle) {
                        var index = (grayColor.R * 10) / 255;
                        sb.Append(AsciiChars[index]);
                    }
                }

                if (!toggle) {
                    sb.Append(Environment.NewLine);
                    toggle = true;
                } else {
                    toggle = false;
                }
            }
            return sb.ToString();
        }

        public Bitmap GetResizedImage(Bitmap inputBitmap, int asciiWidth) {
            var asciiHeight = 0;
            //Calculate the new Height of the image from its width
            asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);
            //Create a new Bitmap and define its resolution
            var result = new Bitmap(asciiWidth, asciiHeight);
            var g = Graphics.FromImage((Image)result);
            //The interpolation mode produces high quality images
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
            g.Dispose();
            return result;
        }
    }

    class Program {
        static void Main(string[] args) {
            var image = "/Users/wk/Source/AsciiArt/images/Rocket.jpg";
            var art = new Art();
            var small = art.GetResizedImage(Bitmap.FromFile(image) as Bitmap, 70);
            var ascii = art.ConvertToAscii(small);
            Console.WriteLine(ascii);
        }
    }
}
