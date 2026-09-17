# TFC · AppEventos

Aplicación full-stack para la gestión de torneos y combates de deportes de combate. Permite a los **organizadores** crear y gestionar torneos, inscribir participantes y organizar combates desde una web de administración, mientras que los **luchadores** consultan sus torneos, combates y resultados desde una app móvil.

Trabajo Fin de Ciclo (TFC) del Técnico Superior en Desarrollo de Aplicaciones Multiplataforma (DAM) — IES Segundo de Chomón, Teruel. Finalizado en junio de 2025.

## Funcionalidades principales

- Registro y autenticación de usuarios con **JWT**, con roles diferenciados (organizador / luchador).
- Creación y gestión de torneos por parte del organizador.
- Inscripción de participantes en un torneo.
- Organización de combates entre luchadores dentro de un torneo.
- Registro de resultados de combates.
- Consulta de torneos y combates propios desde la app móvil (luchador).
- Documentación de la API interactiva con Swagger.

## Arquitectura

El backend sigue los principios de **Clean Architecture**, separando responsabilidades en capas independientes:

```
TFC-AppEventos/
├── TFC.AppEventos.Domain.Entities          # Entidades del dominio (Tournament, Fight, Fighter, User...)
├── TFC.AppEventos.Application.DTO          # DTOs de entrada/salida de la API
├── TFC.AppEventos.Application.Interface    # Contratos de los casos de uso
├── TFC.AppEventos.Application.Main         # Implementación de la lógica de negocio
├── TFC.AppEventos.Infraestructure.Interface# Contratos de acceso a datos
├── TFC.AppEventos.Interface.Repository     # Implementación de los repositorios (EF Core)
├── TFC.AppEventos.Database.Context         # DbContext y configuración de Entity Framework Core
├── TFC.AppEventos.Transversal.Utils        # Utilidades comunes (hashing de contraseñas, respuestas base...)
├── TFC.AppEventos.Service.WebApi           # API REST (ASP.NET Core Web API + JWT + Swagger)
└── OrganizerWeb                            # Panel web del organizador (ASP.NET Core Razor Pages)
```

La API expone los datos que consumen tanto el panel web de organizador como la app móvil de los luchadores, de forma que la lógica de negocio vive en un único lugar.

## Stack tecnológico

**Backend**
- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core 9 (proveedor SQL Server)
- Autenticación JWT
- Swagger / Swashbuckle para documentación de la API

**Web (panel de organizador)**
- ASP.NET Core Razor Pages
- Bootstrap

**App móvil (luchadores)**
- Flutter
- Dio (cliente HTTP)
- Provider (gestión de estado)
- Flutter Secure Storage (almacenamiento seguro del token)
- Google Sign-In

**Base de datos**
- SQL Server

**Testing de API**
- Colección de [Bruno](https://www.usebruno.com/) incluida en `/Bruno` con peticiones de ejemplo para autenticación, luchadores y torneos.

## Puesta en marcha

### Requisitos previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server o SQL Server LocalDB
- [Flutter SDK](https://docs.flutter.dev/get-started/install) (para ejecutar la app móvil)

### 1. Base de datos
Ejecuta el script `TFC-Database.sql` en tu instancia de SQL Server para crear la base de datos y las tablas necesarias. Ajusta las rutas de los ficheros `.mdf`/`.ldf` del script a tu entorno local.

### 2. API (backend)
```bash
cd TFC-AppEventos
dotnet restore
```
Configura la cadena de conexión y la clave JWT en `TFC.AppEventos.Service.WebApi/appsettings.Development.json` (no subas credenciales reales a un repositorio público; usa `dotnet user-secrets` en local).

```bash
cd TFC.AppEventos.Service.WebApi
dotnet run
```
La documentación interactiva de la API estará disponible en Swagger una vez levantado el servicio.

### 3. Panel web del organizador
```bash
cd TFC-AppEventos/OrganizerWeb
dotnet run
```

### 4. App móvil (Flutter)
```bash
cd Flutter/app_eventos
flutter pub get
flutter run
```
Configura la URL base de la API en el fichero de entorno correspondiente (`flutter_dotenv`) antes de ejecutar la app.

### 5. Probar la API con Bruno
Importa la colección incluida en `/Bruno` desde [Bruno](https://www.usebruno.com/) para probar los endpoints de autenticación, luchadores y torneos sin necesidad de levantar la web o la app móvil.

## Autor

**Mario Pérez Gracia**
[LinkedIn](https://www.linkedin.com/in/mario-p%C3%A9rez-88222b374/) · mperezgracia@proton.me
