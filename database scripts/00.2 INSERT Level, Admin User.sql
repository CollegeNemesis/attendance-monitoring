USE AMS

print '##########################################################'
print 'INSERT Level'
print '##########################################################'
select * from Level

insert into Level
select '8a1c7fcc-2dd4-43b1-926d-3483fe3d2225', 'Nursery I',0

insert into Level
select '100fdda6-6946-46c1-8a8b-e0cd4831432b', 'Nursery II',1

insert into Level
select 'ca30c0d8-3fb8-4b67-a1f0-694cc21d4fc4', 'Kinder',2

insert into Level
select '26adef3e-4bb3-464b-b3e2-bd27d6fa3e5b', 'Grade 1',3

insert into Level
select '0d8d831a-ce2f-4b7c-ae3f-79fa55c8e855', 'Grade 2',4

insert into Level
select '3c7357fa-fda7-44b1-982f-b2a9f75158a8', 'Grade 3',5

insert into Level
select '6de09e5f-ee62-4a4b-91b6-db5748870bc7', 'Grade 4',6

insert into Level
select '1636a33d-ccd5-4de6-9026-41d7218cef8d', 'Grade 5',7

insert into Level
select '6b343f75-963c-4c77-91d9-f9245d43bcd6', 'Grade 6',8

insert into Level
select '5d029197-e84b-41b9-a788-93daa244aee3', 'Grade 7',9

insert into Level
select '75571afc-60b2-4685-8cec-66d731b1581f', 'Grade 8',10

insert into Level
select '9aa95d14-07b2-483c-bf17-0f78ec2221ce', 'Grade 9',11

insert into Level
select 'b9934fff-4a77-4172-b391-985a0593dc13', 'Grade 10',12

insert into Level
select 'ea310b87-5347-48a2-9bb9-a0efd09c2a38', 'Grade 11',13

insert into Level
select 'b3a357d6-aca3-4e48-b14a-41c20df6150d', 'Grade 12',14

select * from Level

if @@ERROR <> 0
	raiserror('Error encountered in inserting levels', 20, -1) with log
else
	print 'Successful insert - Level'


print '##########################################################'
print 'INSERT Users'
print '##########################################################'


select * from [User]

insert into [User]
select '872899E9-72CE-4D47-98BB-0DA139DEF833','user1234','QTByOd1Uz66tema0xqJolQ==','User'

insert into [User]
select '4A568C72-7B14-4414-8AB3-99352A976A08','admin123','ahb42+JkDpHkqQSYq/BCAw==','Admin'

select * from [User]

if @@ERROR <> 0
	raiserror('Error encountered in inserting users', 20, -1) with log
else
	print 'Successful insert - Users'
GO

print '##########################################################'
print 'INSERT Reports and Filters'
print '##########################################################'

select * from Reports

insert into Reports values ('Attendance Report','PRC_AttendanceReport_ByGradeAndSection',0)
insert into Reports values ('Attendance Report','PRC_AttendanceReport_ByGrade',1)
insert into Reports values ('Attendance Report','PRC_AttendanceReport_ByFirstName',2)
insert into Reports values ('Attendance Report','PRC_AttendanceReport_ByLastName',3)
insert into Reports values ('Attendance Report','PRC_AttendanceReport_ByStudentID',4)
insert into Reports values ('Absentees Report','PRC_AttendanceReport_Absentees',0)
insert into Reports values ('Consolidated Report','PRC_ConsolidatedReport_ByGradeAndSection',0)
insert into Reports values ('No Time Out Report','PRC_AttendanceReport_NoTimeOut',0)

select * from Reports

go

select * from Filters
insert into Filters values ('By Grade and Section',0)
insert into Filters values ('By Grade',1)
insert into Filters values ('By First Name',2)
insert into Filters values ('By Last Name',3)
insert into Filters values ('By Student ID',4)

select * from Filters

if @@ERROR <> 0
	raiserror('Error encountered in inserting reports and filters', 20, -1) with log
else
	print 'Successful insert - Reports and Filters'
GO

print '##########################################################'
print 'INSERT Calendar entries'
print '##########################################################'


SET NOCOUNT ON 
DECLARE @dt SMALLDATETIME 
SET @dt = '20180101' 
WHILE @dt < '20510101' 
BEGIN 
    INSERT dbo.Calendar(dt) SELECT @dt 
    SET @dt = @dt + 1 
END


UPDATE dbo.Calendar
SET isWeekday = CASE 
		WHEN DATEPART(DW, dt) IN (
				1,
				7
				)
			THEN 0
		ELSE 1
		END,
	isHoliday = 0,
	Y = YEAR(dt),
	FY = YEAR(dt),
	/* 
    -- if our fiscal year 
    -- starts on May 1st: 
 
    FY = CASE  
        WHEN MONTH(dt) < 5 
        THEN YEAR(dt)-1  
        ELSE YEAR(dt) END, 
    */
	Q = CASE 
		WHEN MONTH(dt) <= 3
			THEN 1
		WHEN MONTH(dt) <= 6
			THEN 2
		WHEN MONTH(dt) <= 9
			THEN 3
		ELSE 4
		END,
	M = MONTH(dt),
	D = DAY(dt),
	DW = DATEPART(DW, dt),
	monthname = DATENAME(MONTH, dt),
	dayname = DATENAME(DW, dt),
	W = DATEPART(WK, dt)


if @@ERROR <> 0
	raiserror('Error encountered in inserting calendar entries', 20, -1) with log
else
	print 'Successful insert - Calendar'
GO
