
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/18/2019 14:28:46
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

IF OBJECT_ID(N'[dbo].[FK_UserFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FieldUserSet] DROP CONSTRAINT [FK_UserFieldUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSet] DROP CONSTRAINT [FK_UserPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_EventPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSet] DROP CONSTRAINT [FK_EventPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_EventBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSet] DROP CONSTRAINT [FK_EventBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PositionSet] DROP CONSTRAINT [FK_FieldPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSet] DROP CONSTRAINT [FK_BadgeBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_BadgeEventPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PositionSet] DROP CONSTRAINT [FK_BadgeEventPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldEventField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldSet] DROP CONSTRAINT [FK_FieldEventField];
GO
IF OBJECT_ID(N'[dbo].[FK_EventEventField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventFieldSet] DROP CONSTRAINT [FK_EventEventField];
GO
IF OBJECT_ID(N'[dbo].[FK_EventFieldFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FieldUserSet] DROP CONSTRAINT [FK_EventFieldFieldUser];
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
IF OBJECT_ID(N'[dbo].[FieldUserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FieldUserSet];
GO
IF OBJECT_ID(N'[dbo].[PrintBadgeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrintBadgeSet];
GO
IF OBJECT_ID(N'[dbo].[BadgeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BadgeSet];
GO
IF OBJECT_ID(N'[dbo].[BadgeEventSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BadgeEventSet];
GO
IF OBJECT_ID(N'[dbo].[PositionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PositionSet];
GO
IF OBJECT_ID(N'[dbo].[EventFieldSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventFieldSet];
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
    [Dimension_Y] float  NOT NULL
);
GO

-- Creating table 'BadgeEventSet'
CREATE TABLE [dbo].[BadgeEventSet] (
    [BadgeID_Badge] int  NOT NULL,
    [EventID_Event] int  NOT NULL,
    [TypeBadge] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PositionSet'
CREATE TABLE [dbo].[PositionSet] (
    [BadgeEventBadgeID_Badge] int  NOT NULL,
    [BadgeEventEventID_Event] int  NOT NULL,
    [FieldID_Field] int  NOT NULL,
    [Position_X] float  NOT NULL,
    [Position_Y] float  NOT NULL
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

-- Creating primary key on [BadgeID_Badge], [EventID_Event] in table 'BadgeEventSet'
ALTER TABLE [dbo].[BadgeEventSet]
ADD CONSTRAINT [PK_BadgeEventSet]
    PRIMARY KEY CLUSTERED ([BadgeID_Badge], [EventID_Event] ASC);
GO

-- Creating primary key on [BadgeEventBadgeID_Badge], [BadgeEventEventID_Event], [FieldID_Field] in table 'PositionSet'
ALTER TABLE [dbo].[PositionSet]
ADD CONSTRAINT [PK_PositionSet]
    PRIMARY KEY CLUSTERED ([BadgeEventBadgeID_Badge], [BadgeEventEventID_Event], [FieldID_Field] ASC);
GO

-- Creating primary key on [FieldID_Field], [EventID_Event] in table 'EventFieldSet'
ALTER TABLE [dbo].[EventFieldSet]
ADD CONSTRAINT [PK_EventFieldSet]
    PRIMARY KEY CLUSTERED ([FieldID_Field], [EventID_Event] ASC);
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

-- Creating foreign key on [BadgeID_Badge] in table 'BadgeEventSet'
ALTER TABLE [dbo].[BadgeEventSet]
ADD CONSTRAINT [FK_BadgeBadgeEvent]
    FOREIGN KEY ([BadgeID_Badge])
    REFERENCES [dbo].[BadgeSet]
        ([ID_Badge])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [BadgeEventBadgeID_Badge], [BadgeEventEventID_Event] in table 'PositionSet'
ALTER TABLE [dbo].[PositionSet]
ADD CONSTRAINT [FK_BadgeEventPosition]
    FOREIGN KEY ([BadgeEventBadgeID_Badge], [BadgeEventEventID_Event])
    REFERENCES [dbo].[BadgeEventSet]
        ([BadgeID_Badge], [EventID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------