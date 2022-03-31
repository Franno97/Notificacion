Add-Migration "Inicial"  -Context NotificacionDbContext


Update-Database -Context  NotificacionDbContext

# Generar script migracion
# Generar script desde la primera migracion hasta la ultima
Script-Migration -Context NotificacionDbContext 0

# .NET Core CLI

dotnet ef migrations add InitialCreate --startup-project ../../host/Mre.Sb.Notification.HttpApi.Host --context NotificacionDbContext

En la carpeta del Ef. "NotificationDbContext\src\Mre.Sb.NotificacionDbContext.EntityFrameworkCore", ejecutar el comando para actualizar la base de datos.

```
dotnet ef database update --startup-project ../../host/Mre.Sb.Notification.HttpApi.Host --context NotificacionDbContext
```