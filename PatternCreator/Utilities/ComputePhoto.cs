using System;
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
        public static byte[] Compute(UserModel user, PictureModel template, List<PositionModel> positions, bool substrate)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(template.Image))
            {
                bmp = new Bitmap(ms);
                Bitmap bmp2;
                if (substrate)
                {
                    bmp2 = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppPArgb);
                }
                else
                {
                    bmp2 = new Bitmap(bmp.Width, bmp.Height);
                }

                var g = Graphics.FromImage(bmp2);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                foreach (var position in positions)
                {
                    var rectf = new RectangleF((float) position.PosX, (float) position.PosY,
                        (float) position.Width, (float) position.Height);
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
                    //SolidBrush b = new SolidBrush(Color.Red);  //кисть для заливки
                    //g.FillRectangle(b, rectf); //заполняю
                    //b.Dispose();
                    var format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    g.DrawString(text, new Font("Tahoma", position.FontSize, GraphicsUnit.Pixel), Brushes.Black, rectf, format);
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