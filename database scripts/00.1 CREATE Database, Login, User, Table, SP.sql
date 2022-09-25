use master

--##########################################################
--CREATE DATABASE
--##########################################################
IF NOT EXISTS (select * from master..sysdatabases WHERE name = 'AMS')
	BEGIN
		CREATE DATABASE [AMS]
		print 'AMS DATABASE has been created'
	END

GO



USE AMS


--##########################################################
--CREATE LOGIN
--##########################################################
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'sjbcs_login')
	BEGIN
		CREATE LOGIN sjbcs_login WITH PASSWORD = 'P@ss00word!'
		print 'sjbcs_login - login has been created'
	END
GO  



--##########################################################
--CREATE USER
--##########################################################
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'sjbcs_login_user')
	BEGIN
		CREATE USER sjbcs_login_user FOR LOGIN sjbcs_login
		print 'sjbcs_login_user - user has been created'
	END		
GO  

sp_addrolemember 'db_owner',  'sjbcs_login_user'
print 'sjbcs_login_user added role as db_owner'


USE [AMS]
GO

print '############################################################'
print 'TABLE CREATION'
print '############################################################'

print 'Creation of Table - Level'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Level] (
	[LevelID] [uniqueidentifier] NOT NULL,
	[GradeLevel] [nchar](10) NOT NULL,
	[LevelOrder] [int] NOT NULL,
	CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED ([LevelID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Level', 20, -1) with log
ELSE
	print 'Successful creation of table - Level' 
GO

print '############################################################'
print 'Creation of Table - Section'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Section] (
	[SectionID] [uniqueidentifier] NOT NULL,
	[SectionName] [nvarchar](15) NULL,
	[LevelID] [uniqueidentifier] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	CONSTRAINT [PK_Section] PRIMARY KEY CLUSTERED ([SectionID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Section', 20, -1) with log
ELSE
	print 'Successful creation of table - Section' 
GO

print '############################################################'
print 'Creation of Table - Student'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student] (
	[StudentGuid] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](25) NOT NULL,
	[MiddleName] [nvarchar](25) NULL,
	[LastName] [nvarchar](25) NOT NULL,
	[BirthDate] [date] NULL,
	[ImageData] [image] NULL,
	[Gender] [nvarchar](50) NULL,
	[Street] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[SectionID] [uniqueidentifier] NOT NULL,
	[LevelID] [uniqueidentifier] NOT NULL,
	[StudentID] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([StudentGuid] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Student', 20, -1) with log
ELSE
	print 'Successful creation of table - Student' 
GO



print '############################################################'
print 'Creation of Table - Attendance'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance] (
	[AttendanceID] [uniqueidentifier] NOT NULL,
	[StudentID] [uniqueidentifier] NOT NULL,
	[TimeIn] [smalldatetime] NOT NULL,
	[TimeInSMSID] [nvarchar](50) NULL,
	[TimeInSMSStatus] [nvarchar](50) NULL,
	[TimeOut] [smalldatetime] NULL,
	[TimeOutSMSID] [nvarchar](50) NULL,
	[TimeOutSMSStatus] [nvarchar](50) NULL,
	[IsLate] [bit] NULL,
	[IsOverstay] [bit] NULL,
	[IsEarlyOut] [bit] NULL CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED ([AttendanceID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Attendance', 20, -1) with log
ELSE
	print 'Successful creation of table - Attendance' 
GO

print '############################################################'
print 'Creation of Table - Biometric'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Biometric] (
	[FingerID] [uniqueidentifier] NOT NULL,
	[FingerPrintTemplate] [varbinary](max) NOT NULL,
	[FingerName] [varchar](50) NOT NULL,
	CONSTRAINT [PK_Biometric] PRIMARY KEY CLUSTERED ([FingerID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Biometric', 20, -1) with log
ELSE
	print 'Successful creation of table - Biometric' 
GO

print '############################################################'
print 'Creation of Table - Contact'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact] (
	[ContactID] [uniqueidentifier] NOT NULL,
	[ContactNumber] [nvarchar](50) NOT NULL,
	[StudentID] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([ContactID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Contact', 20, -1) with log
ELSE
	print 'Successful creation of table - Contact' 
GO

print '############################################################'
print 'Creation of Table - DistributionList'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DistributionList] (
	[DistributionListID] [uniqueidentifier] NOT NULL,
	[DistributionListName] [varchar](50) NOT NULL,
	CONSTRAINT [PK_DistributionList] PRIMARY KEY CLUSTERED ([DistributionListID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - DistributionList', 20, -1) with log
ELSE
	print 'Successful creation of table - DistributionList' 
GO

print '############################################################'
print 'Creation of Table - Organization'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization] (
	[OrganizationID] [uniqueidentifier] NOT NULL,
	[OrganizationName] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([OrganizationID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Organization', 20, -1) with log
ELSE
	print 'Successful creation of table - Organization' 
GO

print '############################################################'
print 'Creation of Table - RelBiometric'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelBiometric] (
	[RelBiometricID] [uniqueidentifier] NOT NULL,
	[FingerID] [uniqueidentifier] NOT NULL,
	[StudentID] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_RelBiometric] PRIMARY KEY CLUSTERED ([RelBiometricID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - RelBiometric', 20, -1) with log
ELSE
	print 'Successful creation of table - RelBiometric' 
GO

print '############################################################'
print 'Creation of Table - RelDistributionList'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelDistributionList] (
	[RelDistributionListID] [uniqueidentifier] NOT NULL,
	[StudentID] [uniqueidentifier] NOT NULL,
	[DistributionListID] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_RelDistributionList] PRIMARY KEY CLUSTERED ([RelDistributionListID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - RelDistributionList', 20, -1) with log
ELSE
	print 'Successful creation of table - RelDistributionList' 
GO

print '############################################################'
print 'Creation of Table - RelOrganization'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelOrganization] (
	[RelOrganizationID] [uniqueidentifier] NOT NULL,
	[StudentID] [uniqueidentifier] NOT NULL,
	[OrganizationID] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_RelOrganization] PRIMARY KEY CLUSTERED ([RelOrganizationID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - RelOrganization', 20, -1) with log
ELSE
	print 'Successful creation of table - RelOrganization' 
GO

print '############################################################'
print 'Creation of Table - Calendar'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Calendar  
(  
    dt SMALLDATETIME NOT NULL 
        PRIMARY KEY CLUSTERED,  
     
    isWeekday BIT, 
    isHoliday BIT,  
    Y SMALLINT,  
    FY SMALLINT,  
    Q TINYINT,  
    M TINYINT,  
    D TINYINT,  
    DW TINYINT, 
    monthname VARCHAR(9), 
    dayname VARCHAR(9), 
    W TINYINT,
	UTCOffset TINYINT NULL 
) 
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Calendar', 20, -1) with log
ELSE
	print 'Successful creation of table - Calendar' 
GO

print '############################################################'
print 'Creation of Table - User'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User] (
	[UserID] [uniqueidentifier] NOT NULL,
	[Username] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[Type] [varchar](max) NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - User', 20, -1) with log
ELSE
	print 'Successful creation of table - User' 
GO

print '############################################################'
print 'Creation of Table - Reports'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports] (
	[ReportName] [nvarchar](100) NOT NULL,
	[Report_SP] [nvarchar](100) NOT NULL,
	[Filter] [int] NULL
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Reports', 20, -1) with log
ELSE
	print 'Successful creation of table - Reports' 
GO

print '############################################################'
print 'Creation of Table - Filters'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Filters] (
	[FilterName] [nvarchar](50) NOT NULL,
	[FilterValue] [int] NOT NULL
	) ON [PRIMARY]
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating table - Filters', 20, -1) with log
ELSE
	print 'Successful creation of table - Filters' 
GO
print '############################################################'
print 'STORED PROCEDURE CREATION'
print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_ByGradeAndSection'

IF OBJECT_ID('PRC_AttendanceReport_ByGradeAndSection', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_ByGradeAndSection
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_ByGradeAndSection

--------------
--PARAMETERS--
--------------
@startDate			date			= NULL,
@endDate			date			= NULL,
@GradeLevel			nchar(20)		= NULL,
@Section			nvarchar(30)	= NULL

AS

DECLARE 
	@defaultTime		nvarchar(100)	= rtrim(ltrim(str('0')) + ':' + rtrim(ltrim((str('0')))));

;
WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
Sec.SectionName "Section",
St.StudentID "Student ID", 
isnull(FORMAT(at.TimeIn,'hh:mm tt'),'--') "Time In",
isnull(FORMAT(at.TimeOut,'hh:mm tt'),'--') "Time Out",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) % 60) AS VARCHAR (2)), 2))
"Standard Hours",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) % 60) AS VARCHAR (2)), 2)
) "Hours Rendered",

(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) % 60) AS VARCHAR (2)), 2)
	) 
END "Late",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)) < 60
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Overstay",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Undertime"

				
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid

where 
1=1 

and St.LevelID = (select LevelID from Level where GradeLevel = @GradeLevel)
and Sec.SectionName = @Section

--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)

order by dr.expected_date, Sec.SectionName, st.LastName, St.FirstName

GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_ByGradeAndSection', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_ByGradeAndSection' 
GO


print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_ByGrade'

IF OBJECT_ID('PRC_AttendanceReport_ByGrade', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_ByGrade
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_ByGrade

--------------
--PARAMETERS--
--------------
@startDate	date		= NULL,
@endDate	date		= NULL,
@GradeLevel	nchar(20)	= NULL

AS

DECLARE 
	@defaultTime		nvarchar(100)	= rtrim(ltrim(str('0')) + ':' + rtrim(ltrim((str('0')))));

;
WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
Sec.SectionName "Section",
St.StudentID "Student ID", 
isnull(FORMAT(at.TimeIn,'hh:mm tt'),'--') "Time In",
isnull(FORMAT(at.TimeOut,'hh:mm tt'),'--') "Time Out",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) % 60) AS VARCHAR (2)), 2))
"Standard Hours",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) % 60) AS VARCHAR (2)), 2)
) "Hours Rendered",

