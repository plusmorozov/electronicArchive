-------------------------------------------------------------------------------
-- Export Status
-------------------------------------------------------------------------------
-- Date:           07.11.2022 15:31:58
-- Server version: 5.7.27-log
-- Host:           10.43.61.4
-- Database:       archive
-- User:           root
-------------------------------------------------------------------------------
-- Options
-------------------------------------------------------------------------------
-- compatible:               None
-- charset:                  utf8
-- add-database-definition:  No
-- use-drop-create-database: Yes
-- only-structure:           Yes
-- add-lock:                 No
-- disable-keys:             No
-- single-transactions:      No
-- use-replace:              No
-- use-insert-delayed:       No
-- use-insert-ignore:        No
-------------------------------------------------------------------------------
-- Objects
-------------------------------------------------------------------------------
-- Tables:   33/33
-- Views:    0/0
-- Routines: 13/13
-- Events:   0/0
-------------------------------------------------------------------------------

SET NAMES 'utf8';

--
-- Drop tables
--

DROP TABLE IF EXISTS `a_company`;
DROP TABLE IF EXISTS `a_company_type`;
DROP TABLE IF EXISTS `a_content`;
DROP TABLE IF EXISTS `a_content_type`;
DROP TABLE IF EXISTS `a_doc`;
DROP TABLE IF EXISTS `a_doc_department`;
DROP TABLE IF EXISTS `a_doc_type`;
DROP TABLE IF EXISTS `a_grif`;
DROP TABLE IF EXISTS `a_log`;
DROP TABLE IF EXISTS `a_object`;
DROP TABLE IF EXISTS `a_object_type`;
DROP TABLE IF EXISTS `a_params`;
DROP TABLE IF EXISTS `a_request`;
DROP TABLE IF EXISTS `a_section`;
DROP TABLE IF EXISTS `a_spr_content_carrier`;
DROP TABLE IF EXISTS `a_spr_department`;
DROP TABLE IF EXISTS `a_spr_status_doc`;
DROP TABLE IF EXISTS `a_spr_storage_building`;
DROP TABLE IF EXISTS `a_spr_storage_place`;
DROP TABLE IF EXISTS `a_users`;

--
-- Drop stored procedures and functions
--

DROP PROCEDURE IF EXISTS `Add_Log`;
DROP PROCEDURE IF EXISTS `Add_Log_SHD`;
DROP PROCEDURE IF EXISTS `Show_Log_SHD`;
DROP FUNCTION IF EXISTS `Document_Create`;
DROP FUNCTION IF EXISTS `GetContentAction`;
DROP FUNCTION IF EXISTS `GetFIO`;
DROP FUNCTION IF EXISTS `Get_Doc_Finish_Descr`;
DROP FUNCTION IF EXISTS `Get_Doc_Finish_Descr2`;
DROP FUNCTION IF EXISTS `Get_Doc_Status`;
DROP FUNCTION IF EXISTS `Get_Doc_Status_Edit`;
DROP FUNCTION IF EXISTS `Get_Doc_Users`;
DROP FUNCTION IF EXISTS `Get_Name_Department`;
DROP FUNCTION IF EXISTS `Section_Create`;

--
-- Definition for table "a_company"
--

CREATE TABLE `a_company`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `ID_Company_Type` INT(11) NOT NULL DEFAULT '0' COMMENT 'Идентификатор типа компании (sprCompany_Type.ID)',
  `strName` VARCHAR(250) NOT NULL DEFAULT '!!! Наименование' COMMENT 'Среднее наименование организации',
  `strNameShort` VARCHAR(250) NOT NULL DEFAULT '!!! Сокращенное наименование' COMMENT 'Сокращенное наименование организации',
  `strNameFull` MEDIUMTEXT NOT NULL COMMENT 'Полное наименование организации',
  `strAddress` MEDIUMTEXT NOT NULL COMMENT 'Местоположение организации, адрес',
  `strCode_UESP` VARCHAR(10) NOT NULL DEFAULT '!!!' COMMENT 'Код структурной единицы УЭСП',
  `strCode_Num` VARCHAR(10) NOT NULL DEFAULT '!!!' COMMENT 'Код для формирования номера документа',
  `boolDesigner` BOOL NOT NULL DEFAULT '-1' COMMENT 'Является проектировщиком (True-Да, False-нет)',
  `boolExecutor` BOOL NOT NULL DEFAULT '-1' COMMENT 'Является исполнителем (True-Да, False-нет)',
  `boolVisible` BOOL NOT NULL DEFAULT '-1' COMMENT 'Признак отображения при выборе из списка',
  `intSort` TINYINT(3) UNSIGNED NOT NULL DEFAULT '250' COMMENT 'Ключ сортировки',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 155
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_company_type"
--

