

<!-- local-development:start -->
# Desarrollo local

Puedes abrir este repositorio en VS Code, Antigravity o Sublime Text. El compilador y los servicios necesarios dependen del componente indicado abajo.

## Visual Studio 2022 / .NET Framework

Instala la carga de trabajo de escritorio .NET y el paquete de destino indicado en la tabla. Los proyectos ASP.NET también requieren la carga web e IIS Express. Abre la solución `.sln` correspondiente y selecciona el proyecto de aplicación para iniciar.

```powershell
# Ejecutar desde Developer PowerShell de Visual Studio 2022
$solutionDir = (Resolve-Path 'EtzJaimWebPage').Path + '\'
MSBuild.exe 'EtzJaimWebPage/EtzJaimWebPage/EtzJaimWebPage.csproj' /restore /p:RestorePackagesConfig=true /p:Configuration=Debug /p:DeployOnBuild=false "/p:SolutionDir=$solutionDir"
```

## Bases de datos

Revisa los scripts de esquema y crea una base de datos de prueba separada. SSMS 20 y pgAdmin 4 son clientes; SQL Server y PostgreSQL son motores distintos. Los scripts SQL Server no se ejecutan directamente en PostgreSQL. Las restauraciones deben hacerse sobre una base de prueba nueva. PostgreSQL 17 y 18 requieren comprobarse por separado.

## Componentes y validación

Inventario y compilaciones locales del 2026-11-15. Una compilación correcta no acredita todavía interacción, compatibilidad de base de datos, instalador ni despliegue.

| Componente | Tipo / destino | Validación |
| --- | --- | --- |
| [EtzJaimWebPage/EtzJaimWebPage.sln](<EtzJaimWebPage/EtzJaimWebPage.sln>) | solution | Sin ejecutar |
| [EtzJaimWebPage/EtzJaimWebPage/EtzJaimWebPage.csproj](<EtzJaimWebPage/EtzJaimWebPage/EtzJaimWebPage.csproj>) | dotnet / v4.8.1 | Compila; prueba funcional pendiente |
| [WebService/WebService/WebService.csproj](<WebService/WebService/WebService.csproj>) | dotnet / v4.8.1 | Compila; prueba funcional pendiente |
| [WebService/WebServiceEtzJaim.sln](<WebService/WebServiceEtzJaim.sln>) | solution | Sin ejecutar |

## Archivos ignorados

`.gitignore` excluye credenciales locales, estado del editor y salidas de las herramientas detectadas. No deja de versionar archivos que ya estaban en Git. Los manifiestos, archivos de bloqueo, recursos fuente y scripts de esquema forman parte del código.
<!-- local-development:end -->

<!-- improvements-20261119:start -->
## Mejoras y verificación — 2026-11-19 (Seguridad de autenticación/autorización, anti-SQLi y resiliencia PostgreSQL/WCF)

- Corregida la inyección SQL en `UsuarioPost.user_Ajax` y parametrizadas `habilitarUsuario` y `cargarAdmins_Clientes`; eliminada la exposición de hashes en listados y añadido hash SHA-256 con verificación compatible hacia atrás con MD5.
- Corregido el bug `Instancias > 1` en `AccesoDatosPostgreSql.cs` (dejaba conexiones sin abrir), añadido soporte de variables de entorno `ETZJAIM_PG_*` y cierre seguro de clientes SOAP en `service.cs`.
- Corregidos los 3 bugs de autoasignación `pro_estado` y el bypass de autorización en acciones `POST` (`Create`, `Borrar`, `EditProduct`) de `ProductosController`.
- Validación: compilación MSBuild de `WebService` y `EtzJaimWebPage`, más 13 aserciones automatizadas de configuración PG, hashing y control de acceso en `ProductosController`.
<!-- improvements-20261119:end -->