(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) % 60) AS VARCHAR (2)), 2)
	) 
END "Late",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)) < 60
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Overstay",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Undertime"

				
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid


where 
1=1 

and St.LevelID = (select LevelID from Level where GradeLevel = @GradeLevel)

--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)

order by dr.expected_date, Sec.SectionName, st.LastName, St.FirstName
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_ByGrade', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_ByGrade' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_ByFirstName'

IF OBJECT_ID('PRC_AttendanceReport_ByFirstName', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_ByFirstName
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_ByFirstName

--------------
--PARAMETERS--
--------------
@startDate			date			= NULL,
@endDate			date			= NULL,
@FirstName			nvarchar(60)	= NULL

AS

DECLARE 
	@defaultTime		nvarchar(100)	= rtrim(ltrim(str('0')) + ':' + rtrim(ltrim((str('0')))));

;
WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
Sec.SectionName "Section",
St.StudentID "Student ID", 
isnull(FORMAT(at.TimeIn,'hh:mm tt'),'--') "Time In",
isnull(FORMAT(at.TimeOut,'hh:mm tt'),'--') "Time Out",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) % 60) AS VARCHAR (2)), 2))
"Standard Hours",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) % 60) AS VARCHAR (2)), 2)
) "Hours Rendered",

(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) % 60) AS VARCHAR (2)), 2)
	) 
