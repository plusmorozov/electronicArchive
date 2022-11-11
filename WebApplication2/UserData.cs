using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using FuncProc;

namespace UserData
{
    //  -------------------------------------------------------------------------------------------------------------
    //  Класс данных о пользователе и его правах 
    //  -------------------------------------------------------------------------------------------------------------
    public class TUserData
    {
        CommonFuncProc CFP = new CommonFuncProc();

        // Структура параметров пользователя
        public struct TUserParamDescr
        {
            public int      Code;           // Код параметра пользователя
            public string   Descr;          // Описание параметра
        }

        public TUserParamDescr[] UPD = new TUserParamDescr[]
        {
            new TUserParamDescr{ Code = 100, Descr = "Роль пользователя"},
            new TUserParamDescr{ Code = 101, Descr = "Список грифов контентов которые доступны для скачивания пользователю"},                                           // str_Grif_Read
            new TUserParamDescr{ Code = 102, Descr = "Список типов документов которые доступны пользователю"},                                                          // str_DocType_Read
            new TUserParamDescr{ Code = 103, Descr = "Список архивов подразделений документы которых может скачать пользователь"},                                      // str_Arch_Read
            new TUserParamDescr{ Code = 104, Descr = "Разрешение на доступ к журналу выдачи"},
            new TUserParamDescr{ Code = 105, Descr = "Просмотр запросов на доступ"},
            new TUserParamDescr{ Code = 106, Descr = "Разрешение администрирование пользователей"},
            //new TUserParamDescr{ Code = 107, Descr = "Разрешение на добавление/изменение пользователей"},
            //new TUserParamDescr{ Code = 117, Descr = "Разрешение на просмотр журнала выдачи документов"},
            new TUserParamDescr{ Code = 108, Descr = "Список архивов подразделений документы которых может выдавать архивариус"},
            new TUserParamDescr{ Code = 109, Descr = "Разрешение на доступ к странице справочников"},
            //new TUserParamDescr{ Code = 110, Descr = "Разрешение на добавление записи в справочники"},
            new TUserParamDescr{ Code = 113, Descr = "Разрешение на создание документа"},
            new TUserParamDescr{ Code = 114, Descr = "Разрешение на администрирование"},
            new TUserParamDescr{ Code = 111, Descr = "Список архивов подразеделений в которых архивариус может создавать/редактировать документы"},                   // str_Doc_Write
            new TUserParamDescr{ Code = 116, Descr = "Разрешение на создание контента"},                                                                              // bool_Cont_Create      
            new TUserParamDescr{ Code = 112, Descr = "Список архивов подразеделений в которых архивариус может создавать/редактировать контент"},                     // str_Cont_Write
            new TUserParamDescr{ Code = 115, Descr = "Разрешение на вывод отладочной информации"},
            new TUserParamDescr{ Code = 200, Descr = "Список документов - Фильтр по типу документа"},
            new TUserParamDescr{ Code = 201, Descr = "Список документов - Фильтр по году"},
            new TUserParamDescr{ Code = 202, Descr = "Список документов - Фильтр по подразделению"},
            new TUserParamDescr{ Code = 203, Descr = "Список документов - Фильтр по статусу оцифровки"},
            new TUserParamDescr{ Code = 777, Descr = "Список ID контентов к которым архивариус предоставил доступ"},
        };

        // Строки запроса чтения, изменения, добавления данных пользователя 
        const string sqlGetUserUID      = "SELECT ID FROM a_Users WHERE strLogin='{0}'";                                                                                                // Строка запроса на данные пользователя
        const string sqlGetUserData     = "SELECT U.strLogin, U.strFam, U.strName, U.strOtch, U.strPodr, U.strPost, U.strPhone, U.strDepartment, U.strEmail, U.boolEnable, P.intValue as intRole, strValue as strRole " +                  // Строка запроса на данные пользователя
                                            "FROM a_Users U LEFT JOIN a_Params P ON ( U.ID = P.ID_User) WHERE U.ID={0} and P.intCode=100";
        
        const string sqlGetUserRight    = "SELECT intCode, intValue, strValue FROM a_Params WHERE ID_User={0}";                                                                         // Запрос на все параметры пользователя

        public string sqlInsertParamStr = "INSERT INTO a_Params( ID_User, intCode, strValue, strDescription ) VALUES( {0}, {1}, '{2}', '{3}' )";
        public string sqlInsertParamInt = "INSERT INTO a_Params( ID_User, intCode, intValue, strDescription ) VALUES( {0}, {1},  {2}, '{3}' )";
        public string sqlUpdateParamStr = "UPDATE a_Params SET strValue='{2}' WHERE ID_User={0} AND intCode={1}";
        public string sqlUpdateParamInt = "UPDATE a_Params SET intValue={2}   WHERE ID_User={0} AND intCode={1}";

