using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Constants_Common
{
    public class Const_Common
    {
        // ------ Создание констант и присвоение им значений по умолчанию. При запуске приложения значения этих констант переназначениются значениями из WebConfig ------
        public string App_Ver        = "Архив";
        public string CopyRight      = "УЭСП 2018";
        public string strConn        = "Database=####; Datasource=##.##.##.##; User=####; Password=####";    // Строка подключения
        public string AttachFilePath = "";                                                                   // Путь к прикрепленным файлам
        public string URLFilePath    = "";                                                                   // Ссылка на папку на сервере где лежат прикрепленные файлы
        public int UID_Guest         = 1;                                                                    // Идектификатор пользователя с правами ГОСТЬ
        //public string ContentPath    = "D:\\SHD_Test\\";
        public string ContentPath = "D:\\WEB_APP\\public\\Archive_Files\\";

        // ------ Общие и системные страницы   --------------------------------------------------------
        public string pageMain                  = "~/Main.aspx";                    // страница главная
        public string pageHelp                  = "~/Help.aspx";                    // страница справки и помощи
        public string pageDocCreate             = "~/DocCreate.aspx";               // страница создания документа
        public string pageDocView               = "~/DocView.aspx";                 // страница просмотра документа
        public string pageAdminDocView          = "~/AdminDocView.aspx";            // страница просмотра документа для администратора
        public string pageDocEdit               = "~/DocEdit.aspx";                 // страница редактирования документа
        public string pageContCreate            = "~/ContCreate.aspx";              // страница создания контента
        public string pageContEdit              = "~/ContEdit.aspx";                // страница редактирования контента
        public string pageContPrevVersion       = "~/ContPrevVersion.aspx";         // страница с предыдущими версиями контента
        public string pageAdminContPrevVersion  = "~/AdminContPrevVersion.aspx";    // страница с предыдущими версиями контента
        public string pageRequest               = "~/Request.aspx";                 // страница с запросами доступа к контенту
        public string pageJournal               = "~/Journal.aspx";                 // страница журнала выдачи документов
        public string pageUsers                 = "~/Users.aspx";                   // страница пользователей архива
        public string pageSpr                   = "~/Spr.aspx";                     // страница справочников
        public string pageAdmin                 = "~/Admin.aspx";                   // страница администрирования
        public string pageTestUser              = "~/TestUser.aspx";                // страница тестирования пользователей
        public string pageSprArchive            = "~/SprArchive.aspx";              // страница редактирования справочника архивов
        public string pageSprCompany            = "~/SprCompany.aspx";              // страница редактирования справочника компаний заказчиков/исполнителей
        public string pageSprStorageBuilding    = "~/SprStorageBuilding.aspx";      // страница редактирования справочника зданий хранения
        public string pageSprStorageCabinet     = "~/SprStorageCabinet.aspx";       // страница редактирования справочника мест хранения
        public string pageSprObject             = "~/SprObject.aspx";               // страница редактирования справочника объектов
        public string pageSprDepartment         = "~/SprDepartment.aspx";           // страница редактирования справочника отделов
        public string pageSprStatus             = "~/SprStatus.aspx";               // страница редактирования справочника статусов оцифровки
        public string pageSprObjectType         = "~/SprObjectType.aspx";           // страница редактирования справочника типов объектов
        public string pageSprDocType            = "~/SprDocType.aspx";              // страница редактирования справочника типов документов
        

        // ------ Общие запросы --------------------------------------------------------------------------
        public string sqlSprDocType          = "SELECT ID, strName FROM a_doc_type WHERE boolVisible ORDER BY intSort";                  // строка запроса для получения списка типов документов
        public string sqlSprSection          = "SELECT ID, strName FROM a_section ORDER BY intSort";                                     // строка запроса для получения списка подразделений
        public string sqlSectionOfRights     = "SELECT S.ID, S.strName FROM a_section S, a_params P WHERE p.ID_User = {0} AND p.intCode = 111 AND S.ID IN ({1}) ORDER BY ID";   // строка запроса для получения доступных пользователю архивов для создания документов
        public string sqlSprStatus           = "SELECT ID, strName FROM a_spr_status_doc ORDER BY intSort";                              // строка запроса для получения списка статусов оцифровки
        public string sqlSprObjectType       = "SELECT ID, strName FROM a_object_type ORDER BY intSort";                                 // строка запроса для получения списка типов объектов
        public string sqlSprCustomer         = "SELECT ID, strName FROM a_company ORDER BY intSort";                                     // строка запроса для получения списка заказчиков и исполнителей
        public string sqlSprStorageBuild     = "SELECT ID, strName FROM a_spr_storage_building ORDER BY intSort";                        // строка запроса для получения списка зданий
        public string sqlSprDepartment       = "SELECT ID, strName FROM a_spr_department ORDER BY intSort";                              // строка запроса для получения списка отделов
        public string sqlSprDepartmentCheked = "SELECT D.ID, D.strName, IFNULL( DD.ID, 0 )  "+
                                                    "FROM a_spr_department D "+
                                                    "LEFT JOIN a_doc_department DD ON ( DD.ID_Department = D.ID AND dd.id_doc={0})";     // строка запроса для получения списка заказчиков и исполнителей
        public string sqlSprContType         = "SELECT ID, strName FROM a_content_type WHERE boolVisible ORDER BY intSort";              // строка запроса для получения списка типов документов
        public string sqlSprGrif             = "SELECT ID, strName FROM a_grif WHERE boolVisible ORDER BY intSort";                      // строка запроса для получения списка грифов
        public string sqlSprContCarrier      = "SELECT ID, strName FROM a_spr_content_carrier WHERE boolVisible ORDER BY intSort";       // строка запроса для получения списка типов физических носителей
        //public string sqlUsers = "SELECT U.ID, CONCAT( U.strPodr, ' - ' , U.strFIO, ' ', U.strPost , ' - ', U.strLogin ) as strName " +  // строка запроса для получения списка пользователей
        public string sqlUsers = "SELECT U.ID, CONCAT( U.strPodr, ' - ' , GETFIO(U.strFam,U.strName,U.strOtch,2), ' ', U.strPost , ' - ', U.strLogin ) as strName " +  // строка запроса для получения списка пользователей
                                 "FROM a_users U "+
                                 "ORDER BY U.strName";    

        // ------ Параметры сессии (Session Params) -------------------------------------------------
        public string sp_ID_Doc         = "ID_Doc";         // ID документа
        public string sp_DocType        = "DocType";        // Тип документа
        public string sp_Year           = "Year";           // Год
        public string sp_Section        = "Section";        // Подразделение
        public string sp_Status         = "Status";         // Статус оцифровки
        public string sp_ID_Cont        = "ID_Cont";        // ID контента

        // ------- Запросы относящиеся к правам пользователей
        // Проверка доступных пользователю грифов
        public string sqlRightsGrifRead = "SELECT G.ID, G.strName, IFNULL(G1.ID,0) FROM a_grif G " +
                                       "LEFT JOIN (SELECT ID, strName FROM a_grif WHERE ID IN ({0})) G1 ON (G1.ID = G.ID)";
        // Проверка доступных пользователю типов документов
        public string sqlRightsDocTypeRead = "SELECT DT.ID, DT.strName, IFNULL(DT1.ID,0) FROM a_doc_type DT " +
                                         "LEFT JOIN (SELECT ID, strName FROM a_doc_type WHERE id in ({0})) DT1 ON (DT1.ID = DT.ID)";

        // Проверка доступных пользователю типов документов
        public string sqlRightsArchRead = "SELECT S.ID, S.strName, IFNULL(S1.ID,0) FROM a_section S " +
                                 "LEFT JOIN (SELECT ID, strName FROM a_section WHERE id in ({0})) S1 ON (S1.ID = S.ID)";

        // Проверка доступных пользователю архивов для создания контентов
        public string sqlRightsContCreate = "SELECT S.ID, S.strName, IFNULL(S1.ID,0) FROM a_section S " +
                                 "LEFT JOIN (SELECT ID, strName FROM a_section WHERE id in ({0})) S1 ON (S1.ID = S.ID)";

        // Проверка доступных пользователю архивов для создания документов
        public string sqlRightsDocCreate = "SELECT S.ID, S.strName, IFNULL(S1.ID,0) FROM a_section S " +
                                 "LEFT JOIN (SELECT ID, strName FROM a_section WHERE id in ({0})) S1 ON (S1.ID = S.ID)";

        // Проверка доступных архивариусу подразделений архивов из которых он может выдавать документы
        public string sqlRightsDocDelivery = "SELECT S.ID, S.strName, IFNULL(S1.ID,0) FROM a_section S " +
                                 "LEFT JOIN (SELECT ID, strName FROM a_section WHERE id in ({0})) S1 ON (S1.ID = S.ID)";

        // Добавление записи в a_request с запросом на доступ к контенту
        public string sqlUserRequest = "INSERT INTO a_request (ID_User, ID_Content, sys_DateRequest) "+
                                       "VALUES ({0}, {1}, NOW())";
        
        // Проверка существования запроса к контенту в таблице a_request
        public string sqlCountRequest = "SELECT COUNT(id) " +
                                        "FROM a_request " +
                                        "WHERE id_content = {0} AND id_user = {1} AND intStatus = 0";

        // Вывод информации для архивариуса о текущих запросах на доступ к контенту
        public string sqlShowRequest = "SELECT R.ID as ID, R.ID_Content as ID_Cont, R.Sys_DateRequest as TimeRequest, D.strName as strDocName, C.strName as strContentName, G.strName as strGrifName, S.strName as strArchiveName, CONCAT(GETFIO(U.strFam, U.strName, U.strOtch,2), ' ', U.strPost, ' ', U.strDepartment, ' ',  U.strPodr) as strUserInfo " +
                                       "FROM a_request R "+
                                       "LEFT JOIN a_users U ON (U.ID = R.ID_User) "+
                                       "LEFT JOIN a_content C ON (C.ID = R.ID_Content) "+
                                       "LEFT JOIN a_doc D ON (D.ID = C.ID_Doc) "+
                                       "LEFT JOIN a_grif G ON (G.ID = C.ID_Grif) "+
                                       "LEFT JOIN a_section S ON (S.ID = D.ID_Section) " +
                                       "WHERE R.intStatus = 0 AND S.ID IN ({0})";

        // Обновление запроса на доступ
        public string sqlUpdateRequest = "UPDATE a_request SET sys_DateSolution = NOW(), sys_DateStart = NOW(), sys_DateFinish = DATE_ADD(NOW(), INTERVAL {0} DAY), intStatus = {1}, ID_Archiver = {3} WHERE ID = {2}";

        // Вывод журнала выдачи документов
        public string sqlJournalList = "SELECT  R.sys_DateSolution as strDateSolution, "+
                                                "D.strName as strDocName, " +
                                                "C.strName as strContName, "+
                                                "G.strName as strGrif, "+
                                                "CONCAT(GETFIO(U.strFam, U.strName, U.strOtch,2), ' ', U.strPost, ' ', U.strDepartment, ' ',  U.strPodr) as strUser, "+
                                                "GETFIO(U2.strFam, U2.strName, U2.strOtch,2) as strArchiver, "+
                                                "intStatus as strStatus "+
                                        "FROM a_request R "+
                                        "LEFT JOIN a_users   U   ON  ( U.ID = R.ID_User ) "+
                                        "LEFT JOIN a_users   U2  ON  ( U2.ID = R.ID_Archiver ) "+
                                        "LEFT JOIN a_content C   ON  ( C.ID = R.ID_Content ) "+
                                        "LEFT JOIN a_doc     D   ON  ( D.ID = C.ID_Doc) "+
                                        "LEFT JOIN a_grif    G   ON  ( G.ID = C.ID_Grif) "+
                                        "WHERE {0}";
    }
}