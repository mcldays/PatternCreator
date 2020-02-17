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
        public static byte[] Compute(UserModel user, PictureModel template, List<PositionModel> positions)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(template.Image))
                bmp = new Bitmap(ms);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            foreach (var position in positions)
            {
                RectangleF rectf = new RectangleF((float)position.PosX + 2, (float)position.PosY + 20, (float)position.Width - 4, 20);
                string text = "";
                switch (position.Type)
                {
                    case "Имя":
                        text = user.Name;
                        break;
                    case "Фамилия":
                        text = user.Surname;
                        break;
                    case "Отчество":
                        text = user.Patronymic;
                        break;
                    case "Статичный текст":
                        break;
                }
                g.DrawString(text, new Font("Tahoma", 14), Brushes.Black, rectf);
            }
            g.Flush();

            return ImageToByte(bmp);
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}