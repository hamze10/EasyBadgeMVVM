
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/19/2019 11:39:03
-- Generated from EDMX file: C:\Users\onetec\Documents\DocsPerso\BABADGEDGE\EasyBadgeMVVM\EasyBadgeMVVM\Models\EasyModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_UserFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldUserSet] DROP CONSTRAINT [FK_UserFieldUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSet] DROP CONSTRAINT [FK_UserPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_EventPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSet] DROP CONSTRAINT [FK_EventPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldEventField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldSet] DROP CONSTRAINT [FK_FieldEventField];
GO
IF OBJECT_ID(N'[dbo].[FK_EventEventField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldSet] DROP CONSTRAINT [FK_EventEventField];
GO
IF OBJECT_ID(N'[dbo].[FK_EventFieldFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldUserSet] DROP CONSTRAINT [FK_EventFieldFieldUser];
GO
IF OBJECT_ID(N'[dbo].[FK_EventBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSet] DROP CONSTRAINT [FK_EventBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSet] DROP CONSTRAINT [FK_BadgeBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PositionSet] DROP CONSTRAINT [FK_FieldPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeEventPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PositionSet] DROP CONSTRAINT [FK_BadgeEventPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_EventFieldFilter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilterSet] DROP CONSTRAINT [FK_EventFieldFilter];
GO
IF OBJECT_ID(N'[dbo].[FK_FilterRule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RuleSet] DROP CONSTRAINT [FK_FilterRule];
GO
IF OBJECT_ID(N'[dbo].[FK_TargetRule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RuleSet] DROP CONSTRAINT [FK_TargetRule];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeEventRule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RuleSet] DROP CONSTRAINT [FK_BadgeEventRule];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[EventSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventSet];
GO
IF OBJECT_ID(N'[dbo].[FieldSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FieldSet];
GO
IF OBJECT_ID(N'[dbo].[EventFieldUserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventFieldUserSet];
GO
IF OBJECT_ID(N'[dbo].[PrintBadgeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrintBadgeSet];
GO
IF OBJECT_ID(N'[dbo].[BadgeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BadgeSet];
GO
IF OBJECT_ID(N'[dbo].[EventFieldSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventFieldSet];
GO
IF OBJECT_ID(N'[dbo].[PositionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PositionSet];
GO
IF OBJECT_ID(N'[dbo].[BadgeEventSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BadgeEventSet];
GO
IF OBJECT_ID(N'[dbo].[FilterSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FilterSet];
GO
IF OBJECT_ID(N'[dbo].[RuleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RuleSet];
GO
IF OBJECT_ID(N'[dbo].[TargetSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TargetSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [ID_User] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Active] bit  NOT NULL,
    [Barcode] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventSet'
CREATE TABLE [dbo].[EventSet] (
    [ID_Event] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateOfEvent] datetime  NOT NULL
);
GO

-- Creating table 'FieldSet'
CREATE TABLE [dbo].[FieldSet] (
    [ID_Field] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventFieldUserSet'
CREATE TABLE [dbo].[EventFieldUserSet] (
    [UserID_User] int  NOT NULL,
    [EventFieldFieldID_Field] int  NOT NULL,
    [EventFieldEventID_Event] int  NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [AdditionnalInformation] nvarchar(max)  NULL
);
GO

-- Creating table 'PrintBadgeSet'
CREATE TABLE [dbo].[PrintBadgeSet] (
    [UserID_User] int  NOT NULL,
    [EventID_Event] int  NOT NULL,
    [PrintDate] datetime  NOT NULL,
    [PrintBy] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NULL
);
GO

-- Creating table 'BadgeSet'
CREATE TABLE [dbo].[BadgeSet] (
    [ID_Badge] int IDENTITY(1,1) NOT NULL,
    [Dimension_X] float  NOT NULL,
    [Dimension_Y] float  NOT NULL,
    [TypeBadge] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventFieldSet'
CREATE TABLE [dbo].[EventFieldSet] (
    [FieldID_Field] int  NOT NULL,
    [EventID_Event] int  NOT NULL,
    [Visibility] bit  NOT NULL,
    [Unique] bit  NOT NULL
);
GO

-- Creating table 'PositionSet'
CREATE TABLE [dbo].[PositionSet] (
    [ID_Position] int IDENTITY(1,1) NOT NULL,
    [FieldID_Field] int  NOT NULL,
    [BadgeEventID_BadgeEvent] int  NOT NULL,
    [Position_X] float  NOT NULL,
    [Position_Y] float  NOT NULL,
    [FontFamily] nvarchar(max)  NOT NULL,
    [FontSize] float  NOT NULL,
    [FontStyle] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BadgeEventSet'
CREATE TABLE [dbo].[BadgeEventSet] (
    [ID_BadgeEvent] int IDENTITY(1,1) NOT NULL,
    [EventID_Event] int  NOT NULL,
    [BadgeID_Badge] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FilterSet'
CREATE TABLE [dbo].[FilterSet] (
    [ID_Filter] int IDENTITY(1,1) NOT NULL,
    [EventFieldFieldID_Field] int  NOT NULL,
    [EventFieldEventID_Event] int  NOT NULL,
    [LogicalOperator] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RuleSet'
CREATE TABLE [dbo].[RuleSet] (
    [ID_Rule] int IDENTITY(1,1) NOT NULL,
    [FilterID_Filter] int  NOT NULL,
    [HexaCode] nvarchar(max)  NOT NULL,
    [TargetID_Target] int  NOT NULL,
    [BadgeEventID_BadgeEvent] int  NOT NULL
);
GO

-- Creating table 'TargetSet'
CREATE TABLE [dbo].[TargetSet] (
    [ID_Target] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID_User] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([ID_User] ASC);
GO

-- Creating primary key on [ID_Event] in table 'EventSet'
ALTER TABLE [dbo].[EventSet]
ADD CONSTRAINT [PK_EventSet]
    PRIMARY KEY CLUSTERED ([ID_Event] ASC);
GO

-- Creating primary key on [ID_Field] in table 'FieldSet'
ALTER TABLE [dbo].[FieldSet]
ADD CONSTRAINT [PK_FieldSet]
    PRIMARY KEY CLUSTERED ([ID_Field] ASC);
GO

-- Creating primary key on [UserID_User], [EventFieldFieldID_Field], [EventFieldEventID_Event] in table 'EventFieldUserSet'
ALTER TABLE [dbo].[EventFieldUserSet]
ADD CONSTRAINT [PK_EventFieldUserSet]
    PRIMARY KEY CLUSTERED ([UserID_User], [EventFieldFieldID_Field], [EventFieldEventID_Event] ASC);
GO

-- Creating primary key on [UserID_User], [EventID_Event] in table 'PrintBadgeSet'
ALTER TABLE [dbo].[PrintBadgeSet]
ADD CONSTRAINT [PK_PrintBadgeSet]
    PRIMARY KEY CLUSTERED ([UserID_User], [EventID_Event] ASC);
GO

-- Creating primary key on [ID_Badge] in table 'BadgeSet'
ALTER TABLE [dbo].[BadgeSet]
ADD CONSTRAINT [PK_BadgeSet]
    PRIMARY KEY CLUSTERED ([ID_Badge] ASC);
GO

-- Creating primary key on [FieldID_Field], [EventID_Event] in table 'EventFieldSet'
ALTER TABLE [dbo].[EventFieldSet]
ADD CONSTRAINT [PK_EventFieldSet]
    PRIMARY KEY CLUSTERED ([FieldID_Field], [EventID_Event] ASC);
GO

-- Creating primary key on [ID_Position] in table 'PositionSet'
ALTER TABLE [dbo].[PositionSet]
ADD CONSTRAINT [PK_PositionSet]
    PRIMARY KEY CLUSTERED ([ID_Position] ASC);
GO

-- Creating primary key on [ID_BadgeEvent] in table 'BadgeEventSet'
ALTER TABLE [dbo].[BadgeEventSet]
ADD CONSTRAINT [PK_BadgeEventSet]
    PRIMARY KEY CLUSTERED ([ID_BadgeEvent] ASC);
GO

-- Creating primary key on [ID_Filter] in table 'FilterSet'
ALTER TABLE [dbo].[FilterSet]
ADD CONSTRAINT [PK_FilterSet]
    PRIMARY KEY CLUSTERED ([ID_Filter] ASC);
GO

-- Creating primary key on [ID_Rule] in table 'RuleSet'
ALTER TABLE [dbo].[RuleSet]
ADD CONSTRAINT [PK_RuleSet]
    PRIMARY KEY CLUSTERED ([ID_Rule] ASC);
GO

-- Creating primary key on [ID_Target] in table 'TargetSet'
ALTER TABLE [dbo].[TargetSet]
ADD CONSTRAINT [PK_TargetSet]
    PRIMARY KEY CLUSTERED ([ID_Target] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserID_User] in table 'EventFieldUserSet'
ALTER TABLE [dbo].[EventFieldUserSet]
ADD CONSTRAINT [FK_UserFieldUser]
    FOREIGN KEY ([UserID_User])
    REFERENCES [dbo].[UserSet]
        ([ID_User])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserID_User] in table 'PrintBadgeSet'
ALTER TABLE [dbo].[PrintBadgeSet]
ADD CONSTRAINT [FK_UserPrintBadge]
    FOREIGN KEY ([UserID_User])
    REFERENCES [dbo].[UserSet]
        ([ID_User])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EventID_Event] in table 'PrintBadgeSet'
ALTER TABLE [dbo].[PrintBadgeSet]
ADD CONSTRAINT [FK_EventPrintBadge]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSet]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventPrintBadge'
CREATE INDEX [IX_FK_EventPrintBadge]
ON [dbo].[PrintBadgeSet]
    ([EventID_Event]);
GO

-- Creating foreign key on [FieldID_Field] in table 'EventFieldSet'
ALTER TABLE [dbo].[EventFieldSet]
ADD CONSTRAINT [FK_FieldEventField]
    FOREIGN KEY ([FieldID_Field])
    REFERENCES [dbo].[FieldSet]
        ([ID_Field])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EventID_Event] in table 'EventFieldSet'
ALTER TABLE [dbo].[EventFieldSet]
ADD CONSTRAINT [FK_EventEventField]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSet]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventEventField'
CREATE INDEX [IX_FK_EventEventField]
ON [dbo].[EventFieldSet]
    ([EventID_Event]);
GO

-- Creating foreign key on [EventFieldFieldID_Field], [EventFieldEventID_Event] in table 'EventFieldUserSet'
ALTER TABLE [dbo].[EventFieldUserSet]
ADD CONSTRAINT [FK_EventFieldFieldUser]
    FOREIGN KEY ([EventFieldFieldID_Field], [EventFieldEventID_Event])
    REFERENCES [dbo].[EventFieldSet]
        ([FieldID_Field], [EventID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventFieldFieldUser'
CREATE INDEX [IX_FK_EventFieldFieldUser]
ON [dbo].[EventFieldUserSet]
    ([EventFieldFieldID_Field], [EventFieldEventID_Event]);
GO

-- Creating foreign key on [EventID_Event] in table 'BadgeEventSet'
ALTER TABLE [dbo].[BadgeEventSet]
ADD CONSTRAINT [FK_EventBadgeEvent]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSet]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventBadgeEvent'
CREATE INDEX [IX_FK_EventBadgeEvent]
ON [dbo].[BadgeEventSet]
    ([EventID_Event]);
GO

-- Creating foreign key on [BadgeID_Badge] in table 'BadgeEventSet'
ALTER TABLE [dbo].[BadgeEventSet]
ADD CONSTRAINT [FK_BadgeBadgeEvent]
    FOREIGN KEY ([BadgeID_Badge])
    REFERENCES [dbo].[BadgeSet]
        ([ID_Badge])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BadgeBadgeEvent'
CREATE INDEX [IX_FK_BadgeBadgeEvent]
ON [dbo].[BadgeEventSet]
    ([BadgeID_Badge]);
GO

-- Creating foreign key on [FieldID_Field] in table 'PositionSet'
ALTER TABLE [dbo].[PositionSet]
ADD CONSTRAINT [FK_FieldPosition]
    FOREIGN KEY ([FieldID_Field])
    REFERENCES [dbo].[FieldSet]
        ([ID_Field])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldPosition'
CREATE INDEX [IX_FK_FieldPosition]
ON [dbo].[PositionSet]
    ([FieldID_Field]);
GO

-- Creating foreign key on [BadgeEventID_BadgeEvent] in table 'PositionSet'
ALTER TABLE [dbo].[PositionSet]
ADD CONSTRAINT [FK_BadgeEventPosition]
    FOREIGN KEY ([BadgeEventID_BadgeEvent])
    REFERENCES [dbo].[BadgeEventSet]
        ([ID_BadgeEvent])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BadgeEventPosition'
CREATE INDEX [IX_FK_BadgeEventPosition]
ON [dbo].[PositionSet]
    ([BadgeEventID_BadgeEvent]);
GO

-- Creating foreign key on [EventFieldFieldID_Field], [EventFieldEventID_Event] in table 'FilterSet'
ALTER TABLE [dbo].[FilterSet]
ADD CONSTRAINT [FK_EventFieldFilter]
    FOREIGN KEY ([EventFieldFieldID_Field], [EventFieldEventID_Event])
    REFERENCES [dbo].[EventFieldSet]
        ([FieldID_Field], [EventID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventFieldFilter'
CREATE INDEX [IX_FK_EventFieldFilter]
ON [dbo].[FilterSet]
    ([EventFieldFieldID_Field], [EventFieldEventID_Event]);
GO

-- Creating foreign key on [FilterID_Filter] in table 'RuleSet'
ALTER TABLE [dbo].[RuleSet]
ADD CONSTRAINT [FK_FilterRule]
    FOREIGN KEY ([FilterID_Filter])
    REFERENCES [dbo].[FilterSet]
        ([ID_Filter])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FilterRule'
CREATE INDEX [IX_FK_FilterRule]
ON [dbo].[RuleSet]
    ([FilterID_Filter]);
GO

-- Creating foreign key on [TargetID_Target] in table 'RuleSet'
ALTER TABLE [dbo].[RuleSet]
ADD CONSTRAINT [FK_TargetRule]
    FOREIGN KEY ([TargetID_Target])
    REFERENCES [dbo].[TargetSet]
        ([ID_Target])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetRule'
CREATE INDEX [IX_FK_TargetRule]
ON [dbo].[RuleSet]
    ([TargetID_Target]);
GO

-- Creating foreign key on [BadgeEventID_BadgeEvent] in table 'RuleSet'
ALTER TABLE [dbo].[RuleSet]
ADD CONSTRAINT [FK_BadgeEventRule]
    FOREIGN KEY ([BadgeEventID_BadgeEvent])
    REFERENCES [dbo].[BadgeEventSet]
        ([ID_BadgeEvent])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BadgeEventRule'
CREATE INDEX [IX_FK_BadgeEventRule]
ON [dbo].[RuleSet]
    ([BadgeEventID_BadgeEvent]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------