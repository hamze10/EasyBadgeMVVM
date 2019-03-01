
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/19/2019 10:44:46
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
IF OBJECT_ID(N'[dbo].[FK_UserUserEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEventSet] DROP CONSTRAINT [FK_UserUserEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSet] DROP CONSTRAINT [FK_UserPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_EventUserEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEventSet] DROP CONSTRAINT [FK_EventUserEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_EventPrintBadge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintBadgeSet] DROP CONSTRAINT [FK_EventPrintBadge];
GO
IF OBJECT_ID(N'[dbo].[FK_EventBadgeEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BadgeEventSet] DROP CONSTRAINT [FK_EventBadgeEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldFieldUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FieldUserSet] DROP CONSTRAINT [FK_FieldFieldUser];
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
IF OBJECT_ID(N'[dbo].[FK_FieldUserUserEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEventSet] DROP CONSTRAINT [FK_FieldUserUserEvent];
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
IF OBJECT_ID(N'[dbo].[UserEventSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserEventSet];
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

-- Creating table 'FieldUserSet'
CREATE TABLE [dbo].[FieldUserSet] (
    [ID_FieldUser] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [AdditionnalInformation] nvarchar(max)  NULL,
    [UserID_User] int  NOT NULL,
    [FieldID_Field] int  NOT NULL
);
GO

-- Creating table 'UserEventSet'
CREATE TABLE [dbo].[UserEventSet] (
    [UserID_User] int  NOT NULL,
    [EventID_Event] int  NOT NULL,
    [FieldUserID_FieldUser1] int  NOT NULL
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

-- Creating primary key on [ID_FieldUser] in table 'FieldUserSet'
ALTER TABLE [dbo].[FieldUserSet]
ADD CONSTRAINT [PK_FieldUserSet]
    PRIMARY KEY CLUSTERED ([ID_FieldUser] ASC);
GO

-- Creating primary key on [UserID_User], [EventID_Event], [FieldUserID_FieldUser1] in table 'UserEventSet'
ALTER TABLE [dbo].[UserEventSet]
ADD CONSTRAINT [PK_UserEventSet]
    PRIMARY KEY CLUSTERED ([UserID_User], [EventID_Event], [FieldUserID_FieldUser1] ASC);
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserID_User] in table 'FieldUserSet'
ALTER TABLE [dbo].[FieldUserSet]
ADD CONSTRAINT [FK_UserFieldUser]
    FOREIGN KEY ([UserID_User])
    REFERENCES [dbo].[UserSet]
        ([ID_User])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserFieldUser'
CREATE INDEX [IX_FK_UserFieldUser]
ON [dbo].[FieldUserSet]
    ([UserID_User]);
GO

-- Creating foreign key on [UserID_User] in table 'UserEventSet'
ALTER TABLE [dbo].[UserEventSet]
ADD CONSTRAINT [FK_UserUserEvent]
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

-- Creating foreign key on [EventID_Event] in table 'UserEventSet'
ALTER TABLE [dbo].[UserEventSet]
ADD CONSTRAINT [FK_EventUserEvent]
    FOREIGN KEY ([EventID_Event])
    REFERENCES [dbo].[EventSet]
        ([ID_Event])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventUserEvent'
CREATE INDEX [IX_FK_EventUserEvent]
ON [dbo].[UserEventSet]
    ([EventID_Event]);
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

-- Creating foreign key on [FieldID_Field] in table 'FieldUserSet'
ALTER TABLE [dbo].[FieldUserSet]
ADD CONSTRAINT [FK_FieldFieldUser]
    FOREIGN KEY ([FieldID_Field])
    REFERENCES [dbo].[FieldSet]
        ([ID_Field])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldFieldUser'
CREATE INDEX [IX_FK_FieldFieldUser]
ON [dbo].[FieldUserSet]
    ([FieldID_Field]);
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

-- Creating foreign key on [FieldUserID_FieldUser1] in table 'UserEventSet'
ALTER TABLE [dbo].[UserEventSet]
ADD CONSTRAINT [FK_FieldUserUserEvent]
    FOREIGN KEY ([FieldUserID_FieldUser1])
    REFERENCES [dbo].[FieldUserSet]
        ([ID_FieldUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldUserUserEvent'
CREATE INDEX [IX_FK_FieldUserUserEvent]
ON [dbo].[UserEventSet]
    ([FieldUserID_FieldUser1]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------