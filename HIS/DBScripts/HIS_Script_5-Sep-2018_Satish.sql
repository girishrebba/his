USE [master]
GO
/****** Object:  Database [HIS]    Script Date: 9/5/2018 11:28:19 PM ******/
CREATE DATABASE [HIS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HIS', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\HIS.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HIS_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\HIS_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HIS] SET COMPATIBILITY_LEVEL = 110
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
ALTER DATABASE [HIS] SET AUTO_CREATE_STATISTICS ON 
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
ALTER DATABASE [HIS] SET RECOVERY FULL 
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
EXEC sys.sp_db_vardecimal_storage_format N'HIS', N'ON'
GO
USE [HIS]
GO
/****** Object:  StoredProcedure [dbo].[ConvertOutPatientToInPatient]    Script Date: 9/5/2018 11:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Satish>
-- Create date: <Aug 02 2018>
-- Description:	<To Convert OutPatient to InPatient>
-- =============================================
CREATE PROCEDURE [dbo].[ConvertOutPatientToInPatient] 
	@ENMRNO VARCHAR(30),
	@EstAmount MONEY,
	@AdvAmount MONEY
AS
BEGIN
	SET NOCOUNT ON;

    -- Copy OP RECORD TO IP Record
	INSERT INTO [dbo].[InPatients]
           ([ENMRNO],[FirstName],[MiddleName],[LastName],[Gender],[DOB],[BirthPlace],[Profession],[Email],[Phone],[BloodGroupID]
           ,[MaritalStatus],[ReferredBy],[RefPhone],[Address1],[Address2],[City],[State],[PinCode],[Enrolled],[Purpose],[DoctorID],
		   [PatientHistory],[Height],[Weight],[BMI],[HeartBeat],[BP],[Temperature], [EstAmoount], [AdvAmount])
	  SELECT [ENMRNO],[FirstName],[MiddleName],[LastName],[Gender],[DOB],[BirthPlace],[Profession],[Email],[Phone],[BloodGroupID]
           ,[MaritalStatus],[ReferredBy],[RefPhone],[Address1],[Address2],[City],[State],[PinCode],[Enrolled],[Purpose],[DoctorID],
		   [PatientHistory],[Height],[Weight],[BMI],[HeartBeat],[BP],[Temperature], @EstAmount, @AdvAmount
    FROM [dbo].[OutPatients] WHERE ENMRNO = @ENMRNO

	-- Make the Out Patient to Inactive
	Update OutPatients SET [Status] = 1 WHERE ENMRNO = @ENMRNO

	-- Insert Fee Collection
	INSERT INTO FeeCollection ([ENMRNO],[Amount],[PaidOn],[Purpose],[PaymentMode])
	VALUES(@ENMRNO, @AdvAmount, Getdate(),'Advance Payment', 1)

END



GO
/****** Object:  StoredProcedure [dbo].[CreateMasterLabTest]    Script Date: 9/5/2018 11:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Satish>
-- Create date: <Aug 28 2018>
-- Description:	<To Create Master Lab>
-- =============================================
CREATE PROCEDURE [dbo].[CreateMasterLabTest] 
	@ENMRNO VARCHAR(30),
	@DoctorID INT,
	@VisitID INT,
	@LTMID int output

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[LabTestMaster]
           ([ENMRNO],[PrescribedBy],[VisitID],[DatePrescribed]) VALUES(@ENMRNO, @DoctorID, @VisitID, GETDATE())
	  
	
	SET @LTMID = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[CreateMasterPrescription]    Script Date: 9/5/2018 11:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Satish>
-- Create date: <Aug 28 2018>
-- Description:	<To Create Master Prescription>
-- =============================================
CREATE PROCEDURE [dbo].[CreateMasterPrescription] 
	@ENMRNO VARCHAR(30),
	@DoctorID INT,
	@VisitID INT,
	@PMID int output

AS
BEGIN
	SET NOCOUNT ON;

    -- Copy OP RECORD TO IP Record
	INSERT INTO [dbo].[PrescriptionMaster]
           ([ENMRNO],[PrescribedBy],[VisitID],[DatePrescribed]) VALUES(@ENMRNO, @DoctorID, @VisitID, GETDATE())
	  
	
	SET @PMID = SCOPE_IDENTITY()

END



GO
/****** Object:  Table [dbo].[Beds]    Script Date: 9/5/2018 11:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Beds](
	[BedNo] [int] IDENTITY(1,1) NOT NULL,
	[BedName] [nvarchar](30) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[BedStatus] [int] NULL,
	[BedTypeID] [int] NOT NULL,
	[RoomNo] [int] NULL,
	[NextAvailbility] [date] NULL,
 CONSTRAINT [PK_Beds] PRIMARY KEY CLUSTERED 
(
	[BedNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BedType]    Script Date: 9/5/2018 11:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BedType](
	[BedTypeID] [int] IDENTITY(1,1) NOT NULL,
	[BedType] [nvarchar](50) NOT NULL,
	[BedTypeDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_BedType] PRIMARY KEY CLUSTERED 
(
	[BedTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BloodGroup]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[BrandCategories]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[Brands]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[ConsultationFee]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[ConsultationType]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[FeeCollection]    Script Date: 9/5/2018 11:28:19 PM ******/
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
	[PaymentMode] [int] NOT NULL,
	[LastFourDigits] [varchar](10) NULL,
 CONSTRAINT [PK_FeeCollection] PRIMARY KEY CLUSTERED 
(
	[FeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InPatientHistory]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[InPatients]    Script Date: 9/5/2018 11:28:19 PM ******/
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
	[OthContact] [varchar](10) NULL,
	[PrevENMR] [varchar](30) NULL,
	[DiscSummary] [varchar](max) NULL,
	[IsDischarged] [bit] NULL,
	[DischargedOn] [datetime] NULL,
	[EstAmoount] [money] NULL,
	[AdvAmount] [money] NULL,
 CONSTRAINT [PK_InPatients] PRIMARY KEY CLUSTERED 
(
	[ENMRNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IntakeFrequency]    Script Date: 9/5/2018 11:28:19 PM ******/
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
/****** Object:  Table [dbo].[LabTestMaster]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[LabTestMaster](
	[LTMID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[PrescribedBy] [int] NOT NULL,
	[DatePrescribed] [date] NULL,
	[VisitID] [int] NULL,
	[IsBillPaid] [bit] NULL,
	[IsDelivered] [bit] NULL,
	[Discount] [decimal](4, 2) NULL,
	[PaidAmount] [money] NULL,
	[TotalAmount] [money] NULL,
 CONSTRAINT [PK_LabTestMaster] PRIMARY KEY CLUSTERED 
(
	[LTMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MedicineInventory]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicineInventory](
	[MedInventoryID] [int] IDENTITY(1,1) NOT NULL,
	[MedicineID] [int] NOT NULL,
	[AvailableQty] [int] NULL,
	[PricePerItem] [decimal](6, 2) NULL,
 CONSTRAINT [PK_MedicineInventory] PRIMARY KEY CLUSTERED 
(
	[MedInventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MedicineMaster]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[MedicineMaster](
	[MMID] [int] IDENTITY(1,1) NOT NULL,
	[BrandID] [int] NOT NULL,
	[BrandCategoryID] [int] NOT NULL,
	[MedicineName] [varchar](100) NOT NULL,
	[MedDose] [varchar](100) NOT NULL,
 CONSTRAINT [PK_MedicineMaster] PRIMARY KEY CLUSTERED 
(
	[MMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OutPatients]    Script Date: 9/5/2018 11:28:20 PM ******/
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
	[OthContact] [varchar](10) NULL,
	[PrevENMR] [varchar](30) NULL,
 CONSTRAINT [PK_OutPatients] PRIMARY KEY CLUSTERED 
(
	[ENMRNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientPrescription]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientPrescription](
	[PTID] [int] IDENTITY(1,1) NOT NULL,
	[PMID] [int] NOT NULL,
	[MedicineID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IntakeFrequencyID] [int] NOT NULL,
	[Comments] [varchar](max) NULL,
	[BatchNo] [varchar](30) NULL,
	[LotNo] [varchar](30) NULL,
	[DeliverQty] [int] NULL,
 CONSTRAINT [PK_PatientPrescription] PRIMARY KEY CLUSTERED 
(
	[PTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientRoomAllocation]    Script Date: 9/5/2018 11:28:20 PM ******/
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
	[BedNo] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[AllocationStatus] [bit] NULL,
 CONSTRAINT [PK_PatientRoomAllocation] PRIMARY KEY CLUSTERED 
(
	[AllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientTests]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientTests](
	[PTID] [int] IDENTITY(1,1) NOT NULL,
	[LTMID] [int] NOT NULL,
	[TestID] [int] NOT NULL,
	[TestDate] [datetime] NULL,
	[RecordedValues] [decimal](4, 2) NULL,
	[TestImpression] [nvarchar](max) NULL,
	[ReportPath] [nvarchar](200) NULL,
	[IsSampleCollected] [bit] NULL,
 CONSTRAINT [PK_PatientTests] PRIMARY KEY CLUSTERED 
(
	[PTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PatientVisitHistory]    Script Date: 9/5/2018 11:28:20 PM ******/
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
	[Fee] [money] NOT NULL,
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
/****** Object:  Table [dbo].[PaymentModes]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentModes](
	[ModeID] [int] IDENTITY(1,1) NOT NULL,
	[Mode] [nvarchar](30) NULL,
 CONSTRAINT [PK_PaymentModes] PRIMARY KEY CLUSTERED 
(
	[ModeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Permission_Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionDescription] [nvarchar](50) NOT NULL,
	[PermissionStatus] [bit] NOT NULL,
	[Toottip] [nvarchar](max) NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Permission_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PrescriptionMaster]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PrescriptionMaster](
	[PMID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[PrescribedBy] [int] NOT NULL,
	[DatePrescribed] [date] NULL,
	[VisitID] [int] NULL,
	[IsDelivered] [bit] NULL,
	[Discount] [decimal](4, 2) NULL,
	[PaidAmount] [money] NULL,
	[TotalAmount] [money] NULL,
 CONSTRAINT [PK_PrescriptionMaster] PRIMARY KEY CLUSTERED 
(
	[PMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[PONumber] [varchar](30) NOT NULL,
	[MedicineID] [int] NOT NULL,
	[OrderedQty] [int] NOT NULL,
	[OrderedDate] [date] NOT NULL,
	[PricePerItem] [decimal](6, 2) NULL,
	[PricePerSheet] [decimal](9, 2) NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[PurchaseOrder] ADD [BatchNo] [varchar](30) NULL
ALTER TABLE [dbo].[PurchaseOrder] ADD [LotNo] [varchar](30) NULL
ALTER TABLE [dbo].[PurchaseOrder] ADD [ExpiryDate] [date] NULL
ALTER TABLE [dbo].[PurchaseOrder] ADD [ApprovedStatus] [bit] NULL
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rooms](
	[RoomNo] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](30) NOT NULL,
	[RoomTypeID] [int] NOT NULL,
	[Description] [varchar](max) NULL,
	[CostPerDay] [int] NOT NULL,
	[RoomStatus] [int] NULL,
	[NextAvailbility] [date] NULL,
	[RoomBedCapacity] [int] NULL,
 CONSTRAINT [PK_Rooms_1] PRIMARY KEY CLUSTERED 
(
	[RoomNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RoomType]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomType](
	[RoomTypeID] [int] IDENTITY(1,1) NOT NULL,
	[RoomType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RoomType] PRIMARY KEY CLUSTERED 
(
	[RoomTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Specialization]    Script Date: 9/5/2018 11:28:20 PM ******/
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
/****** Object:  Table [dbo].[TestTypes]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestTypes](
	[TestID] [int] IDENTITY(1,1) NOT NULL,
	[TestName] [nvarchar](max) NOT NULL,
	[TestCost] [money] NULL,
 CONSTRAINT [PK_TestTypes] PRIMARY KEY CLUSTERED 
(
	[TestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserPermission]    Script Date: 9/5/2018 11:28:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermission](
	[UserTypeID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
 CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC,
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/5/2018 11:28:20 PM ******/
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
/****** Object:  Table [dbo].[UserType]    Script Date: 9/5/2018 11:28:20 PM ******/
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
SET IDENTITY_INSERT [dbo].[Beds] ON 

INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (1, N'101', N'Ordinary Bed', NULL, 1, 1, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (2, N'101', N'bed1', NULL, 0, 1, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (3, N'101', N'bed1', NULL, 1, 1, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (4, N'201', N'bed1', NULL, 2, 2, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (5, N'201', N'bed1', NULL, 2, 2, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (6, N'301', N'bed2', NULL, 1, 2, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (7, N'IC1', N'ICU bed 1', NULL, 2, 3, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (8, N'IC2', N'ICU bed 2', NULL, 2, 3, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (9, N'208', N'delux bed', NULL, 2, 4, NULL)
SET IDENTITY_INSERT [dbo].[Beds] OFF
SET IDENTITY_INSERT [dbo].[BedType] ON 

INSERT [dbo].[BedType] ([BedTypeID], [BedType], [BedTypeDescription]) VALUES (1, N'Normal', N'NormalBed')
INSERT [dbo].[BedType] ([BedTypeID], [BedType], [BedTypeDescription]) VALUES (2, N'Delux', N'DeluxBed')
INSERT [dbo].[BedType] ([BedTypeID], [BedType], [BedTypeDescription]) VALUES (3, N'Luxury', N'LuxuryBed')
INSERT [dbo].[BedType] ([BedTypeID], [BedType], [BedTypeDescription]) VALUES (4, N'Equiped', N'Fully Equiped bed')
SET IDENTITY_INSERT [dbo].[BedType] OFF
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
INSERT [dbo].[ConsultationFee] ([ConsultationID], [DoctorID], [ConsultTypeID], [Fee]) VALUES (3, 1, 3, 0.0000)
SET IDENTITY_INSERT [dbo].[ConsultationFee] OFF
SET IDENTITY_INSERT [dbo].[ConsultationType] ON 

INSERT [dbo].[ConsultationType] ([ConsultTypeID], [ConsultType]) VALUES (1, N'New Visit')
INSERT [dbo].[ConsultationType] ([ConsultTypeID], [ConsultType]) VALUES (2, N'Review Visit')
INSERT [dbo].[ConsultationType] ([ConsultTypeID], [ConsultType]) VALUES (3, N'Report Check')
SET IDENTITY_INSERT [dbo].[ConsultationType] OFF
SET IDENTITY_INSERT [dbo].[FeeCollection] ON 

INSERT [dbo].[FeeCollection] ([FeeID], [ENMRNO], [Amount], [PaidOn], [Purpose], [PaymentMode], [LastFourDigits]) VALUES (1, N'101-00021', 5000.0000, CAST(0x0000A94F00000000 AS DateTime), N'Room Advance', 2, N'1234')
INSERT [dbo].[FeeCollection] ([FeeID], [ENMRNO], [Amount], [PaidOn], [Purpose], [PaymentMode], [LastFourDigits]) VALUES (2, N'101-00022', 5000.0000, CAST(0x0000A95000000000 AS DateTime), N'Room', 2, N'1234')
INSERT [dbo].[FeeCollection] ([FeeID], [ENMRNO], [Amount], [PaidOn], [Purpose], [PaymentMode], [LastFourDigits]) VALUES (3, N'101-00016', -2000.0000, CAST(0x0000A95000000000 AS DateTime), N'Refund', 1, NULL)
INSERT [dbo].[FeeCollection] ([FeeID], [ENMRNO], [Amount], [PaidOn], [Purpose], [PaymentMode], [LastFourDigits]) VALUES (4, N'101-00019', 6000.0000, CAST(0x0000A951009168E7 AS DateTime), N'Advance Payment', 1, NULL)
INSERT [dbo].[FeeCollection] ([FeeID], [ENMRNO], [Amount], [PaidOn], [Purpose], [PaymentMode], [LastFourDigits]) VALUES (5, N'101-00019', -4500.0000, CAST(0x0000A95200000000 AS DateTime), N'Refund', 1, NULL)
SET IDENTITY_INSERT [dbo].[FeeCollection] OFF
SET IDENTITY_INSERT [dbo].[InPatientHistory] ON 

INSERT [dbo].[InPatientHistory] ([SNO], [ENMRNO], [Observations], [ObservationDate], [DoctorID]) VALUES (1, N'101-00021', N'Low BP', CAST(0xA83E0B00 AS Date), 1)
INSERT [dbo].[InPatientHistory] ([SNO], [ENMRNO], [Observations], [ObservationDate], [DoctorID]) VALUES (2, N'101-00022', N'Low Bp', CAST(0xAB3E0B00 AS Date), 1)
SET IDENTITY_INSERT [dbo].[InPatientHistory] OFF
SET IDENTITY_INSERT [dbo].[InPatients] ON 

INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (1, N'101-00001', N'00001', NULL, N'Patient1', 0, CAST(0x420F0B00 AS Date), N'Hyderabad', NULL, NULL, N'2323235626', 1, NULL, NULL, N'2312256458', N'Address1', NULL, NULL, NULL, N'123456', CAST(0x0000A91D00000000 AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (3, N'101-00002', N'00002', NULL, N'Patient2', NULL, CAST(0xFA140B00 AS Date), NULL, NULL, NULL, N'9696969696', 2, NULL, NULL, NULL, N'Address1', N'Address2', NULL, NULL, N'500072', CAST(0x0000A91D00000000 AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (5, N'101-00003', N'00003', NULL, N'Patient 3', 1, CAST(0x3C080B00 AS Date), NULL, NULL, NULL, N'2323235626', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (7, N'101-00004', N'00004', NULL, N'Patient4', 0, CAST(0x1F080B00 AS Date), NULL, NULL, NULL, N'9696969696', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (9, N'101-00005', N'00005', NULL, N'Patient5', 0, CAST(0xEF150B00 AS Date), NULL, NULL, NULL, N'2323235626', 4, 0, NULL, NULL, N'Address1', NULL, NULL, NULL, N'562265', CAST(0x0000A91D00000000 AS DateTime), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (11, N'101-00006', N'00006', NULL, N'Patient6', 0, CAST(0x783E0B00 AS Date), NULL, NULL, NULL, N'9696969696', 4, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Patient has been discharging', 1, CAST(0x0000A9500083AD35 AS DateTime), NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (13, N'101-00007', N'00007', NULL, N'Patient7', 0, NULL, NULL, NULL, NULL, N'9696969696', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'This ENMR: 101-000017 has been discharging from the hospital who got admitted with the stomacache', 1, CAST(0x0000A9500083AD35 AS DateTime), NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (15, N'101-00008', N'00008', NULL, N'Patient8', 1, NULL, NULL, NULL, NULL, N'9696969696', 6, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (16, N'101-00009', N'00009', NULL, N'Patient9', 0, NULL, NULL, NULL, NULL, N'5898988989', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (17, N'101-00010', N'00010', NULL, N'Patient10', NULL, NULL, NULL, NULL, NULL, N'2323235626', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91C00000000 AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (19, N'101-00011', N'00011', NULL, N'Patient11', 1, NULL, NULL, NULL, NULL, N'2323235626', 2, NULL, NULL, NULL, N'Address11', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (21, N'101-00012', N'00012', NULL, N'Patient12', NULL, NULL, NULL, NULL, NULL, N'9696969696', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (22, N'101-00013', N'00013', NULL, N'Patient13', NULL, NULL, NULL, NULL, NULL, N'9696969696', 3, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91D00000000 AS DateTime), NULL, 1, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (23, N'101-00014', N'00014', NULL, N'Patient14', NULL, NULL, NULL, NULL, NULL, N'2323235626', 3, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91E00000000 AS DateTime), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (24, N'101-00015', N'00015', NULL, N'Patient 15', 1, NULL, NULL, NULL, NULL, N'9696969696', 2, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91E00000000 AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (25, N'101-00016', N'00016', NULL, N'Patient 16', 1, NULL, NULL, NULL, NULL, N'9696969696', 2, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A91E00000000 AS DateTime), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (29, N'101-00019', N'101-00019', N'R', N'Patient19', 0, CAST(0xBA060B00 AS Date), NULL, NULL, N'19@test.com', N'4545454456', 1, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A94200000000 AS DateTime), N'Fever, vomits', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 10000.0000, 6000.0000)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (27, N'101-00020', N'101-00020', NULL, N'Patient20', 0, CAST(0x1C1C0B00 AS Date), NULL, NULL, N'test@test.com', N'1231231232', 1, 0, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A94A00000000 AS DateTime), N'Fever, vomits', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Patient has been discharged', 1, CAST(0x0000A95000000000 AS DateTime), NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (26, N'101-00021', N'101-00021', NULL, N'Patient21', 0, CAST(0xF9140B00 AS Date), NULL, NULL, N'test@test.com', N'9640950062', 2, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A94B00000000 AS DateTime), N'Fever, vomits', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Patient has been discharged', 1, CAST(0x0000A94F0083C23B AS DateTime), NULL, NULL)
INSERT [dbo].[InPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR], [DiscSummary], [IsDischarged], [DischargedOn], [EstAmoount], [AdvAmount]) VALUES (28, N'101-00022', N'00022', NULL, N'Patient22', 1, CAST(0xAB3E0B00 AS Date), NULL, NULL, N'test@test.com', N'9640950062', 1, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A95000000000 AS DateTime), N'Fever', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Patient has been discharged', 1, CAST(0x0000A95000000000 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[InPatients] OFF
SET IDENTITY_INSERT [dbo].[IntakeFrequency] ON 

INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (1, N'1 Table Spoon')
INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (2, N'2 Table Spoons')
INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (3, N'Before Breakfast')
SET IDENTITY_INSERT [dbo].[IntakeFrequency] OFF
SET IDENTITY_INSERT [dbo].[LabTestMaster] ON 

INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1, N'101-00017', 1, CAST(0x9C3E0B00 AS Date), 9, 1, 0, CAST(50.00 AS Decimal(4, 2)), 200.0000, 400.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (2, N'101-00017', 1, CAST(0x9C3E0B00 AS Date), 10, 1, 0, CAST(50.00 AS Decimal(4, 2)), 200.0000, 400.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (3, N'101-00017', 1, CAST(0x9C3E0B00 AS Date), 12, 1, 0, CAST(50.00 AS Decimal(4, 2)), 200.0000, 400.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (4, N'101-00019', 1, CAST(0x9C3E0B00 AS Date), 14, 1, 1, CAST(50.00 AS Decimal(4, 2)), 125.0000, 250.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (5, N'101-00021', 1, CAST(0xA63E0B00 AS Date), 16, 1, 0, CAST(50.00 AS Decimal(4, 2)), 300.0000, 600.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (10, N'101-00017', 1, CAST(0xA93E0B00 AS Date), 17, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (11, N'101-00017', 1, CAST(0xA93E0B00 AS Date), 18, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (12, N'101-00020', 1, CAST(0xA93E0B00 AS Date), 20, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (14, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 21, 1, 1, CAST(50.00 AS Decimal(4, 2)), 50.0000, 100.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (15, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 22, 1, 1, CAST(50.00 AS Decimal(4, 2)), 50.0000, 100.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (16, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 23, 1, 1, CAST(50.00 AS Decimal(4, 2)), 50.0000, 100.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (17, N'101-00018', 1, CAST(0xAB3E0B00 AS Date), 24, 1, 1, CAST(50.00 AS Decimal(4, 2)), 50.0000, 100.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (18, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 25, 1, 1, CAST(50.00 AS Decimal(4, 2)), 50.0000, 100.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (19, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 26, 1, 1, CAST(0.00 AS Decimal(4, 2)), 1300.0000, 1300.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (20, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 27, 1, 1, CAST(0.00 AS Decimal(4, 2)), 1300.0000, 1300.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1018, N'101-00022', 1, CAST(0xAB3E0B00 AS Date), 1025, 1, 1, CAST(10.00 AS Decimal(4, 2)), 1170.0000, 1300.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1019, N'101-00022', 1, CAST(0xAB3E0B00 AS Date), 1026, 1, 1, CAST(10.00 AS Decimal(4, 2)), 315.0000, 350.0000)
INSERT [dbo].[LabTestMaster] ([LTMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsBillPaid], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1020, N'101-00017', 1, CAST(0xAD3E0B00 AS Date), 1027, 0, 0, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[LabTestMaster] OFF
SET IDENTITY_INSERT [dbo].[MedicineInventory] ON 

INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (1, 1, 230, CAST(5.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (2, 2, 318, CAST(3.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (3, 3, 160, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (4, 4, 139, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (1002, 1002, 155, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (1003, 1003, 215, CAST(3.00 AS Decimal(6, 2)))
SET IDENTITY_INSERT [dbo].[MedicineInventory] OFF
SET IDENTITY_INSERT [dbo].[MedicineMaster] ON 

INSERT [dbo].[MedicineMaster] ([MMID], [BrandID], [BrandCategoryID], [MedicineName], [MedDose]) VALUES (1, 3, 4, N'Folic Acid', N'5mg')
INSERT [dbo].[MedicineMaster] ([MMID], [BrandID], [BrandCategoryID], [MedicineName], [MedDose]) VALUES (2, 3, 4, N'Folic Acid', N'15mg')
INSERT [dbo].[MedicineMaster] ([MMID], [BrandID], [BrandCategoryID], [MedicineName], [MedDose]) VALUES (3, 3, 4, N'Folic Acid', N'25mg')
INSERT [dbo].[MedicineMaster] ([MMID], [BrandID], [BrandCategoryID], [MedicineName], [MedDose]) VALUES (4, 3, 4, N'Folic Acid', N'50mg')
INSERT [dbo].[MedicineMaster] ([MMID], [BrandID], [BrandCategoryID], [MedicineName], [MedDose]) VALUES (1002, 2, 5, N'Cipla', N'5mg')
INSERT [dbo].[MedicineMaster] ([MMID], [BrandID], [BrandCategoryID], [MedicineName], [MedDose]) VALUES (1003, 2, 5, N'Cipla', N'10mg')
SET IDENTITY_INSERT [dbo].[MedicineMaster] OFF
SET IDENTITY_INSERT [dbo].[OutPatients] ON 

INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR]) VALUES (1, N'101-00017', N'00017', N'M', N'Patient17', 1, CAST(0x4B080B00 AS Date), N'Hyderabad', N'Software Engineer', N'abc@test.com', N'5898988989', 3, 0, N'Someone', N'2312256458', N'Address1', N'Address2', N'City', N'State', N'562265', CAST(0x0000A91E00000000 AS DateTime), N'Fever', 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR]) VALUES (2, N'101-00018', N'101-00018', N'M', N'Patient18', 1, CAST(0xC1060B00 AS Date), NULL, NULL, N'testuser@test.com', N'9696969696', 3, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A93B00000000 AS DateTime), N'Fever, Stomach pain, vomitings', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR]) VALUES (3, N'101-00019', N'101-00019', N'R', N'Patient19', 0, CAST(0xBA060B00 AS Date), NULL, NULL, N'19@test.com', N'4545454456', 1, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A94200000000 AS DateTime), N'Fever, vomits', 1, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR]) VALUES (4, N'101-00020', N'101-00020', NULL, N'Patient20', 0, CAST(0x1C1C0B00 AS Date), NULL, NULL, N'test@test.com', N'1231231232', 1, 0, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A94A00000000 AS DateTime), N'Fever, vomits', 1, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR]) VALUES (5, N'101-00021', N'101-00021', NULL, N'Patient21', 0, CAST(0xF9140B00 AS Date), NULL, NULL, N'test@test.com', N'9640950062', 2, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A94B00000000 AS DateTime), N'Fever, vomits', 1, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature], [OthContact], [PrevENMR]) VALUES (6, N'101-00022', N'00022', NULL, N'Patient22', 1, CAST(0xAB3E0B00 AS Date), NULL, NULL, N'test@test.com', N'9640950062', 1, NULL, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(0x0000A95000000000 AS DateTime), N'Fever', 1, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[OutPatients] OFF
SET IDENTITY_INSERT [dbo].[PatientPrescription] ON 

INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (1, 1, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (2, 2, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (3, 3, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (4, 4, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (5, 5, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (6, 6, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (7, 7, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (8, 8, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (9, 9, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (10, 10, 1, 1, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (11, 20, 1, 1, 1, NULL, NULL, NULL, 20)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (15, 28, 4, 5, 1, NULL, NULL, NULL, 5)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (16, 29, 1003, 5, 1, NULL, N'342', N'3423', 5)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (17, 30, 2, 5, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (18, 31, 4, 5, 1, NULL, NULL, NULL, 5)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (20, 36, 1, 10, 3, NULL, NULL, NULL, NULL)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (21, 37, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (22, 38, 2, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (23, 39, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (24, 40, 1, 10, 1, NULL, N'30.00', NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (25, 41, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (26, 42, 1, 10, 3, NULL, NULL, NULL, NULL)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (27, 43, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (28, 44, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (29, 45, 2, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (30, 46, 2, 2, 1, N'test', NULL, NULL, 2)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (31, 47, 4, 1, 1, N'test', NULL, NULL, 1)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (1029, 1045, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (1030, 1046, 1, 10, 3, NULL, NULL, NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (1031, 1047, 1, 10, 3, NULL, N'50.00', NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (1032, 1047, 1003, 10, 3, NULL, N'30.00', NULL, 10)
INSERT [dbo].[PatientPrescription] ([PTID], [PMID], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [BatchNo], [LotNo], [DeliverQty]) VALUES (1033, 1048, 1, 10, 3, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PatientPrescription] OFF
SET IDENTITY_INSERT [dbo].[PatientRoomAllocation] ON 

INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (2, N'101-00014', 3, 7, CAST(0x0000A7C400000000 AS DateTime), NULL, 1)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (6, N'101-00015', 3, 8, CAST(0x0000A93100000000 AS DateTime), CAST(0x0000A93400000000 AS DateTime), 0)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (7, N'101-00007', 1, 1, CAST(0x0000A89D00000000 AS DateTime), CAST(0x0000A95000810370 AS DateTime), 0)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (10, N'101-00016', 1, 4, CAST(0x0000A89D00000000 AS DateTime), CAST(0x0000A97300000000 AS DateTime), 0)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (12, N'101-00021', 1, 8, CAST(0x0000A94F00000000 AS DateTime), CAST(0x0000A94F00000000 AS DateTime), 0)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (13, N'101-00022', 4, 9, CAST(0x0000A95000000000 AS DateTime), CAST(0x0000A95000000000 AS DateTime), 0)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [AllocationStatus]) VALUES (14, N'101-00019', 4, 9, CAST(0x0000A95100000000 AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[PatientRoomAllocation] OFF
SET IDENTITY_INSERT [dbo].[PatientTests] ON 

INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (1, 1, 1, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (2, 2, 1, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (3, 3, 1, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (4, 3, 2, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (5, 4, 1, CAST(0x0000A94F00000000 AS DateTime), CAST(2.50 AS Decimal(4, 2)), N'Consult Physician', NULL, 1)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (6, 5, 3, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (7, 10, 1, CAST(0x0000A94E00000000 AS DateTime), CAST(3.00 AS Decimal(4, 2)), NULL, NULL, NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (13, 11, 2, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (14, 12, 1, CAST(0x0000A94E00000000 AS DateTime), CAST(3.00 AS Decimal(4, 2)), N'Normal', NULL, NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (16, 14, 3, CAST(0x0000A95000995196 AS DateTime), CAST(2.50 AS Decimal(4, 2)), N'Consult Physician', NULL, NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (17, 15, 4, CAST(0x0000A95000A1B3AF AS DateTime), CAST(2.50 AS Decimal(4, 2)), N'Consult Physician', N'..\PatientRecords\101-00019\Labs.JPG', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (18, 16, 5, CAST(0x0000A95000A2D615 AS DateTime), CAST(8.50 AS Decimal(4, 2)), N'Consult Physician', N'..\PatientRecords\101-00019\Labs.JPG', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (19, 17, 5, CAST(0x0000A95000000000 AS DateTime), CAST(2.89 AS Decimal(4, 2)), N'Consult Physician', N'..\PatientRecords\101-00018\Labs.JPG', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (20, 18, 4, CAST(0x0000A95000000000 AS DateTime), CAST(2.90 AS Decimal(4, 2)), N'Consult Physician', N'..\PatientRecords\101-00019\Capture1.JPG', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (21, 19, 2, CAST(0x0000A94E00000000 AS DateTime), CAST(2.50 AS Decimal(4, 2)), N'Normal', N'..\PatientRecords\101-00019\Labs.JPG', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (22, 20, 2, CAST(0x0000A94E00000000 AS DateTime), CAST(2.89 AS Decimal(4, 2)), N'Normal', N'../PatientRecords/101-00019/SBI Card - Paynet.pdf', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (1020, 1018, 2, CAST(0x0000A95000000000 AS DateTime), CAST(2.89 AS Decimal(4, 2)), N'Consult Physician', N'../PatientRecords/101-00022/Labs.JPG', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (1021, 1019, 1, CAST(0x0000A95000000000 AS DateTime), CAST(3.50 AS Decimal(4, 2)), N'Consult Physician', N'../PatientRecords/101-00022/SBI Card - Paynet.pdf', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (1022, 1019, 4, CAST(0x0000A95000000000 AS DateTime), CAST(2.89 AS Decimal(4, 2)), N'Normal', N'../PatientRecords/101-00022/SBI Card - Paynet.pdf', NULL)
INSERT [dbo].[PatientTests] ([PTID], [LTMID], [TestID], [TestDate], [RecordedValues], [TestImpression], [ReportPath], [IsSampleCollected]) VALUES (1023, 1020, 2, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PatientTests] OFF
SET IDENTITY_INSERT [dbo].[PatientVisitHistory] ON 

INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (3, CAST(0x793E0B00 AS Date), 1, N'101-00017', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (5, CAST(0x7A3E0B00 AS Date), 2, N'101-00017', 150.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (6, CAST(0x7A3E0B00 AS Date), 2, N'101-00017', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (7, CAST(0x7A3E0B00 AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (8, CAST(0x7B3E0B00 AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (9, CAST(0x7D3E0B00 AS Date), 2, N'101-00017', 100.0000, 200.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (10, CAST(0x953E0B00 AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (11, CAST(0x963E0B00 AS Date), 1, N'101-00018', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (12, CAST(0x9B3E0B00 AS Date), 2, N'101-00017', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (13, CAST(0x9B3E0B00 AS Date), 2, N'101-00017', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (14, CAST(0x9D3E0B00 AS Date), 1, N'101-00019', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (15, CAST(0xA53E0B00 AS Date), 1, N'101-00020', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (16, CAST(0xA63E0B00 AS Date), 1, N'101-00021', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (17, CAST(0xA93E0B00 AS Date), 3, N'101-00017', 0.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (18, CAST(0xA93E0B00 AS Date), 3, N'101-00017', 0.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (19, CAST(0xA93E0B00 AS Date), 3, N'101-00017', 0.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (20, CAST(0xA93E0B00 AS Date), 2, N'101-00020', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (21, CAST(0xAB3E0B00 AS Date), 2, N'101-00019', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (22, CAST(0xAB3E0B00 AS Date), 2, N'101-00019', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (23, CAST(0xAB3E0B00 AS Date), 3, N'101-00019', 0.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (24, CAST(0xAB3E0B00 AS Date), 2, N'101-00018', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (25, CAST(0xA93E0B00 AS Date), 2, N'101-00019', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (26, CAST(0xA93E0B00 AS Date), 2, N'101-00019', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (27, CAST(0xAB3E0B00 AS Date), 2, N'101-00019', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (1025, CAST(0xAB3E0B00 AS Date), 1, N'101-00022', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (1026, CAST(0xAB3E0B00 AS Date), 2, N'101-00022', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (1027, CAST(0xAD3E0B00 AS Date), 2, N'101-00017', 300.0000, NULL, 1)
SET IDENTITY_INSERT [dbo].[PatientVisitHistory] OFF
SET IDENTITY_INSERT [dbo].[PaymentModes] ON 

INSERT [dbo].[PaymentModes] ([ModeID], [Mode]) VALUES (1, N'Cash')
INSERT [dbo].[PaymentModes] ([ModeID], [Mode]) VALUES (2, N'Credit Card')
INSERT [dbo].[PaymentModes] ([ModeID], [Mode]) VALUES (3, N'Debit Card')
SET IDENTITY_INSERT [dbo].[PaymentModes] OFF
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (65, N'Account', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (66, N'Beds', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (67, N'Beds-Index', 0, N' -  - Beds master view permission.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (68, N'Beds-AddModify', 0, N' -  - Beds master Add/Edit permission.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (69, N'BedType', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (70, N'BedType-Index', 0, N' - BedType view page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (71, N'BedType-GetBedtypes', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (72, N'BedType-AddModify', 0, N' - BedType Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (73, N'BloodGroup', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (74, N'BloodGroup-Index', 0, N' - Blood Group view page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (75, N'BloodGroup-AddModify', 0, N' - Blood Group Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (76, N'BrandCategory', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (77, N'BrandCategory-Index', 0, N' - Brand Category view page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (78, N'BrandCategory-AddModify', 0, N' - Brand Category Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (79, N'Brand', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (80, N'Brand-Index', 0, N' - Brand view page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (81, N'Brand-AddModify', 0, N' - Brand Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (82, N'ConsultationFee', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (83, N'ConsultationFee-Index', 0, N' - Consultation Fee View page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (84, N'ConsultationFee-AddModify', 0, N' - Consultation Fee Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (85, N'ConsultType', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (86, N'ConsultType-Index', 0, N' - ConsultType View page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (87, N'ConsultType-AddModify', 0, N' - ConsultType Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (88, N'Home', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (89, N'InPatient', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (90, N'InPatient-Index', 0, N' - Inpatient View page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (91, N'InPatient-ViewPatient', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (92, N'InPatient-AddModify', 0, N' - Inpatient Add/Edit page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (93, N'InPatient-Fee', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (94, N'InPatient-GetFeeHistory', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (95, N'InPatient-GetObservationHistory', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (96, N'InPatient-Observations', 0, N' -  Inpatient Observations page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (97, N'InPatient-BedAllocation', 0, N' -  Inpatient Bedallocation page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (98, N'InPatient-SavePatinetRoomAllocation', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (99, N'Intakes', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (100, N'Intakes-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (101, N'Intakes-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (102, N'Lab', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (103, N'Lab-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (104, N'Lab-GetTestTypes', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (105, N'Lab-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (106, N'Login', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (107, N'Login-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (108, N'Login-Login', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (109, N'Login-LogOut', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (110, N'MedicineMaster', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (111, N'MedicineMaster-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (112, N'MedicineMaster-GetMasterMedicines', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (113, N'MedicineMaster-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (114, N'MedicineMaster-GetCategories', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (115, N'OutPatient', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (116, N'OutPatient-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (117, N'OutPatient-GetOutPatients', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (118, N'OutPatient-ViewPatient', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (119, N'OutPatient-DeliverPrescription', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (120, N'OutPatient-ManualDrugRequest', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (121, N'OutPatient-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (122, N'OutPatient-GetPatientVisits', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (123, N'OutPatient-NewVisit', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (124, N'OutPatient-GetConsultationFee', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (125, N'OutPatient-GetOPPatientPrescriptions', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (126, N'OutPatient-OPPrescription', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (127, N'OutPatient-ConvertOpToIp', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (128, N'OutPatient-PatientTests', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (129, N'Permissions', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (130, N'Permissions-Index', 0, N' - Permissions view page.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (131, N'Permissions-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (132, N'PurchaseOrder', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (133, N'PurchaseOrder-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (134, N'PurchaseOrder-GetPoNumbers', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (135, N'PurchaseOrder-GetShippedMedicines', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (136, N'PurchaseOrder-Approve', 0, N'Post')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (137, N'PurchaseOrder-DeletePO', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (138, N'PurchaseOrder-ViewPO', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (139, N'PurchaseOrder-Delete', 0, N'Post')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (140, N'PurchaseOrder-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (141, N'PurchaseOrder-CreatePO', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (142, N'PurchaseOrder-EditPO', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (143, N'RolePermissions', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (144, N'RolePermissions-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (145, N'Rooms', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (146, N'Rooms-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (147, N'Rooms-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (148, N'RoomType', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (149, N'RoomType-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (150, N'RoomType-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (151, N'Specialization', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (152, N'Specialization-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (153, N'Specialization-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (154, N'Specialization-Delete', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (155, N'User', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (156, N'User-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (157, N'User-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (158, N'UserType', 1, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (159, N'UserType-Index', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (160, N'UserType-AddModify', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (161, N'UserType-Delete', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (162, N'OutPatient-LabTestBillPay', 0, N' - OutPatient Lab Test Bill Payment Form.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (163, N'InPatient-PrintHistory', 0, N'')
GO
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (164, N'OutPatient-Admission', 0, N'')
SET IDENTITY_INSERT [dbo].[Permissions] OFF
SET IDENTITY_INSERT [dbo].[PrescriptionMaster] ON 

INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1, N'101-00017', 1, CAST(0x793E0B00 AS Date), 3, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (2, N'101-00017', 1, CAST(0x7A3E0B00 AS Date), 5, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (3, N'101-00017', 1, CAST(0x7A3E0B00 AS Date), 6, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (4, N'101-00017', 1, CAST(0x7A3E0B00 AS Date), 7, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (5, N'101-00017', 1, CAST(0x7B3E0B00 AS Date), 8, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (6, N'101-00017', 1, CAST(0x7B3E0B00 AS Date), 9, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (7, N'101-00017', 1, CAST(0x953E0B00 AS Date), 10, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (8, N'101-00018', 1, CAST(0x963E0B00 AS Date), 11, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (9, N'101-00017', 1, CAST(0x9B3E0B00 AS Date), 12, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (10, N'101-00017', 1, CAST(0x9B3E0B00 AS Date), 13, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (20, N'101-00020', 1, CAST(0xA53E0B00 AS Date), 15, 0, CAST(50.00 AS Decimal(4, 2)), 20.0000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (28, N'101-00017', 1, CAST(0xA53E0B00 AS Date), 0, 1, CAST(10.00 AS Decimal(4, 2)), 9.0000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (29, N'101-00020', 1, CAST(0xA53E0B00 AS Date), 0, 1, CAST(10.00 AS Decimal(4, 2)), 9.0000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (30, N'101-00021', 1, CAST(0xA63E0B00 AS Date), 16, 1, CAST(10.00 AS Decimal(4, 2)), 22.5000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (31, N'101-00021', 1, CAST(0xA63E0B00 AS Date), 0, 1, CAST(5.00 AS Decimal(4, 2)), 9.5000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (36, N'101-00017', 1, CAST(0xA93E0B00 AS Date), 17, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (37, N'101-00017', 1, CAST(0xA93E0B00 AS Date), 18, 1, CAST(50.00 AS Decimal(4, 2)), 15.0000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (38, N'101-00020', 1, CAST(0xA93E0B00 AS Date), 20, 1, CAST(20.00 AS Decimal(4, 2)), 16.0000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (39, N'101-00019', 1, CAST(0xA93E0B00 AS Date), 14, 1, CAST(50.00 AS Decimal(4, 2)), 15.0000, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (40, N'101-00019', 1, CAST(0xAA3E0B00 AS Date), 0, 1, CAST(50.00 AS Decimal(4, 2)), 15.0000, 30.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (41, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 21, 1, CAST(50.00 AS Decimal(4, 2)), 15.0000, 30.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (42, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 22, 0, NULL, NULL, NULL)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (43, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 23, 1, CAST(50.00 AS Decimal(4, 2)), 15.0000, 30.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (44, N'101-00018', 1, CAST(0xAB3E0B00 AS Date), 24, 1, CAST(40.00 AS Decimal(4, 2)), 18.0000, 30.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (45, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 25, 1, CAST(50.00 AS Decimal(4, 2)), 20.0000, 40.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (46, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 26, 1, CAST(0.00 AS Decimal(4, 2)), 8.0000, 8.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (47, N'101-00019', 1, CAST(0xAB3E0B00 AS Date), 27, 1, CAST(0.00 AS Decimal(4, 2)), 2.0000, 2.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1045, N'101-00022', 1, CAST(0xAB3E0B00 AS Date), 1025, 1, CAST(10.00 AS Decimal(4, 2)), 45.0000, 50.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1046, N'101-00022', 1, CAST(0xAB3E0B00 AS Date), 1026, 1, CAST(10.00 AS Decimal(4, 2)), 45.0000, 50.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1047, N'101-00022', 1, CAST(0xAB3E0B00 AS Date), 0, 1, CAST(10.00 AS Decimal(4, 2)), 72.0000, 80.0000)
INSERT [dbo].[PrescriptionMaster] ([PMID], [ENMRNO], [PrescribedBy], [DatePrescribed], [VisitID], [IsDelivered], [Discount], [PaidAmount], [TotalAmount]) VALUES (1048, N'101-00017', 1, CAST(0xAD3E0B00 AS Date), 1027, 0, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PrescriptionMaster] OFF
SET IDENTITY_INSERT [dbo].[PurchaseOrder] ON 

INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (3, N'23456', 1, 30, CAST(0x7E3E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'123', CAST(0xC63E0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (4, N'23456', 4, 50, CAST(0x7E3E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(100.00 AS Decimal(9, 2)), N'XYZ', N'123', CAST(0xC63E0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (5, N'11111', 1002, 30, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(0x413F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (6, N'11111', 1003, 30, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(0x413F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (7, N'11111', 2, 30, CAST(0x953E0B00 AS Date), CAST(3.00 AS Decimal(6, 2)), CAST(90.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(0x413F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (8, N'11111', 3, 30, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(0x413F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (11, N'5555', 1, 20, CAST(0x9D3E0B00 AS Date), CAST(3.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'342', N'3423', CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (12, N'5555', 3, 30, CAST(0x9D3E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'342', N'3423', CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (13, N'5555', 2, 20, CAST(0x9D3E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(40.00 AS Decimal(9, 2)), N'342', N'3423', CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (14, N'5555', 1002, 30, CAST(0x9D3E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'342', N'3423', CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (17, N'6666', 1, 100, CAST(0xA73E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(200.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (18, N'2222', 1, 100, CAST(0x953E0B00 AS Date), CAST(3.00 AS Decimal(6, 2)), CAST(300.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (19, N'2222', 3, 100, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(200.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (20, N'2222', 1002, 100, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(200.00 AS Decimal(9, 2)), N'342', N'3423', CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (21, N'2222', 1003, 200, CAST(0x953E0B00 AS Date), CAST(3.00 AS Decimal(6, 2)), CAST(600.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x8F400B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (22, N'2222', 2, 100, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(200.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (23, N'2222', 4, 100, CAST(0x953E0B00 AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(200.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (24, N'1000', 1, 100, CAST(0xAB3E0B00 AS Date), CAST(5.00 AS Decimal(6, 2)), CAST(500.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (25, N'1000', 2, 100, CAST(0xAB3E0B00 AS Date), CAST(4.00 AS Decimal(6, 2)), CAST(400.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (27, N'10001', 2, 100, CAST(0xAB3E0B00 AS Date), CAST(3.00 AS Decimal(6, 2)), CAST(300.00 AS Decimal(9, 2)), NULL, NULL, CAST(0x223F0B00 AS Date), 1)
SET IDENTITY_INSERT [dbo].[PurchaseOrder] OFF
SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (1, N'G101', 1, N'Ground floor room no1', 1000, 1, CAST(0xBD3D0B00 AS Date), 2)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (2, N'101', 0, N'Firstfloor room1', 500, 1, CAST(0xBD3D0B00 AS Date), 10)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (3, N'ICU', 1, N'Intensive care Unit', 5000, NULL, NULL, 2)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (4, N'Ward', 3, N'10 Bed Ward', 1500, NULL, NULL, 10)
SET IDENTITY_INSERT [dbo].[Rooms] OFF
SET IDENTITY_INSERT [dbo].[RoomType] ON 

INSERT [dbo].[RoomType] ([RoomTypeID], [RoomType]) VALUES (1, N'Non-A/C')
INSERT [dbo].[RoomType] ([RoomTypeID], [RoomType]) VALUES (2, N'A/C')
INSERT [dbo].[RoomType] ([RoomTypeID], [RoomType]) VALUES (3, N'10 bed Ward')
SET IDENTITY_INSERT [dbo].[RoomType] OFF
SET IDENTITY_INSERT [dbo].[Specialization] ON 

INSERT [dbo].[Specialization] ([SpecializationID], [DoctorType]) VALUES (1, N'Neurologist')
INSERT [dbo].[Specialization] ([SpecializationID], [DoctorType]) VALUES (2, N'Radiologist')
SET IDENTITY_INSERT [dbo].[Specialization] OFF
SET IDENTITY_INSERT [dbo].[TestTypes] ON 

INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (1, N'TSH', 250.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (2, N'CBP', 1300.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (3, N'Test1', 100.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (4, N'Test1111', 100.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (5, N'Test11', 100.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (6, N'Test12', 100.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (7, N'Test1345', 100.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (8, N'Test123', 100.0000)
INSERT [dbo].[TestTypes] ([TestID], [TestName], [TestCost]) VALUES (9, N'Test13', 100.0000)
SET IDENTITY_INSERT [dbo].[TestTypes] OFF
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 66)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 67)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 68)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 69)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 70)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 71)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 72)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 73)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 74)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 75)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 76)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 77)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 78)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 79)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 80)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 81)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 82)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 83)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 84)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 85)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 86)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 87)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 88)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 89)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 90)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 91)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 92)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 93)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 94)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 95)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 96)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 97)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 98)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 99)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 100)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 101)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 102)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 103)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 104)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 105)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 106)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 107)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 108)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 109)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 110)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 111)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 112)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 113)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 114)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 115)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 116)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 117)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 118)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 119)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 120)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 121)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 122)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 123)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 124)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 125)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 126)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 127)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 128)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 129)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 130)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 131)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 132)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 133)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 134)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 135)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 136)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 137)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 138)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 139)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 140)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 141)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 142)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 143)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 144)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 145)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 146)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 147)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 148)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 149)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 150)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 151)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 152)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 153)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 154)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 155)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 156)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 157)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 158)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 159)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 160)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 161)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 162)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 163)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 164)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 66)
GO
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 67)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 68)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 69)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 70)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 72)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 82)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 83)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 84)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 89)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 90)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 92)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 97)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (7, 137)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (7, 142)
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1, N'Girish', N'Kumar', N'Rebba', 0, N'grebba', N'His@123', N'girishrebba@gmail.com', CAST(0x51F10A00 AS Date), N'8095178654', 1, N'MBBS', 0, 1, 1)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (2, N'Satish', NULL, N'R', 0, N'sareddi', N'His@123', N'satishreddi9@gmail.com', CAST(0x46130B00 AS Date), N'8095178654', 1, N'MBBS', 1, 1, 1)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1002, N'Abc', NULL, N'Test', 0, N'abctest', N'His@123', N'abc@test.com', CAST(0x5E130B00 AS Date), N'9696969696', 0, N'MBBS', 0, 2, 2)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1003, N'Test', N'A', N'User', 1, N'testuser', N'His@123', N'testuser@test.com', CAST(0x56050B00 AS Date), N'9696969696', 0, N'MBBS DGO', 0, 2, 2)
INSERT [dbo].[Users] ([UserID], [FirstName], [MiddleName], [LastName], [Gender], [UserName], [Password], [Email], [DOB], [Phone], [MaritalStatus], [Qualification], [UserStatus], [SpecializationID], [UserTypeID]) VALUES (1004, N'Admin', N'Office', N'User', 0, N'adminuser', N'His@123', N'adminuser@test.com', CAST(0xA4080B00 AS Date), N'9696969696', 1, N'MBBS', 1, 1, 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[UserType] ON 

INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (1, N'Doctor')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (2, N'Nurse')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (4, N'Admin')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (5, N'Accountant')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (6, N'Pharmacist')
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (7, N'Lab Technician')
SET IDENTITY_INSERT [dbo].[UserType] OFF
ALTER TABLE [dbo].[InPatients] ADD  CONSTRAINT [Default_IP_IsDischarged]  DEFAULT ((0)) FOR [IsDischarged]
GO
ALTER TABLE [dbo].[LabTestMaster] ADD  CONSTRAINT [Default_IP_IsBillPaid]  DEFAULT ((0)) FOR [IsBillPaid]
GO
ALTER TABLE [dbo].[LabTestMaster] ADD  CONSTRAINT [Default_LTM_IsDelivered]  DEFAULT ((0)) FOR [IsDelivered]
GO
ALTER TABLE [dbo].[PatientRoomAllocation] ADD  CONSTRAINT [Default_PM_AllocationStatus]  DEFAULT ((0)) FOR [AllocationStatus]
GO
ALTER TABLE [dbo].[PatientTests] ADD  CONSTRAINT [Default_PT_IsGivenSample]  DEFAULT ((0)) FOR [IsSampleCollected]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_PermissionStatus]  DEFAULT ((0)) FOR [PermissionStatus]
GO
ALTER TABLE [dbo].[PrescriptionMaster] ADD  CONSTRAINT [Default_PM_IsDelivered]  DEFAULT ((0)) FOR [IsDelivered]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  DEFAULT ((0)) FOR [ApprovedStatus]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UserStatus]  DEFAULT ((1)) FOR [UserStatus]
GO
ALTER TABLE [dbo].[Beds]  WITH CHECK ADD  CONSTRAINT [FK_Beds_RoomNo] FOREIGN KEY([RoomNo])
REFERENCES [dbo].[Rooms] ([RoomNo])
GO
ALTER TABLE [dbo].[Beds] CHECK CONSTRAINT [FK_Beds_RoomNo]
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
ALTER TABLE [dbo].[LabTestMaster]  WITH CHECK ADD  CONSTRAINT [FK_LTM_Doctor] FOREIGN KEY([PrescribedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[LabTestMaster] CHECK CONSTRAINT [FK_LTM_Doctor]
GO
ALTER TABLE [dbo].[MedicineInventory]  WITH CHECK ADD  CONSTRAINT [FK_MI_MedicineID] FOREIGN KEY([MedicineID])
REFERENCES [dbo].[MedicineMaster] ([MMID])
GO
ALTER TABLE [dbo].[MedicineInventory] CHECK CONSTRAINT [FK_MI_MedicineID]
GO
ALTER TABLE [dbo].[MedicineMaster]  WITH CHECK ADD  CONSTRAINT [FK_MM_BrandID] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([BrandID])
GO
ALTER TABLE [dbo].[MedicineMaster] CHECK CONSTRAINT [FK_MM_BrandID]
GO
ALTER TABLE [dbo].[MedicineMaster]  WITH CHECK ADD  CONSTRAINT [FK_MM_CategoryID] FOREIGN KEY([BrandCategoryID])
REFERENCES [dbo].[BrandCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[MedicineMaster] CHECK CONSTRAINT [FK_MM_CategoryID]
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
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_FrequencyID] FOREIGN KEY([IntakeFrequencyID])
REFERENCES [dbo].[IntakeFrequency] ([FrequencyID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_FrequencyID]
GO
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_Medicine] FOREIGN KEY([MedicineID])
REFERENCES [dbo].[MedicineMaster] ([MMID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_Medicine]
GO
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_PMID] FOREIGN KEY([PMID])
REFERENCES [dbo].[PrescriptionMaster] ([PMID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_PMID]
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
ALTER TABLE [dbo].[PatientTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientTests_LTMID] FOREIGN KEY([LTMID])
REFERENCES [dbo].[LabTestMaster] ([LTMID])
GO
ALTER TABLE [dbo].[PatientTests] CHECK CONSTRAINT [FK_PatientTests_LTMID]
GO
ALTER TABLE [dbo].[PatientTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientTests_TestID] FOREIGN KEY([TestID])
REFERENCES [dbo].[TestTypes] ([TestID])
GO
ALTER TABLE [dbo].[PatientTests] CHECK CONSTRAINT [FK_PatientTests_TestID]
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
ALTER TABLE [dbo].[PrescriptionMaster]  WITH CHECK ADD  CONSTRAINT [FK_PM_Doctor] FOREIGN KEY([PrescribedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PrescriptionMaster] CHECK CONSTRAINT [FK_PM_Doctor]
GO
ALTER TABLE [dbo].[PurchaseOrder]  WITH CHECK ADD  CONSTRAINT [FK_PO_MedicineID] FOREIGN KEY([MedicineID])
REFERENCES [dbo].[MedicineMaster] ([MMID])
GO
ALTER TABLE [dbo].[PurchaseOrder] CHECK CONSTRAINT [FK_PO_MedicineID]
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