END "Late",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)) < 60
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Overstay",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Undertime"

				
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid


where 
1=1 

and St.FirstName like '%'+@FirstName+'%'
--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)
order by dr.expected_date, Sec.SectionName, st.LastName, St.FirstName
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_ByFirstName', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_ByFirstName' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_ByLastName'

IF OBJECT_ID('PRC_AttendanceReport_ByLastName', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_ByLastName
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_ByLastName

--------------
--PARAMETERS--
--------------
@startDate			date			= NULL,
@endDate			date			= NULL,
@LastName			nvarchar(60)	= NULL

AS

DECLARE 
	@defaultTime		nvarchar(100)	= rtrim(ltrim(str('0')) + ':' + rtrim(ltrim((str('0')))));

;
WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
Sec.SectionName "Section",
St.StudentID "Student ID", 
isnull(FORMAT(at.TimeIn,'hh:mm tt'),'--') "Time In",
isnull(FORMAT(at.TimeOut,'hh:mm tt'),'--') "Time Out",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) % 60) AS VARCHAR (2)), 2))
"Standard Hours",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) % 60) AS VARCHAR (2)), 2)
) "Hours Rendered",

(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) % 60) AS VARCHAR (2)), 2)
	) 
END "Late",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)) < 60
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Overstay",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Undertime"

				
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid


where 
1=1 

and St.LastName like '%'+@LastName+'%'
--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)

order by dr.expected_date, Sec.SectionName, st.LastName, St.FirstName
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_ByLastName', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_ByLastName' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_ByStudentID'

IF OBJECT_ID('PRC_AttendanceReport_ByStudentID', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_ByStudentID
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_ByStudentID

--------------
--PARAMETERS--
--------------
@startDate		date			= NULL,
@endDate		date			= NULL,
@StudentID		nvarchar(60)	= NULL

AS

DECLARE 
	@defaultTime		nvarchar(100)	= rtrim(ltrim(str('0')) + ':' + rtrim(ltrim((str('0')))));

;
WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
Sec.SectionName "Section",
St.StudentID "Student ID", 
isnull(FORMAT(at.TimeIn,'hh:mm tt'),'--') "Time In",
isnull(FORMAT(at.TimeOut,'hh:mm tt'),'--') "Time Out",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((SELECT CONVERT(int, DATEDIFF(MINUTE, StartTime, Endtime)) from Section where SectionID = (select SectionID from Student where StudentID = st.StudentID group by sectionid)), 0) % 60) AS VARCHAR (2)), 2))
"Standard Hours",

(SELECT
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
RIGHT('0' + CAST (FLOOR(COALESCE ((select DATEDIFF(MINUTE, timein, timeout) from attendance aaa where CONVERT(date, aaa.timein) = dr.expected_date and  aaa.StudentID = St.StudentGuid), 0) % 60) AS VARCHAR (2)), 2)
) "Hours Rendered",

(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.StartTime, convert(time,at.TimeIn))),0)), 0) % 60) AS VARCHAR (2)), 2)
	) 
END "Late",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)) < 60
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, Sec.EndTime, convert(time,at.TimeOut))),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Overstay",

CASE 
WHEN (select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)) < 0
THEN '00:00'
ELSE 
	(SELECT
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) / 60) AS VARCHAR (8)), 2) + ':' + 
	RIGHT('0' + CAST (FLOOR(COALESCE ((select isnull(CONVERT(int,DATEDIFF(MINUTE, convert(time,at.TimeOut), Sec.EndTime)),0)), 0) % 60) AS VARCHAR (2)), 2)
	)
END "Undertime"

				
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid

where 
1=1 

and St.StudentID = @StudentID
--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)

order by dr.expected_date, Sec.SectionName, st.LastName, St.FirstName
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_ByStudentID', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_ByStudentID' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_Absentees'

