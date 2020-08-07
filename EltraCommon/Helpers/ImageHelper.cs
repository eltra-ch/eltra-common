using EltraCommon.Logger;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EltraEnkaCloudStorage.Helpers
{
    /// <summary>
    /// ImageHelper
    /// </summary>
    public static class ImageHelper
    {
        private static Size GetThumbnailSize(Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 60;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        private static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// GetThumbnail
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetThumbnail(string content)
        {
            string result = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    var imageStreamSource = new MemoryStream(Convert.FromBase64String(content));
                    var img = Image.FromStream(imageStreamSource);
                    var thumbnailSize = GetThumbnailSize(img);
                    var thumbnail = img.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    var byteArray = ImageToByteArray(thumbnail);

                    result = Convert.ToBase64String(byteArray);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception("ImageHelper - GetThumbnail", e);
            }

            return result;
        }
    }
}
