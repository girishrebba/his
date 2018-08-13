USE [HIS]
GO
/****** Object:  Table [dbo].[Beds]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[BedType]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[BloodGroup]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[BrandCategories]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[Brands]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[ConsultationFee]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[ConsultationType]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[FeeCollection]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[InPatientHistory]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[InPatients]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[IntakeFrequency]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[MedicineInventory]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[MedicineMaster]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[OutPatients]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[PatientPrescription]    Script Date: 8/13/2018 9:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientPrescription](
	[PrescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [varchar](30) NOT NULL,
	[MedicineID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IntakeFrequencyID] [int] NOT NULL,
	[Comments] [varchar](max) NULL,
	[PrescribedBy] [int] NOT NULL,
	[DatePrescribed] [date] NULL,
	[BatchNo] [varchar](30) NULL,
	[LotNo] [varchar](30) NULL,
	[VisitID] [int] NULL,
	[IsDelivered] [bit] NULL CONSTRAINT [Default_PP_IsDelivered]  DEFAULT ((0)),
	[DeliverQty] [int] NULL,
 CONSTRAINT [PK_PatientPrescription] PRIMARY KEY CLUSTERED 
(
	[PrescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientRoomAllocation]    Script Date: 8/13/2018 9:09:36 PM ******/
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
	[DischargeSummary] [varchar](max) NULL,
	[AllocationStatus] [int] NOT NULL,
 CONSTRAINT [PK_PatientRoomAllocation] PRIMARY KEY CLUSTERED 
