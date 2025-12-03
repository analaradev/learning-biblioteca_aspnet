# Mi Biblioteca Personal - Proyecto ASP.NET Core

Este repositorio contiene un proyecto de gestión de biblioteca personal desarrollado con ASP.NET Core, 
implementando el patrón Modelo-Vista-Controlador (MVC) para organizar y catalogar tu colección de libros.

## 📚 Características principales

- **Aplicación web construida con ASP.NET Core MVC**
- **Autenticación de usuarios** (cada usuario gestiona su propia biblioteca)
- **Gestión completa de inventario de libros** con información detallada:
  - Título y Autor
  - ISBN
  - Editorial y Año de Publicación
  - Género literario
  - Sinopsis/Descripción
  - Portada del libro (imagen)
  - Estado de lectura (leído/pendiente)
  
- **Funcionalidades CRUD completas:**
  - Crear nuevos registros de libros
  - Visualizar biblioteca completa
  - Actualizar información de libros
  - Eliminar libros del inventario

- **Características adicionales:**
  - Búsqueda por título, autor, ISBN o género
  - Filtrado por estado de lectura
  - Drag & Drop para reordenar libros
  - Upload de portadas de libros

## 🚀 Instrucciones para ejecutar el proyecto

### 1. Clonar el repositorio
```bash
git clone https://github.com/Sofifdz/ASP.NET-Core.git
cd ASP.NET-Core
```

### 2. Abrir el proyecto
Abre la solución en Visual Studio o Visual Studio Code

### 3. Configurar la base de datos
- La aplicación usa SQLite por defecto (no requiere configuración adicional)
- Ejecuta las migraciones para crear la base de datos:
```bash
dotnet ef migrations add InitialCreateBooks
dotnet ef database update
```

### 4. Ejecutar la aplicación
```bash
dotnet run
```
Accede a la aplicación en: **https://localhost:7236**

## 🛠️ Tecnologías utilizadas

- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **SQLite** (base de datos)
- **ASP.NET Core Identity** (autenticación)
- **Bootstrap 5** (diseño responsivo)
- **Bootstrap Icons**
- **SortableJS** (drag & drop)

## 🔐 Credenciales de prueba

- **Usuario:** `prueba@gmail.com`
- **Contraseña:** `Prueba01.`

## 📖 Estructura del proyecto

```
Taller ASP.NET Core/
├── Controllers/        # BooksController, HomeController
├── Models/            # Book.cs
├── Views/             # Vistas Razor (Books, Home, Shared)
├── Data/              # ApplicationDbContext
├── wwwroot/           # Archivos estáticos (CSS, JS, imágenes)
└── Program.cs         # Configuración de la aplicación
```

## 🎯 Funcionalidades destacadas

### Panel de Biblioteca
- Visualización en tarjetas con información resumida
- Vista detallada al hacer clic en cada libro
- Indicadores visuales de estado de lectura

### Formularios inteligentes
- Validación de datos en cliente y servidor
- Preview de portadas antes de guardar
- Campos específicos para datos bibliográficos

### Experiencia de usuario
- Interfaz responsiva que se adapta a cualquier dispositivo
- Búsqueda en tiempo real
- Animaciones suaves y feedback visual

---

**Desarrollado como proyecto educativo de ASP.NET Core MVC**
