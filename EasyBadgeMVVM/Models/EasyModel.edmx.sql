
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/31/2019 11:16:18
-- Generated from EDMX file: C:\Users\onetec\Documents\EasyBadgeMVVM\EasyBadgeMVVM\EasyBadgeMVVM\Models\EasyModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EasyBadgeDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BadgeBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSets] DROP CONSTRAINT [FK_BadgeBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeEventPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PositionSets] DROP CONSTRAINT [FK_BadgeEventPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_EventBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSets] DROP CONSTRAINT [FK_EventBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_EventEventField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldSets] DROP CONSTRAINT [FK_EventEventField];
GO
IF OBJECT_ID(N'[dbo].[FK_EventFieldFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldUserSets] DROP CONSTRAINT [FK_EventFieldFieldUser];
GO
IF OBJECT_ID(N'[dbo].[FK_EventFieldFilter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilterSets] DROP CONSTRAINT [FK_EventFieldFilter];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldEventField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldSets] DROP CONSTRAINT [FK_FieldEventField];
GO
IF OBJECT_ID(N'[dbo].[FK_UserFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldUserSets] DROP CONSTRAINT [FK_UserFieldUser];
GO
IF OBJECT_ID(N'[dbo].[FK_EventPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSets] DROP CONSTRAINT [FK_EventPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PositionSets] DROP CONSTRAINT [FK_FieldPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_FilterRule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RuleSets] DROP CONSTRAINT [FK_FilterRule];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSets] DROP CONSTRAINT [FK_UserPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_TargetRule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RuleSets] DROP CONSTRAINT [FK_TargetRule];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeEventSetRuleSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RuleSets] DROP CONSTRAINT [FK_BadgeEventSetRuleSet];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BadgeEventSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BadgeEventSets];
GO
IF OBJECT_ID(N'[dbo].[BadgeSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BadgeSets];
GO
IF OBJECT_ID(N'[dbo].[EventFieldSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventFieldSets];
GO
IF OBJECT_ID(N'[dbo].[EventFieldUserSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventFieldUserSets];
GO
IF OBJECT_ID(N'[dbo].[EventSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventSets];
GO
IF OBJECT_ID(N'[dbo].[FieldSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FieldSets];
GO
IF OBJECT_ID(N'[dbo].[FilterSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FilterSets];
GO
IF OBJECT_ID(N'[dbo].[PositionSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PositionSets];
GO
IF OBJECT_ID(N'[dbo].[PrintBadgeSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrintBadgeSets];
GO
IF OBJECT_ID(N'[dbo].[RuleSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RuleSets];
GO
IF OBJECT_ID(N'[dbo].[TargetSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TargetSets];
GO
IF OBJECT_ID(N'[dbo].[UserSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSets];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BadgeEventSets'
CREATE TABLE [dbo].[BadgeEventSets] (
    [ID_BadgeEvent] int IDENTITY(1,1) NOT NULL,
    [EventID_Event] int  NOT NULL,
    [BadgeID_Badge] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DefaultPrint] bit  NOT NULL,
    [BackgroundImage] nvarchar(max)  NULL
);
GO

-- Creating table 'BadgeSets'
CREATE TABLE [dbo].[BadgeSets] (
    [ID_Badge] int IDENTITY(1,1) NOT NULL,
    [Dimension_X] float  NOT NULL,
    [Dimension_Y] float  NOT NULL,
    [TypeBadge] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventFieldSets'
CREATE TABLE [dbo].[EventFieldSets] (
    [FieldID_Field] int  NOT NULL,
    [EventID_Event] int  NOT NULL,
    [Visibility] bit  NOT NULL,
    [Unique] bit  NOT NULL
);
GO

-- Creating table 'EventFieldUserSets'
CREATE TABLE [dbo].[EventFieldUserSets] (
    [UserID_User] int  NOT NULL,
    [EventFieldFieldID_Field] int  NOT NULL,
    [EventFieldEventID_Event] int  NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [AdditionnalInformation] nvarchar(max)  NULL
);
GO

-- Creating table 'EventSets'
CREATE TABLE [dbo].[EventSets] (
    [ID_Event] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateOfEvent] datetime  NOT NULL
);
GO

-- Creating table 'FieldSets'
CREATE TABLE [dbo].[FieldSets] (
    [ID_Field] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FilterSets'
CREATE TABLE [dbo].[FilterSets] (
    [ID_Filter] int IDENTITY(1,1) NOT NULL,
    [EventFieldFieldID_Field] int  NOT NULL,
    [EventFieldEventID_Event] int  NOT NULL,
    [LogicalOperator] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PositionSets'
CREATE TABLE [dbo].[PositionSets] (
    [ID_Position] int IDENTITY(1,1) NOT NULL,
    [FieldID_Field] int  NOT NULL,
    [BadgeEventID_BadgeEvent] int  NOT NULL,
    [Position_X] float  NOT NULL,
    [Position_Y] float  NOT NULL,
    [FontFamily] nvarchar(max)  NOT NULL,
    [FontSize] float  NOT NULL,
    [FontStyle] nvarchar(max)  NOT NULL,
    [AngleRotation] float  NULL
);
GO

-- Creating table 'PrintBadgeSets'
CREATE TABLE [dbo].[PrintBadgeSets] (
    [ID_PrintBadge] int IDENTITY(1,1) NOT NULL,
    [PrintDate] datetime  NOT NULL,
    [PrintBy] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [UserID_User] int  NOT NULL,
    [EventID_Event] int  NOT NULL
);
GO

-- Creating table 'RuleSets'
CREATE TABLE [dbo].[RuleSets] (
    [ID_Rule] int IDENTITY(1,1) NOT NULL,
    [FilterID_Filter] int  NOT NULL,
    [HexaCode] nvarchar(max)  NOT NULL,
    [TargetID_Target] int  NOT NULL,
    [BadgeEventID_BadgeEvent] int  NOT NULL,
    [BadgeEventSetID_BadgeEvent] int  NULL
);
GO

-- Creating table 'TargetSets'
CREATE TABLE [dbo].[TargetSets] (
    [ID_Target] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserSets'
CREATE TABLE [dbo].[UserSets] (
    [ID_User] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Active] bit  NOT NULL,
    [Barcode] nvarchar(max)  NOT NULL,
    [Onsite] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID_BadgeEvent] in table 'BadgeEventSets'
ALTER TABLE [dbo].[BadgeEventSets]
ADD CONSTRAINT [PK_BadgeEventSets]
    PRIMARY KEY CLUSTERED ([ID_BadgeEvent] ASC);
GO

-- Creating primary key on [ID_Badge] in table 'BadgeSets'
ALTER TABLE [dbo].[BadgeSets]
ADD CONSTRAINT [PK_BadgeSets]
    PRIMARY KEY CLUSTERED ([ID_Badge] ASC);
GO

-- Creating primary key on [FieldID_Field], [EventID_Event] in table 'EventFieldSets'
ALTER TABLE [dbo].[EventFieldSets]
ADD CONSTRAINT [PK_EventFieldSets]
    PRIMARY KEY CLUSTERED ([FieldID_Field], [EventID_Event] ASC);
GO

-- Creating primary key on [UserID_User], [EventFieldFieldID_Field], [EventFieldEventID_Event] in table 'EventFieldUserSets'
ALTER TABLE [dbo].[EventFieldUserSets]
ADD CONSTRAINT [PK_EventFieldUserSets]
    PRIMARY KEY CLUSTERED ([UserID_User], [EventFieldFieldID_Field], [EventFieldEventID_Event] ASC);
GO

-- Creating primary key on [ID_Event] in table 'EventSets'
ALTER TABLE [dbo].[EventSets]
ADD CONSTRAINT [PK_EventSets]
    PRIMARY KEY CLUSTERED ([ID_Event] ASC);
GO

-- Creating primary key on [ID_Field] in table 'FieldSets'
ALTER TABLE [dbo].[FieldSets]
ADD CONSTRAINT [PK_FieldSets]
    PRIMARY KEY CLUSTERED ([ID_Field] ASC);
GO

-- Creating primary key on [ID_Filter] in table 'FilterSets'
ALTER TABLE [dbo].[FilterSets]
ADD CONSTRAINT [PK_FilterSets]
    PRIMARY KEY CLUSTERED ([ID_Filter] ASC);
GO

-- Creating primary key on [ID_Position] in table 'PositionSets'
ALTER TABLE [dbo].[PositionSets]
ADD CONSTRAINT [PK_PositionSets]
    PRIMARY KEY CLUSTERED ([ID_Position] ASC);
GO

-- Creating primary key on [ID_PrintBadge] in table 'PrintBadgeSets'
ALTER TABLE [dbo].[PrintBadgeSets]
ADD CONSTRAINT [PK_PrintBadgeSets]
    PRIMARY KEY CLUSTERED ([ID_PrintBadge] ASC);
GO

-- Creating primary key on [ID_Rule] in table 'RuleSets'
ALTER TABLE [dbo].[RuleSets]
ADD CONSTRAINT [PK_RuleSets]
    PRIMARY KEY CLUSTERED ([ID_Rule] ASC);
GO

-- Creating primary key on [ID_Target] in table 'TargetSets'
ALTER TABLE [dbo].[TargetSets]
ADD CONSTRAINT [PK_TargetSets]
    PRIMARY KEY CLUSTERED ([ID_Target] ASC);
GO

-- Creating primary key on [ID_User] in table 'UserSets'
ALTER TABLE [dbo].[UserSets]
ADD CONSTRAINT [PK_UserSets]
    PRIMARY KEY CLUSTERED ([ID_User] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BadgeID_Badge] in table 'BadgeEventSets'
ALTER TABLE [dbo].[BadgeEventSets]
ADD CONSTRAINT [FK_BadgeBadgeEvent]
    FOREIGN KEY ([BadgeID_Badge])
    REFERENCES [dbo].[BadgeSets]
        ([ID_Badge])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BadgeBadgeEvent'
CREATE INDEX [IX_FK_BadgeBadgeEvent]
ON [dbo].[BadgeEventSets]
    ([BadgeID_Badge]);
GO

-- Creating foreign key on [BadgeEventID_BadgeEvent] in table 'PositionSets'
ALTER TABLE [dbo].[PositionSets]
ADD CONSTRAINT [FK_BadgeEventPosition]
    FOREIGN KEY ([BadgeEventID_BadgeEvent])
    REFERENCES [dbo].[BadgeEventSets]
        ([ID_BadgeEvent])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BadgeEventPosition'
CREATE INDEX [IX_FK_BadgeEventPosition]
ON [dbo].[PositionSets]
    ([BadgeEventID_BadgeEvent]);
GO

-- Creating foreign key on [EventID_Event] in table 'BadgeEventSets'
ALTER TABLE [dbo].[BadgeEventSets]
ADD CONSTRAINT [FK_EventBadgeEvent]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSets]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventBadgeEvent'
CREATE INDEX [IX_FK_EventBadgeEvent]
ON [dbo].[BadgeEventSets]
    ([EventID_Event]);
GO

-- Creating foreign key on [EventID_Event] in table 'EventFieldSets'
ALTER TABLE [dbo].[EventFieldSets]
ADD CONSTRAINT [FK_EventEventField]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSets]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventEventField'
CREATE INDEX [IX_FK_EventEventField]
ON [dbo].[EventFieldSets]
    ([EventID_Event]);
GO

-- Creating foreign key on [EventFieldFieldID_Field], [EventFieldEventID_Event] in table 'EventFieldUserSets'
ALTER TABLE [dbo].[EventFieldUserSets]
ADD CONSTRAINT [FK_EventFieldFieldUser]
    FOREIGN KEY ([EventFieldFieldID_Field], [EventFieldEventID_Event])
    REFERENCES [dbo].[EventFieldSets]
        ([FieldID_Field], [EventID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventFieldFieldUser'
CREATE INDEX [IX_FK_EventFieldFieldUser]
ON [dbo].[EventFieldUserSets]
    ([EventFieldFieldID_Field], [EventFieldEventID_Event]);
GO

-- Creating foreign key on [EventFieldFieldID_Field], [EventFieldEventID_Event] in table 'FilterSets'
ALTER TABLE [dbo].[FilterSets]
ADD CONSTRAINT [FK_EventFieldFilter]
    FOREIGN KEY ([EventFieldFieldID_Field], [EventFieldEventID_Event])
    REFERENCES [dbo].[EventFieldSets]
        ([FieldID_Field], [EventID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventFieldFilter'
CREATE INDEX [IX_FK_EventFieldFilter]
ON [dbo].[FilterSets]
    ([EventFieldFieldID_Field], [EventFieldEventID_Event]);
GO

-- Creating foreign key on [FieldID_Field] in table 'EventFieldSets'
ALTER TABLE [dbo].[EventFieldSets]
ADD CONSTRAINT [FK_FieldEventField]
    FOREIGN KEY ([FieldID_Field])
    REFERENCES [dbo].[FieldSets]
        ([ID_Field])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserID_User] in table 'EventFieldUserSets'
ALTER TABLE [dbo].[EventFieldUserSets]
ADD CONSTRAINT [FK_UserFieldUser]
    FOREIGN KEY ([UserID_User])
    REFERENCES [dbo].[UserSets]
        ([ID_User])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EventID_Event] in table 'PrintBadgeSets'
ALTER TABLE [dbo].[PrintBadgeSets]
ADD CONSTRAINT [FK_EventPrintBadge]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSets]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventPrintBadge'
CREATE INDEX [IX_FK_EventPrintBadge]
ON [dbo].[PrintBadgeSets]
    ([EventID_Event]);
GO

-- Creating foreign key on [FieldID_Field] in table 'PositionSets'
ALTER TABLE [dbo].[PositionSets]
ADD CONSTRAINT [FK_FieldPosition]
    FOREIGN KEY ([FieldID_Field])
    REFERENCES [dbo].[FieldSets]
        ([ID_Field])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldPosition'
CREATE INDEX [IX_FK_FieldPosition]
ON [dbo].[PositionSets]
    ([FieldID_Field]);
GO

-- Creating foreign key on [FilterID_Filter] in table 'RuleSets'
ALTER TABLE [dbo].[RuleSets]
ADD CONSTRAINT [FK_FilterRule]
    FOREIGN KEY ([FilterID_Filter])
    REFERENCES [dbo].[FilterSets]
        ([ID_Filter])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FilterRule'
CREATE INDEX [IX_FK_FilterRule]
ON [dbo].[RuleSets]
    ([FilterID_Filter]);
GO

-- Creating foreign key on [UserID_User] in table 'PrintBadgeSets'
ALTER TABLE [dbo].[PrintBadgeSets]
ADD CONSTRAINT [FK_UserPrintBadge]
    FOREIGN KEY ([UserID_User])
    REFERENCES [dbo].[UserSets]
        ([ID_User])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPrintBadge'
CREATE INDEX [IX_FK_UserPrintBadge]
ON [dbo].[PrintBadgeSets]
    ([UserID_User]);
GO

-- Creating foreign key on [TargetID_Target] in table 'RuleSets'
ALTER TABLE [dbo].[RuleSets]
ADD CONSTRAINT [FK_TargetRule]
    FOREIGN KEY ([TargetID_Target])
    REFERENCES [dbo].[TargetSets]
        ([ID_Target])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetRule'
CREATE INDEX [IX_FK_TargetRule]
ON [dbo].[RuleSets]
    ([TargetID_Target]);
GO

-- Creating foreign key on [BadgeEventSetID_BadgeEvent] in table 'RuleSets'
ALTER TABLE [dbo].[RuleSets]
ADD CONSTRAINT [FK_BadgeEventSetRuleSet]
    FOREIGN KEY ([BadgeEventSetID_BadgeEvent])
    REFERENCES [dbo].[BadgeEventSets]
        ([ID_BadgeEvent])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BadgeEventSetRuleSet'
CREATE INDEX [IX_FK_BadgeEventSetRuleSet]
ON [dbo].[RuleSets]
    ([BadgeEventSetID_BadgeEvent]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------