CREATE TABLE `a_company_type`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strName` VARCHAR(250) NOT NULL DEFAULT 'Тип организации' COMMENT 'Наименование типа организации',
  `strNameShort` VARCHAR(30) NOT NULL DEFAULT 'Тип орг.' COMMENT 'Сокращенное наименование типа организации',
  `strDoWork` VARCHAR(30) NOT NULL DEFAULT 'Работы выполняются' COMMENT 'Кем выполняются работы',
  `boolVisible` BOOL NOT NULL DEFAULT '-1' COMMENT 'Признак отображения при выборе из списка',
  `intSort` TINYINT(4) NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 4
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_content"
--

CREATE TABLE `a_content`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `ID_Doc` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на документ (a_Doc.ID)',
  `ID_Type` INT(11) NOT NULL DEFAULT '0' COMMENT 'Тип части документа. Указатель на тип контента (a_Content_type.ID)',
  `ID_Grif` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на гриф секрености (a_Grif.ID)',
  `ID_StoragePlace` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на место хранения (a_spr_storage_place.ID)',
  `ID_StorageBuilding` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на здание хранения (a_spr_storage_building.ID)',
  `ID_ContentCarrier` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на тип носителя (a_spr_content_carrier.ID)',
  `strName` VARCHAR(250) NOT NULL DEFAULT '!!! Название документа' COMMENT 'Название контента',
  `sys_DateCreate` DATETIME NOT NULL COMMENT 'Дата создания контента',
  `sys_UserCreate` VARCHAR(45) NOT NULL DEFAULT '!!! Создатель контента' COMMENT 'Пользователь, создавший контент',
  `sys_DateModify` DATETIME NOT NULL COMMENT 'Дата изменения контента',
  `sys_UserModify` VARCHAR(45) NOT NULL DEFAULT '!!! Пользователь, изменивший контент' COMMENT 'Пользователь, изменивший контент',
  `sys_VerContent` INT(11) NOT NULL DEFAULT '0' COMMENT 'Версия контента',
  `strNameReal` VARCHAR(150) NOT NULL DEFAULT '!!! Реальное имя контента в файловой системе' COMMENT 'Реальное имя контента',
  `strNameSystem` VARCHAR(150) NOT NULL DEFAULT '!!! Имя файла после загрузки на сервер' COMMENT 'Имя файла после загрузки на сервер',
  `strDescr` VARCHAR(250) NOT NULL DEFAULT '!!!Описание контента' COMMENT 'Описание контента',
  `ID_Data` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на первую версию контента',
  `boolIsActual` BOOL NOT NULL DEFAULT '1' COMMENT 'Актуальный контент или нет',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 10
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_content_type"
--

CREATE TABLE `a_content_type`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strName` VARCHAR(250) NOT NULL DEFAULT '!Наименование' COMMENT 'Наименование направления работ в документе',
  `strCode` VARCHAR(10) NOT NULL DEFAULT '!Код' COMMENT 'Код направления работ в документе',
  `intSort` TINYINT(3) UNSIGNED NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL DEFAULT '0' COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`),
  KEY `intSort` (`intSort`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 57
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_doc"
--

CREATE TABLE `a_doc`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strName` VARCHAR(200) NOT NULL DEFAULT '!!! Наименование документа' COMMENT 'Наименование документа',
  `ID_Section` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на раздел-подразделение (a_Section.ID)',
  `ID_Type` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на тип документации (a_Doc_Type.ID)',
  `strShifr` VARCHAR(100) NOT NULL DEFAULT '!!! Шифр документа' COMMENT 'Шифр документа',
  `ID_Object_Type` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на тип объекта (a_Object_Type.ID)',
  `strName_Work` MEDIUMTEXT NOT NULL COMMENT 'Наименование работ документа',
  `strDescr` MEDIUMTEXT NOT NULL COMMENT 'Дополнительное описание документа',
  `intYear` INT(11) NOT NULL DEFAULT '0' COMMENT 'Год создания документа',
  `ID_Customer` INT(11) NOT NULL DEFAULT '0' COMMENT 'Заказчик. Указатель на организацию (a_Company.ID)',
  `ID_Executor` INT(11) NOT NULL DEFAULT '0' COMMENT 'Разработчик/Исполнитель. Указатель на организацию (a_Company.ID)',
  `strArchived_Human` VARCHAR(200) NOT NULL DEFAULT '!!! Кто сдал в архив' COMMENT 'Человек сдавший документ в архив',
  `strArchived_Data` DATETIME NOT NULL COMMENT 'Дата сдачи документа в архив',
  `sys_UserCreate` VARCHAR(100) NOT NULL COMMENT 'Пользователь создавший запись в БД',
  `sys_DateCreate` DATETIME NOT NULL COMMENT 'Дата создания записи в БД',
  `sys_UserModify` VARCHAR(100) NOT NULL COMMENT 'Пользователь последний изменивший запись в БД',
  `sys_DateModify` DATETIME NOT NULL COMMENT 'Дата последнего изменения записи в БД',
  `ID_Status` INT(11) NOT NULL COMMENT 'Статус оцифровки документа',
  `ID_StoragePlace` INT(11) NOT NULL COMMENT 'Место хранения, место',
  `ID_StorageBuild` INT(11) NOT NULL COMMENT 'Место хранения, здание',
  `ID_Object` INT(11) NOT NULL COMMENT 'Указатель на объект (a_Object.ID)',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 5
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_doc_department"
--

