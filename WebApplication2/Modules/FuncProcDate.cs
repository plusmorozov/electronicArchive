using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuncProcDate
{
    public class DatePeriodExpression
    {
        protected string WherePattern = "( {0} BETWEEN STR_TO_DATE( '{1}', '%Y-%m-%d %H:%i:%s' ) AND STR_TO_DATE( '{2}', '%Y-%m-%d %H:%i:%s' ) )";

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Year { get; set; }
        public string Format_Start { get; set; }
        public string Format_End { get; set; }
        public string Format_Text { get; set; }
        public string Field { get; set; }
        public string Text { get { return Get_Text(); } }
        public string TextPeriod { get { return Get_TextPeriod(); } }
        public string SQL_Where { get { return Generate_Date_Range_Expression(); } }
        public bool Generated { get { return SQL_Where_Generated(); } }

        public void Generate(string strExpressionDate)
        {
            switch (strExpressionDate)
            {
                case "CD00": Set_Current_Day(0); break;            // сегодня
                case "CD+1": Set_Current_Day(1); break;            // завтра
                case "CD-1": Set_Current_Day(-1); break;            // вчера
                case "CW": Set_Current_Week(); break;            // текущая неделя
                case "PW": Set_Previous_Week(); break;            // предыдущая неделя
                case "CM": Set_Current_Month(); break;            // текущий месяц
                case "CQ": Set_Current_Quarter(); break;            // текущий квартал
                case "FQ1": Set_Fixed_Quarter(1, Year); break;            // 1 квартал
                case "FQ2": Set_Fixed_Quarter(2, Year); break;            // 2 квартал
                case "FQ3": Set_Fixed_Quarter(3, Year); break;            // 3 квартал
                case "FQ4": Set_Fixed_Quarter(4, Year); break;            // 4 квартал
                case "FHY1": Set_Fixed_HalfYear(1, Year); break;            // 1 полугодие
                case "FHY2": Set_Fixed_HalfYear(1, Year); break;            // 2 полугодие
                case "FM01": Set_Fixed_Month(1, Year); break;            // месяцы
                case "FM02": Set_Fixed_Month(2, Year); break;
                case "FM03": Set_Fixed_Month(3, Year); break;
                case "FM04": Set_Fixed_Month(4, Year); break;
                case "FM05": Set_Fixed_Month(5, Year); break;
                case "FM06": Set_Fixed_Month(6, Year); break;
                case "FM07": Set_Fixed_Month(7, Year); break;
                case "FM08": Set_Fixed_Month(8, Year); break;
                case "FM09": Set_Fixed_Month(9, Year); break;
                case "FM10": Set_Fixed_Month(10, Year); break;
                case "FM11": Set_Fixed_Month(11, Year); break;
                case "FM12": Set_Fixed_Month(12, Year); break;
                case "FY": Set_Fixed_Year(Year); break;             // год
                default: Set_Unknown_ExpressionDate(); break;
            }

        }
        
        protected void Set_Unknown_ExpressionDate()
        {
            Start = DateTime.Now;
            End = Start;
            Year = Start.Year;
            Format_Start = "";
            Format_End = "";
            Format_Text = "";
        }

        protected void Set_Current_Day(int Shift)
        {
            Start = DateTime.Now.AddDays(Shift);
            End = Start;
            Year = Start.Year;
            Format_Start = "dd MMMM yyyy";
            Format_End = "";
            Format_Text = "на {0}";
        }

        protected void Set_Current_Week()
        {
            Start = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            End = Start.AddDays(+6);
            Year = Start.Year;
            Format_Start = "dd MMMM";
            Format_End = "dd MMMM yyyy";
            Format_Text = "с {0} по {1}";
        }


        protected void Set_Previous_Week()
        {
            Start = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1 - 7);
            End = Start.AddDays(+6);
            Year = Start.Year;
            Format_Start = "dd MMMM";
            Format_End = "dd MMMM yyyy";
            Format_Text = "с {0} по {1}";
        }
        
        protected void Set_Current_Month()
        {
            Set_Fixed_Month(DateTime.Now.Month, DateTime.Now.Year);
        }

        protected void Set_Current_Quarter()
        {
            Set_Fixed_Quarter(Get_Quarter_Num(DateTime.Now), DateTime.Now.Year);
        }

        protected void Set_Fixed_Quarter(int Quarter, int Year)
        {
            Set_Quarter(Quarter, Year);
            Format_Start = "yyyy";
            Format_End = "";
            Format_Text = "за " + Quarter.ToString() + " квартал {0} года";
        }
        
        protected void Set_Fixed_Month(int Month, int Year)
        {
            if ((Month < 1) | (Month > 12)) throw new Exception("Номер месяца вне допустимого диапазона");
            Start = new DateTime(Year, Month, 1);
            End = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
            Format_Start = "MMMM yyyy";
            Format_End = "";
            Format_Text = "за {0} года";
        }

        protected void Set_Fixed_HalfYear(int Half, int Year)
        {
            Set_HalfYear(Half, Year);
            Format_Start = "yyyy";
            Format_End = "";
            Format_Text = "за " + Half.ToString() + " полугодие {0} года";
        }

        protected void Set_Fixed_Year(int Year)
        {
            Start = new DateTime(Year, 1, 1);
            End = new DateTime(Year, 12, 31);
            Format_Start = "yyyy";
            Format_End = "";
            Format_Text = "за {0} год";
        }

        protected void Get_Fixed_Range(string strExpressionDate, int Year)
        {
            string[] arrR = strExpressionDate.Substring(2).Split('-');
            Start = DateTime.Parse(arrR[0] + '.' + Year.ToString());
            End = DateTime.Parse(arrR[1] + '.' + Year.ToString());
        }

        protected string Generate_Date_Range_Expression()
        {
            if (Format_Text == "") return "";
            return string.Format(WherePattern, Field, Start.ToString("yyyy-MM-dd 00:00:00"), End.ToString("yyyy-MM-dd 23:59:59"));
        }

        protected bool SQL_Where_Generated()
        {
            return SQL_Where != "";
        }

        protected string Get_Text()
        {
            return string.Format(Format_Text, Start.ToString(Format_Start), End.ToString(Format_End));
        }

        protected string Get_TextPeriod()
        {
            if (Format_Text == "") return "";
            string E = End.ToString("dd.MM.yyyy");
            string S = "";
            if (Start.Year != End.Year) S = Start.ToString("dd.MM.yyyy");
            else if (Start.Month != End.Month) S = Start.ToString("dd.MM");
            else if (Start.Day != End.Day) S = Start.ToString("dd");
            if (S != "") return S + "-" + E;
            else return E;
        }

        protected void Set_Quarter(int Quarter, int Year)
        {
            if ((Quarter < 1) | (Quarter > 4)) throw new Exception("Номер квартала вне допустимого диапазона");

            switch (Quarter)
            {
                case 1: Start = new DateTime(Year, 1, 1);
                    End = new DateTime(Year, 3, 31);
                    break;
                case 2: Start = new DateTime(Year, 4, 1);
                    End = new DateTime(Year, 6, 30);
                    break;
                case 3: Start = new DateTime(Year, 7, 1);
                    End = new DateTime(Year, 9, 30);
                    break;
                case 4: Start = new DateTime(Year, 10, 1);
                    End = new DateTime(Year, 12, 31);
                    break;
            }
        }

        protected void Set_HalfYear(int Half, int Year)
        {
            if ((Half < 1) | (Half > 2)) throw new Exception("Номер полугодия вне допустимого диапазона");

            switch (Half)
            {
                case 1: Start = new DateTime(Year, 1, 1);
                    End = new DateTime(Year, 6, 30);
                    break;
                case 2: Start = new DateTime(Year, 7, 1);
                    End = new DateTime(Year, 12, 31);
                    break;
            }
        }
        
        protected int Get_Quarter_Num(DateTime DT)
        {
            return (DT.Month - 1) / 3 + 1;
        }

        protected int Get_HalfYear_Num(DateTime DT)
        {
            return (DT.Month - 1) / 6 + 1;
        }
    }
}