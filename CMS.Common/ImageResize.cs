using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS
{
   public class ImageResize
    {
        public enum ResizeState
        {
            Done,
            Failed,
            previous
        }

        public static ResizeState Resize(string webRoot, string uploadFolder , string fileName , Image source, int width, int height , out string fileSaveAddress)
        {
            if (source.Width == width && source.Height == height)
            {
                fileSaveAddress = "/uploads/" + uploadFolder + "/minipic/" + fileName;
                return ResizeState.previous;
            }
            try
            {
                using (var result = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                {
                    result.SetResolution(source.HorizontalResolution, source.VerticalResolution);
                    using (var g = Graphics.FromImage(result))
                        g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);

                    var uploadDirectory = Path.Combine(webRoot, "uploads/" + uploadFolder + "/minipic");
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }
                    var thumbPath = Path.Combine(uploadDirectory, fileName);
                    result.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileSaveAddress = "/uploads/" + uploadFolder + "/minipic/" + fileName;
                    return ResizeState.Done;
                }
            }
            catch
            {
                fileSaveAddress = string.Empty;
                return ResizeState.Failed;
            }
           
        }
    }
}