CREATE TABLE `a_doc_department`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `ID_Doc` INT(11) NOT NULL COMMENT 'Указатель на ID документа',
  `ID_Department` INT(11) NOT NULL COMMENT 'Указатель на ID отдела',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 14
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_doc_type"
--

CREATE TABLE `a_doc_type`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strName` VARCHAR(250) NOT NULL DEFAULT '!Наименование' COMMENT 'Наименование типа документа',
  `strCode` VARCHAR(10) NOT NULL DEFAULT '!Код' COMMENT 'Код типа документа',
  `intSort` TINYINT(3) UNSIGNED NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL DEFAULT '0' COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`),
  KEY `intSort` (`intSort`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 3
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_grif"
--

CREATE TABLE `a_grif`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strName` VARCHAR(100) NOT NULL DEFAULT '!Гриф' COMMENT 'Гриф документа *(Общедоступно, секретноб КИ, ПД, КД, ...)',
  `strCode` VARCHAR(10) NOT NULL DEFAULT '!Код' COMMENT 'Код важности документа',
  `boolRead_Everyone` BOOL NOT NULL DEFAULT '0' COMMENT 'Разрешено ли читать документ с этим грифом всем (True-разрешено, False-запрещено)',
  `intSort` TINYINT(3) UNSIGNED NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL DEFAULT '0' COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`),
  KEY `intSort` (`intSort`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 8
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_log"
--

CREATE TABLE `a_log`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `dtAction` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Дата и время совершаемого пользователем действия',
  `strAppName` VARCHAR(250) NOT NULL DEFAULT 'Неизвестно' COMMENT 'Сокр.название приложения + версия',
  `strIP` VARCHAR(100) NOT NULL DEFAULT '0.0.0.0' COMMENT 'IP-адрес пользователя',
  `strUser` VARCHAR(250) NOT NULL DEFAULT 'Неизвестно' COMMENT 'ФИО пользователя',
  `strUserDB` VARCHAR(100) NOT NULL DEFAULT 'Неизвестно' COMMENT 'Пользователь базы данных',
  `strAction` MEDIUMTEXT NOT NULL COMMENT 'Действие совершаемое пользователем',
  `ID_Cont` INT(11) NULL DEFAULT NULL COMMENT 'ID контента к которому запрошен доступ',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 82
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_object"
--

CREATE TABLE `a_object`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `ID_Object_Type` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на тип объекта (sprObject_Type.ID )',
  `strName` VARCHAR(250) NOT NULL DEFAULT '!!!Наименование объекта' COMMENT 'Наименование объекта на котором производятся работы',
  `strNameShort` VARCHAR(100) NOT NULL DEFAULT '!!!' COMMENT 'Сокращенное наименование объекта на котором производятся работы',
  `strInvNum` VARCHAR(20) NOT NULL DEFAULT '#' COMMENT 'Инвентарный номер объекта',
  `intSort` TINYINT(3) UNSIGNED NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL DEFAULT '0' COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`),
  KEY `intSort` (`intSort`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 109
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_object_type"
--

CREATE TABLE `a_object_type`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strName` VARCHAR(250) NOT NULL DEFAULT 'Наименование типа объекта' COMMENT 'Наименование типа объекта',
  `strNameShort` VARCHAR(30) NOT NULL DEFAULT 'Сокр.наим.типа объекта' COMMENT 'Сокращенное наименование типа объекта',
  `intSort` TINYINT(3) UNSIGNED NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL DEFAULT '0' COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`),
  KEY `intSort` (`intSort`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 17
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_params"
--

CREATE TABLE `a_params`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `ID_User` INT(11) NOT NULL COMMENT 'Идентификатор пользователя',
  `intCode` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указаетль на параметр',
  `intValue` INT(11) NOT NULL DEFAULT '0' COMMENT 'Целое значение параметра',
  `flValue` DOUBLE NOT NULL DEFAULT '0' COMMENT 'Вещественное значение параметра',
  `strValue` VARCHAR(250) NOT NULL DEFAULT '!!!' COMMENT 'Строковое значение параметра',
  `strValue2` VARCHAR(250) NOT NULL DEFAULT '!!!',
  `strDescription` VARCHAR(250) NOT NULL DEFAULT '!!!' COMMENT 'Описание',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 170
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_request"
--

CREATE TABLE `a_request`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `ID_User` INT(11) NOT NULL COMMENT 'ID пользователя, запросившего доступ',
  `ID_Content` INT(11) NOT NULL COMMENT 'ID контента к которому запрошен доступ',
  `ID_Archiver` INT(11) NOT NULL DEFAULT '0' COMMENT 'ID архивариуса, обработавшего запрос на доступ к контенту',
  `sys_DateRequest` DATETIME NOT NULL COMMENT 'Дата запроса',
  `sys_DateSolution` DATETIME NULL DEFAULT NULL COMMENT 'Дата принятия решения по запросу',
  `sys_DateStart` DATETIME NULL DEFAULT NULL COMMENT 'С какой даты разрешен доступ к контенту',
  `sys_DateFinish` DATETIME NULL DEFAULT NULL COMMENT 'До какой даты разрешен доступ к контенту',
  `intStatus` INT(11) NOT NULL DEFAULT '0' COMMENT 'Статус запроса: 0 - не обработан, 1 - доступ разрешен, 2 - доступ запрещен',
  `strDescription` VARCHAR(250) NOT NULL DEFAULT '!!!' COMMENT 'Описание',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 104
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_section"
--

CREATE TABLE `a_section`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `PID` INT(11) NOT NULL DEFAULT '0' COMMENT 'Указатель на вышестоящую структуру (a_Section.ID)',
  `strName` VARCHAR(100) NOT NULL DEFAULT '!Наименование' COMMENT 'Наименование раздела',
  `strDescr` MEDIUMTEXT NOT NULL COMMENT 'Описание раздела, доп.информация',
  `intSort` TINYINT(3) NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 7
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_spr_content_carrier"
--

CREATE TABLE `a_spr_content_carrier`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `strName` VARCHAR(50) NOT NULL COMMENT 'Название физического носителя',
  `intSort` TINYINT(3) NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL DEFAULT '1' COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 3
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_spr_department"
--

CREATE TABLE `a_spr_department`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `strName` VARCHAR(250) NOT NULL COMMENT 'Отдел, имеющий отношение к документу',
  `strNameShort` VARCHAR(250) NULL DEFAULT NULL COMMENT 'Сокращенное название отдела',
  `strFullName` MEDIUMTEXT NULL DEFAULT NULL COMMENT 'Полное название отдела',
  `intSort` TINYINT(3) NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 6
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_spr_status_doc"
--

CREATE TABLE `a_spr_status_doc`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `strName` VARCHAR(50) NOT NULL COMMENT 'Статус оцифровки документа',
  `IntSort` TINYINT(4) NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NULL DEFAULT NULL COMMENT 'Видимость в списке выбора',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 4
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_spr_storage_building"
--

CREATE TABLE `a_spr_storage_building`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `strName` VARCHAR(250) NOT NULL COMMENT 'Название места хранения. Здание',
  `intSort` TINYINT(3) NOT NULL COMMENT 'Ключ сортировки',
  `boolVisible` BOOL NOT NULL COMMENT 'Видимость в списке выбора',
  `strNameShort` VARCHAR(250) NULL DEFAULT NULL COMMENT 'Краткое название места хранения',
  `strNameFull` MEDIUMTEXT NULL DEFAULT NULL COMMENT 'Полное название места хранения',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 3
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_spr_storage_place"
--

CREATE TABLE `a_spr_storage_place`(
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `strName` VARCHAR(250) NULL DEFAULT NULL COMMENT 'Название места хранения. Кабинет',
  `intSort` TINYINT(3) NOT NULL DEFAULT '100' COMMENT 'Ключ сортировки',
  `ID_StorageBuild` INT(11) NOT NULL COMMENT 'Указатель на здание, в котором находится кабинет',
  `boolVisible` BOOL NOT NULL DEFAULT '1' COMMENT 'Видимость в списке выбора',
  `strNameShort` VARCHAR(250) NULL DEFAULT NULL COMMENT 'Короткое название места хранения. Кабинет',
  `strFullName` MEDIUMTEXT NULL DEFAULT NULL COMMENT 'Полное название места хранения. Кабинет',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 3
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for table "a_users"
--

CREATE TABLE `a_users`(
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Уникальный идентификатор записи',
  `strLogin` VARCHAR(30) NOT NULL DEFAULT '!Login' COMMENT 'Логин пользователя',
  `strFam` VARCHAR(25) NOT NULL DEFAULT '!Фамилия' COMMENT 'Фамилия пользователя',
  `strName` VARCHAR(25) NOT NULL DEFAULT '!Имя' COMMENT 'Имя пользователя',
  `strOtch` VARCHAR(25) NOT NULL DEFAULT '!Отчество' COMMENT 'Отчество пользователя',
  `intTabNum` INT(11) NOT NULL DEFAULT '0' COMMENT 'Табельный номер пользователя',
  `strPodr` CHAR(150) NOT NULL DEFAULT '!Подразделение' COMMENT 'Подразделение',
  `strDepartment` CHAR(100) NOT NULL DEFAULT '!Отдел' COMMENT 'Отдел/служба',
  `strPost` VARCHAR(100) NOT NULL DEFAULT '!Должность' COMMENT 'Должность пользователя',
  `strPhone` VARCHAR(25) NOT NULL DEFAULT '!Рабочий телефон' COMMENT 'Рабочий телефон пользователя',
  `strEmail` VARCHAR(40) NOT NULL DEFAULT '!Электронная почта' COMMENT 'Адрес электронной почты',
  `strAccess` VARCHAR(100) NOT NULL DEFAULT '0' COMMENT 'Разрешенный доступ на чтение документов. Через запятую список разрешенных грифов документов (a_Doc_Grif.ID)',
  `boolEnable` BOOL NULL DEFAULT '0' COMMENT 'Включить/выключить пользователя',
  `strDescr` MEDIUMTEXT NULL DEFAULT NULL COMMENT 'Дополнительное описание пользователя',
  `sys_UserModify` VARCHAR(30) NOT NULL COMMENT 'Кто внес последние изменения',
  `sys_DateModify` DATETIME NOT NULL COMMENT 'Дата последнего изменения пользователя',
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
AUTO_INCREMENT = 34
COLLATE = utf8_general_ci
ROW_FORMAT = COMPACT;

--
-- Definition for procedure "Add_Log"
--

CREATE PROCEDURE `Add_Log` ( AppName VARCHAR(250), ID_User INT, IP VARCHAR(100), IN MSG MEDIUMTEXT )
BEGIN
/*------------------------------------------------------------------------------------------------------*/
/*  Функция записывает сообщение в таблицу логов                                                        */
/*     AppName   - Сокращенное наименование приложения с версией                                        */
/*     ID_User   - ID пользователя (a_Users.ID)                                                        */
/*     IP        - IP адрес пользователя (REMOTE_ADDR)                                                  */
/*     MSG       - текстовое сообщение                                                                  */
/*------------------------------------------------------------------------------------------------------*/
  DECLARE FIO VARCHAR(100) DEFAULT '';
  DECLARE CountRec INT DEFAULT 0;
  SET AppName = IF( AppName='', 'Неизвестно', AppName );
  SELECT Count(*), CONCAT( strFam, ' ', strName, ' ', strOtch, ', ', strPodr ) INTO CountRec, FIO FROM a_Users WHERE ID=ID_User LIMIT 1;
  SET FIO = IF( CountRec=0, 'Неизвестно', FIO );
  INSERT INTO a_Log( strAppName, strIP, strUser, strUserDB, strAction ) VALUES(  AppName, IP, FIO, USER(), MSG );
END||

--
-- Definition for function "GetContentAction"
--

CREATE FUNCTION `GetContentAction` ( UID int, ID_Cont int, ID_Doc int) RETURNS int(11)
BEGIN
/*------------------------------------------------------------------------------------------------------*/
/*  Функция возвращает код состояния запроса доступа к контенту                                         */
/*     UID        - ID пользователя                                                                     */
/*     ID_Cont    - ID контента                                                                         */
/*     ID_Doc     - ID документа                                                                        */
/*         0      - необработанный запрос                                                               */
/*         1      - доступ разрешен                                                                     */
/*         2      - доступ запрещен                                                                     */
/*------------------------------------------------------------------------------------------------------*/
  DECLARE Result        int       DEFAULT 2;
  DECLARE intID_Section INT       DEFAULT 0;
  DECLARE intID_Grif    INT       DEFAULT 0;
  DECLARE FindGrif      INT       DEFAULT 0;
  DECLARE FindSection   INT       DEFAULT 0;
  DECLARE FindRight     INT       DEFAULT 0;
  DECLARE FindRequest   INT       DEFAULT 0;
/* Получение ID грифа контента*/
  SELECT C.ID_Grif INTO intID_Grif
    FROM a_content C
    WHERE C.ID = ID_Cont
    LIMIT 1;
/* Если гриф контента - общедоступно, то предоставляем доступ, иначе проверяем права дальше*/
  IF intID_Grif = 1 THEN
    SET Result = 1;
  ELSE
/* Проверяем права пользователя*/
/* Получение ID подразделения которому принадлежит документ*/
    SELECT D.ID_Section INTO intID_Section
      FROM a_doc D
      WHERE D.ID = ID_Doc
      LIMIT 1;
/* Узнаем, входит ли гриф контента в список разрешенных пользователю грифов*/
    SELECT FIND_IN_SET( intID_Grif, P.strValue )INTO FindGrif
      FROM a_params P
      WHERE P.ID_User = UID AND P.intCode = 101
      LIMIT 1;
/* Узнаем, входит ли документ который хочет скачать пользователь в список разрешенных для пользователя архивов подразделений */
    SELECT FIND_IN_SET( intID_Section, P.strValue ) INTO FindSection
      FROM a_params P
      WHERE P.ID_User = UID AND P.intCode = 103
      LIMIT 1;
/* Если есть есть права на доступ, то разрешить доступ, иначе проверяем наличие разрешения на доступ от архивариуса*/
    IF FindGrif != 0 AND FindSection != 0 THEN
      SET Result = 1;
    ELSE
/* Поиск актуального разрешения на доступ к контенту*/
      SELECT COUNT(*) INTO FindRight
        FROM a_request R
        WHERE R.ID_User = UID AND R.ID_Content = ID_Cont AND NOW()>R.sys_DateStart AND NOW()<R.sys_DateFinish AND R.intStatus = 1
        LIMIT 1;
/* Если есть актуальное разрешение на доступ от архивариуса, то разрешить доступ, иначе проверяем наличие необработанного запроса на доступ*/
      IF FindRight = 1 THEN
        SET Result = 1;
      ELSE
/* Поиск необработанного запроса на доступ к контенту*/
        SELECT COUNT(*) INTO FindRequest
          FROM a_request R
          WHERE R.ID_User = UID AND R.ID_Content = ID_Cont AND R.intStatus = 0
          LIMIT 1;
/* Если есть необработанный запрос на доступ, то показывать пользоватю пиктограмму "Обработка запроса", иначе вернуть код 2 - доступ запрещен*/
        IF FindRequest = 1 THEN
          SET Result = 0;
        END IF;
      END IF;
    END IF;
  END IF;
  RETURN Result;
END||

--
-- Definition for function "GetFIO"
--

CREATE FUNCTION `GetFIO` ( strFam VARCHAR(30), strName VARCHAR(30), strOtch VARCHAR(30), intFormat TINYINT ) RETURNS varchar(90) CHARSET utf8
BEGIN
/*------------------------------------------------------------------------------------------------------*/
/*  Функция возвращает ФИО согласно указанному формату                                                  */
/*     strFam     - Фамилия                                                                             */
/*     strName    - Имя                                                                                 */
/*     strOtch    - Отчество                                                                            */
/*     intFormat  - Формат возвращаемой строки                                                          */
/*         0      - Фамилия Имя Отчество                                                                */
/*         1      - Имя Отчество Фамилия                                                                */
/*         2      - Фамилия И.О.                                                                        */
/*         3      - И.О. Фамилия                                                                        */
/*------------------------------------------------------------------------------------------------------*/
  DECLARE Result   VARCHAR(90) DEFAULT '';
  CASE intFormat
       WHEN 1 THEN
            SET Result = CONCAT( strName, ' ', strOtch, ' ', strFam  );
       WHEN 2 THEN
            SET Result = CONCAT( strFam, ' ', MID( strName, 1, 1 ), '.', MID( strOtch, 1, 1 ), '.' );
       WHEN 3 THEN
            SET Result = CONCAT( MID( strName, 1, 1 ), '.', MID( strOtch, 1, 1 ), '. ', strFam  );
       ELSE
            SET Result = CONCAT( strFam,  ' ', strName, ' ', strOtch );
  END CASE;
  RETURN Result;
END||

--
-- Definition for function "Get_Name_Department"
--

CREATE FUNCTION `Get_Name_Department` ( intID INT ) RETURNS text CHARSET utf8
BEGIN
/*------------------------------------------------------------------------------------------------------*/
/*  Функция создает строку-список отделов, имеющих отношение к документу                                */
/*     intID     - ID документа                                                                         */
/*  Возврат                                                                                             */
/*     строка-список отделов через запятую                                                              */
/*------------------------------------------------------------------------------------------------------*/
  DECLARE _Name               VARCHAR(250)    DEFAULT '';
  DECLARE IsFirst             BOOL            DEFAULT TRUE;
  DECLARE curEND              INT             DEFAULT 0;
  DECLARE _DepartmentList             TEXT   DEFAULT '';
  /* Создаем курсор для перебора о найденых детях, для создания строки */
  DECLARE Department_List CURSOR FOR SELECT  strName
                                     FROM a_spr_department SD
                                     WHERE SD.ID IN (SELECT DD.Id_Department FROM a_doc_department DD WHERE DD.ID_Doc = intID);
  /* Создаем условие окончания перебора записей */
  DECLARE CONTINUE HANDLER FOR NOT FOUND SET curEND = 1;
  OPEN Department_List;
  /* В цикле читаем данные с курсора */
  RET :  LOOP
            /* Получаем данные из записи */
            FETCH Department_List INTO _Name;
            /* Если сработало условие окончания перебора записей, то выход из цикла */
            IF curEND = 1 THEN
                   LEAVE RET;
            END IF;
            /* Добавляем к строке очередной ID */
            IF IsFirst THEN
                     SET _DepartmentList = _Name;
                     SET IsFirst = FALSE;
                ELSE
                     SET _DepartmentList = CONCAT( _DepartmentList, ', ', _Name );
            END IF;
         /* Конец цикла */
         END LOOP RET;
  /* Закрываем курсор */
  CLOSE Department_List;
  RETURN _DepartmentList;
END||
DELIMITER ;

