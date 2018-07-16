USE [master]
GO
/****** Object:  Database [HIS]    Script Date: 7/16/2018 10:19:03 PM ******/
CREATE DATABASE [HIS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HIS', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS2014\MSSQL\DATA\HIS.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HIS_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS2014\MSSQL\DATA\HIS_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HIS] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HIS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HIS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HIS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HIS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HIS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HIS] SET ARITHABORT OFF 
GO
ALTER DATABASE [HIS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HIS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HIS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HIS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HIS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HIS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HIS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HIS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HIS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HIS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HIS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HIS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HIS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HIS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HIS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HIS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HIS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HIS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HIS] SET  MULTI_USER 
GO
ALTER DATABASE [HIS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HIS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HIS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HIS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [HIS] SET DELAYED_DURABILITY = DISABLED 
GO
USE [HIS]
GO
/****** Object:  Table [dbo].[BloodGroup]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BloodGroup](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_BloodGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BrandCategories]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[BrandCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [varchar](100) NOT NULL,
	[BrandID] [int] NOT NULL,
 CONSTRAINT [PK_BrandCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Brands](
	[BrandID] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Brands] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConsultationFee]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultationFee](
	[ConsultationID] [int] IDENTITY(1,1) NOT NULL,
	[DoctorID] [int] NOT NULL,
	[ConsultTypeID] [int] NOT NULL,
	[Fee] [money] NULL,
 CONSTRAINT [PK_ConsultationFee] PRIMARY KEY CLUSTERED 
(
	[ConsultationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ConsultationType]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConsultationType](
	[ConsultTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ConsultType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ConsultationType] PRIMARY KEY CLUSTERED 
(
	[ConsultTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FeeCollection]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FeeCollection](
	[FeeID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[Amount] [money] NOT NULL,
	[PaidOn] [datetime] NOT NULL,
	[Purpose] [varchar](300) NULL,
	[PaymentMode] [varchar](300) NULL,
 CONSTRAINT [PK_FeeCollection] PRIMARY KEY CLUSTERED 
(
	[FeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InPatientHistory]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[InPatientHistory](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[Observations] [varchar](max) NULL,
	[ObservationDate] [date] NULL,
	[DoctorID] [int] NOT NULL,
 CONSTRAINT [PK_InPatientHistory] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InPatients]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InPatients](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](30) NULL,
	[LastName] [varchar](50) NULL,
	[Gender] [int] NULL,
	[DOB] [date] NULL,
	[BirthPlace] [varchar](50) NULL,
	[Profession] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](10) NULL,
	[BloodGroupID] [int] NOT NULL,
	[MaritalStatus] [int] NULL,
	[ReferredBy] [varchar](50) NULL,
	[RefPhone] [varchar](10) NULL,
	[Address1] [varchar](max) NULL,
	[Address2] [varchar](max) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[PinCode] [varchar](6) NULL,
	[Enrolled] [datetime] NULL,
	[Purpose] [varchar](100) NULL,
	[DoctorID] [int] NOT NULL,
	[Mask] [bit] NULL,
	[PatientHistory] [varchar](max) NULL,
	[Height] [decimal](6, 2) NULL,
	[Weight] [decimal](6, 2) NULL,
	[BMI] [decimal](10, 4) NULL,
	[HeartBeat] [int] NULL,
	[BP] [decimal](10, 4) NULL,
	[Temperature] [decimal](10, 4) NULL,
 CONSTRAINT [PK_InPatients] PRIMARY KEY CLUSTERED 
(
	[ENMRNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IntakeFrequency]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[IntakeFrequency](
	[FrequencyID] [int] IDENTITY(1,1) NOT NULL,
	[Frequency] [varchar](100) NOT NULL,
 CONSTRAINT [PK_IntakeFrequency] PRIMARY KEY CLUSTERED 
(
	[FrequencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MedicineInventory]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[MedicineInventory](
	[MedInventoryID] [int] IDENTITY(1,1) NOT NULL,
	[BrandID] [int] NOT NULL,
	[BrandCategoryID] [int] NOT NULL,
	[MedicineName] [varchar](100) NOT NULL,
	[AvailableQty] [int] NOT NULL,
	[PricePerItem] [decimal](6, 2) NULL,
	[PricePerSheet] [decimal](9, 2) NULL,
	[BatchNo] [varchar](30) NOT NULL,
	[LotNo] [varchar](30) NOT NULL,
	[ExpiryDate] [date] NULL,
 CONSTRAINT [PK_MedicineInventory] PRIMARY KEY CLUSTERED 
(
	[MedInventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OutPatients]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OutPatients](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](30) NULL,
	[LastName] [varchar](50) NULL,
	[Gender] [int] NULL,
	[DOB] [date] NULL,
	[BirthPlace] [varchar](50) NULL,
	[Profession] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](10) NULL,
	[BloodGroupID] [int] NOT NULL,
	[MaritalStatus] [int] NULL,
	[ReferredBy] [varchar](50) NULL,
	[RefPhone] [varchar](10) NULL,
	[Address1] [varchar](max) NULL,
	[Address2] [varchar](max) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[PinCode] [varchar](6) NULL,
	[Enrolled] [datetime] NULL,
	[Purpose] [varchar](100) NULL,
	[DoctorID] [int] NOT NULL,
	[Mask] [bit] NULL,
	[PatientHistory] [varchar](max) NULL,
	[Status] [bit] NULL,
	[Height] [decimal](6, 2) NULL,
	[Weight] [decimal](6, 2) NULL,
	[BMI] [decimal](10, 4) NULL,
	[HeartBeat] [int] NULL,
	[BP] [decimal](10, 4) NULL,
	[Temperature] [decimal](10, 4) NULL,
 CONSTRAINT [PK_OutPatients] PRIMARY KEY CLUSTERED 
(
	[ENMRNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientPrescription]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientPrescription](
	[PrescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[MedInventoryID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IntakeFrequencyID] [int] NOT NULL,
	[Comments] [varchar](max) NULL,
	[PrescribedBy] [int] NOT NULL,
 CONSTRAINT [PK_PatientPrescription] PRIMARY KEY CLUSTERED 
(
	[PrescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientRoomAllocation]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientRoomAllocation](
	[AllocationID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[RoomNo] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[DischargeSummary] [varchar](max) NULL,
 CONSTRAINT [PK_PatientRoomAllocation] PRIMARY KEY CLUSTERED 
(
	[AllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientVisitHistory]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientVisitHistory](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[DateOfVisit] [date] NOT NULL,
	[ConsultTypeID] [int] NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[FEE] [money] NOT NULL,
	[Discount] [money] NULL,
	[DoctorID] [int] NOT NULL,
 CONSTRAINT [PK_PatientVisitHistory] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rooms](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[RoomNo] [int] NOT NULL,
	[RoomType] [varchar](20) NOT NULL,
	[Description] [varchar](max) NULL,
	[CostPerDay] [int] NOT NULL,
	[RoomStatus] [int] NULL,
	[NextAvailbility] [date] NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[RoomNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Specialization]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Specialization](
	[SpecializationID] [int] IDENTITY(1,1) NOT NULL,
	[DoctorType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Specialization] PRIMARY KEY CLUSTERED 
(
	[SpecializationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](30) NULL,
	[LastName] [varchar](50) NULL,
	[Gender] [int] NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[DOB] [date] NULL,
	[Phone] [varchar](10) NULL,
	[MaritalStatus] [int] NULL,
	[Qualification] [varchar](20) NULL,
	[UserStatus] [bit] NULL CONSTRAINT [DF_Users_UserStatus]  DEFAULT ((1)),
	[SpecializationID] [int] NOT NULL,
	[UserTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 7/16/2018 10:19:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserType](
	[UserTypeID] [int] IDENTITY(1,1) NOT NULL,
	[UserTypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[BloodGroup] ON 

INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (1, N'A +ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (2, N'A -ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (3, N'B +ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (4, N'B -ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (5, N'O -ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (6, N'O +ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (7, N'AB +ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (9, N'AB -ve')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (10, N'text')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (11, N'text 2')
INSERT [dbo].[BloodGroup] ([GroupID], [GroupName]) VALUES (12, N'text 3')
SET IDENTITY_INSERT [dbo].[BloodGroup] OFF
SET IDENTITY_INSERT [dbo].[BrandCategories] ON 

INSERT [dbo].[BrandCategories] ([CategoryID], [Category], [BrandID]) VALUES (3, N'Folicules', 1)
INSERT [dbo].[BrandCategories] ([CategoryID], [Category], [BrandID]) VALUES (4, N'Test', 3)
INSERT [dbo].[BrandCategories] ([CategoryID], [Category], [BrandID]) VALUES (5, N'Anacin', 2)
SET IDENTITY_INSERT [dbo].[BrandCategories] OFF
SET IDENTITY_INSERT [dbo].[Brands] ON 

INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (1, N'Abott')
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (2, N'A Cold')
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (3, N'A B Z (400mg)')
SET IDENTITY_INSERT [dbo].[Brands] OFF
SET IDENTITY_INSERT [dbo].[ConsultationFee] ON 

INSERT [dbo].[ConsultationFee] ([ConsultationID], [DoctorID], [ConsultTypeID], [Fee]) VALUES (1, 1, 1, 600.0000)
INSERT [dbo].[ConsultationFee] ([ConsultationID], [DoctorID], [ConsultTypeID], [Fee]) VALUES (2, 1, 2, 300.0000)
SET IDENTITY_INSERT [dbo].[ConsultationFee] OFF
SET IDENTITY_INSERT [dbo].[ConsultationType] ON 

INSERT [dbo].[ConsultationType] ([ConsultTypeID], [ConsultType]) VALUES (1, N'New Visit')
INSERT [dbo].[ConsultationType] ([ConsultTypeID], [ConsultType]) VALUES (2, N'Review Visit')
SET IDENTITY_INSERT [dbo].[ConsultationType] OFF
SET IDENTITY_INSERT [dbo].[InPatients] ON 

INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (1, N'101-00001', N'00001', NULL, N'Patient1', 0, CAST(N'1985-06-11' AS Date), N'Hyderabad', NULL, NULL, N'2323235626', 1, NULL, NULL, N'2312256458', N'Address1', NULL, NULL, NULL, N'123456', CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (3, N'101-00002', N'00002', NULL, N'Patient2', NULL, CAST(N'1989-06-14' AS Date), NULL, NULL, NULL, N'9696969696', 2, NULL, NULL, NULL, N'Address1', N'Address2', NULL, NULL, N'500072', CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (5, N'101-00003', N'00003', NULL, N'Patient 3', 1, CAST(N'1980-07-09' AS Date), NULL, NULL, NULL, N'2323235626', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (7, N'101-00004', N'00004', NULL, N'Patient4', 0, CAST(N'1980-06-10' AS Date), NULL, NULL, NULL, N'9696969696', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (9, N'101-00005', N'00005', NULL, N'Patient5', 0, CAST(N'1990-02-14' AS Date), NULL, NULL, NULL, N'2323235626', 4, 0, NULL, NULL, N'Address1', NULL, NULL, NULL, N'562265', CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (11, N'101-00006', N'00006', NULL, N'Patient6', 0, CAST(N'2018-07-14' AS Date), NULL, NULL, NULL, N'9696969696', 4, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (13, N'101-00007', N'00007', NULL, N'Patient7', 0, NULL, NULL, NULL, NULL, N'9696969696', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (15, N'101-00008', N'00008', NULL, N'Patient8', 1, NULL, NULL, NULL, NULL, N'9696969696', 6, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (16, N'101-00009', N'00009', NULL, N'Patient9', 0, NULL, NULL, NULL, NULL, N'5898988989', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (17, N'101-00010', N'00010', NULL, N'Patient10', NULL, NULL, NULL, NULL, NULL, N'2323235626', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-13 00:00:00.000' AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (19, N'101-00011', N'00011', NULL, N'Patient11', 1, NULL, NULL, NULL, NULL, N'2323235626', 2, NULL, NULL, NULL, N'Address11', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (21, N'101-00012', N'00012', NULL, N'Patient12', NULL, NULL, NULL, NULL, NULL, N'9696969696', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (22, N'101-00013', N'00013', NULL, N'Patient13', NULL, NULL, NULL, NULL, NULL, N'9696969696', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-14 00:00:00.000' AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (23, N'101-00014', N'00014', NULL, N'Patient14', NULL, NULL, NULL, NULL, NULL, N'2323235626', 3, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-15 00:00:00.000' AS DateTime), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (24, N'101-00015', N'00015', NULL, N'Patient 15', 1, NULL, NULL, NULL, NULL, N'9696969696', 2, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-15 00:00:00.000' AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (25, N'101-00016', N'00016', NULL, N'Patient 16', 1, NULL, NULL, NULL, NULL, N'9696969696', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-07-15 00:00:00.000' AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[InPatients] OFF
SET IDENTITY_INSERT [dbo].[IntakeFrequency] ON 

INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (1, N'1 Table Spoon')
INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (2, N'2 Table Spoons')
INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (3, N'Before Breakfast')
SET IDENTITY_INSERT [dbo].[IntakeFrequency] OFF
SET IDENTITY_INSERT [dbo].[MedicineInventory] ON 

INSERT [dbo].[MedicineInventory] ([MedInventoryID], [BrandID], [BrandCategoryID], [MedicineName], [AvailableQty], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate]) VALUES (1, 1, 3, N'Cipla', 100, CAST(6.00 AS Decimal(6, 2)), CAST(120.00 AS Decimal(9, 2)), N'ABC', N'1', CAST(N'2019-01-31' AS Date))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [BrandID], [BrandCategoryID], [MedicineName], [AvailableQty], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate]) VALUES (2, 2, 5, N'Crocin', 100, CAST(1.00 AS Decimal(6, 2)), CAST(45.00 AS Decimal(9, 2)), N'XYZ', N'123', CAST(N'2019-12-31' AS Date))
SET IDENTITY_INSERT [dbo].[MedicineInventory] OFF
SET IDENTITY_INSERT [dbo].[OutPatients] ON 

INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (1, N'101-00017', N'00017', N'M', N'Patient17', 1, CAST(N'1980-07-24' AS Date), N'Hyderabad', N'Software Engineer', N'abc@test.com', N'5898988989', 3, 0, N'Someone', N'2312256458', N'Address1', N'Address2', N'City', N'State', N'562265', CAST(N'2018-07-15 00:00:00.000' AS DateTime), N'Fever', 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[OutPatients] OFF
SET IDENTITY_INSERT [dbo].[PatientVisitHistory] ON 

INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [FEE], [Discount], [DoctorID]) VALUES (3, CAST(N'2018-07-15' AS Date), 1, N'101-00017', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [FEE], [Discount], [DoctorID]) VALUES (5, CAST(N'2018-07-16' AS Date), 2, N'101-00017', 150.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [FEE], [Discount], [DoctorID]) VALUES (6, CAST(N'2018-07-16' AS Date), 2, N'101-00017', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [FEE], [Discount], [DoctorID]) VALUES (7, CAST(N'2018-07-16' AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [FEE], [Discount], [DoctorID]) VALUES (8, CAST(N'2018-07-17' AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [FEE], [Discount], [DoctorID]) VALUES (9, CAST(N'2018-07-19' AS Date), 2, N'101-00017', 100.0000, 200.0000, 1)
SET IDENTITY_INSERT [dbo].[PatientVisitHistory] OFF
SET IDENTITY_INSERT [dbo].[Specialization] ON 

INSERT [dbo].[Specialization] ([SpecializationID], [DoctorType]) VALUES (1, N'Neurologist')
INSERT [dbo].[Specialization] ([SpecializationID], [DoctorType]) VALUES (2, N'Radiologist')
SET IDENTITY_INSERT [dbo].[Specialization] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1, N'Girish', N'Kumar', N'Rebba', 0, N'grebba', N'His@123', N'girishrebba@gmail.com', CAST(N'1964-06-16' AS Date), N'8095178654', 1, N'MBBS', 0, 1, 1)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (2, N'Satish', NULL, N'R', 0, N'sareddi', N'His@123', N'satishreddi9@gmail.com', CAST(N'1988-04-04' AS Date), N'8095178654', 1, N'MBBS', 1, 1, 1)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1002, N'Abc', NULL, N'Test', 0, N'abctest', N'His@123', N'abc@test.com', CAST(N'1988-04-28' AS Date), N'9696969696', 0, N'MBBS', 0, 2, 2)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1003, N'Test', N'A', N'User', 1, N'testuser', N'His@123', N'testuser@test.com', CAST(N'1978-06-28' AS Date), N'9696969696', 0, N'MBBS DGO', 0, 2, 2)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1004, N'Admin', N'Office', N'User', 0, N'adminuser', N'His@123', N'adminuser@test.com', CAST(N'1980-10-21' AS Date), N'9696969696', 1, N'MBBS', 1, 1, 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[UserType] ON 

INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (1, N'Doctor')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (2, N'Nurse')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (4, N'Admin')
SET IDENTITY_INSERT [dbo].[UserType] OFF
ALTER TABLE [dbo].[BrandCategories]  WITH CHECK ADD  CONSTRAINT [FK_BrandCategories_BrandID] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([BrandID])
GO
ALTER TABLE [dbo].[BrandCategories] CHECK CONSTRAINT [FK_BrandCategories_BrandID]
GO
ALTER TABLE [dbo].[ConsultationFee]  WITH CHECK ADD  CONSTRAINT [FK_Consultation_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ConsultationFee] CHECK CONSTRAINT [FK_Consultation_Doctor]
GO
ALTER TABLE [dbo].[ConsultationFee]  WITH CHECK ADD  CONSTRAINT [FK_Consultation_Type] FOREIGN KEY([ConsultTypeID])
REFERENCES [dbo].[ConsultationType] ([ConsultTypeID])
GO
ALTER TABLE [dbo].[ConsultationFee] CHECK CONSTRAINT [FK_Consultation_Type]
GO
ALTER TABLE [dbo].[FeeCollection]  WITH CHECK ADD  CONSTRAINT [FK_FeeCollection_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[InPatients] ([ENMRNO])
GO
ALTER TABLE [dbo].[FeeCollection] CHECK CONSTRAINT [FK_FeeCollection_ENMRNO]
GO
ALTER TABLE [dbo].[InPatientHistory]  WITH CHECK ADD  CONSTRAINT [FK_InPatientHistory_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[InPatientHistory] CHECK CONSTRAINT [FK_InPatientHistory_Doctor]
GO
ALTER TABLE [dbo].[InPatientHistory]  WITH CHECK ADD  CONSTRAINT [FK_IPH_InPatients_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[InPatients] ([ENMRNO])
GO
ALTER TABLE [dbo].[InPatientHistory] CHECK CONSTRAINT [FK_IPH_InPatients_ENMRNO]
GO
ALTER TABLE [dbo].[InPatients]  WITH CHECK ADD  CONSTRAINT [FK_InPatients_BloodGroup] FOREIGN KEY([BloodGroupID])
REFERENCES [dbo].[BloodGroup] ([GroupID])
GO
ALTER TABLE [dbo].[InPatients] CHECK CONSTRAINT [FK_InPatients_BloodGroup]
GO
ALTER TABLE [dbo].[InPatients]  WITH CHECK ADD  CONSTRAINT [FK_InPatients_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[InPatients] CHECK CONSTRAINT [FK_InPatients_Doctor]
GO
ALTER TABLE [dbo].[MedicineInventory]  WITH CHECK ADD  CONSTRAINT [FK_MI_BrandID] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([BrandID])
GO
ALTER TABLE [dbo].[MedicineInventory] CHECK CONSTRAINT [FK_MI_BrandID]
GO
ALTER TABLE [dbo].[MedicineInventory]  WITH CHECK ADD  CONSTRAINT [FK_MI_CategoryID] FOREIGN KEY([BrandCategoryID])
REFERENCES [dbo].[BrandCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[MedicineInventory] CHECK CONSTRAINT [FK_MI_CategoryID]
GO
ALTER TABLE [dbo].[OutPatients]  WITH CHECK ADD  CONSTRAINT [FK_OutPatients_BloodGroup] FOREIGN KEY([BloodGroupID])
REFERENCES [dbo].[BloodGroup] ([GroupID])
GO
ALTER TABLE [dbo].[OutPatients] CHECK CONSTRAINT [FK_OutPatients_BloodGroup]
GO
ALTER TABLE [dbo].[OutPatients]  WITH CHECK ADD  CONSTRAINT [FK_OutPatients_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[OutPatients] CHECK CONSTRAINT [FK_OutPatients_Doctor]
GO
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_Doctor] FOREIGN KEY([PrescribedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_Doctor]
GO
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_FrequencyID] FOREIGN KEY([IntakeFrequencyID])
REFERENCES [dbo].[IntakeFrequency] ([FrequencyID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_FrequencyID]
GO
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_MedicineName] FOREIGN KEY([MedInventoryID])
REFERENCES [dbo].[MedicineInventory] ([MedInventoryID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_MedicineName]
GO
ALTER TABLE [dbo].[PatientRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_PRA_InPatients_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[InPatients] ([ENMRNO])
GO
ALTER TABLE [dbo].[PatientRoomAllocation] CHECK CONSTRAINT [FK_PRA_InPatients_ENMRNO]
GO
ALTER TABLE [dbo].[PatientRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_PRA_RoomNo] FOREIGN KEY([RoomNo])
REFERENCES [dbo].[Rooms] ([RoomNo])
GO
ALTER TABLE [dbo].[PatientRoomAllocation] CHECK CONSTRAINT [FK_PRA_RoomNo]
GO
ALTER TABLE [dbo].[PatientVisitHistory]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitHistory_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PatientVisitHistory] CHECK CONSTRAINT [FK_PatientVisitHistory_Doctor]
GO
ALTER TABLE [dbo].[PatientVisitHistory]  WITH CHECK ADD  CONSTRAINT [FK_PVH_Consultation_Type] FOREIGN KEY([ConsultTypeID])
REFERENCES [dbo].[ConsultationType] ([ConsultTypeID])
GO
ALTER TABLE [dbo].[PatientVisitHistory] CHECK CONSTRAINT [FK_PVH_Consultation_Type]
GO
ALTER TABLE [dbo].[PatientVisitHistory]  WITH CHECK ADD  CONSTRAINT [FK_PVH_OutPatient_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[OutPatients] ([ENMRNO])
GO
ALTER TABLE [dbo].[PatientVisitHistory] CHECK CONSTRAINT [FK_PVH_OutPatient_ENMRNO]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_User_Specialization] FOREIGN KEY([SpecializationID])
REFERENCES [dbo].[Specialization] ([SpecializationID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_User_Specialization]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_User_Type] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([UserTypeID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_User_Type]
GO
USE [master]
GO
ALTER DATABASE [HIS] SET  READ_WRITE 
GO
