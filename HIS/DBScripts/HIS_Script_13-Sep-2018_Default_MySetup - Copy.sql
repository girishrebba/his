USE [HIS]
GO
/****** Object:  StoredProcedure [dbo].[ConvertOutPatientToInPatient]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  StoredProcedure [dbo].[CreateMasterLabTest]    Script Date: 9/13/2018 8:25:47 AM ******/
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
	@ISIP BIT = 0,
	@LTMID int output

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[LabTestMaster]
           ([ENMRNO],[PrescribedBy],[VisitID],[DatePrescribed], [ISIP]) VALUES(@ENMRNO, @DoctorID, @VisitID, GETDATE(), @ISIP)
	  
	
	SET @LTMID = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[CreateMasterPrescription]    Script Date: 9/13/2018 8:25:47 AM ******/
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
	@ISIP BIT = 0,
	@PMID int output

AS
BEGIN
	SET NOCOUNT ON;

    -- Copy OP RECORD TO IP Record
	INSERT INTO [dbo].[PrescriptionMaster]
           ([ENMRNO],[PrescribedBy],[VisitID],[DatePrescribed],[ISIP]) VALUES(@ENMRNO, @DoctorID, @VisitID, GETDATE(),@ISIP)
	  
	
	SET @PMID = SCOPE_IDENTITY()

END



GO
/****** Object:  Table [dbo].[Beds]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[BedType]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[BloodGroup]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[BrandCategories]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[Brands]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[ConsultationFee]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[ConsultationType]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[FeeCollection]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[InPatientHistory]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[InPatients]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[IntakeFrequency]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[LabTestMaster]    Script Date: 9/13/2018 8:25:47 AM ******/
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
	[ISIP] [bit] NULL,
 CONSTRAINT [PK_LabTestMaster] PRIMARY KEY CLUSTERED 
(
	[LTMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MedicineInventory]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[MedicineMaster]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[OutPatients]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[PatientPrescription]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[PatientRoomAllocation]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[PatientTests]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[PatientVisitHistory]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[PaymentModes]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[Permissions]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[PrescriptionMaster]    Script Date: 9/13/2018 8:25:47 AM ******/
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
	[ISIP] [bit] NULL,
 CONSTRAINT [PK_PrescriptionMaster] PRIMARY KEY CLUSTERED 
(
	[PMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[Rooms]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[RoomType]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[Specialization]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[TestTypes]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[UserPermission]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 9/13/2018 8:25:47 AM ******/
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
/****** Object:  Table [dbo].[UserType]    Script Date: 9/13/2018 8:25:47 AM ******/
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
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (4, N'201', N'bed - 201', NULL, 2, 2, NULL)
INSERT [dbo].[Beds] ([BedNo], [BedName], [Description], [BedStatus], [BedTypeID], [RoomNo], [NextAvailbility]) VALUES (5, N'201', N'bed - duplicate', NULL, 2, 2, NULL)
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
INSERT [dbo].[BedType] ([BedTypeID], [BedType], [BedTypeDescription]) VALUES (5, N'Genearl Ward', N'Ward contains normal Beds')
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

SET IDENTITY_INSERT [dbo].[IntakeFrequency] ON 

INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (1, N'1 Table Spoon')
INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (2, N'2 Table Spoons')
INSERT [dbo].[IntakeFrequency] ([FrequencyID], [Frequency]) VALUES (3, N'Before Breakfast')
SET IDENTITY_INSERT [dbo].[IntakeFrequency] OFF

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
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (165, N'InPatient-Prescription', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (166, N'InPatient-PatientTests', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (167, N'InPatient-LabTestBillPay', 0, N' - InPatient Lab Test Bill Payment Form.')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (168, N'InPatient-DeliverPrescription', 0, N'')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription], [PermissionStatus], [Toottip]) VALUES (169, N'Home-Index', 0, N'')
SET IDENTITY_INSERT [dbo].[Permissions] OFF

SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (1, N'G101', 1, N'Ground floor room no1', 1000, 1, CAST(0xBD3D0B00 AS Date), 2)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (2, N'101', 0, N'Firstfloor room1', 500, 1, CAST(0xBD3D0B00 AS Date), 10)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (3, N'ICU', 1, N'Intensive care Unit', 5000, NULL, NULL, 2)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (4, N'Ward', 3, N'10 Bed Ward', 1000, NULL, NULL, 10)
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
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 165)
GO
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 166)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 167)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 168)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (1, 169)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 66)
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
ALTER TABLE [dbo].[LabTestMaster] ADD  CONSTRAINT [Default_LTM_ISIP]  DEFAULT ((0)) FOR [ISIP]
GO
ALTER TABLE [dbo].[PatientRoomAllocation] ADD  CONSTRAINT [Default_PM_AllocationStatus]  DEFAULT ((0)) FOR [AllocationStatus]
GO
ALTER TABLE [dbo].[PatientTests] ADD  CONSTRAINT [Default_PT_IsGivenSample]  DEFAULT ((0)) FOR [IsSampleCollected]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_PermissionStatus]  DEFAULT ((0)) FOR [PermissionStatus]
GO
ALTER TABLE [dbo].[PrescriptionMaster] ADD  CONSTRAINT [Default_PM_IsDelivered]  DEFAULT ((0)) FOR [IsDelivered]
GO
ALTER TABLE [dbo].[PrescriptionMaster] ADD  CONSTRAINT [Default_PM_ISIP]  DEFAULT ((0)) FOR [ISIP]
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