        public string sqlIsParamFoundInt = "SELECT COUNT(*) + IFNULL( intValue={2},   0 ) FROM a_Params WHERE ID_User={0} AND intCode={1}";
        public string sqlIsParamFoundStr = "SELECT COUNT(*) + IFNULL( strValue='{2}', 0 ) FROM a_Params WHERE ID_User={0} AND intCode={1}";

        public string strLog_UpdateParam = "таб Параметры - изменение UID={0}, Code({1})=[ {2} ]";
        public string strLog_InsertParam = "таб Параметры - добавление UID={0}, Code({1})=[ {2} ]";

        // Структура прав пользователя
        public struct TUserRight
        {
            public string   str_Role;           // 100 Роль пользователя
            public string   str_Grif_Read;      // 101 Список грифов контентов с которые доступны для скачивания пользователю
            public string   str_DocType_Read;   // 102 Список типов документов которые доступны пользователю
            public string   str_Arch_Read;      // 103 Список архивов подразделений документы которых может скачать пользователь
            public bool     bool_Journal_Read;  // 104 Разрешение на доступ к журналу выдачи
            public bool     bool_Request_Read;  // 105 Просмотр запросов на доступ
            public bool     bool_User_Read;     // 106 Разрешение на администрирование пользователей    
            //public bool     bool_User_Write;  // 107 Разрешение на добавление/изменение пользователей
            public string   str_Doc_Delivery;   // 108 Список архивов подразделений документы которых может выдавать архивариус
            public bool     bool_Spr_Read;      // 109 Разрешение на доступ к странице справочников
            public bool     bool_Spr_Write;     // 110 Разрешение на добавление записи в справочники
            public string   str_Doc_Write;      // 111 Список архивов подразеделений в которых пользователь может создавать/редактировать документы
            public string   str_Cont_Write;     // 112 Список архивов подразеделений в которых пользователь может создавать/редактировать контент
            public bool     bool_Doc_Write;     // 113 Разрешение на создание документа
            public bool     bool_DebugInfo;     // 115 Вывод отладочной информации
            public bool     bool_Admin;         // 114 Доступ на страницу администрирования
            public bool     bool_Cont_Create;   // 116 Разрешение на создание контента
            //public string   str_Allowed_Cont;   // 777 Список архивов подразеделений в которых пользователь может создавать/редактировать контент

        }

        // Структура параметров пользователя
        public struct TUserParam
        { 
            public int M_Year;                  // 201 Список документов - Фильтр по году
            public int M_Podr;                  // 203 Список документов - Фильтр по подразделению      
        }

        public TUserRight   Rights      = new TUserRight(); // Права пользователя
        public TUserParam   Params      = new TUserParam(); // Параметры пользователя
        public int          UID         = 0;                // Данные пользователя
        public string   UserLogin   = "Неизвестно";
        public string   UserFam     = "Неизвестно";
        public string   UserName    = "Неизвестно";
        public string   UserOtch    = "Неизвестно";
        public string   UserPodr    = "Неизвестно";
        public string   UserPost    = "Неизвестно";
        public string   UserPhone   = "Неизвестно";
        public string   UserDepartment = "Неизвестно";
        public string   UserEmail = "Неизвестно";
        public bool     UserEnable  = false;
        public int      intRole     = 0;
        public string   strRole     = "Неизвестно";
        public string   AppVer      = "";                    // Для записи логов

        // Очистка данных пользователя
        public void UserDataClear()
        {
            UserLogin   = "Неизвестно";
            UserFam     = "Неизвестно";
            UserName    = "Неизвестно";
            UserOtch    = "Неизвестно";
            UserPodr    = "Неизвестно";
            UserPost    = "Неизвестно";
            UserPhone   = "Неизвестно";
            UserDepartment = "Неизвестно";
            UserEmail = "Неизвестно";
            intRole     = 0;
            strRole     = "Неизвестно";
            UserEnable  = false;
        }

