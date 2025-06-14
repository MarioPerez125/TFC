USE [master]
GO
/****** Object:  Database [TFC-DB2]    Script Date: 10/06/2025 14:19:25 ******/
CREATE DATABASE [TFC-DB2]
 CONTAINMENT = NONE
 ON  PRIMARY 
 /*
 CAMBIA LAS RUTAS CON LAS DE TU PC C:\Users\...\TFC-DB2.mdf
 */
( NAME = N'TFC-DB2', FILENAME = N'C:\Users\PC\TFC-DB2.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TFC-DB2_log', FILENAME = N'C:\Users\PC\TFC-DB2_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TFC-DB2] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TFC-DB2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TFC-DB2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TFC-DB2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TFC-DB2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TFC-DB2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TFC-DB2] SET ARITHABORT OFF 
GO
ALTER DATABASE [TFC-DB2] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [TFC-DB2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TFC-DB2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TFC-DB2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TFC-DB2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TFC-DB2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TFC-DB2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TFC-DB2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TFC-DB2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TFC-DB2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TFC-DB2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TFC-DB2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TFC-DB2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TFC-DB2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TFC-DB2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TFC-DB2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TFC-DB2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TFC-DB2] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TFC-DB2] SET  MULTI_USER 
GO
ALTER DATABASE [TFC-DB2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TFC-DB2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TFC-DB2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TFC-DB2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TFC-DB2] SET DELAYED_DURABILITY = DISABLED 
GO
USE [TFC-DB2]
GO
/****** Object:  Table [dbo].[Fighters]    Script Date: 10/06/2025 14:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fighters](
	[FighterId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[WeightClass] [nvarchar](50) NULL,
	[Height] [int] NULL,
	[Reach] [int] NULL,
	[Wins] [int] NULL,
	[Losses] [int] NULL,
	[Draws] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[FighterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FightResults]    Script Date: 10/06/2025 14:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FightResults](
	[FightResultId] [int] IDENTITY(1,1) NOT NULL,
	[FightId] [int] NOT NULL,
	[WinnerId] [int] NULL,
	[Method] [nvarchar](50) NOT NULL,
	[Duration] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[FightResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fights]    Script Date: 10/06/2025 14:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fights](
	[FightId] [int] IDENTITY(1,1) NOT NULL,
	[TournamentId] [int] NOT NULL,
	[Fighter1Id] [int] NOT NULL,
	[Fighter2Id] [int] NOT NULL,
	[WinnerId] [int] NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[FightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participantes]    Script Date: 10/06/2025 14:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participantes](
	[ParticipanteId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TournamentId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParticipanteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Participante_User_Tournament] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[TournamentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tournaments]    Script Date: 10/06/2025 14:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tournaments](
	[TournamentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[StartDate] [nvarchar](50) NULL,
	[EndDate] [nvarchar](50) NULL,
	[SportType] [nvarchar](50) NOT NULL,
	[OrganizerId] [int] NOT NULL,
	[Arena] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TournamentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/06/2025 14:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Phone] [int] NULL,
	[BirthDate] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Fighters] ADD  DEFAULT ((0)) FOR [Height]
GO
ALTER TABLE [dbo].[Fighters] ADD  DEFAULT ((0)) FOR [Reach]
GO
ALTER TABLE [dbo].[Fighters] ADD  DEFAULT ((0)) FOR [Wins]
GO
ALTER TABLE [dbo].[Fighters] ADD  DEFAULT ((0)) FOR [Losses]
GO
ALTER TABLE [dbo].[Fighters] ADD  DEFAULT ((0)) FOR [Draws]
GO
ALTER TABLE [dbo].[Fights] ADD  DEFAULT ('Scheduled') FOR [Status]
GO
ALTER TABLE [dbo].[Fighters]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Fighters]  WITH CHECK ADD  CONSTRAINT [FK_Fighters_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Fighters] CHECK CONSTRAINT [FK_Fighters_Users]
GO
ALTER TABLE [dbo].[FightResults]  WITH CHECK ADD  CONSTRAINT [FK_FightResults_Fights] FOREIGN KEY([FightId])
REFERENCES [dbo].[Fights] ([FightId])
GO
ALTER TABLE [dbo].[FightResults] CHECK CONSTRAINT [FK_FightResults_Fights]
GO
ALTER TABLE [dbo].[Fights]  WITH CHECK ADD FOREIGN KEY([Fighter1Id])
REFERENCES [dbo].[Fighters] ([FighterId])
GO
ALTER TABLE [dbo].[Fights]  WITH CHECK ADD FOREIGN KEY([Fighter2Id])
REFERENCES [dbo].[Fighters] ([FighterId])
GO
ALTER TABLE [dbo].[Fights]  WITH CHECK ADD FOREIGN KEY([TournamentId])
REFERENCES [dbo].[Tournaments] ([TournamentId])
GO
ALTER TABLE [dbo].[Fights]  WITH CHECK ADD FOREIGN KEY([WinnerId])
REFERENCES [dbo].[Fighters] ([FighterId])
GO
ALTER TABLE [dbo].[Participantes]  WITH CHECK ADD  CONSTRAINT [FK_Participantes_Tournaments] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[Tournaments] ([TournamentId])
GO
ALTER TABLE [dbo].[Participantes] CHECK CONSTRAINT [FK_Participantes_Tournaments]
GO
ALTER TABLE [dbo].[Participantes]  WITH CHECK ADD  CONSTRAINT [FK_Participantes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Participantes] CHECK CONSTRAINT [FK_Participantes_Users]
GO
ALTER TABLE [dbo].[Tournaments]  WITH CHECK ADD FOREIGN KEY([OrganizerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Fighter' OR [Role]='Organizer' OR [Role]='Admin'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Fighter' OR [Role]='Organizer' OR [Role]='Admin'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Fighter' OR [Role]='Organizer' OR [Role]='Admin'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Fighter' OR [Role]='Organizer' OR [Role]='Admin'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Fighter' OR [Role]='Organizer' OR [Role]='Admin'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Fighter' OR [Role]='Organizer' OR [Role]='Admin'))
GO
USE [master]
GO
ALTER DATABASE [TFC-DB2] SET  READ_WRITE 
GO
