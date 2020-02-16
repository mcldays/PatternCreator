using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public static class ComputePhoto
    {
        public static string Compute(UserModel user, PictureModel template, List<PositionModel> positions)
        {
            Bitmap bmp;

            RectangleF rectf = new RectangleF(70, 90, 90, 50);

            using (var ms = new MemoryStream(template.Image))
                bmp = new Bitmap(ms);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            foreach (var position in positions)
            {
                g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf);
            }

            g.Flush();
            return "";
        }
    }
}