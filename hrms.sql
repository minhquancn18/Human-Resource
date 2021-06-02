USE [master]
GO
/****** Object:  Database [hrms]    Script Date: 6/2/2021 9:12:42 PM ******/
CREATE DATABASE [hrms]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'hrms', FILENAME = N'D:\Study\CSDL\MSSQL15.MSSQLSERVER\MSSQL\DATA\hrms.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'hrms_log', FILENAME = N'D:\Study\CSDL\MSSQL15.MSSQLSERVER\MSSQL\DATA\hrms_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [hrms] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [hrms].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [hrms] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [hrms] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [hrms] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [hrms] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [hrms] SET ARITHABORT OFF 
GO
ALTER DATABASE [hrms] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [hrms] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [hrms] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [hrms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [hrms] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [hrms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [hrms] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [hrms] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [hrms] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [hrms] SET  ENABLE_BROKER 
GO
ALTER DATABASE [hrms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [hrms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [hrms] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [hrms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [hrms] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [hrms] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [hrms] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [hrms] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [hrms] SET  MULTI_USER 
GO
ALTER DATABASE [hrms] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [hrms] SET DB_CHAINING OFF 
GO
ALTER DATABASE [hrms] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [hrms] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [hrms] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [hrms] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'hrms', N'ON'
GO
ALTER DATABASE [hrms] SET QUERY_STORE = OFF
GO
USE [hrms]
GO
/****** Object:  Table [dbo].[DELETE]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DELETE](
	[ID_DELETE] [int] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [int] NULL,
	[ISDELETED] [bit] NULL,
	[MONTH] [date] NULL,
	[NOTE] [varchar](50) NULL,
 CONSTRAINT [PK_DELETE] PRIMARY KEY CLUSTERED 
(
	[ID_DELETE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DEPARTMENT]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DEPARTMENT](
	[DEPT_ID] [int] IDENTITY(1,1) NOT NULL,
	[DEPT_NAME] [varchar](200) NULL,
	[ROOM_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[DEPT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMPLOYEE]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPLOYEE](
	[EMPLOYEE_ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_CARD] [int] NULL,
	[NAME] [varchar](100) NULL,
	[AGE] [int] NULL,
	[GENDER] [varchar](20) NULL,
	[PASSWORD] [varchar](20) NULL,
	[ACADEMIC_LEVEL] [varchar](100) NULL,
	[BIRTH_DATE] [date] NULL,
	[BIRTH_PLACE] [varchar](200) NULL,
	[EMAIL] [varchar](40) NULL,
	[PHONE] [varchar](20) NULL,
	[CITIZENSHIP] [varchar](200) NULL,
	[DEPT_ID] [int] NULL,
	[ROLE_ID] [int] NULL,
	[IMAGE] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[EMPLOYEE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RECORD]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RECORD](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [int] NULL,
	[DEPT_ID] [int] NULL,
	[EMPLOYEE_CHANGE_ID] [int] NULL,
	[EMPLOYEE_CHANGE_NAME] [varchar](100) NULL,
	[CHANGE] [varchar](1000) NULL,
	[DATE_CHANGE] [smalldatetime] NULL,
	[MONTH_CHANGE] [date] NULL,
 CONSTRAINT [PK_RECORD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLE]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLE](
	[ROLE_ID] [int] IDENTITY(1,1) NOT NULL,
	[PERMISSION] [int] NULL,
	[ROLE_NAME] [varchar](30) NULL,
	[ROLE_DESC] [text] NULL,
	[DEPT_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ROLE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROOM]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM](
	[ROOM_ID] [int] IDENTITY(1,1) NOT NULL,
	[ROOM_NUMBER] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ROOM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SALARY]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SALARY](
	[SALARY_ID] [int] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [int] NULL,
	[OVERTIME_SALARY] [bigint] NULL,
	[COEFFICIENT] [float] NULL,
	[BONUS] [bigint] NULL,
	[BASIC_WAGE] [bigint] NULL,
	[WELFARE] [bigint] NULL,
	[TAX] [bigint] NULL,
	[SOCIAL_INSURANCE] [bigint] NULL,
	[HEALTH_INSURANCE] [bigint] NULL,
	[DATE_START] [date] NULL,
	[DATE_END] [date] NULL,
	[TOTAL_SALARY] [bigint] NULL,
	[NOTE] [varchar](1000) NULL,
	[MONTH] [date] NULL,
 CONSTRAINT [PK__SALARY__6C935C0614A6D4C5] PRIMARY KEY CLUSTERED 
(
	[SALARY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TIMEKEEPING]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TIMEKEEPING](
	[TIMEKEEPING_ID] [int] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [int] NULL,
	[NUMBER_OF_WORK_DAY] [int] NULL,
	[NUMBER_OF_ABSENT_DAY] [int] NULL,
	[NUMBER_OF_OVERTIME_DAY] [int] NULL,
	[DATE_START] [date] NULL,
	[DATE_END] [date] NULL,
	[MONTH] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[TIMEKEEPING_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TIMEKEEPING_DETAIL]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TIMEKEEPING_DETAIL](
	[TIMEKEEPING_DETAIL_ID] [int] IDENTITY(1,1) NOT NULL,
	[TIMEKEEPING_ID] [int] NULL,
	[CHECK_DATE] [date] NULL,
	[CHECK_TIME] [time](7) NULL,
	[EMPLOYEE_ID] [int] NULL,
	[TIMEKEEPING_DETAIL_TYPE] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TIMEKEEPING_DETAIL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER]    Script Date: 6/2/2021 9:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER](
	[ID_USER] [int] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [int] NULL,
	[USERNAME] [varchar](50) NULL,
	[PASSWORD] [varchar](50) NULL,
 CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED 
(
	[ID_USER] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[DELETE] ON 

INSERT [dbo].[DELETE] ([ID_DELETE], [EMPLOYEE_ID], [ISDELETED], [MONTH], [NOTE]) VALUES (1, 1, 0, NULL, NULL)
INSERT [dbo].[DELETE] ([ID_DELETE], [EMPLOYEE_ID], [ISDELETED], [MONTH], [NOTE]) VALUES (2, 2, 0, NULL, NULL)
INSERT [dbo].[DELETE] ([ID_DELETE], [EMPLOYEE_ID], [ISDELETED], [MONTH], [NOTE]) VALUES (3, 3, 0, NULL, NULL)
INSERT [dbo].[DELETE] ([ID_DELETE], [EMPLOYEE_ID], [ISDELETED], [MONTH], [NOTE]) VALUES (4, 4, 0, NULL, NULL)
INSERT [dbo].[DELETE] ([ID_DELETE], [EMPLOYEE_ID], [ISDELETED], [MONTH], [NOTE]) VALUES (6, 6, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[DELETE] OFF
GO
SET IDENTITY_INSERT [dbo].[DEPARTMENT] ON 

INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (1, N'HUMAN RESOURCE DEPT ', NULL)
INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (2, N'ACOUNTING DEPT', NULL)
INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (3, N'DIRECTOR DEPT', NULL)
INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (4, N'SOFTWARE DEPT', NULL)
INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (5, N'QUALITY MANAGEMENT DEPT', NULL)
INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (6, N'BUSSINESS DEPT', NULL)
INSERT [dbo].[DEPARTMENT] ([DEPT_ID], [DEPT_NAME], [ROOM_ID]) VALUES (7, N'SUPPORT DEPT', NULL)
SET IDENTITY_INSERT [dbo].[DEPARTMENT] OFF
GO
SET IDENTITY_INSERT [dbo].[EMPLOYEE] ON 

INSERT [dbo].[EMPLOYEE] ([EMPLOYEE_ID], [ID_CARD], [NAME], [AGE], [GENDER], [PASSWORD], [ACADEMIC_LEVEL], [BIRTH_DATE], [BIRTH_PLACE], [EMAIL], [PHONE], [CITIZENSHIP], [DEPT_ID], [ROLE_ID], [IMAGE]) VALUES (1, 10001, N'TRAN DUC TAM', 20, N'MALE', N'AB', N'POST DOCTOR', CAST(N'2001-11-25' AS Date), N'BINH DUONG', N'19522131@gm.uit.edu.vn', N'0356178239', N'QUANG NAM', 4, 1, NULL)
INSERT [dbo].[EMPLOYEE] ([EMPLOYEE_ID], [ID_CARD], [NAME], [AGE], [GENDER], [PASSWORD], [ACADEMIC_LEVEL], [BIRTH_DATE], [BIRTH_PLACE], [EMAIL], [PHONE], [CITIZENSHIP], [DEPT_ID], [ROLE_ID], [IMAGE]) VALUES (2, 10002, N'CAO NGUYEN MINH QUAN', 20, N'MALE', N'O', N'BACHELOR', CAST(N'2001-09-18' AS Date), N'BEN TRE', N'19522074@gm.uit.edu.vn', N'0793990898', N'BEN TRE', 2, 1, NULL)
INSERT [dbo].[EMPLOYEE] ([EMPLOYEE_ID], [ID_CARD], [NAME], [AGE], [GENDER], [PASSWORD], [ACADEMIC_LEVEL], [BIRTH_DATE], [BIRTH_PLACE], [EMAIL], [PHONE], [CITIZENSHIP], [DEPT_ID], [ROLE_ID], [IMAGE]) VALUES (3, 10003, N'DANG HAI HOANG SON', 20, N'MALE', N'B', N'MASTER', CAST(N'2001-05-31' AS Date), N'DONG NAI', N'19522166@gm.uit.edu.vn', N'0708677905', N'DONG NAI', 3, 4, NULL)
INSERT [dbo].[EMPLOYEE] ([EMPLOYEE_ID], [ID_CARD], [NAME], [AGE], [GENDER], [PASSWORD], [ACADEMIC_LEVEL], [BIRTH_DATE], [BIRTH_PLACE], [EMAIL], [PHONE], [CITIZENSHIP], [DEPT_ID], [ROLE_ID], [IMAGE]) VALUES (4, 10004, N'HOANG QUOC TRONG', 20, N'MALE', N'A', N'BACHELOR', CAST(N'2001-11-11' AS Date), N'BA RIA - VUNG TAU', N'19622408@gm.uit.edu.vn', N'0797635128', N'QUANG NAM', 1, 1, NULL)
INSERT [dbo].[EMPLOYEE] ([EMPLOYEE_ID], [ID_CARD], [NAME], [AGE], [GENDER], [PASSWORD], [ACADEMIC_LEVEL], [BIRTH_DATE], [BIRTH_PLACE], [EMAIL], [PHONE], [CITIZENSHIP], [DEPT_ID], [ROLE_ID], [IMAGE]) VALUES (6, 23320, N'TEST SALARY 2', 0, NULL, NULL, N'Master', CAST(N'2021-06-02' AS Date), NULL, NULL, NULL, NULL, 2, 1, NULL)
SET IDENTITY_INSERT [dbo].[EMPLOYEE] OFF
GO
SET IDENTITY_INSERT [dbo].[RECORD] ON 

INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (1, 2, 2, 1, N'TRAN DUC TAM', N'SOCIAL INSUARANCE (0 -> 100000)     HEALTH INSUARANCE (0 -> 200000)     ,TOTAL SALARY (0 -> -300000)     ,NOTE ('''' -> ''Update Salary'')     ,', CAST(N'2021-05-30T14:15:00' AS SmallDateTime), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (2, 2, 2, 2, N'CAO NGUYEN MINH QUAN', N'BONUS (370000 -> 3700000)     ,TOTAL SALARY (6660000 -> 9990000)     ,', CAST(N'2021-05-30T14:20:00' AS SmallDateTime), CAST(N'2021-04-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (3, 4, 1, 4, N'HOANG QUOC TRONG', N'BONUS (360000 -> 3600000)     ,TOTAL SALARY (5697000 -> 8937000)     ,', CAST(N'2021-05-30T14:27:00' AS SmallDateTime), CAST(N'2021-04-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (4, 1, 4, 1, N'TRAN DUC TAM', N'BONUS (0 -> 1000000)     ,TOTAL SALARY (-300000 -> 700000)     ,', CAST(N'2021-05-30T14:29:00' AS SmallDateTime), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (5, 4, 1, 1, N'TRAN DUC TAM', N'BONUS (1000000 -> 100000)     ,TOTAL SALARY (700000 -> -200000)     ,', CAST(N'2021-05-30T14:32:00' AS SmallDateTime), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (6, 2, 2, 1, N'TRAN DUC TAM', N'NOTE ('''' -> ''Update Salary'')     ,', CAST(N'2021-05-30T14:37:00' AS SmallDateTime), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (7, 2, 2, 1, N'TRAN DUC TAM', N'NOTE ('''' -> ''UPDATE'')     ,', CAST(N'2021-05-30T14:38:00' AS SmallDateTime), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (8, 4, 1, 0, N'Hoang Quoc Trong', N'Added Name = Hoang Quoc Trong', CAST(N'2021-06-02T16:02:00' AS SmallDateTime), CAST(N'2021-06-01' AS Date))
INSERT [dbo].[RECORD] ([ID], [EMPLOYEE_ID], [DEPT_ID], [EMPLOYEE_CHANGE_ID], [EMPLOYEE_CHANGE_NAME], [CHANGE], [DATE_CHANGE], [MONTH_CHANGE]) VALUES (9, 4, 1, 0, N'TEST SALARY 2', N'Added Name = TEST SALARY 2', CAST(N'2021-06-02T16:40:00' AS SmallDateTime), CAST(N'2021-06-01' AS Date))
SET IDENTITY_INSERT [dbo].[RECORD] OFF
GO
SET IDENTITY_INSERT [dbo].[ROLE] ON 

INSERT [dbo].[ROLE] ([ROLE_ID], [PERMISSION], [ROLE_NAME], [ROLE_DESC], [DEPT_ID]) VALUES (1, 2, N'MANAGER', N'Dept Manager', 1)
INSERT [dbo].[ROLE] ([ROLE_ID], [PERMISSION], [ROLE_NAME], [ROLE_DESC], [DEPT_ID]) VALUES (2, 1, N'VICE MANAGER', N'Dept Vice Manager', 1)
INSERT [dbo].[ROLE] ([ROLE_ID], [PERMISSION], [ROLE_NAME], [ROLE_DESC], [DEPT_ID]) VALUES (3, 0, N'EMPLOYEE', N'Dept Employee', 1)
INSERT [dbo].[ROLE] ([ROLE_ID], [PERMISSION], [ROLE_NAME], [ROLE_DESC], [DEPT_ID]) VALUES (4, 2, N'DIRECTOR', N'Director', 2)
INSERT [dbo].[ROLE] ([ROLE_ID], [PERMISSION], [ROLE_NAME], [ROLE_DESC], [DEPT_ID]) VALUES (5, 1, N'VICE DIRECTOR', N'Vice-Director', 2)
SET IDENTITY_INSERT [dbo].[ROLE] OFF
GO
SET IDENTITY_INSERT [dbo].[SALARY] ON 

INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (1, 1, 500000, 3.3, 4500000, 3000000, 2000000, 3000000, 600000, 260000, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), 14900000, N'', CAST(N'2021-02-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (4, 1, 550000, 3.3, 6000000, 3000000, 2000000, 3000000, 500000, 260000, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), 17842000, N'', CAST(N'2021-02-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (6, 1, 600000, 3.3, 5500000, 3000000, 2000000, 3000000, 500000, 260000, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), 17721000, N'', CAST(N'2021-02-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (7, 2, 500000, 2.5, 300000, 2500000, 1500000, 2500000, 500000, 260000, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), 6055000, N'', CAST(N'2021-02-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (8, 2, 550000, 2.5, 350000, 2500000, 1500000, 2500000, 500000, 260000, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), 6490000, N'', CAST(N'2021-03-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (9, 2, 600000, 2.5, 3700000, 2500000, 1500000, 2500000, 500000, 260000, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), 9990000, N'', CAST(N'2021-03-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (10, 3, 500000, 3.5, 700000, 3500000, 2500000, 3500000, 500000, 260000, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), 7167000, N'', CAST(N'2021-03-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (11, 3, 550000, 3.5, 740000, 3500000, 2500000, 3500000, 500000, 260000, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), 8870000, N'', CAST(N'2021-03-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (12, 3, 600000, 3.5, 780000, 3500000, 2500000, 3500000, 500000, 260000, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), 9858000, N'', CAST(N'2021-04-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (13, 4, 500000, 2.5, 300000, 2500000, 1500000, 2500000, 500000, 260000, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), 4585000, N'', CAST(N'2021-04-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (14, 4, 550000, 2.5, 340000, 2500000, 1500000, 2500000, 500000, 260000, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), 5310000, N'', CAST(N'2021-04-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (15, 4, 600000, 2.5, 3600000, 2500000, 1500000, 2500000, 500000, 260000, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), 8937000, N'', CAST(N'2021-04-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (16, 1, 400000, 3.3, 100000, 3000000, 0, 0, 100000, 200000, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), -200000, N'UPDATE', CAST(N'2021-05-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (17, 2, 400000, 2.5, 0, 2500000, 0, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), 0, N'', CAST(N'2021-05-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (18, 3, 400000, 3.5, 0, 3500000, 0, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), 0, N'', CAST(N'2021-05-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (19, 4, 400000, 2.5, 0, 2500000, 0, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), 0, N'', CAST(N'2021-05-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (21, 1, 400000, 3.3, 0, 3000000, 0, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), 0, N'', CAST(N'2021-06-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (22, 2, 400000, 2.5, 0, 2500000, 0, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), 0, N'', CAST(N'2021-06-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (23, 3, 400000, 3.5, 0, 3500000, 0, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), 0, N'', CAST(N'2021-06-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (24, 4, 400000, 2.5, 0, 2500000, 0, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), 0, N'', CAST(N'2021-06-01' AS Date))
INSERT [dbo].[SALARY] ([SALARY_ID], [EMPLOYEE_ID], [OVERTIME_SALARY], [COEFFICIENT], [BONUS], [BASIC_WAGE], [WELFARE], [TAX], [SOCIAL_INSURANCE], [HEALTH_INSURANCE], [DATE_START], [DATE_END], [TOTAL_SALARY], [NOTE], [MONTH]) VALUES (25, 6, 400000, 0.3, 0, 300000, 0, 0, 0, 0, CAST(N'2021-06-02' AS Date), CAST(N'2021-07-01' AS Date), 0, N'FDF', CAST(N'2021-06-01' AS Date))
SET IDENTITY_INSERT [dbo].[SALARY] OFF
GO
SET IDENTITY_INSERT [dbo].[TIMEKEEPING] ON 

INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (1, 1, 17, 0, 4, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), CAST(N'2021-02-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (2, 2, 16, 1, 3, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), CAST(N'2021-02-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (3, 3, 12, 4, 0, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), CAST(N'2021-02-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (4, 4, 15, 2, 1, CAST(N'2021-02-01' AS Date), CAST(N'2021-03-01' AS Date), CAST(N'2021-02-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (5, 1, 27, 0, 5, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), CAST(N'2021-03-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (6, 2, 25, 2, 3, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), CAST(N'2021-03-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (7, 3, 21, 6, 0, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), CAST(N'2021-03-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (8, 4, 23, 4, 2, CAST(N'2021-03-01' AS Date), CAST(N'2021-04-01' AS Date), CAST(N'2021-03-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (9, 1, 24, 0, 5, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), CAST(N'2021-04-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (10, 2, 22, 2, 3, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), CAST(N'2021-04-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (11, 3, 19, 5, 1, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), CAST(N'2021-04-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (12, 4, 21, 3, 2, CAST(N'2021-04-01' AS Date), CAST(N'2021-05-01' AS Date), CAST(N'2021-04-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (14, 1, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (15, 2, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (16, 3, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (17, 4, 0, 0, 0, CAST(N'2021-05-01' AS Date), CAST(N'2021-06-01' AS Date), CAST(N'2021-05-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (18, 1, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), CAST(N'2021-06-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (19, 2, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), CAST(N'2021-06-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (20, 3, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), CAST(N'2021-06-01' AS Date))
INSERT [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID], [EMPLOYEE_ID], [NUMBER_OF_WORK_DAY], [NUMBER_OF_ABSENT_DAY], [NUMBER_OF_OVERTIME_DAY], [DATE_START], [DATE_END], [MONTH]) VALUES (21, 4, 0, 0, 0, CAST(N'2021-06-01' AS Date), CAST(N'2021-06-30' AS Date), CAST(N'2021-06-01' AS Date))
SET IDENTITY_INSERT [dbo].[TIMEKEEPING] OFF
GO
SET IDENTITY_INSERT [dbo].[TIMEKEEPING_DETAIL] ON 

INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (1, 1, CAST(N'2018-01-29' AS Date), CAST(N'12:13:00' AS Time), 1, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (2, 1, CAST(N'2018-02-01' AS Date), CAST(N'10:13:00' AS Time), 1, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (3, 1, CAST(N'2018-02-02' AS Date), CAST(N'11:13:00' AS Time), 1, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (4, 1, CAST(N'2018-02-03' AS Date), CAST(N'01:13:00' AS Time), 1, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (5, 1, CAST(N'2018-01-29' AS Date), CAST(N'12:13:00' AS Time), 2, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (6, 1, CAST(N'2018-02-01' AS Date), CAST(N'10:13:00' AS Time), 2, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (7, 1, CAST(N'2018-02-02' AS Date), CAST(N'11:13:00' AS Time), 2, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (8, 1, CAST(N'2018-02-03' AS Date), CAST(N'01:13:00' AS Time), 2, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (9, 1, CAST(N'2018-01-29' AS Date), CAST(N'12:13:00' AS Time), 3, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (10, 1, CAST(N'2018-02-01' AS Date), CAST(N'10:13:00' AS Time), 3, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (11, 1, CAST(N'2018-02-02' AS Date), CAST(N'11:13:00' AS Time), 3, NULL)
INSERT [dbo].[TIMEKEEPING_DETAIL] ([TIMEKEEPING_DETAIL_ID], [TIMEKEEPING_ID], [CHECK_DATE], [CHECK_TIME], [EMPLOYEE_ID], [TIMEKEEPING_DETAIL_TYPE]) VALUES (12, 1, CAST(N'2018-02-03' AS Date), CAST(N'01:13:00' AS Time), 3, NULL)
SET IDENTITY_INSERT [dbo].[TIMEKEEPING_DETAIL] OFF
GO
SET IDENTITY_INSERT [dbo].[USER] ON 

INSERT [dbo].[USER] ([ID_USER], [EMPLOYEE_ID], [USERNAME], [PASSWORD]) VALUES (1, 1, N'DUCTAM', N'TESTPASS')
INSERT [dbo].[USER] ([ID_USER], [EMPLOYEE_ID], [USERNAME], [PASSWORD]) VALUES (2, 2, N'MINHQUAN', N'TESTPASS2')
INSERT [dbo].[USER] ([ID_USER], [EMPLOYEE_ID], [USERNAME], [PASSWORD]) VALUES (3, 3, N'HOANGSON', N'TESTPASS3')
INSERT [dbo].[USER] ([ID_USER], [EMPLOYEE_ID], [USERNAME], [PASSWORD]) VALUES (4, 4, N'QUOCTRONG', N'TESTPASS4')
INSERT [dbo].[USER] ([ID_USER], [EMPLOYEE_ID], [USERNAME], [PASSWORD]) VALUES (6, 6, N'365899', N'haqh3658HA')
SET IDENTITY_INSERT [dbo].[USER] OFF
GO
/****** Object:  Index [UQ__EMPLOYEE__7A1680B4566F7D14]    Script Date: 6/2/2021 9:12:42 PM ******/
ALTER TABLE [dbo].[EMPLOYEE] ADD UNIQUE NONCLUSTERED 
(
	[ID_CARD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DELETE]  WITH CHECK ADD FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[DEPARTMENT]  WITH CHECK ADD FOREIGN KEY([ROOM_ID])
REFERENCES [dbo].[ROOM] ([ROOM_ID])
GO
ALTER TABLE [dbo].[EMPLOYEE]  WITH CHECK ADD FOREIGN KEY([DEPT_ID])
REFERENCES [dbo].[DEPARTMENT] ([DEPT_ID])
GO
ALTER TABLE [dbo].[EMPLOYEE]  WITH CHECK ADD FOREIGN KEY([ROLE_ID])
REFERENCES [dbo].[ROLE] ([ROLE_ID])
GO
ALTER TABLE [dbo].[RECORD]  WITH CHECK ADD FOREIGN KEY([DEPT_ID])
REFERENCES [dbo].[DEPARTMENT] ([DEPT_ID])
GO
ALTER TABLE [dbo].[RECORD]  WITH CHECK ADD FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[ROLE]  WITH CHECK ADD FOREIGN KEY([DEPT_ID])
REFERENCES [dbo].[DEPARTMENT] ([DEPT_ID])
GO
ALTER TABLE [dbo].[SALARY]  WITH CHECK ADD  CONSTRAINT [FK__SALARY__EMPLOYEE__49C3F6B7] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[SALARY] CHECK CONSTRAINT [FK__SALARY__EMPLOYEE__49C3F6B7]
GO
ALTER TABLE [dbo].[TIMEKEEPING]  WITH CHECK ADD FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[TIMEKEEPING_DETAIL]  WITH CHECK ADD FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[TIMEKEEPING_DETAIL]  WITH CHECK ADD FOREIGN KEY([TIMEKEEPING_ID])
REFERENCES [dbo].[TIMEKEEPING] ([TIMEKEEPING_ID])
GO
ALTER TABLE [dbo].[USER]  WITH CHECK ADD FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE] ([EMPLOYEE_ID])
GO
USE [master]
GO
ALTER DATABASE [hrms] SET  READ_WRITE 
GO
