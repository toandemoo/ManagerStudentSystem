USE TaekwondoDB;
GO

CREATE TABLE Taekwondo.Users(
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender BIT NOT NULL,
    PhoneNumber VARCHAR(15),    
    Email VARCHAR(100) NOT NULL UNIQUE,
    Address NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1, --1: Active, 0: Inactive
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdateAt DATETIME NULL  
);
GO

CREATE TABLE Taekwondo.Roles(
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);
GO

CREATE TABLE Taekwondo.UserRoles(
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),

    CONSTRAINT fk_UserRole_Users FOREIGN KEY (UserId) 
    REFERENCES Taekwondo.Users(UserId) ON DELETE CASCADE,

    CONSTRAINT fk_UserRole_Roles FOREIGN KEY (RoleId) 
    REFERENCES Taekwondo.Roles(RoleId) ON DELETE CASCADE
);
GO 

CREATE TABLE Taekwondo.Levels(
    LevelId INT PRIMARY KEY IDENTITY(1,1),
    LevelName NVARCHAR(50) NOT NULL,
    DESCRIPTION NVARCHAR(255)
);
GO

CREATE TABLE Taekwondo.Students(
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    StudentCode VARCHAR(20) NOT NULL DEFAULT 'VXX-XXXX',
    LevelId INT NOT NULL DEFAULT 1,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active', --Active, Inactive, Graduated
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_Students_Levels FOREIGN KEY (LevelId)
    REFERENCES Taekwondo.Levels(LevelId) on DELETE CASCADE,
    CONSTRAINT FK_Students_Users FOREIGN KEY (UserId)
    REFERENCES Taekwondo.Users(UserId) on DELETE CASCADE
);
GO

-------------------------------- Fee Plans -------------------------------
CREATE TABLE Taekwondo.FeePeriods(
    FeePeriodId INT PRIMARY KEY IDENTITY(1,1),
    FeePeriodCode VARCHAR(20) NOT NULL UNIQUE DEFAULT 'FP-XXXX',
    PeriodName NVARCHAR(50) NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1, --1: Active, 0: Inactive
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),   
    PublishedAt DATETIME NULL
);
GO

