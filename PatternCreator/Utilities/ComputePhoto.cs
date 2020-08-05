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
        public static byte[] Compute(DocumentModel doc,  bool substrate)
        {
            var template = doc.PictureModel;
            var user = doc.UserModel;
            Bitmap bmp;
            using (var ms = new MemoryStream(template.Image))
            {
                bmp = (Bitmap)Image.FromStream(ms);
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
                g.CompositingQuality = CompositingQuality.HighQuality;

                foreach (var position in template.PositionModels)
                {
                    RectangleF rectf;
                    if (substrate)
                    {
                        rectf = new RectangleF((float)position.PosX, (float)position.PosY,
                            (float)position.Width, (float)position.Height);
                    }
                    else
                    {
                        rectf = new RectangleF((float)position.PosX, (float)position.PosY,
                            (float)position.Width, (float)position.Height);
                    }
                    var text = "";
                    switch (position.Type)
                    {
                        case "Имя":
                            text = user.Name;
                            break;
                        case "Имя(Д.п)":
                            text = user.Name_DativeCase;
                            break;
                        case "Фамилия":
                            text = user.Surname;
                            break;
                        case "Фамилия(Д.п)":
                            text = user.Surname_DativeCase;
                            break;
                        case "Отчество":
                            text = user.Patronymic;
                            break;
                        case "Отчество(Д.п)":
                            text = user.Patronymic_DativeCase;
                            break;
                        case "Должность":
                            text = user.Position;
                            break;
                        case "Образование":
                            text = user.Education;
                            break;
                        case "Число":
                            text = doc.Date.Day.ToString();
                            break;
                        case "Месяц":
                            text = doc.Date.Month.ToString();
                            break;
                        case "Год":
                            text = doc.Date.Year.ToString();
                            break;
                        case "Номер корочки":
                            text = doc.DocumentId.ToString();
                            break;
                        default: 
                            text = position.Text;
                            break;
                    }
                    //SolidBrush b = new SolidBrush(Color.Red);  //кисть для заливки
                    //g.FillRectangle(b, rectf); //заполняю
                    //b.Dispose();
                    var format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    g.DrawString(text, new Font("Tahoma", position.FontSize, GraphicsUnit.Pixel), Brushes.Black, rectf, format);
                }
                if (substrate)
                    foreach (var position in template.StampPositions)
                {
                    RectangleF rectf;
                    if (substrate)
                    {
                        rectf = new RectangleF((float)position.PosX, (float)position.PosY,
                            (float)position.Width, (float)position.Height);
                    }
                    else
                    {
                        rectf = new RectangleF((float)position.PosX, (float)position.PosY,
                            (float)position.Width, (float)position.Height);
                    }

                    using (var st = new MemoryStream(position.StampModel.Image))
                    {

                        Image bmpst = Image.FromStream(st);
                       
                            g.DrawImage(bmpst, rectf);
                    }
                   
                }

                g.Flush();

                return ImageToByte(bmp2);
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            
                //img.Save(st, ImageFormat.Jpeg);
                //return st.ToArray();
                var converter = new ImageConverter();
                return (byte[])converter.ConvertTo(img, typeof(byte[]));
            
           
        }
    }
}