USE [huellasDB]
GO
/****** Object:  Table [dbo].[Formularios]    Script Date: 2/7/2024 7:37:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Formularios](
	[IdFormulario] [int] IDENTITY(1,1) NOT NULL,
	[Persona1] [nvarchar](100) NULL,
	[Persona2] [nvarchar](100) NULL,
	[Persona3] [nvarchar](100) NULL,
	[Bebe] [bit] NOT NULL,
	[Cerramiento] [bit] NOT NULL,
	[MalaExperiencia] [nvarchar](100) NULL,
	[TipoAnimal] [nvarchar](100) NULL,
	[SexoAnimal] [nvarchar](100) NULL,
	[Fallecio] [nvarchar](100) NULL,
	[PorqueAdopta] [nvarchar](100) NULL,
	[QuePasaria] [nvarchar](100) NULL,
	[SiViaja] [nvarchar](100) NULL,
	[TiempoSolo] [nvarchar](100) NULL,
	[Dia] [nvarchar](100) NULL,
	[Noche] [nvarchar](100) NULL,
	[Necesidades] [nvarchar](100) NULL,
	[Comida] [nvarchar](100) NULL,
	[Enferma] [nvarchar](100) NULL,
	[Dinero] [nvarchar](100) NULL,
	[Visita] [nvarchar](100) NULL,
	[Municipal] [nvarchar](100) NULL,
	[Familia] [nvarchar](100) NULL,
	[Aprobada] [nvarchar](100) NULL,
 CONSTRAINT [PK_Formularios] PRIMARY KEY CLUSTERED 
(
	[IdFormulario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mascotas]    Script Date: 2/7/2024 7:37:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mascotas](
	[IdMascota] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Edad] [nvarchar](100) NOT NULL,
	[Sexo] [nvarchar](1) NOT NULL,
	[FotoUrl] [nvarchar](500) NULL,
	[Inmueble] [nvarchar](100) NOT NULL,
	[Tamano] [nvarchar](200) NOT NULL,
	[Esterilizado] [nvarchar](1) NOT NULL,
	[IdUsuario] [int] NULL,
 CONSTRAINT [PK_Mascotas] PRIMARY KEY CLUSTERED 
(
	[IdMascota] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mensajes]    Script Date: 2/7/2024 7:37:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mensajes](
	[IdMensaje] [int] IDENTITY(1,1) NOT NULL,
	[IdMascota] [int] NOT NULL,
	[IdRemitente] [int] NOT NULL,
	[IdDestinatario] [int] NOT NULL,
	[IdFormulario] [int] NOT NULL,
 CONSTRAINT [PK_Mensajes] PRIMARY KEY CLUSTERED 
(
	[IdMensaje] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2/7/2024 7:37:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 2/7/2024 7:37:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[NombreUsuario] [nvarchar](100) NOT NULL,
	[Clave] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[FotoUrl] [nvarchar](500) NULL,
	[IdRol] [int] NOT NULL,
	[Sexo] [nvarchar](1) NULL,
	[Inmueble] [nvarchar](100) NULL,
	[Tamano] [nvarchar](200) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([IdRol], [Descripcion]) VALUES (1, N'Administrador')
GO
INSERT [dbo].[Roles] ([IdRol], [Descripcion]) VALUES (2, N'Rescatador')
GO
INSERT [dbo].[Roles] ([IdRol], [Descripcion]) VALUES (3, N'Adoptante')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 
GO
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [NombreUsuario], [Clave], [Email], [FotoUrl], [IdRol], [Sexo], [Inmueble], [Tamano]) VALUES (5, N'"Huellitas ', N'Centralinas"', N'hcentralinas', N'UuwB2BiZEMmIebltg01PMw==', N'huellitascentralinas@gmail.com', N'images\usuario\1f05c11c-011c-407c-81a7-182d1357e2c0.png', 1, NULL, N'', NULL)
GO
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
ALTER TABLE [dbo].[Mascotas]  WITH CHECK ADD  CONSTRAINT [FK_Mascotas_Usuarios_IdUsuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Mascotas] CHECK CONSTRAINT [FK_Mascotas_Usuarios_IdUsuario]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Formularios_IdFormulario] FOREIGN KEY([IdFormulario])
REFERENCES [dbo].[Formularios] ([IdFormulario])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Formularios_IdFormulario]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Mascotas_IdMascota] FOREIGN KEY([IdMascota])
REFERENCES [dbo].[Mascotas] ([IdMascota])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Mascotas_IdMascota]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Usuarios_IdDestinatario] FOREIGN KEY([IdDestinatario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Usuarios_IdDestinatario]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Usuarios_IdRemitente] FOREIGN KEY([IdRemitente])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Usuarios_IdRemitente]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Roles_IdRol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Roles_IdRol]
GO
