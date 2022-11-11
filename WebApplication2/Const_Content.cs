using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Constants_Content
{
    public class Const_Content
    {
        // запрос для записи в БД информации о контенте
        public string sqlAttachFile = "INSERT INTO a_content (ID_Doc, strName, ID_Type, ID_Grif, ID_StorageBuilding, ID_StoragePlace, ID_ContentCarrier, strNameReal, strNameSystem, sys_DateCreate, "+
                                                             "sys_UserCreate, sys_DateModify, sys_UserModify, sys_VerContent, strDescr) "+
                                                             "VALUES ({0}, '{1}', {2}, {3}, {4}, {5}, {6}, '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', {13}, '{14}')";

        // запрос для получения ID созданного контента
        public string sqlGetCont_ID = "SELECT ID FROM a_content WHERE strDescr='{0}'";

        // запрос для получения версии изменяемого контента
        public string sqlGetVerContent = "SELECT sys_VerContent FROM a_content WHERE ID='{0}'";

        // запрос для получения даты создания первой версии контента
        public string sqlGetDateCreate = "(SELECT sys_DateCreate FROM a_content WHERE ID={0})";

        // запрос для удаления из поля descr уникального ключа для идентификации записи для получения ID контента
        public string sqlClearDescr = "UPDATE a_content SET strDescr='{0}' WHERE ID={1}";

        // запрос для получения ID_Data
        public string sqlGetID_Data = "(SELECT ID_Data FROM a_content WHERE ID={0})";
        
        // запрос для обновления поля ID_Data (ID родительского контента)
        public string sqlUpdateID_Data = "UPDATE a_content SET ID_DATA='{0}' WHERE ID={1}";

        // запрос для обновления поля boolIsActual (ID родительского контента)
        public string sqlUpdateBoolIsActual = "UPDATE a_content SET boolIsActual=false WHERE ID={0}";

        // запрос для получения имени контента на сервере
        public string sqlNameContent = "SELECT strNameSystem as strNameContentSystem, strNameReal as strNameContentReal FROM a_content WHERE ID={0}";

        public string sqlContReadEdit = "SELECT ID_Doc, ID_Type, ID_Grif, ID_StoragePlace, ID_StorageBuilding, ID_ContentCarrier, strName, strNameReal, strNameSystem, strDescr, sys_DateCreate, sys_VerContent " +
                                        "FROM a_Content "+
                                        "WHERE ID={0}";

        // запрос данных о кабинетах хранения документов
        public string sqlSprStoragePlace = "SELECT SP.ID, SP.strName " +
                                           "FROM a_spr_storage_place SP " +
                                           "WHERE " +
                                           "SP.ID_StorageBuild = {0} AND " +
                                           "SP.boolVisible";

    }
}