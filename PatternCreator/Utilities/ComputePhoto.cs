using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public static class ComputePhoto
    {
        public static List<string> TrimWrapping(string sourcestring, int part, int wrapcount)
        {
            if (string.IsNullOrEmpty(sourcestring) || wrapcount >= sourcestring.Length)
            {
                List<string> l = new List<string>();
                l.Add(sourcestring);
                for (int i = 1; i < part; i++)
                    l.Add("");
                return l;
            }
            List<string> stringList = new List<string>();
            int lastwrapindex = 0;
            int startcount = 0;
            while (stringList.Count<part)
            {
                if (sourcestring.Length==0)
                {
                    stringList.Add("");
                    continue;
                }
                int wrap = wrapcount >= sourcestring.Length ? sourcestring.Length - 1 : wrapcount;
                for (int i = wrap; i >= 0; i--)
                {
                    if (sourcestring[i] == ' ')
                    {
                        stringList.Add(sourcestring.Substring(0, i).TrimEnd());
                        sourcestring = sourcestring.Remove(0, i + 1);
                        break;
                    }
                }

                if (sourcestring.Length<=wrap)
                {
                    stringList.Add(sourcestring);
                    sourcestring = "";
                }
                if (startcount == stringList.Count)
                    stringList.Add("");
                startcount = stringList.Count;
            }
                
            
            
            
            
            
            
            
            
            return stringList;
        }

        public static DocumentPrintViewModel GetDocToPrint(DocumentModel doc)
        {
            DocumentPrintViewModel dpvm = new DocumentPrintViewModel();
            dpvm.DocId = doc.DocumentId;
            switch (doc.PatternId)
            {
                case 45: dpvm.Identity = DocumentPrintViewModel.TypeDoc.WorkHighSecurityWithQuality; break;
                case 47: dpvm.Identity = DocumentPrintViewModel.TypeDoc.WorkSecurity; break;
                case 53: dpvm.Identity = DocumentPrintViewModel.TypeDoc.WorkHighSecurity; break;
                case 54: dpvm.Identity = DocumentPrintViewModel.TypeDoc.Pplam; break;
                case 51: dpvm.Identity = DocumentPrintViewModel.TypeDoc.Ptm; break;
                case 52: dpvm.Identity = DocumentPrintViewModel.TypeDoc.WorkIdentity; break;
                default: dpvm.Identity = DocumentPrintViewModel.TypeDoc.Other; break;

            }
            if (doc.PatternId == 45 || doc.PatternId == 53|| doc.PatternId==54)
                return dpvm;
            
            var template = doc.PictureModel;
            var user = doc.UserModel;
            
            
            dpvm.NaturalHeight = template.NaturalHeight;
            dpvm.NaturalWidth = template.NaturalWidth;
            foreach (var position in template.PositionModels)
            {
                RectangleF rectf = new RectangleF((float) (position.PosX ), (float) (position.PosY ),
                        (float) (position.Width ), (float) (position.Height ));
                var text = "";
                string[] HandWriteFields = JsonConvert.DeserializeObject<string[]>(doc.HandWriteFields);
                switch (position.Type)
                    {
                        case "Компания":
                            text = user.CompanyModel.CompanyName;
                            break;
                        case "Ф.И.О":
                            text = $"{user.Surname} {user.Name} {user.Patronymic}";
                            break;
                        case "Ф.И.О(Д.п.)":
                            text = $"{user.Surname_DativeCase} {user.Name_DativeCase} {user.Patronymic_DativeCase}";
                            break;
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
                        case "Число *конец":
                            text = doc.Date.Day.ToString("d2");
                            break;
                        case "Месяц *конец":
                            text = doc.Date.Month.ToString("d2");
                            break;
                        case "Год *конец":
                            text = doc.Date.Year.ToString("d4");
                            break;
                        case "(дд.мм.гггг) г. *конец":
                            text = doc.Date.Date.ToString("dd.MM.yyyy")+" г.";
                            break;
                        case "(дд.мм.гггг) *конец":
                            text = doc.Date.Date.ToString("dd.MM.yyyy");
                            break;
                        case "Число *начало":
                            text = doc.StartDate.Day.ToString("d2");
                            break;
                        case "Месяц *начало":
                            text = doc.StartDate.Month.ToString("d2");
                            break;
                        case "Год *начало":
                            text = doc.StartDate.Year.ToString("d4");
                            break;
                        case "(дд.мм.гггг) г. *начало":
                            text = doc.StartDate.Date.ToString("dd.MM.yyyy") + " г.";
                            break;
                        case "(дд.мм.гггг) *начало":
                            text = doc.StartDate.Date.ToString("dd.MM.yyyy");
                            break;
                        case "Число *действителено до":
                            text = doc.Date.AddYears(doc.SpecialtyModel.ValidUntil).Day.ToString("d2");
                            break;
                        case "Месяц *действителено до":
                            text = doc.Date.AddYears(doc.SpecialtyModel.ValidUntil).Month.ToString("d2");
                            break;
                        case "Год *действителено до":
                            text = doc.Date.AddYears(doc.SpecialtyModel.ValidUntil).Year.ToString("d4");
                            break;
                        case "(дд.мм.гггг) г. *действителено до":
                            text = doc.Date.AddYears(doc.SpecialtyModel.ValidUntil).Date.ToString("dd.MM.yyyy") + " г.";
                            break;
                        case "(дд.мм.гггг) *действителено до":
                            text = doc.Date.AddYears(doc.SpecialtyModel.ValidUntil).Date.ToString("dd.MM.yyyy");
                            break;
                    case "Программа обучения":
                            text = doc.SpecialtyModel.SpecialityName;
                            break;
                        case "Обучающая организация":
                            text = doc.Organization.OrganizationName;
                            break;
                        case "Количество учебных часов":
                            text = doc.EducationTime;
                            break;
                        case "Номер протокола":
                            text = doc.ProtocolName+"-"+doc.SpecialtyModel.Prefix;
                            break;
                        case "Номер бланка":
                            text = doc.PictureModel.DocumentModels.OrderBy(t=>t.DocumentId).ToList().IndexOf(doc).ToString("D5");
                            break;
                    case "Номер корочки":
                            text = doc.DocumentId.ToString();
                            break;
                        case "Ручной ввод":
                            text = HandWriteFields[template.PositionModels.Where(t => t.Type == "Ручной ввод").ToList().IndexOf(position)];
                            break;
                        case "Квалификация":
                            text = doc.SpecialtyModel.Quality;
                            break;
                        case "Сфера деятельности":
                            text = doc.SpecialtyModel.FieldSpecialty;
                            break;
                        case "Лицензия":
                            text = doc.Organization.License;
                            break;

                    default:
                            text = position.Text;
                            break;
                    }
               
                         var format = new StringFormat();
                format.LineAlignment = StringAlignment.Near;
                switch (position.Alignment)
                    {
                        case "Слева":
                            format.Alignment = StringAlignment.Near;
                            break;
                        case "По центру":
                            format.Alignment = StringAlignment.Center;
                            break;
                        case "По ширине":
                            format.Alignment = StringAlignment.Near;
                            format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                        break;
                    case "Справа":
                            format.Alignment = StringAlignment.Far;
                            break;
                    }

                format.Trimming = StringTrimming.None;
                FontStyle weight;
                bool underline = false;
                switch (position.FontWeight)
                    {
                        case "Regular":
                            weight = FontStyle.Regular;
                            break;
                        case "Regular Underline":
                            weight = FontStyle.Regular;
                            underline = true;
                            break;
                        case "Bold":
                            weight = FontStyle.Bold;
                            break;
                        case "Bold Underline":
                            weight = FontStyle.Bold;
                            underline = true;
                            break;
                    default:
                            weight = FontStyle.Regular;
                            break;
                    }

                dpvm.Items.Add(new ItemModel
                {
                    FontSize = position.FontSize,
                    Format = format,
                    Rectf = rectf,
                    Weight = weight,
                    Text = text,
                    Underline = underline
                });
                
            }
            foreach (var position in template.StampPositions)
            {
                RectangleF rectf = new RectangleF((float)(position.PosX), (float)(position.PosY),
                        (float)(position.Width), (float)(position.Height));
                dpvm.Stamps.Add(new Stamp
                {
                    Rectf = rectf,
                    Image = Convert.ToBase64String(position.StampModel.Image)
                });

            }
            foreach (var position in template.PhotoModels)
            {
                RectangleF rectf = new RectangleF((float)(position.PosX), (float)(position.PosY),
                    (float)(position.Width), (float)(position.Height));
                dpvm.Photos.Add(new Stamp
                {
                    Rectf = rectf,
                    UserId = user.Id 
                });

            }
            dpvm.Image = template.Id;
            return dpvm;
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