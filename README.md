# StockMaster Pro - Backend API

Sistema completo de gestión de inventario con .NET 9.0, Entity Framework Core y SQL Server.

## 🚀 Características

- ✅ API REST completa con .NET 9.0
- ✅ Entity Framework Core con SQL Server
- ✅ CRUD completo para Productos, Categorías, Proveedores y Movimientos
- ✅ Búsqueda y filtrado de productos
- ✅ Gestión de stock en tiempo real
- ✅ Alertas de stock bajo
- ✅ Historial de movimientos de inventario
- ✅ Documentación con Swagger
- ✅ CORS habilitado

## 📋 Requisitos Previos

- .NET 9.0 SDK o superior
- SQL Server 2019 o superior (puede usar SQL Server Express gratuito)
- Visual Studio 2022, VS Code o Rider (opcional)

## ⚙️ Configuración

### 1. Instalar SQL Server

Si no tienes SQL Server instalado:

1. Descarga SQL Server Express (gratuito): https://www.microsoft.com/es-es/sql-server/sql-server-downloads
2. Descarga SQL Server Management Studio (SSMS): https://learn.microsoft.com/es-es/sql/ssms/download-sql-server-management-studio-ssms

### 2. Crear la Base de Datos

Opción A - Usando el script SQL:
```sql
-- Ejecuta este script en SQL Server Management Studio
-- El archivo está en: Database/StockMasterDB_Script.sql
```

Opción B - Usando Entity Framework Migrations:
```bash
cd StockMaster.API
dotnet ef database update
```

### 3. Configurar la Cadena de Conexión

Edita el archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StockMasterDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Notas:**
- Si usas SQL Server con autenticación de usuario, cambia a: `"Server=localhost;Database=StockMasterDB;User Id=tu_usuario;Password=tu_contraseña;TrustServerCertificate=True;"`
- Si tu servidor SQL tiene un nombre diferente, cámbialo en lugar de `localhost`

### 4. Ejecutar la API

```bash
cd StockMaster.API
dotnet run
```

La API estará disponible en:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5000
- Swagger UI: https://localhost:7001/swagger

## 📚 Endpoints de la API

### Productos
- `GET /api/Products` - Obtener todos los productos
- `GET /api/Products/{id}` - Obtener producto por ID
- `GET /api/Products/search?q={query}` - Buscar productos
- `GET /api/Products/low-stock` - Productos con stock bajo
- `GET /api/Products/stats` - Estadísticas de productos
- `POST /api/Products` - Crear nuevo producto
- `PUT /api/Products/{id}` - Actualizar producto
- `DELETE /api/Products/{id}` - Eliminar producto (soft delete)

### Categorías
- `GET /api/Categories` - Obtener todas las categorías
- `GET /api/Categories/{id}` - Obtener categoría por ID
- `POST /api/Categories` - Crear nueva categoría
- `PUT /api/Categories/{id}` - Actualizar categoría
- `DELETE /api/Categories/{id}` - Eliminar categoría

### Proveedores
- `GET /api/Suppliers` - Obtener todos los proveedores
- `GET /api/Suppliers/active` - Obtener proveedores activos
- `GET /api/Suppliers/{id}` - Obtener proveedor por ID
- `POST /api/Suppliers` - Crear nuevo proveedor
- `PUT /api/Suppliers/{id}` - Actualizar proveedor
- `DELETE /api/Suppliers/{id}` - Eliminar proveedor
- `PATCH /api/Suppliers/{id}/toggle-status` - Activar/desactivar proveedor

### Movimientos
- `GET /api/Movements` - Obtener todos los movimientos
- `GET /api/Movements/{id}` - Obtener movimiento por ID
- `GET /api/Movements/product/{productId}` - Movimientos por producto
- `GET /api/Movements/stats` - Estadísticas de movimientos
- `POST /api/Movements` - Registrar nuevo movimiento

## 🔧 Estructura del Proyecto

```
StockMaster.API/
├── Controllers/          # Controladores de la API
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── SuppliersController.cs
│   └── MovementsController.cs
├── Models/              # Modelos de datos
│   ├── Product.cs
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Movement.cs
├── Data/                # Contexto de base de datos
│   └── StockMasterDbContext.cs
├── Migrations/          # Migraciones de EF Core
├── appsettings.json    # Configuración
└── Program.cs          # Punto de entrada
```

## 🎯 Conectar con el Frontend

El frontend (StockMaster Pro) debe conectarse a esta API.

1. Asegúrate de que la API esté corriendo
2. En el frontend, edita `js/api-service.js`:
   ```javascript
   const API_BASE_URL = 'https://localhost:7001/api';
   ```
3. Cambia el puerto si es necesario

## 📦 Datos de Prueba

La base de datos incluye datos iniciales:
- 6 Categorías (Electrónica, Ropa, Alimentos, etc.)
- 4 Proveedores de Argentina
- 8 Productos de ejemplo
- Stock configurado para cada producto

## 🐛 Solución de Problemas

### Error de conexión a SQL Server

```
A network-related or instance-specific error occurred
```

**Solución:**
1. Verifica que SQL Server esté corriendo
2. Abre "SQL Server Configuration Manager"
3. Habilita TCP/IP en "SQL Server Network Configuration"
4. Reinicia el servicio SQL Server

### Error de certificado SSL

```
The certificate chain was issued by an authority that is not trusted
```

**Solución:**
Agrega `TrustServerCertificate=True` a tu cadena de conexión.

### Puerto en uso

```
Unable to bind to https://localhost:7001
```

**Solución:**
Edita `Properties/launchSettings.json` y cambia los puertos.

## 📝 Licencia

Este proyecto es propiedad de Matías Di Girolamo y puede ser comercializado.

## 👨‍💻 Autor

Desarrollado por **Matías Di Girolamo**
- LinkedIn: https://www.linkedin.com/in/matiasdigirolamo/
- Email: matidigi23@gmail.com
