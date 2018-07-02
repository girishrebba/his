USE [master]
GO
/****** Object:  Database [HIS]    Script Date: 7/2/2018 4:27:04 PM ******/
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
/****** Object:  Table [dbo].[BloodGroup]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[BrandCategories]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[Brands]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[ConsultationFee]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[ConsultationType]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[InPatientHistory]    Script Date: 7/2/2018 4:27:06 PM ******/
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
 CONSTRAINT [PK_InPatientHistory] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IntakeFrequency]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[MedicineInventory]    Script Date: 7/2/2018 4:27:06 PM ******/
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
	[ExpirDate] [date] NULL,
 CONSTRAINT [PK_MedicineInventory] PRIMARY KEY CLUSTERED 
(
	[MedInventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientMetrics]    Script Date: 7/2/2018 4:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PatientMetrics](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[Height] [decimal](6, 2) NULL,
	[Weight] [decimal](6, 2) NULL,
	[BMI] [decimal](10, 4) NULL,
	[HeartBeat] [int] NULL,
	[BP] [decimal](10, 4) NULL,
	[Temperature] [decimal](10, 4) NULL,
 CONSTRAINT [PK_PatientMetrics] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientPrescription]    Script Date: 7/2/2018 4:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PatientPrescription](
	[PrescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[MedInventoryID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IntakeFrequencyID] [int] NOT NULL,
	[Cooments] [varchar](max) NULL,
 CONSTRAINT [PK_PatientPrescription] PRIMARY KEY CLUSTERED 
(
	[PrescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientRoomAllocation]    Script Date: 7/2/2018 4:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PatientRoomAllocation](
	[AllocationID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[RoomNo] [int] NOT NULL,
	[BrandID] [int] NOT NULL,
 CONSTRAINT [PK_PatientRoomAllocation] PRIMARY KEY CLUSTERED 
(
	[AllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 7/2/2018 4:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Patients](
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
	[PatientType] [int] NULL,
	[PatientHistory] [varchar](max) NULL,
 CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED 
(
	[ENMRNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientVisitHistory]    Script Date: 7/2/2018 4:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PatientVisitHistory](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[DateOfVisit] [date] NOT NULL,
	[ConsultTypeID] [int] NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[FEE] [money] NOT NULL,
	[DiscountPercent] [decimal](6, 2) NULL,
	[PatientType] [int] NULL,
 CONSTRAINT [PK_PatientVisitHistory] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[Specialization]    Script Date: 7/2/2018 4:27:06 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 7/2/2018 4:27:06 PM ******/
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
	[UserStatus] [bit] NULL,
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
/****** Object:  Table [dbo].[UserType]    Script Date: 7/2/2018 4:27:06 PM ******/
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
ALTER TABLE [dbo].[InPatientHistory]  WITH CHECK ADD  CONSTRAINT [FK_IPH_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[Patients] ([ENMRNO])
GO
ALTER TABLE [dbo].[InPatientHistory] CHECK CONSTRAINT [FK_IPH_ENMRNO]
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
ALTER TABLE [dbo].[PatientMetrics]  WITH CHECK ADD  CONSTRAINT [FK_PatientMetrics_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[Patients] ([ENMRNO])
GO
ALTER TABLE [dbo].[PatientMetrics] CHECK CONSTRAINT [FK_PatientMetrics_ENMRNO]
GO
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[Patients] ([ENMRNO])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_ENMRNO]
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
ALTER TABLE [dbo].[PatientRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_PRA_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[Patients] ([ENMRNO])
GO
ALTER TABLE [dbo].[PatientRoomAllocation] CHECK CONSTRAINT [FK_PRA_ENMRNO]
GO
ALTER TABLE [dbo].[PatientRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_PRA_RoomNo] FOREIGN KEY([RoomNo])
REFERENCES [dbo].[Rooms] ([RoomNo])
GO
ALTER TABLE [dbo].[PatientRoomAllocation] CHECK CONSTRAINT [FK_PRA_RoomNo]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_Patients_BloodGroup] FOREIGN KEY([BloodGroupID])
REFERENCES [dbo].[BloodGroup] ([GroupID])
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_Patients_BloodGroup]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_Patients_Doctor] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_Patients_Doctor]
GO
ALTER TABLE [dbo].[PatientVisitHistory]  WITH CHECK ADD  CONSTRAINT [FK_PVH_Consultation_Type] FOREIGN KEY([ConsultTypeID])
REFERENCES [dbo].[ConsultationType] ([ConsultTypeID])
GO
ALTER TABLE [dbo].[PatientVisitHistory] CHECK CONSTRAINT [FK_PVH_Consultation_Type]
GO
ALTER TABLE [dbo].[PatientVisitHistory]  WITH CHECK ADD  CONSTRAINT [FK_PVH_Patient_ENMRNO] FOREIGN KEY([ENMRNO])
REFERENCES [dbo].[Patients] ([ENMRNO])
GO
ALTER TABLE [dbo].[PatientVisitHistory] CHECK CONSTRAINT [FK_PVH_Patient_ENMRNO]
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
