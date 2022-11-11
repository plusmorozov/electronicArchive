using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Constants_Docs
{
    public class Const_Docs
    {
        // запрос, формирующий список документов после применения фильтров на странице main
        /*public string sqlDocList = "SELECT D.ID as IDDoc, D.strName as strNameDoc, (SELECT COUNT(*) FROM a_content WHERE ID_DOC = D.ID AND boolIsActual) as intCountContent, D.intYear as intYear, DT.strName as strNameTypeDoc, " +
                                  "SEC.strName as strNameSec, StatusDoc.strName as strNameStatusDoc " +
                                  "FROM a_doc D " +
                                  "LEFT JOIN a_doc_type DT              ON (DT.ID        = D.ID_Type) " +
                                  "LEFT JOIN a_section SEC              ON (SEC.ID       = D.ID_Section) " +
                                  "LEFT JOIN a_spr_status_doc StatusDoc ON (StatusDoc.ID = D.Id_Status )" +
                                  "WHERE D.intYear>0 {0}";
        */
        public string sqlDocList = "SELECT D.ID, D.strName as strNameDoc, (SELECT COUNT(*) FROM a_content WHERE ID_DOC = D.ID AND boolIsActual) as intCountContent, D.intYear as intYear, DT.strName as strNameTypeDoc, " +
                                  "SEC.strName as strNameSec, StatusDoc.strName as strNameStatusDoc " +
                                  "FROM a_doc D " +
                                  "LEFT JOIN a_doc_type DT              ON (DT.ID        = D.ID_Type) " +
                                  "LEFT JOIN a_section SEC              ON (SEC.ID       = D.ID_Section) " +
                                  "LEFT JOIN a_spr_status_doc StatusDoc ON (StatusDoc.ID = D.Id_Status )" +
                                  "WHERE D.intYear>0 {0}";
        
        // запрос данных об объектах подразделения
        public string sqlSprObject = "SELECT O.ID, O.strName " +
                                     "FROM a_object O " +
                                     "WHERE " +
                                     "O.ID_Object_type = {0} AND " +
                                     "O.boolVisible";
        
        // запрос данных о кабинетах хранения документов
        public string sqlSprStoragePlace = "SELECT SP.ID, SP.strName " +
                                           "FROM a_spr_storage_place SP " +
                                           "WHERE " +
                                           "SP.ID_StorageBuild = {0} AND " +
                                           "SP.boolVisible";

        // запрос для просмотра данных документа по его ID
        public string sqlReadDocView = "SELECT D.strName as strDocName, D.strShifr as strShifr, D.intYear as intYear, DT.strName as strDocType, D.strArchived_Human as strArchivedHuman, S.strName as strSection, OT.strName as strObjectType, "+
                                        "O.strName as strObject, C1.strName as strCustomer, C2.strName as strExecutor, D.strName_Work as strNameWork, "+
                                        "(SELECT  strName FROM a_spr_storage_building SB WHERE SB.ID = D.ID_StorageBuild) as strStorageBuilding, " +
                                        "(SELECT  strName FROM a_spr_storage_place SP WHERE SP.ID = D.ID_StoragePlace) as strStoragePlace, " +
                                        "SD.strName as strStatus, (SELECT Get_Name_Department(D.ID)) as strDepartment, D.strDescr as strDescription " +
                                        "FROM a_doc D "+
                                        "LEFT JOIN a_section        S   ON (S.ID = D.ID_Section) "+
                                        "LEFT JOIN a_doc_type       DT  ON (DT.ID = D.ID_Type) "+
                                        "LEFT JOIN a_object_type    OT  ON (OT.ID = D.ID_Object_type) "+
                                        "LEFT JOIN a_object         O   ON (O.ID = D.ID_Object) "+
                                        "LEFT JOIN a_company        C1  ON (C1.ID = D.ID_Customer) "+
                                        "LEFT JOIN a_company        C2  ON (C2.ID = D.ID_Executor) "+
                                        "LEFT JOIN a_spr_status_doc SD  ON (SD.ID = D.ID_Status) "+
                                        "LEFT JOIN a_doc_department DD  ON (DD.ID_Doc = {0}) " +
                                        "WHERE D.ID = {0}";

        // запрос для добавления данных документа в таблицу a_doc
        public string sqlInsertDoc = "INSERT INTO a_doc (strName, strShifr, intYear, ID_Type, strArchived_Human, ID_Section,  ID_Object_Type, ID_Object, ID_Customer, ID_Executor, strName_Work, "+
                                                        "ID_StorageBuild, ID_StoragePlace, ID_Status, strDescr, strArchived_Data, sys_UserCreate, sys_DateCreate, sys_UserModify, sys_DateModify) "+
                                      "VALUES ('{0}', '{1}', {2}, {3}, '{4}', {5}, {6}, {7}, {8}, {9}, '{10}', {11}, {12}, {13}, '{14}', NOW(), 'Admin', NOW(), 'Admin', NOW())";

        // запрос для получения ID созданного документа
        public string sqlGetDoc_ID = "SELECT ID FROM a_doc WHERE strDescr='{0}'";

        // запрос для удаления из поля descr уникального ключа для идентификации записи для получения ID документа
        public string sqlClearDescr = "UPDATE a_doc SET strDescr='' WHERE ID={0}";

        // запрос для удаления из поля descr уникального ключа для идентификации записи для получения ID документа и записи в него реального значения описания документа
        public string sqlUpdateDescr = "UPDATE a_doc SET strDescr='{0}' WHERE ID={1}";

        // запрос для добавления записи в таблицу a_doc_department, содержащую данные о связке отделов и документов
        public string sqlInsertDocDepartment = "INSERT INTO a_doc_department (ID_Doc, ID_Department) VALUES ({0},{1})";

        // запрос для выставления значений DDL при редактировании документа
        public string sqlDocReadEdit = "SELECT D.strName, D.strShifr, D.intYear, D.ID_Type, D.strArchived_Human, D.ID_Section, D.ID_Object_Type, " +
                                       "D.ID_Object, D.ID_Customer, D.ID_Executor, D.strName_Work, " +
                                       "D.ID_StorageBuild, D.ID_StoragePlace, D.ID_Status, " +
                                       "D.strDescr " +
                                       "FROM a_doc D " +
                                       "WHERE D.ID = {0}";
        
        // запрос для получения строки, содержащей id отделов, имеющих отношение к документу
        public string sqlSelectDepartmentDoc = "SELECT GROUP_Concat(ID_Department) FROM a_doc_department WHERE ID_Doc = {0}";

        public string sqlDeleteDepartmentDoc = "DELETE FROM a_doc_department WHERE ID_Doc = {0}";
    }
}