        // Очистка прав пользователя
        public void UserRightParamClear()
        {
            //Rights.str_Role           = "0";      // Идентификатор роли пользователя
            Rights.str_Grif_Read        = "1";      // Разрешение на чтение общедоступных документов
            Rights.str_DocType_Read     = "-1";     // Разрешение на чтение проектно-сметной и исполнительно-технической документации
            Rights.str_Arch_Read        = "-1";     // Список архивов подразделений к котором имеет доступ пользователь
            Rights.bool_Request_Read    = false;    // Просмотр запросов на доступ
            Rights.bool_Journal_Read    = false;    // Разрешение на доступ к странице журнала выдачи документов
            Rights.bool_Admin           = false;    // Разрешение на администрирование
            Rights.bool_User_Read       = false;    // Разрешение на просмотр пользователей
            //Rights.bool_User_Write      = false;    // Разрешение на добавление/изменение пользователей
            Rights.str_Doc_Delivery     = "-1";     // Разрешение для архивариуса на выдачу документов (согласно списку идентификаторов архивов)
            Rights.bool_Spr_Read        = false;    // Разрешение на просмотр страницы справочников
            Rights.bool_Spr_Write       = false;    // Разрешение на добавление записи в справочник Отделы
            Rights.str_Doc_Write        = "-1";     // Право на создание документа в разрешенных подразделениях архива (согласно списку идентификаторов архивов)
            Rights.str_Cont_Write       ="-1";      // Разрешение на создание контента (согласно списку идентификаторов архивов)
            Rights.bool_Doc_Write       = false;    // Разрешение на создание документа
            Rights.bool_DebugInfo       = false;    // Вывод отладочной информации
            Rights.bool_Cont_Create     = false;    // Разрешение на создание контента
            //Rights.str_Allowed_Cont     = "-1";      // Список ID контентов к которым архивариус разрешил доступ

            Params.M_Year               = 0;        // Год по умолчанию
            Params.M_Podr               = 0;        // Подразделение по умолчанию
        }
    
        // Получение UID
        public void GetUserUID_for_Login(MySqlCommand qrySQL, string strLogin, int Default_UID)
        {
            UserDataClear();
            UserRightParamClear();
            UID = Default_UID;

            qrySQL.CommandText = string.Format(sqlGetUserUID, strLogin);
            MySqlDataReader RS = qrySQL.ExecuteReader();
            if (RS.HasRows & RS.Read())
            {
                UID = RS.GetInt32(0);   // Читаем UID
            }
            RS.Close();

            // Задание UID в целях тестирования
            int TestUID = CFP.GetDigitParam("FhSsrb5684bhsfgBabtnaVsdv", 0);
            if (TestUID > 0) UID = TestUID;
        }

        // Чтение данных пользователя
        public void GetUserData(MySqlCommand qrySQL)
        {
            UserDataClear();
            UserRightParamClear();

            qrySQL.CommandText = string.Format(sqlGetUserData, UID);
            MySqlDataReader RS = qrySQL.ExecuteReader();
            if (RS.HasRows & RS.Read())
            { 
                // Читаем данные пользователя
                UserLogin       = RS.GetString( RS.GetOrdinal ( "strLogin"      ));
                UserFam         = RS.GetString( RS.GetOrdinal ( "strFam"        ));
                UserName        = RS.GetString( RS.GetOrdinal ( "strName"       ));
                UserOtch        = RS.GetString( RS.GetOrdinal ( "strOtch"       ));
                UserPodr        = RS.GetString( RS.GetOrdinal ( "strPodr"       ));
                UserPost        = RS.GetString( RS.GetOrdinal ( "strPost"       ));
                UserPhone       = RS.GetString( RS.GetOrdinal ( "strPhone"      ));
                UserDepartment  = RS.GetString( RS.GetOrdinal ( "strDepartment" ));
                UserEmail       = RS.GetString( RS.GetOrdinal ( "strEmail"      ));
                UserEnable      = RS.GetInt32 ( RS.GetOrdinal ( "boolEnable"    )) != 0;
                intRole         = RS.GetInt32 ( RS.GetOrdinal ( "intRole"       ));
                strRole         = RS.GetString( RS.GetOrdinal ( "strRole"       ));
            }
            RS.Close();
            if (UserEnable) GetRights(qrySQL);  // Если учетная запись не заблокирована, считать права и параметры
            else UserRightParamClear();
        }

