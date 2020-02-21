using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public static class ComputePhoto
    {
        public static byte[] Compute(UserModel user, PictureModel template, List<PositionModel> positions)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(template.Image))
            {
                bmp = new Bitmap(ms);
                var bmp2 = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppPArgb);
                var g = Graphics.FromImage(bmp2);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                foreach (var position in positions)
                {
                    var rectf = new RectangleF((float) position.PosX, (float) position.PosY,
                        (float) position.Width, 20);
                    var text = "";
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

                    g.DrawString(text, new Font("Tahoma", 20), Brushes.Black, rectf);
                }

                g.Flush();
                return ImageToByte(bmp2);
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }
    }
}