CREATE TABLE Taekwondo.StudentFees(
    StudentFeeId INT PRIMARY KEY IDENTITY(1,1), 
    StudentId INT NOT NULL,
    FeePeriodId INT NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    DiscountAmount DECIMAL(10, 2) NOT NULL,
    FinalAmount DECIMAL(10, 2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    IssuedAt DATETIME NULL,
    DueDate DATETIME NULL,
    PaidAt DATETIME NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    UpdatedAt DATETIME NULL,
    CONSTRAINT UQ_StudentFees UNIQUE (StudentId, FeePeriodId),
    CONSTRAINT FK_StudentFees_Students FOREIGN KEY (StudentId)
    REFERENCES Taekwondo.Students(StudentId) ON DELETE CASCADE,
    CONSTRAINT FK_StudentFees_FeePeriods FOREIGN KEY (FeePeriodId)
    REFERENCES Taekwondo.FeePeriods(FeePeriodId) ON DELETE CASCADE
);
GO

CREATE TABLE Taekwondo.FeePlans(
    FeePlanId INT PRIMARY KEY IDENTITY(1,1),
    FeePlanName NVARCHAR(50) NOT NULL,
    FeeAmountMonthly DECIMAL(10, 2) NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    DisCountPercentage DECIMAL(5, 2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1, --1: Active, 0: Inactive
    Description NVARCHAR(255),
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NULL
);
GO

CREATE TABLE Taekwondo.StudentFeePlans(
    StudentId INT NOT NULL,
    FeePlanId INT NOT NULL,
    StartDate DATE NOT NULL, 
    EndDate DATE,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NULL,

    CONSTRAINT PK_StudentFeePlans PRIMARY KEY (StudentId, FeePlanId),
    CONSTRAINT FK_StudentFeePlans_Students FOREIGN KEY (StudentId) 
    REFERENCES Taekwondo.Students(StudentId),
    CONSTRAINT FK_StudentFeePlans_FeePlans FOREIGN KEY (FeePlanId) 
    REFERENCES Taekwondo.FeePlans(FeePlanId)
);
GO

-------------------------------------- Authentication -----------------------------

CREATE TABLE Taekwondo.RefreshTokens(
    RefreshTokenId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    TokenHash VARCHAR(100) NOT NULL,
    CreateAt DATETIME,
    ExpireIn DATETIME,
    RevoteAt DATETIME NULL,
    CONSTRAINT fk_RefreshTokens_Users FOREIGN KEY (UserId) 
    REFERENCES Taekwondo.Users(UserID) ON DELETE CASCADE
);
GO

-------------------------------- Payment Methods -------------------------------
CREATE TABLE Taekwondo.PaymentMethods(
    PaymentMethodId INT PRIMARY KEY IDENTITY(1,1),
    PaymentMethodName NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1, --1: Active, 0: Inactive
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NULL
);
GO
-------------------------------- Product -------------------------------
CREATE TABLE Taekwondo.ItemTypes(
    ItemTypeId TINYINT PRIMARY KEY IDENTITY(1,1),
    ItemTypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
);

CREATE TABLE Taekwondo.Items(
    ItemId INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    ItemTypeId TINYINT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1, --1: Active, 0: Inactive
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Items_ItemTypes FOREIGN KEY (ItemTypeId) 
    REFERENCES Taekwondo.ItemTypes(ItemTypeId)
);

-------------------------------- Invoice -------------------------------
-- CREATE TABLE Taekwondo.InvoiceStatus(
--     StatusId INT PRIMARY KEY IDENTITY(1,1),
--     StatusName NVARCHAR(50) NOT NULL,
--     Description NVARCHAR(255),
--     CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
--     UpdatedAt DATETIME NULL
-- );
-- GO

CREATE TABLE Taekwondo.Invoices(
    InvoiceId INT PRIMARY KEY IDENTITY(1,1),
    InvoiceCode VARCHAR(50) NOT NULL UNIQUE,
    StudentId INT NOT NULL,  
    SubTotal DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    PaidAmount DECIMAL(10, 2) DEFAULT 0,
    RemainingAmount DECIMAL(10, 2),
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    IssuedAt DATETIME2 NULL,
    DueAt DATETIME2 NULL,
    PaidAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Invoices_Students FOREIGN KEY (StudentId) 
    REFERENCES Taekwondo.Students(StudentId)
);
GO

CREATE TABLE Taekwondo.InvoiceDetails(
    InvoiceDetailId INT PRIMARY KEY IDENTITY(1,1),
    InvoiceId INT NOT NULL,
    ItemTypeId TINYINT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,

    CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (InvoiceId) 
    REFERENCES Taekwondo.Invoices(InvoiceId) ON DELETE CASCADE,
    CONSTRAINT FK_InvoiceDetails_ItemTypes FOREIGN KEY (ItemTypeId) 
    REFERENCES Taekwondo.ItemTypes(ItemTypeId) ON DELETE CASCADE
);
GO

--------------------------------------- Payment -------------------------------
CREATE TABLE Taekwondo.PaymentProviders(
    PaymentProviderId INT PRIMARY KEY IDENTITY(1,1),
    PaymentProviderCode VARCHAR(50) NOT NULL,
    PaymentProviderName NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

-- CREATE TABLE Taekwondo.PaymentStatus(
--     PaymentStatusId INT PRIMARY KEY IDENTITY(1,1),
--     StatusName NVARCHAR(50) NOT NULL,
--     Description NVARCHAR(255),
--     CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--     UpdatedAt DATETIME2 NULL
-- );
-- GO


CREATE TABLE Taekwondo.Payments(
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    PaymentCode VARCHAR(50) NOT NULL UNIQUE,
    InvoiceId INT NOT NULL,
    ProviderId INT NOT NULL,
    PaymentMethodId INT NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    PaymentDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Currency CHAR(3) NOT NULL DEFAULT 'VND',
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    PaidAt DATETIME2 NULL,
    CONSTRAINT FK_Payments_Invoices FOREIGN KEY (InvoiceId) 
    REFERENCES Taekwondo.Invoices(InvoiceId) ON DELETE CASCADE,
    CONSTRAINT FK_Payments_PaymentMethods FOREIGN KEY (PaymentMethodId) 
    REFERENCES Taekwondo.PaymentMethods(PaymentMethodId) ON DELETE CASCADE,
    CONSTRAINT FK_Payments_PaymentProvider FOREIGN KEY (ProviderId)
    REFERENCES Taekwondo.PaymentProviders(PaymentProviderId) ON DELETE CASCADE
);
GO

-- CREATE TABLE Taekwondo.PaymentAttemptStatus(
--     StatusId INT PRIMARY KEY IDENTITY(1,1),
--     StatusName NVARCHAR(50) NOT NULL,
--     Description NVARCHAR(255),
--     CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--     UpdatedAt DATETIME2 NULL
-- );
-- GO

CREATE TABLE Taekwondo.PaymentAttempts
(
    PaymentAttemptId INT PRIMARY KEY IDENTITY(1,1),
    PaymentCode VARCHAR(50) NOT NULL,
    AttemptNumber INT NOT NULL,
    ProviderTransactionId VARCHAR(200) NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Created',
    PaymentUrl NVARCHAR(2000) NULL, 
    ErrorCode VARCHAR(100) NULL,
    ErrorMessage NVARCHAR(1000) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    StartedAt DATETIME2 NULL,
    CompletedAt DATETIME2 NULL,
    CONSTRAINT FK_PaymentAttempts_Payments FOREIGN KEY (PaymentCode)
    REFERENCES Taekwondo.Payments(PaymentCode),
    CONSTRAINT UQ_PaymentAttempts UNIQUE (PaymentCode, AttemptNumber)
);
GO

-- CREATE TABLE Taekwondo.PaymentTransactionStatus(
--     PaymentTransactionStatusId INT PRIMARY KEY IDENTITY(1,1),
--     StatusName NVARCHAR(50) NOT NULL,
--     Description NVARCHAR(255),
--     CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--     UpdatedAt DATETIME2 NULL
-- );
-- GO

CREATE TABLE Taekwondo.PaymentTransaction(
    PaymentTransactionId INT PRIMARY KEY IDENTITY(1,1),
    PaymentId INT NOT NULL,
    PaymentAttemptId INT NULL,
    ProviderId INT NOT NULL,
    ProviderTransactionId VARCHAR(200) NOT NULL,
    TransactionType TINYINT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Status VARCHAR(50) NOT NULL,
    TransactionAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_PaymentTransactions_Payments FOREIGN KEY (PaymentId)
    REFERENCES Taekwondo.Payments(PaymentId),

    CONSTRAINT FK_PaymentTransactions_Attempts FOREIGN KEY (PaymentAttemptId)
    REFERENCES Taekwondo.PaymentAttempts(PaymentAttemptId),

    CONSTRAINT FK_PaymentTransactions_Providers FOREIGN KEY (ProviderId)
    REFERENCES Taekwondo.PaymentProviders(PaymentProviderId),

    CONSTRAINT UQ_ProviderTransaction UNIQUE (ProviderId, ProviderTransactionId)
);
GO

CREATE TABLE Taekwondo.WebhookEvents
(
    WebhookEventId INT PRIMARY KEY IDENTITY(1,1),
    ProviderId INT NOT NULL,
    EventId VARCHAR(200) NOT NULL,
    EventType VARCHAR(100) NULL,
    ProviderTransactionId VARCHAR(200) NULL,
    Payload NVARCHAR(MAX) NOT NULL,
    Signature VARCHAR(1000) NULL,
    Status TINYINT NOT NULL,
    ReceivedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ProcessedAt DATETIME2 NULL,
    ErrorMessage NVARCHAR(1000) NULL,
    CONSTRAINT FK_WebhookEvents_Providers FOREIGN KEY (ProviderId)
    REFERENCES Taekwondo.PaymentProviders(PaymentProviderId),
    CONSTRAINT UQ_WebhookEvents UNIQUE (ProviderId, EventId)
);
GO

CREATE TABLE Taekwondo.IdempotencyKeys
(
    IdempotencyKeyId INT PRIMARY KEY IDENTITY(1,1),
    KeyValue VARCHAR(200) NOT NULL UNIQUE,
    Endpoint VARCHAR(200) NOT NULL,
    RequestHash VARCHAR(200) NOT NULL,
    ResponseStatusCode INT NOT NULL,
    ResponseBody NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ExpireAt DATETIME2 NOT NULL 
);
GO

CREATE TABLE Taekwondo.Refunds
(
    RefundId INT PRIMARY KEY IDENTITY(1,1),
    PaymentId INT NOT NULL,
    RefundCode VARCHAR(50) NOT NULL UNIQUE,
    Amount DECIMAL(18,2) NOT NULL,
    Reason NVARCHAR(500) NULL,
    Status TINYINT NOT NULL,
    ProviderRefundId VARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CompletedAt DATETIME2 NULL,
    CONSTRAINT FK_Refunds_Payments FOREIGN KEY (PaymentId)
    REFERENCES Taekwondo.Payments(PaymentId)
);
GO

---------------------------------------- Exam -------------------------------
CREATE TABLE Taekwondo.Exams(
    ExamId INT PRIMARY KEY IDENTITY(1,1),
    ExamName NVARCHAR(100) NOT NULL,
    ExamDate DATE NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    RegistrationDeadline DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Open',
    Description NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

CREATE TABLE Taekwondo.ExamRegistrations(
    ExamRegistrationId INT PRIMARY KEY IDENTITY(1,1),
    ExamId INT NOT NULL,
    StudentId INT NOT NULL,
    RegistrationDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_ExamRegistrations_Exams FOREIGN KEY (ExamId) 
    REFERENCES Taekwondo.Exams(ExamId),
    CONSTRAINT FK_ExamRegistrations_Students FOREIGN KEY (StudentId) 
    REFERENCES Taekwondo.Students(StudentId)
);
GO