(
	[AllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientTests]    Script Date: 8/13/2018 9:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientTests](
	[SNO] [int] IDENTITY(1,1) NOT NULL,
	[ENMRNO] [nvarchar](50) NOT NULL,
	[TestID] [int] NOT NULL,
	[TestDate] [datetime] NULL,
	[PrescribedDoctor] [int] NOT NULL,
	[RecordedValues] [decimal](18, 0) NULL,
	[TestImpression] [nvarchar](max) NULL,
	[ReportPath] [nvarchar](200) NULL,
 CONSTRAINT [PK_PatientTests] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PatientVisitHistory]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[Permissions]    Script Date: 8/13/2018 9:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Permission_Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionDescription] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Permission_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 8/13/2018 9:09:36 PM ******/
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
	[PricePerSheet] [decimal](9, 2) NULL,
	[BatchNo] [varchar](30) NOT NULL,
	[LotNo] [varchar](30) NOT NULL,
	[ExpiryDate] [date] NULL,
	[ApprovedStatus] [bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[RoomType]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[Specialization]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[TestTypes]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[UserPermission]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 8/13/2018 9:09:36 PM ******/
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
/****** Object:  Table [dbo].[UserType]    Script Date: 8/13/2018 9:09:36 PM ******/
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

INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (1, 1, 20, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (2, 2, 30, CAST(3.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (3, 3, 30, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (4, 4, 50, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (1002, 1002, 25, CAST(2.00 AS Decimal(6, 2)))
INSERT [dbo].[MedicineInventory] ([MedInventoryID], [MedicineID], [AvailableQty], [PricePerItem]) VALUES (1003, 1003, 30, CAST(2.00 AS Decimal(6, 2)))
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

INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (1, N'101-00017', N'00017', N'M', N'Patient17', 1, CAST(N'1980-07-24' AS Date), N'Hyderabad', N'Software Engineer', N'abc@test.com', N'5898988989', 3, 0, N'Someone', N'2312256458', N'Address1', N'Address2', N'City', N'State', N'562265', CAST(N'2018-07-15 00:00:00.000' AS DateTime), N'Fever', 2, NULL, N'Fever', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[OutPatients] ([SNO], [ENMRNO], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [BirthPlace], [Profession], [Email], [Phone], [BloodGroupID], [MaritalStatus], [ReferredBy], [RefPhone], [Address1], [Address2], [City], [State], [PinCode], [Enrolled], [Purpose], [DoctorID], [Mask], [PatientHistory], [Status], [Height], [Weight], [BMI], [HeartBeat], [BP], [Temperature]) VALUES (2, N'101-00018', N'101-00018', N'M', N'Patient18', 1, CAST(N'1979-06-26' AS Date), NULL, NULL, N'testuser@test.com', N'9696969696', 3, 1, NULL, NULL, N'Address1', NULL, NULL, NULL, NULL, CAST(N'2018-08-13 00:00:00.000' AS DateTime), N'Fever, Stomach pain, vomitings', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[OutPatients] OFF
SET IDENTITY_INSERT [dbo].[PatientPrescription] ON 

INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (2, N'101-00017', 1003, 5, 3, N'After Breakfast', 1, CAST(N'2018-07-31' AS Date), NULL, NULL, 3, 0, NULL)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (3, N'101-00017', 1003, 2, 3, N'After Breakfast', 1, CAST(N'2018-07-31' AS Date), NULL, NULL, 5, 0, NULL)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (4, N'101-00017', 1003, 5, 3, N'After Breakfast', 1, CAST(N'2018-07-31' AS Date), NULL, NULL, 6, 0, NULL)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (5, N'101-00017', 1002, 5, 3, N'After Breakfast', 1, CAST(N'2018-07-31' AS Date), NULL, NULL, 7, 0, NULL)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (6, N'101-00017', 1, 3, 3, N'After Breakfast', 1, CAST(N'2018-07-31' AS Date), NULL, NULL, 8, 0, NULL)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (7, N'101-00017', 2, 20, 3, N'After Breakfast', 1, CAST(N'2018-07-31' AS Date), NULL, NULL, 9, 0, NULL)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (8, N'101-00017', 1002, 5, 3, N'After tea', 1, CAST(N'2018-08-13' AS Date), NULL, NULL, 10, 1, 5)
INSERT [dbo].[PatientPrescription] ([PrescriptionID], [ENMRNO], [MedicineID], [Quantity], [IntakeFrequencyID], [Comments], [PrescribedBy], [DatePrescribed], [BatchNo], [LotNo], [VisitID], [IsDelivered], [DeliverQty]) VALUES (9, N'101-00017', 1, 10, 3, N'After tea', 1, CAST(N'2018-08-13' AS Date), NULL, NULL, 10, 1, 10)
SET IDENTITY_INSERT [dbo].[PatientPrescription] OFF
SET IDENTITY_INSERT [dbo].[PatientRoomAllocation] ON 

INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [DischargeSummary], [AllocationStatus]) VALUES (2, N'101-00014', 3, 7, CAST(N'2017-08-03 00:00:00.000' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [DischargeSummary], [AllocationStatus]) VALUES (6, N'101-00015', 3, 8, CAST(N'2018-08-03 00:00:00.000' AS DateTime), CAST(N'2018-08-06 00:00:00.000' AS DateTime), NULL, 1)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [DischargeSummary], [AllocationStatus]) VALUES (7, N'101-00007', 1, 1, CAST(N'2018-03-08 00:00:00.000' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[PatientRoomAllocation] ([AllocationID], [ENMRNO], [RoomNo], [BedNo], [FromDate], [EndDate], [DischargeSummary], [AllocationStatus]) VALUES (10, N'101-00016', 1, 4, CAST(N'2018-03-08 00:00:00.000' AS DateTime), CAST(N'2018-10-08 00:00:00.000' AS DateTime), N'test', 1)
SET IDENTITY_INSERT [dbo].[PatientRoomAllocation] OFF
SET IDENTITY_INSERT [dbo].[PatientVisitHistory] ON 

INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (3, CAST(N'2018-07-15' AS Date), 1, N'101-00017', 600.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (5, CAST(N'2018-07-16' AS Date), 2, N'101-00017', 150.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (6, CAST(N'2018-07-16' AS Date), 2, N'101-00017', 300.0000, NULL, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (7, CAST(N'2018-07-16' AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (8, CAST(N'2018-07-17' AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (9, CAST(N'2018-07-19' AS Date), 2, N'101-00017', 100.0000, 200.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (10, CAST(N'2018-08-12' AS Date), 2, N'101-00017', 200.0000, 100.0000, 1)
INSERT [dbo].[PatientVisitHistory] ([SNO], [DateOfVisit], [ConsultTypeID], [ENMRNO], [Fee], [Discount], [DoctorID]) VALUES (11, CAST(N'2018-08-13' AS Date), 1, N'101-00018', 600.0000, NULL, 1)
SET IDENTITY_INSERT [dbo].[PatientVisitHistory] OFF
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription]) VALUES (1, N'Add Beds')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription]) VALUES (2, N'Edit beds')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription]) VALUES (4, N'delete beds')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription]) VALUES (5, N'Dashboard')
INSERT [dbo].[Permissions] ([Permission_Id], [PermissionDescription]) VALUES (7, N'Reports')
SET IDENTITY_INSERT [dbo].[Permissions] OFF
SET IDENTITY_INSERT [dbo].[PurchaseOrder] ON 

INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (3, N'23456', 1, 30, CAST(N'2018-07-20' AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'123', CAST(N'2018-09-30' AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (4, N'23456', 4, 50, CAST(N'2018-07-20' AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(100.00 AS Decimal(9, 2)), N'XYZ', N'123', CAST(N'2018-09-30' AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (5, N'11111', 1002, 30, CAST(N'2018-08-12' AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(N'2019-01-31' AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (6, N'11111', 1003, 30, CAST(N'2018-08-12' AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(N'2019-01-31' AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (7, N'11111', 2, 30, CAST(N'2018-08-12' AS Date), CAST(3.00 AS Decimal(6, 2)), CAST(90.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(N'2019-01-31' AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (8, N'11111', 3, 30, CAST(N'2018-08-12' AS Date), CAST(2.00 AS Decimal(6, 2)), CAST(60.00 AS Decimal(9, 2)), N'XYZ', N'ABC123', CAST(N'2019-01-31' AS Date), 1)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (9, N'2222', 2, 30, CAST(N'2018-08-12' AS Date), CAST(4.00 AS Decimal(6, 2)), CAST(150.00 AS Decimal(9, 2)), N'ABC123', N'ABC123', CAST(N'2019-01-31' AS Date), NULL)
INSERT [dbo].[PurchaseOrder] ([OrderID], [PONumber], [MedicineID], [OrderedQty], [OrderedDate], [PricePerItem], [PricePerSheet], [BatchNo], [LotNo], [ExpiryDate], [ApprovedStatus]) VALUES (10, N'2222', 1003, 30, CAST(N'2018-08-12' AS Date), CAST(5.00 AS Decimal(6, 2)), CAST(150.00 AS Decimal(9, 2)), N'ABC123', N'ABC123', CAST(N'2019-01-31' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[PurchaseOrder] OFF
SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (1, N'G101', 1, N'Ground floor room no1', 1000, 1, CAST(N'2018-01-08' AS Date), 2)
INSERT [dbo].[Rooms] ([RoomNo], [RoomName], [RoomTypeID], [Description], [CostPerDay], [RoomStatus], [NextAvailbility], [RoomBedCapacity]) VALUES (2, N'101', 0, N'Firstfloor room1', 500, 1, CAST(N'2018-01-08' AS Date), 10)
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
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 2)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (2, 4)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (4, 1)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (4, 4)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (5, 1)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (5, 5)
INSERT [dbo].[UserPermission] ([UserTypeID], [PermissionID]) VALUES (5, 7)
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
INSERT [dbo].[UserType] ([UserTypeID], [UserTypeName]) VALUES (5, N'Accountant')
SET IDENTITY_INSERT [dbo].[UserType] OFF
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
ALTER TABLE [dbo].[PatientPrescription]  WITH CHECK ADD  CONSTRAINT [FK_PP_Medicine] FOREIGN KEY([MedicineID])
REFERENCES [dbo].[MedicineMaster] ([MMID])
GO
ALTER TABLE [dbo].[PatientPrescription] CHECK CONSTRAINT [FK_PP_Medicine]
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
ALTER TABLE [dbo].[PatientTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientTests_TestID] FOREIGN KEY([TestID])
REFERENCES [dbo].[TestTypes] ([TestID])
GO
ALTER TABLE [dbo].[PatientTests] CHECK CONSTRAINT [FK_PatientTests_TestID]
GO
ALTER TABLE [dbo].[PatientTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientTests_UserID] FOREIGN KEY([PrescribedDoctor])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PatientTests] CHECK CONSTRAINT [FK_PatientTests_UserID]
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
/****** Object:  StoredProcedure [dbo].[ConvertOutPatientToInPatient]    Script Date: 8/13/2018 9:09:36 PM ******/
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
	@ENMRNO VARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;

    -- Copy OP RECORD TO IP Record
	INSERT INTO [dbo].[InPatients]
           ([ENMRNO],[FirstName],[MiddleName],[LastName],[Gender],[DOB],[BirthPlace],[Profession],[Email],[Phone],[BloodGroupID]
           ,[MaritalStatus],[ReferredBy],[RefPhone],[Address1],[Address2],[City],[State],[PinCode],[Enrolled],[Purpose],[DoctorID],
		   [PatientHistory],[Height],[Weight],[BMI],[HeartBeat],[BP],[Temperature])
	  SELECT [ENMRNO],[FirstName],[MiddleName],[LastName],[Gender],[DOB],[BirthPlace],[Profession],[Email],[Phone],[BloodGroupID]
           ,[MaritalStatus],[ReferredBy],[RefPhone],[Address1],[Address2],[City],[State],[PinCode],[Enrolled],[Purpose],[DoctorID],
		   [PatientHistory],[Height],[Weight],[BMI],[HeartBeat],[BP],[Temperature]
    FROM [dbo].[OutPatients] WHERE ENMRNO = @ENMRNO

	-- Make the Out Patient to Inactive
	Update OutPatients SET [Status] = 1 WHERE ENMRNO = @ENMRNO

END
GO
