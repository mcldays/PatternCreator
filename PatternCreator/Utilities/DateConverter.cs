using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatternCreator.Utilities
{
    public static class DateConverter
    {
        public static string GetMounthName(int number)
        {
            string text = "";
            switch (number)
            {
                case 1: text = "января"; break;
                case 2: text = "февраля"; break;
                case 3: text = "марта"; break;
                case 4: text = "апреля"; break;
                case 5: text = "мая"; break;
                case 6: text = "июня"; break;
                case 7: text = "июля"; break;
                case 8: text = "августа"; break;
                case 9: text = "сентября"; break;
                case 10: text = "октября"; break;
                case 11: text = "ноября"; break;
                case 12: text = "декабря"; break;
                default: text = "";break;
            }
            return text;
        }
    }
}