﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MTG_Librarian
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image img)
        {
            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            var Bytes = ms.ToArray();
            return Bytes;
        }

        public static Bitmap GetCopyOf(this Image originalImage)
        {
            var copy = new Bitmap(originalImage.Width, originalImage.Height);
            using (Graphics graphics = Graphics.FromImage(copy))
            {
                var imageRectangle = new Rectangle(0, 0, copy.Width, copy.Height);
                graphics.DrawImage(originalImage, imageRectangle, imageRectangle, GraphicsUnit.Pixel);
            }
            return copy;
        }

        public static Image FromByteArray(byte[] bytes)
        {
            if (bytes == null) return null;
            var ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// Scales down the image, maintaining original aspect ratio
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="highquality"></param>
        /// <returns></returns>
        public static Image ScaleImage(this Image image, int width, int height, bool highquality = true)
        {
            Image returnImage = null;

            if (image.Width <= width && image.Height <= height)
            {
                returnImage = image;
            }
            else
            {
                double widthRatio = 1;
                double heightRatio = 1;
                if (image.Width > 0)
                    widthRatio = width / (double)image.Width;

                if (image.Height > 0)
                    heightRatio = height / (double)image.Height;

                if (widthRatio < heightRatio)
                {
                    returnImage = highquality
                        ? HighQualityImageResize(image, (int)(image.Width * widthRatio), (int)(image.Height * widthRatio))
                        : LowQualityImageResize(image, (int)(image.Width * widthRatio), (int)(image.Height * widthRatio));
                }
                else
                {
                    returnImage = highquality
                        ? HighQualityImageResize(image, (int)(image.Width * heightRatio), (int)(image.Height * heightRatio))
                        : LowQualityImageResize(image, (int)(image.Width * heightRatio), (int)(image.Height * heightRatio));
                }
            }

            return returnImage;
        }

        private static Image HighQualityImageResize(Image image, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, bmp.Width, bmp.Height);
            }
            return bmp;
        }

        private static Image LowQualityImageResize(Image image, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                graphics.DrawImage(image, 0, 0, bmp.Width, bmp.Height);
            }
            return bmp;
        }

        public static Image SetCanvasSize(this Image image, int canvasWidth, int canvasHeight)
        {
            return canvasWidth >= image.Width && canvasHeight >= image.Height
                ? image.EnlargeCanvas(canvasWidth, canvasHeight)
                : image.ScaleImage(canvasWidth, canvasHeight, true).EnlargeCanvas(canvasWidth, canvasHeight);
        }

        /// <summary>
        /// Enlarges the canvas of the image, placing the image in the center.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="canvasWidth"></param>
        /// <param name="canvasHeight"></param>
        /// <returns></returns>
        public static Image EnlargeCanvas(this Image image, int canvasWidth, int canvasHeight)
        {
            var bmp = new Bitmap(canvasWidth, canvasHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                var topLeft = new Point((canvasWidth - image.Width) / 2, (canvasHeight - image.Height) / 2);
                g.DrawImage(image, new Rectangle(topLeft, new Size(image.Width, image.Height)));
            }
            return bmp;
        }

        public static Image ToOpaque(this Image img, Color backColor)
        {
            var b = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(backColor);
                g.DrawImage(img, 0, 0, img.Width, img.Height);
            }
            return b;
        }
    }

    public static class ColorRemapper
    {
        public static ImageAttributes GetImageAttributes(Color oldColor, Color newColor)
        {
            var colorMap = new ColorMap[] { new ColorMap { OldColor = oldColor, NewColor = newColor } };
            var attr = new ImageAttributes();
            attr.SetRemapTable(colorMap);
            return attr;
        }
    }
}