IF OBJECT_ID('PRC_AttendanceReport_Absentees', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_Absentees
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_Absentees

--------------
--PARAMETERS--
--------------
@startDate		date			= NULL,
@endDate		date			= NULL,
@GradeLevel		nchar(20)		= NULL,
@Section		nvarchar(60)	= NULL

AS

DECLARE 
	@defaultTime	nvarchar(100)	= ltrim(str('0') + ' hrs' + str('0') + ' mins');

WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
St.StudentID "Student ID" 
--(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance"
			
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid


where 
1=1 

and St.LevelID = (select LevelID from Level where GradeLevel = @GradeLevel)
and Sec.SectionName = @Section

and 'Absent' = (SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present'))
and convert(date,dr.expected_date) between @startDate and @endDate

--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)

group by st.StudentID, dr.expected_date, st.LastName, st.FirstName

order by st.LastName, st.FirstName, dr.expected_date
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_Absentees', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_Absentees' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_ConsolidatedReport_ByGradeAndSection'

IF OBJECT_ID('PRC_ConsolidatedReport_ByGradeAndSection', 'P') IS NOT NULL
DROP PROC PRC_ConsolidatedReport_ByGradeAndSection
GO

-------------------------------------------------------
CREATE PROCEDURE
PRC_ConsolidatedReport_ByGradeAndSection

------------
--PARAMETERS--
------------
@startDate		date			= NULL,
@endDate		date			= NULL,
@GradeLevel		nchar(20)		= NULL,
@Section		nvarchar(60)	= NULL

AS

DECLARE 
	@defaultTime	nvarchar(100)	= ltrim(str('0') + ' hrs' + str('0') + ' mins'),
	@weekendDays	int = 0;

SET @weekendDays = (select count(*) from Calendar 
					where year(dt) = year(@startDate) and MONTH(dt) = MONTH(@startDate)  and isWeekday = 0)
print @weekendDays
;
WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	) 

select  
--CONCAT(DateName(Month, @startdate) + ' ',YEAR(@startdate)) "Date",
St.StudentID "Student ID", 
St.LastName + ', '+ +St.FirstName "Name", 

SUM(CASE WHEN At.TimeIn is NULL THEN 1 ELSE 0 END) "Absence", --subtract weekends in total, - @weekendDays

(select count(*) from Attendance at2 where IsLate		= 1 and (convert(date,at2.TimeIn) between @startDate and @endDate) and At2.StudentID = (select ss.StudentGuid from Student ss where ss.StudentID = st.StudentID group by ss.StudentGuid)) "Late",
(select count(*) from Attendance at2 where IsOverstay	= 1 and (convert(date,at2.TimeIn) between @startDate and @endDate) and At2.StudentID = (select ss.StudentGuid from Student ss where ss.StudentID = st.StudentID group by ss.StudentGuid)) "Overstay", 
(select count(*) from Attendance at2 where IsEarlyOut	= 1 and (convert(date,at2.TimeIn) between @startDate and @endDate) and At2.StudentID = (select ss.StudentGuid from Student ss where ss.StudentID = st.StudentID group by ss.StudentGuid)) "Undertime",
(select count(*) from Attendance at2 where TimeOut  is NULL and (convert(date,at2.TimeIn) between @startDate and @endDate) and At2.StudentID = (select ss.StudentGuid from Student ss where ss.StudentID = st.StudentID group by ss.StudentGuid)) "No Timeout"		
				
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

left outer join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid

where 
1=1 

and St.LevelID = (select LevelID from Level where GradeLevel = @GradeLevel)
and Sec.SectionName = @Section

and convert(date,dr.expected_date) between @startDate and @endDate

group by st.StudentID, st.LastName, st.FirstName, st.MiddleName --, At.TimeIn, At.TimeOut, Sec.StartTime,Sec.EndTime
having(SUM(CASE WHEN At.TimeIn IS NULL THEN 1 ELSE 0 END) >= 0)

order by st.LastName, st.FirstName
GO


IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_ConsolidatedReport_ByGradeAndSection', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_ConsolidatedReport_ByGradeAndSection' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_AttendanceReport_NoTimeOut'

IF OBJECT_ID('PRC_AttendanceReport_NoTimeOut', 'P') IS NOT NULL
DROP PROC PRC_AttendanceReport_NoTimeOut
GO

---------------------------------------------------------
CREATE PROCEDURE
PRC_AttendanceReport_NoTimeOut

--------------
--PARAMETERS--
--------------
@startDate		date			= NULL,
@endDate		date			= NULL,
@GradeLevel		nchar(20)		= NULL,
@Section		nvarchar(60)	= NULL

AS

DECLARE 
	@defaultTime	nvarchar(100)	= ltrim(str('0') + ' hrs' + str('0') + ' mins');

WITH date_range
AS (
	SELECT dateadd(dd, v.number, @startDate) /*start date*/ AS expected_date
	FROM master..spt_values v
	WHERE type = 'P'
		AND v.number <= datediff(dd, @startDate, @endDate) /*days between start date and today*/
	)


select  
convert(date, dr.expected_date,105) "Date", 
St.LastName + ', '+ +St.FirstName "Name", 
St.StudentID "Student ID" 
--(SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present')) "Attendance"
			
from date_range dr 

inner join Student St
on 1=1

inner join Section Sec
on St.SectionID = sec.SectionID

full join Attendance at
on convert(date, dr.expected_date) = convert(date, at.TimeIn)
and at.StudentID = st.StudentGuid


where 
1=1 

and St.LevelID = (select LevelID from Level where GradeLevel = @GradeLevel)
and Sec.SectionName = @Section

--and 'Absent' = (SELECT IIF(At.TimeIn IS NULL, 'Absent', 'Present'))
and at.TimeIn is not null and at.TimeOut is null
and convert(date,dr.expected_date) between @startDate and @endDate

--exclude weekends
--and dr.expected_date in (select dt from Calendar where isWeekday = 1)

group by st.StudentID, dr.expected_date, st.LastName, st.FirstName

order by st.LastName, st.FirstName, dr.expected_date
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_AttendanceReport_NoTimeOut', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_AttendanceReport_NoTimeOut' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_FiltersList'


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------
CREATE PROCEDURE [dbo].[PRC_FiltersList] 
@ReportType VARCHAR(50) = 'Absentees Report'
AS
SELECT *
FROM Filters
WHERE FilterValue IN (
		SELECT Filter
		FROM Reports
		WHERE ReportName = @ReportType
		)
ORDER BY FilterValue
	--exec PRC_FiltersList 

GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_FiltersList', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_FiltersList' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_GradeList'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------
CREATE PROCEDURE [dbo].[PRC_GradeList]
AS
SELECT *
FROM LEVEL
ORDER BY LevelOrder--convert(INT, substring(gradelevel, 6, len(gradelevel)))
	--exec PRC_GradeList
GO


IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_GradeList', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_GradeList' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_ReportsList'

IF OBJECT_ID('PRC_ReportsList', 'P') IS NOT NULL
	DROP PROCEDURE PRC_ReportsList
GO
---------------------------------------------------------
CREATE PROCEDURE PRC_ReportsList
AS
SELECT *
FROM Reports
ORDER BY 1
	--exec PRC_ReportsList
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_ReportsList', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_ReportsList' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_SectionList'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------
CREATE PROCEDURE [dbo].[PRC_SectionList] @GradeLevel NCHAR(20) = ''
AS
SELECT SectionName
FROM Section
WHERE levelid IN (
		SELECT LevelID
		FROM LEVEL
		WHERE GradeLevel = @GradeLevel
		)
ORDER BY SectionName
	--exec PRC_SectionList 'Grade 2'
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_SectionList', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_SectionList' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_SPName'

IF OBJECT_ID('PRC_SPName', 'P') IS NOT NULL
	DROP PROCEDURE PRC_SPName
GO
---------------------------------------------------------
CREATE PROCEDURE PRC_SPName @ReportType VARCHAR(50) = 'Absentees Report',
	@Filter INT = 0
AS
SELECT *
FROM Reports
WHERE ReportName = @ReportType
	AND Filter = @Filter
	--exec PRC_SPName 'Attendance Report', 2
GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_SPName', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_SPName' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_ViewUsers'

IF OBJECT_ID('PRC_ViewUsers', 'P') IS NOT NULL
	DROP PROCEDURE PRC_ViewUsers
GO
---------------------------------------------------------
CREATE PROCEDURE PRC_ViewUsers 
AS
SELECT Username, Type 'User Type'
FROM [User] order by Type, Username

GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_ViewUsers', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_ViewUsers' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_InsertUser'

IF OBJECT_ID('PRC_InsertUser', 'P') IS NOT NULL
	DROP PROCEDURE PRC_InsertUser
GO
---------------------------------------------------------
CREATE PROCEDURE PRC_InsertUser 
@UserGuid uniqueidentifier = '515ECC07-3110-4ED5-9202-2E2DA1F5BC96',
@UserType varchar(10) = 'Admin',
@Username varchar(30) = 'admin123',
@Password varchar(30) = 'ahb42+JkDpHkqQSYq/BCAw=='

AS

INSERT INTO [User] VALUES (@UserGuid, @Username, @Password, @UserType)

GO


IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_InsertUser', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_InsertUser' 
GO
print '############################################################'
print 'Creation of Stored Procedure - PRC_SelectUser'

IF OBJECT_ID('PRC_SelectUser', 'P') IS NOT NULL
	DROP PROCEDURE PRC_SelectUser
GO
---------------------------------------------------------
CREATE PROCEDURE PRC_SelectUser 
@Username varchar(30) = 'Admin'
AS
SELECT Top 1 *
FROM [User] where Username = @Username

GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_SelectUser', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_SelectUser' 
GO

print '############################################################'
print 'Creation of Stored Procedure - PRC_DeleteUser'

IF OBJECT_ID('PRC_DeleteUser', 'P') IS NOT NULL
	DROP PROCEDURE PRC_DeleteUser
GO
---------------------------------------------------------
CREATE PROCEDURE PRC_DeleteUser 
@Username varchar(30) = 'admin'
AS
DELETE
FROM [User] where Username = @Username

GO

IF @@ERROR <> 0 
	raiserror('Error encountered in creating Stored Procedure - PRC_DeleteUser', 20, -1) with log
ELSE
	print 'Successful creation of Stored Procedure - PRC_DeleteUser' 
GO


print '############################################################'
print 'END CREATION'
print 'START ALTER - CONSTRAINTS'
print '############################################################'


print '############################################################'
print 'LEVEL'
print '############################################################'

ALTER TABLE [dbo].[Section]  WITH CHECK ADD  CONSTRAINT [FK_Section_Level] FOREIGN KEY([LevelID])
REFERENCES [dbo].[Level] ([LevelID])
GO

ALTER TABLE [dbo].[Section] CHECK CONSTRAINT [FK_Section_Level]
GO

print '############################################################'
print 'SECTION - Altered Constraints'
ALTER TABLE [dbo].[Section] CHECK CONSTRAINT [FK_Section_Level]
GO

print '############################################################'
print 'STUDENT - Altered Constraints'
ALTER TABLE [dbo].[Student]
	WITH CHECK ADD CONSTRAINT [FK_Student_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[Level]([LevelID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Level]
GO
ALTER TABLE [dbo].[Student]
	WITH CHECK ADD CONSTRAINT [FK_Student_Section] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[Section]([SectionID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Section]
GO

print '############################################################'
print 'ATTENDANCE - Altered Constraints'
ALTER TABLE [dbo].[Attendance]
	WITH CHECK ADD CONSTRAINT [FK_Attendance_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentGuid])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Student]
GO

print '############################################################'
print 'CONTACT - Altered Constraints'
ALTER TABLE [dbo].[Contact]
	WITH CHECK ADD CONSTRAINT [FK_Contact_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentGuid])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_Student]
GO

print '############################################################'
print 'RELBIOMETRIC - Altered Constraints'
ALTER TABLE [dbo].[RelBiometric]
	WITH CHECK ADD CONSTRAINT [FK_RelBiometric_Biometric] FOREIGN KEY ([FingerID]) REFERENCES [dbo].[Biometric]([FingerID])
GO
ALTER TABLE [dbo].[RelBiometric] CHECK CONSTRAINT [FK_RelBiometric_Biometric]
GO
ALTER TABLE [dbo].[RelBiometric]
	WITH CHECK ADD CONSTRAINT [FK_RelBiometric_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentGuid])
