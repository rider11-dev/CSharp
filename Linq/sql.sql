create database LinqToSql;
go
use LinqToSql;
go

-- drop table DefectUser
create table DefectUser(
	UserID varchar(36) not null primary key,
	UserName varchar(20) not null,
	UserType varchar(20) not null
);
--drop table Project
create table Project(
	ProjID varchar(36) not null primary key,
	ProjName varchar(20) not null
);
create table NotificationSubscription(
	NotifyID varchar(36) not null primary key,
	ProjectID varchar(36) not null,
	Email varchar(50) not null
);
ALTER TABLE [dbo].[NotificationSubscription]  WITH CHECK ADD  CONSTRAINT [FK_NotificationSubscription_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjID])
GO

ALTER TABLE [dbo].[NotificationSubscription] CHECK CONSTRAINT [FK_NotificationSubscription_Project]
GO

--drop table Defect
create table Defect(
	DefectID varchar(36) not null primary key,
	Project varchar(36) not null,
	DefectSummary varchar(100) not null,
	DefectStatus varchar(20) not null,
	CreatedBy varchar(36) not null,
	AssignedTo varchar(36) not null,
	LastModified datetime not null,
	Severity varchar(20) not null
);

ALTER TABLE [dbo].[Defect]  WITH CHECK ADD  CONSTRAINT [FK_Defect_AssignedTo] FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[DefectUser] ([UserID])
GO

ALTER TABLE [dbo].[Defect] CHECK CONSTRAINT [FK_Defect_AssignedTo]
GO

ALTER TABLE [dbo].[Defect]  WITH CHECK ADD  CONSTRAINT [FK_Defect_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[DefectUser] ([UserID])
GO

ALTER TABLE [dbo].[Defect] CHECK CONSTRAINT [FK_Defect_CreatedBy]
GO

ALTER TABLE [dbo].[Defect]  WITH CHECK ADD  CONSTRAINT [FK_Defect_Project] FOREIGN KEY([Project])
REFERENCES [dbo].[Project] ([ProjID])
GO

ALTER TABLE [dbo].[Defect] CHECK CONSTRAINT [FK_Defect_Project]
GO



insert into Project(ProjID,ProjName)values('B2B','电商项目');
insert into Project(ProjID,ProjName)values('ERP','ERP项目');
insert into Project(ProjID,ProjName)values('CRM','客户资源管理项目');

insert into DefectUser(UserID,UserName,UserType)values('zhangsan','张三','Customer');
insert into DefectUser(UserID,UserName,UserType)values('lisi','李四','Developer');
insert into DefectUser(UserID,UserName,UserType)values('sunsheng','孙胜','Developer');
insert into DefectUser(UserID,UserName,UserType)values('wuliang','吴亮','Developer');
insert into DefectUser(UserID,UserName,UserType)values('zhaosi','赵四','Tester');
insert into DefectUser(UserID,UserName,UserType)values('wangwu','王五','Manager');

insert into Defect(DefectID,Project,DefectSummary,DefectStatus,CreatedBy,AssignedTo,LastModified,Severity)
values(NEWID(),'B2B','登录失败','Created','zhaosi','zhangsan',GETDATE(),'Trival');
insert into Defect(DefectID,Project,DefectSummary,DefectStatus,CreatedBy,AssignedTo,LastModified,Severity)
values(NEWID(),'B2B','session失效','Accepted','zhaosi','zhangsan',GETDATE(),'Minor');
insert into Defect(DefectID,Project,DefectSummary,DefectStatus,CreatedBy,AssignedTo,LastModified,Severity)
values(NEWID(),'B2B','注册失败','Fixed','zhaosi','wuliang',GETDATE(),'Major');

insert into Defect(DefectID,Project,DefectSummary,DefectStatus,CreatedBy,AssignedTo,LastModified,Severity)
values(NEWID(),'ERP','ERP登录失败','Created','zhaosi','sunsheng',GETDATE(),'Showstopper');
insert into Defect(DefectID,Project,DefectSummary,DefectStatus,CreatedBy,AssignedTo,LastModified,Severity)
values(NEWID(),'ERP','ERPsession失效','Accepted','zhaosi','sunsheng',GETDATE(),'Minor');
insert into Defect(DefectID,Project,DefectSummary,DefectStatus,CreatedBy,AssignedTo,LastModified,Severity)
values(NEWID(),'ERP','ERP注册失败','Fixed','zhaosi','sunsheng',GETDATE(),'Major');
