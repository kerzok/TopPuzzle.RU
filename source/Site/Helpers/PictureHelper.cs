using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Toppuzzle.Site.Helpers {
    public class PictureHelper {
        public static IEnumerable<Bitmap> CutImage(Bitmap image, int complexity)
        {
            var list = new List<Bitmap>();
            int countW, countH;
            switch (complexity)
            {
                case 1:
                    countW = 4;
                    countH = 3;
                    break;
                case 2:
                    countW = 6;
                    countH = 4;
                    break;
                case 3:
                    countW = 8;
                    countH = 6;
                    break;
                default:
                    countW = 0;
                    countH = 0;
                    break;
            }
            int width = image.Width/countW;
            int height = image.Height/countH;
            using (var dst = new Bitmap(width, height, image.PixelFormat))
            using (var gfx = Graphics.FromImage(dst))
                for (int y = 0; y < countH; ++y)
                {
                    for (int x = 0; x < countW; ++x)
                    {
                        var destRect = new Rectangle(0, 0, dst.Width, dst.Height);
                        var srcRec = new Rectangle(x*dst.Width, y*dst.Height, dst.Width, dst.Height);
                        gfx.DrawImage(image, destRect, srcRec, GraphicsUnit.Pixel);
                        list.Add(dst);
                    }
                }
            return list;
        }
    }
}