GO
ALTER TABLE [dbo].[RelBiometric] CHECK CONSTRAINT [FK_RelBiometric_Student]
GO

print '############################################################'
print 'RELDISTRIBUTIONLIST - Altered Constraints'
ALTER TABLE [dbo].[RelDistributionList]
	WITH CHECK ADD CONSTRAINT [FK_RelDistributionList_DistributionList] FOREIGN KEY ([DistributionListID]) REFERENCES [dbo].[DistributionList]([DistributionListID])
GO
ALTER TABLE [dbo].[RelDistributionList] CHECK CONSTRAINT [FK_RelDistributionList_DistributionList]
GO
ALTER TABLE [dbo].[RelDistributionList]
	WITH CHECK ADD CONSTRAINT [FK_RelDistributionList_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentGuid])
GO
ALTER TABLE [dbo].[RelDistributionList] CHECK CONSTRAINT [FK_RelDistributionList_Student]
GO

print '############################################################'
print 'RELORGANIZATION - Altered Constraints'
ALTER TABLE [dbo].[RelOrganization]
	WITH CHECK ADD CONSTRAINT [FK_RelOrganization_Organization] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[Organization]([OrganizationID])
GO
ALTER TABLE [dbo].[RelOrganization] CHECK CONSTRAINT [FK_RelOrganization_Organization]
GO
ALTER TABLE [dbo].[RelOrganization]
	WITH CHECK ADD CONSTRAINT [FK_RelOrganization_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentGuid])
GO
ALTER TABLE [dbo].[RelOrganization] CHECK CONSTRAINT [FK_RelOrganization_Student]
GO

print '############################################################'
print 'REPORTS - Altered Constraints'
ALTER TABLE [dbo].[Reports] ADD CONSTRAINT [DF_Reports_ByStudentID] DEFAULT((0))
FOR [Filter]
GO

print '############################################################'
print 'CONTRAINTS ALTER - END'
print '############################################################'
