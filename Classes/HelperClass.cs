using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoTraining.Classes
{
    public static class HelperClass
    {
        public static string ConverterDate(DateTime date,string spare = " ")
        {
            //date.Month
            IDictionary<int, string> NameMonth = new Dictionary<int, string>
            {
                { 1 , "มกราคม" },{ 2 , "กุมภาพันธ์" },
                { 3 , "มีนาคม" },{ 4 , "เมษายน" },
                { 5 , "พฤษภาคม" },{ 6 , "มิถุนายน" },
                { 7 , "กรกฎาคม" },{ 8 , "สิงหาคม" },
                { 9 , "กันยายน" },{ 10 , "ตุลาคม " },
                { 11 , "พฤศจิกายน " },{ 12 , "ธันวาคม " },
            };

            string Month = NameMonth[date.Month];
            string Year = (date.Year < 2500 ? date.Year + 543 : date.Year).ToString();

            return $"{date.Day}{spare}{Month}{spare}{Year}";
        }
    }
}