        // Чтение прав доступа пользователя
        protected void GetRights(MySqlCommand qrySQL)
        {
            qrySQL.CommandText = string.Format(sqlGetUserRight, UID);
            MySqlDataReader RS = qrySQL.ExecuteReader();
            while (RS.Read())
            {
                switch (RS.GetInt32(0))
                { 
                    // Чтение прав
                    case 100:
                        intRole = RS.GetInt32(1);
                        strRole = RS.GetString(2);
                        break;
                    
                    case 101:
                        Rights.str_Grif_Read = RS.GetString(2);
                        break;

                    case 102:
                        Rights.str_DocType_Read = RS.GetString(2);
                        break;

                    case 103:
                        Rights.str_Arch_Read = RS.GetString(2);
                        break;

                    case 104:
                        Rights.bool_Journal_Read = RS.GetInt32(1) != 0;
                        break;

                    case 105:
                        Rights.bool_Request_Read = RS.GetInt32(1) != 0;
                        break;
                    
                    case 106:
                        Rights.bool_User_Read = RS.GetInt32(1) != 0;
                        break;

                    /*case 107:
                        Rights.bool_User_Write = RS.GetInt32(1) != 0;
                        break;
                    */
                    case 108:
                        Rights.str_Doc_Delivery = RS.GetString(2);
                        break;

                    case 109:
                        Rights.bool_Spr_Read = RS.GetInt32(1) != 0;
                        break;

                    case 110:
                        Rights.bool_Spr_Write = RS.GetInt32(1) != 0;
                        break;

                    case 111:
                        Rights.str_Doc_Write = RS.GetString(2);
                        break;

                    case 112:
                        Rights.str_Cont_Write = RS.GetString(2);
                        break;

                    case 113:
                        Rights.bool_Doc_Write = RS.GetInt32(1) != 0;
                        break;
                   
                    case 114:
                        Rights.bool_Admin = RS.GetInt32(1) != 0;
                        break;

                    case 115:
                        Rights.bool_DebugInfo = RS.GetInt32(1) != 0;
                        break;

                    case 116:
                        Rights.bool_Cont_Create = RS.GetInt32(1) != 0;
                        break;

                    /*case 777:
                        Rights.str_Allowed_Cont = RS.GetString(2);
                        break;
                    */
                    // Чтение параметров
                    case 201:
                        Params.M_Year = RS.GetInt32(1);
                        break;

                    case 202:
                        Params.M_Podr = RS.GetInt32(1);
                        break;
                }
            }
            RS.Close();
        }

        public string GetParamDescr(int Code)
        {
            foreach (TUserParamDescr P in UPD)
            {
                if (Code == P.Code) return P.Descr;
            }
            return "Unknown code";
        }

        // Запись параметра типа Int в базу данных для текущего пользователя
        public bool Save_Param_Int(MySqlCommand qrySQL, int intCode, int intValue)
        {
            return Save_Param_Int_for_User(qrySQL, UID, intCode, intValue);
        }

        // Запись параметра типа String в базу данных для указанного пользователя
        public bool Save_Param_Str_for_User(MySqlCommand qrySQL, int UserUID, int intCode, string strValue)
        {
            int Result = 2;
            string strErr = "";

            // Result
            //   0 - запись отсутствует, необходимо создать (Create)
            //   1 - запись найдена, значения различаются , необходимо изменить значение (Update)
            //   2 - запись найдена, значения совпадают, нет необходимости вносить изменения

            // Запрос на существование данного параметра для пользователя в БД
            CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(sqlIsParamFoundStr, UserUID, intCode, strValue), 0, ref Result, ref strErr);

            switch (Result)
            {
                case 0:
                    // Если не найдено записи - создаем параметр для пользователя в БД
                    CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlInsertParamStr, UserUID, intCode, strValue, GetParamDescr(intCode)), ref strErr);
                    //CFP.AddLog(qrySQL, AppVer, UID, string.Format(strLog_InsertParam, UserUID, intCode, strValue));
                    break;

                case 1:
                    // Если найдена запись - обновляем значение параметра для пользователя в БД
                    CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlUpdateParamStr, UserUID, intCode, strValue), ref strErr);
                    //CFP.AddLog(qrySQL, AppVer, UID, string.Format(strLog_UpdateParam, UserUID, intCode, strValue));
                    break;
            }
            return true;
        }
        
        // Запись параметра типа Int в базу данных для указанного пользователя
        public bool Save_Param_Int_for_User(MySqlCommand qrySQL, int UserUID, int intCode, int intValue)
        {
            int Result = 2;
            string strErr = "";
            // Result
            //   0 - запись отсутствует, необходимо создать (Create)
            //   1 - запись найдена, значения различаются , необходимо изменить значение (Update)
            //   2 - запись найдена, значения совпадают, нет необходимости вносить изменения
            
            // Запрос на существование данного параметра для пользователя в БД
            CFP.SQL_GetOneValue_Integer(qrySQL, string.Format(sqlIsParamFoundInt, UserUID, intCode, intValue), 0, ref Result, ref strErr);

            switch (Result)
            {
                case 0:
                    // Если не найдено записи - создаем параметр для пользователя в БД
                    CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlInsertParamInt, UserUID, intCode, intValue, GetParamDescr(intCode)), ref strErr);
                    //CFP.AddLog(qrySQL, AppVer, UID, string.Format(strLog_InsertParam, UserUID, intCode, intValue));
                    break;

                case 1:
                    // Если найдена запись - обновляем значение параметра для пользователя в БД
                    CFP.SQL_ExecuteNonQuery(qrySQL, string.Format(sqlUpdateParamInt, UserUID, intCode, intValue), ref strErr);
                    //CFP.AddLog(qrySQL, AppVer, UID, string.Format(strLog_UpdateParam, UserUID, intCode, intValue));
                    break;
            }
            return true;
        }

        public string GetUserInfo()
        {
            return UserFam + " " + UserName + " " + UserOtch + " " + UserPost + " " + UserPodr;
        }

    
    }


}