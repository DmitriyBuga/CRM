USE [master]
GO
/****** Object:  Database [MiniCRM]    Script Date: 13.05.2016 16:13:08 ******/
CREATE DATABASE [MiniCRM]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_CRM', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\DB_CRM.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DB_CRM_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\DB_CRM_log.ldf' , SIZE = 2304KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MiniCRM] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MiniCRM].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MiniCRM] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MiniCRM] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MiniCRM] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MiniCRM] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MiniCRM] SET ARITHABORT OFF 
GO
ALTER DATABASE [MiniCRM] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MiniCRM] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MiniCRM] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MiniCRM] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MiniCRM] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MiniCRM] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MiniCRM] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MiniCRM] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MiniCRM] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MiniCRM] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MiniCRM] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MiniCRM] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MiniCRM] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MiniCRM] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MiniCRM] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MiniCRM] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MiniCRM] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MiniCRM] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MiniCRM] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MiniCRM] SET  MULTI_USER 
GO
ALTER DATABASE [MiniCRM] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MiniCRM] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MiniCRM] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MiniCRM] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [MiniCRM]
GO
/****** Object:  StoredProcedure [dbo].[delete_images]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[delete_images]
@ticket_id int
as
BEGIN TRANSACTION 
	delete images from images i1
	inner join (select * from select_tickets(
	@ticket_id)) t1 on i1.ticket_id = t1.id
commit transaction


GO
/****** Object:  StoredProcedure [dbo].[delete_tickets]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[delete_tickets]

@ticket_id int
as

BEGIN TRANSACTION 
	delete tickets from tickets t1
	where t1.id in (select id from dbo.select_tickets(@ticket_id))
commit tran

GO
/****** Object:  Table [dbo].[Attachments]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descr] [nvarchar](max) NULL,
	[file_path] [nvarchar](max) NULL,
	[task_id] [int] NOT NULL,
	[cust_id] [int] NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customers]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[phone] [nvarchar](50) NULL,
	[adress] [nvarchar](200) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Images]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Images](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ticket_id] [int] NOT NULL,
	[ImageData] [varbinary](max) NULL,
	[ImageMimeType] [varchar](50) NULL,
 CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Managers]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Managers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[phone] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[cust_id] [int] NOT NULL,
 CONSTRAINT [PK_Managers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tickets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[department_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[status_id] [int] NOT NULL,
	[email] [nvarchar](50) NULL,
	[subject] [nvarchar](50) NULL,
	[body] [nvarchar](max) NULL,
	[mail_data] [smalldatetime] NULL,
	[reference] [nvarchar](10) NULL,
	[responce] [nvarchar](max) NULL,
	[parent_id] [int] NULL,
	[task_id] [int] NOT NULL,
	[manager_id] [int] NOT NULL,
	[cust_id] [int] NOT NULL,
 CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[login] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[position] [nvarchar](50) NOT NULL,
	[department_id] [int] NULL,
	[role_id] [int] NOT NULL,
	[firstname] [nvarchar](50) NULL,
	[lastname] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[select_tickets]    Script Date: 13.05.2016 16:13:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[select_tickets](@ticket_id integer)
returns table
AS
return
(
	with cte
	as
	(
	select id, parent_id from tickets
	where id = @ticket_id
	union all
	select t1.id, t1.parent_id from tickets t1
	inner join cte t2 on 
	t1.parent_id = t2.id
	)
	select * from cte
)
GO
INSERT [dbo].[Role] ([id], [name]) VALUES (0, N'Admin')
INSERT [dbo].[Role] ([id], [name]) VALUES (1, N'User')
INSERT [dbo].[Role] ([id], [name]) VALUES (2, N'Customer')
SET IDENTITY_INSERT [dbo].[Tasks] ON 

INSERT [dbo].[Tasks] ([id], [name]) VALUES (1, N'Зарплата')
INSERT [dbo].[Tasks] ([id], [name]) VALUES (2, N'Кадры')
INSERT [dbo].[Tasks] ([id], [name]) VALUES (3, N'Бухгалтерия')
SET IDENTITY_INSERT [dbo].[Tasks] OFF
SET IDENTITY_INSERT [dbo].[Tickets] ON 

INSERT [dbo].[Tickets] ([id], [department_id], [user_id], [status_id], [email], [subject], [body], [mail_data], [reference], [responce], [parent_id], [task_id], [manager_id], [cust_id]) VALUES (3, 1, 1, 2, N'aaa@mail.com', N'asdasasdasdasd', N'sdfasdkljfsdlj asfdkasdlasdfjl asdflasdffkajasdflj', NULL, NULL, NULL, NULL, 1, 1, 1)
INSERT [dbo].[Tickets] ([id], [department_id], [user_id], [status_id], [email], [subject], [body], [mail_data], [reference], [responce], [parent_id], [task_id], [manager_id], [cust_id]) VALUES (5, 1, 1, 2, N'aaabbb@mail.com', N'sdffwexxbcxc', N'ew23qrerqw vxczxcvv asdfasdffads', NULL, NULL, NULL, 3, 1, 1, 1)
INSERT [dbo].[Tickets] ([id], [department_id], [user_id], [status_id], [email], [subject], [body], [mail_data], [reference], [responce], [parent_id], [task_id], [manager_id], [cust_id]) VALUES (6, 1, 1, 2, N'aaa@mail.com', N'sdffsxcvxcvxcvxvc', N'sdfdsdfdfsdf', NULL, NULL, NULL, 5, 1, 1, 1)
SET IDENTITY_INSERT [dbo].[Tickets] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [name], [login], [password], [position], [department_id], [role_id], [firstname], [lastname]) VALUES (0, N'sa', N'sa', N'123qwe', N'', NULL, 0, NULL, NULL)
INSERT [dbo].[Users] ([id], [name], [login], [password], [position], [department_id], [role_id], [firstname], [lastname]) VALUES (1, N'user1', N'saUser', N'123qwe', N'0', NULL, 1, NULL, NULL)
INSERT [dbo].[Users] ([id], [name], [login], [password], [position], [department_id], [role_id], [firstname], [lastname]) VALUES (2, N'sa11', N'sa11', N'123qwe', N'', NULL, 2, NULL, NULL)
INSERT [dbo].[Users] ([id], [name], [login], [password], [position], [department_id], [role_id], [firstname], [lastname]) VALUES (3, N'bbb', N'bbb', N'123qwe', N'', NULL, 2, NULL, NULL)
INSERT [dbo].[Users] ([id], [name], [login], [password], [position], [department_id], [role_id], [firstname], [lastname]) VALUES (4, N'user', N'user', N'123qwe', N'', NULL, 2, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
USE [master]
GO
ALTER DATABASE [MiniCRM] SET  READ_WRITE 
GO
