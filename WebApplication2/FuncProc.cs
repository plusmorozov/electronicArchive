using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace FuncProc
{
    public class CommonFuncProc
    {
        public string Get_Session_Params()
        {
            string S = "";
            for (int i = 0; i < HttpContext.Current.Session.Keys.Count; i++)
            {
                S = S + Convert.ToString(HttpContext.Current.Session.Keys[i]) + " = " + Convert.ToString(HttpContext.Current.Session[i]) + "<br />";
            }
            S = S + "UserName=" + HttpContext.Current.User.Identity.Name + "<br />";
            S = S + "IsAuthenticated=" + HttpContext.Current.User.Identity.IsAuthenticated.ToString() + "<br />";
            return S;
        }

        // Структура и список для методов обновления записи (UPDATE)
        public struct TUpdatingField
        {
            public string FieldName;                    // Имя поля
            public int FieldType;                       // Тип поля, для определения формата данных
            //    0 - число - число
            //    1 - текст, требуются кавычки - 'текст'
            //    2 - дата, требуются кавычки и спец.формат - 'YY-MM-DD HH:NN:SS'
            public string NewValue;                     // Новое значение поля записи
            public bool NeedUpdate;                     // Требуется ли обновление, т.е. изменится ли значение поля после  обновления
        }

        public List<TUpdatingField> UpdatingFields = new List<TUpdatingField>();

        // Очистка полей в списоке обновляемых полей
        public void UpdatingField_Clear()
        {
            UpdatingFields.Clear();
        }

        // Добавление поля в список обновляемых полей
        public void UpdatingField_Add(string FName, int FType, string NValue)
        {
            UpdatingFields.Add(new TUpdatingField() { FieldName = FName, FieldType = FType, NewValue = NValue, NeedUpdate = false });
        }

        public bool Get_SQL_Update(MySqlCommand qrySQL, string TableName, string KeyField_Name, string KeyField_Value, bool NeedModifyInfo, string UserName, ref string strSQL, ref string strLog)
        {
            const string strSelect                  = "SELECT {0} FROM {1} WHERE {2}={3}";
            string strUpdate                        = "UPDATE {0} SET {1}, sys_UserModify='{4}', sys_DateModify=NOW() WHERE {2}={3}";
            const string strUpdate_NoUserModify     = "UPDATE {0} SET {1} WHERE {2}={3}";
            const string strField_NeedUpdating      = " ( {0}<>{1} ) AS {0}";
            const string strNextField_NeedUpdating  = " {0}, ( {1}<>{2} ) AS {1}";
            const string strField_Update            = " {0}={1}";
            const string strNextField_Update        = "{0}, {1}={2}";

            MySqlDataReader RS;
            bool IsFirst = true;
            bool NeedUpdate = false;
            strSQL = "";

            if (!NeedModifyInfo) strUpdate = strUpdate_NoUserModify;

            for (int i = 0; i < UpdatingFields.Count; i++)
            {
                if (IsFirst)
                {
                    strSQL = string.Format(strField_NeedUpdating, UpdatingFields[i].FieldName, PrepareFieldValue(UpdatingFields[i].NewValue, UpdatingFields[i].FieldType));
                    IsFirst = false;
                }
                else strSQL = string.Format(strNextField_NeedUpdating, strSQL, UpdatingFields[i].FieldName, PrepareFieldValue(UpdatingFields[i].NewValue, UpdatingFields[i].FieldType));
            }

            qrySQL.CommandText = string.Format(strSelect, strSQL, TableName, KeyField_Name, KeyField_Value);
            RS = qrySQL.ExecuteReader();
            if (RS.HasRows & RS.Read())
            {
                IsFirst = true;
                strSQL = "";
                for (int i = 0; i < UpdatingFields.Count; i++)
                {
                    if (RS.GetBoolean(RS.GetOrdinal(UpdatingFields[i].FieldName)))
                        if (IsFirst)
                        {
                            strSQL = string.Format(strField_Update, UpdatingFields[i].FieldName, PrepareFieldValue(UpdatingFields[i].NewValue, UpdatingFields[i].FieldType));
                            IsFirst = false;
                            NeedUpdate = true;
                        }
                        else strSQL = string.Format(strNextField_Update, strSQL, UpdatingFields[i].FieldName, PrepareFieldValue(UpdatingFields[i].NewValue, UpdatingFields[i].FieldType));
                }
            }
            RS.Close();
            if (NeedUpdate)
            {
                strLog = strSQL.Replace("'", "");
                strSQL = string.Format(strUpdate, TableName, strSQL, KeyField_Name, KeyField_Value, UserName);
            }
            else
            {
                strLog = "";
                strSQL = "";
            }
            return NeedUpdate;
        }

        public string PrepareFieldValue(string Value, int FieldType)
        {
            string Result = "";
            switch (FieldType)
            {
                case 0:
                    Result = Value;
                    break;
                case 1:
                    Result = "'" + Value + "'";
                    break;
                case 2:
                    Result = "'" + DateTime.Parse(Value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    break;
                case 3:
                    if (Value == "0") Result = false.ToString();
                    else Result = true.ToString();
                    break;
                default:
                    Result = Value;
                    break;
            }
            return Result;
        }

        // ------ Заполнение Items --------------------------------
        public void Fill_Items(HttpServerUtility HSU, MySqlCommand qrySQL, string strSQL, ListItemCollection Items, bool FirstRow_Need, string FirstRow_Value, string FirstRow_Text)
        {
            qrySQL.CommandText = strSQL;
            MySqlDataReader RS = qrySQL.ExecuteReader();
            Items.Clear();
            if (FirstRow_Need)
            {
                Items.Add(new ListItem { Value = FirstRow_Value, Text = FirstRow_Text });
            }
            while (RS.Read())
            {
                if (HSU != null)
                {
                    Items.Add(new ListItem { Value = RS.GetString(0), Text = HSU.HtmlDecode(RS.GetString(1).Replace(" ", "&nbsp;")) });
                }
                else
                {
                    Items.Add(new ListItem { Value = RS.GetString(0), Text = RS.GetString(1) });
                }
            }
            RS.Close();
        }

        // ----- Чтение справочников для вывода в DDL ------
        public void ReadSpravochnik(MySqlCommand qrySQL, string strSQL, DropDownList DDL, int ID_SelectedItem, bool FirstRow_Need, string FirstRow_Value, string FirstRow_Text)
        {
            Fill_Items(null, qrySQL, strSQL, DDL.Items, FirstRow_Need, FirstRow_Value, FirstRow_Text);
            DDL_SetValue(DDL, ID_SelectedItem.ToString());
        }


        // ------- Установить значение DropDownList -------------------------------------
        public void DDL_SetValue(DropDownList DDL, string Value)
        {
            try
            {
                if (DDL.Items.FindByValue(Value) != null) DDL.SelectedValue = Value;
                else DDL.SelectedIndex = -1;
            }
            catch
            {
            }
        }

        // ----- Чтение справочников для вывода в ChekBoxList ------
        public void ReadCheckBoxList(MySqlCommand qrySQL, string strSQL, CheckBoxList chkblst )
        {
            qrySQL.CommandText = strSQL;
            MySqlDataReader RS = qrySQL.ExecuteReader();
            chkblst.Items.Clear();
            if (RS.FieldCount == 2)
            {
                while (RS.Read())
                chkblst.Items.Add(new ListItem { Value = RS.GetString(0), Text = RS.GetString(1) });
            }
            if (RS.FieldCount == 3)
            {
                while (RS.Read())
                chkblst.Items.Add(new ListItem { Value = RS.GetString(0), Text = RS.GetString(1), Selected = (RS.GetInt32(2) != 0) });
            }
            RS.Close();
        }

        // ---- Чтение строкового параметра из Session или Request, если не существует, то вернуть Default
        public string GetStringParam(string ParamName, string Default)
        {
            if (HttpContext.Current.Session[ParamName] != null)                    // Если есть параметр в Session, то вернуть его значение    
            {
                return Convert.ToString(HttpContext.Current.Session[ParamName]);
            }
            if (HttpContext.Current.Request[ParamName] != null)                     // Если есть параметр в Request, то вернуть его значение
            {
                return Convert.ToString(HttpContext.Current.Request[ParamName]);
            }
            return Default;                                                         // Если параметр не найден, то вернуть значение по умолчанию
        }

        // ---- Чтение числового параметра из Session или Request, если не существует или меньше 0, то вернуть Default
        public int GetDigitParam(string ParamName, int Default)
        {
            string strParam = null;

            if (HttpContext.Current.Request[ParamName] != null)                       // Если есть параметр в Request, то считать его
            {
                strParam = (string)(HttpContext.Current.Request[ParamName]);
            }
            if (HttpContext.Current.Session[ParamName] != null)                       // Если есть параметр в Session, то считать его
            {
                strParam = Convert.ToString(HttpContext.Current.Session[ParamName]);
            }

            if (strParam != null)                                                     // Если параметр считан, и если это число больше 0, то вернуть значение, иначе вернуть значение по умолчанию
            {
                int Digit;
                if (Int32.TryParse(strParam, out Digit))
                {
                    if (Digit > 0)
                    {
                        return Digit;
                    }
                    else return Default;
                }
                else return Default;
            }
            else return Default;
        }

        // Выдать список Value через запятую
        public string Get_List_Selected_Value(ListItemCollection Items, string Default)
        {
            if (Items.Count == 0) return Default;

            string Result = "";
            bool First = true;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Selected)
                {
                    if (First)
                    {
                        Result = Items[i].Value;
                        First = false;
                    }
                    else Result = Result + "," + Items[i].Value;
                }
            }
            // Если в загруженном чекбокслисте есть прочеканные элементы, то вернуть строку из ID этих элементов, иначе вернуть значение по умолчанию
            if (Result != "")
            {
                return Result;
            }
            else
            {
                return Default;
            }
        }

        // ------ Вывод ошибки пользователю в элемент BulletedList --------------------------------
        public void Error_Add_Msg(ListItemCollection Items, string strMsg)
        {
            Items.Add(new ListItem { Text = strMsg });
        }

        // ---- Генерация случайной строки
        public string GetRNDString(int Len)
        {
            Random R = new Random();
            string S = "";
            for (int i = 0; i < Len; i++)
            {
                S = S + Convert.ToChar(R.Next(65, 91));
            }
            return S;
        }

        // Функция выполнения запроса без результата (Insert, Update, Delete)
        public bool SQL_ExecuteNonQuery(MySqlCommand qrySQL, string strSQL, ref string Err)
        {
            bool Result = false;
            try
            {
                qrySQL.CommandText = strSQL;
                qrySQL.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
            return Result;
        }

        // Функция получения одного значения из БД - целого типа
        public bool SQL_GetOneValue_Integer(MySqlCommand qrySQL, string strSQL, int Default, ref int Value, ref string Err)
        {
            bool Result = false;
            Value = Default;
            try
            {
                qrySQL.CommandText = strSQL;
                MySqlDataReader RS = qrySQL.ExecuteReader();
                if (RS.HasRows & RS.Read())
                {
                    Value = RS.GetInt32(0);
                    Result = true;
                }
                RS.Close();
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
            return Result;
        }

        // Функция получения одного значения из БД - вещественного типа
        public bool SQL_GetOneValue_Double(MySqlCommand qrySQL, string strSQL, double Default, ref double Value, ref string Err)
        {
            MySqlDataReader RS;
            bool Result = false;
            Value = Default;
            Err = "";
            try
            {
                qrySQL.CommandText = strSQL;
                RS = qrySQL.ExecuteReader();
                if (RS.HasRows & RS.Read())
                {
                    Value = RS.GetDouble(0);
                    Result = true;
                }
                RS.Close();
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
            return Result;
        }

        // Функция получения одного значения из БД - строкового типа
        public bool SQL_GetOneValue_String(MySqlCommand qrySQL, string strSQL, string Default, ref string Value, ref string Err)
        {
            MySqlDataReader RS;
            bool Result = false;
            Value = Default;
            try
            {
                qrySQL.CommandText = strSQL;
                RS = qrySQL.ExecuteReader();
                if (RS.HasRows & RS.Read())
                {
                    Value = RS.GetString(0);
                    Result = true;
                }
                RS.Close();
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
            return Result;
        }

        // Функция получения одного значения из БД - логического типа
        public bool SQL_GetOneValue_Boolean(MySqlCommand qrySQL, string strSQL, bool Default, ref bool Value, ref string Err)
        {
            MySqlDataReader RS;
            bool Result = false;
            Value = Default;
            try
            {
                qrySQL.CommandText = strSQL;
                RS = qrySQL.ExecuteReader();
                if (RS.HasRows & RS.Read())
                {
                    Value = RS.GetBoolean(0);
                    Result = true;
                }
                RS.Close();
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
            return Result;
        }

        public int BoolToInt(bool B)
        {
            if (B) return 1;
            else return 0;
        }

        // Запись в лога
        public void AddLog(MySqlCommand qrySQL, string App_Ver, int UID, string strAction)
        {
            string strErr = "";
            SQL_ExecuteNonQuery(qrySQL,
                                 string.Format("CALL Add_Log( '{0}', {1}, '{2}', '{3}' )",
                                                 App_Ver, UID,
                                                 Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]),
                                                 strAction),
                                 ref strErr);
        }

        // Заполнить дни месяца
        public void DataBind_Days(ListItemCollection Items, int Year, int Month, bool NullRow)
        {
            int DaysInMonth = DateTime.DaysInMonth(Year, Month);
            Items.Clear();
            if (NullRow) Items.Add(new ListItem { Value = "0", Text = "" });
            for (int i = 1; i <= DaysInMonth; i++) Items.Add(i.ToString());
        }

    }
}