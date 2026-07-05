# SDD.md — Spec Driven Development

## Proyecto: Sistema de Login y Perfil de Usuario (Prueba Técnica ASP.NET)

**Autor:** [Sergio Prieto]
**Fecha:** Julio 2026
**Basado en:** Diagrama de flujo y diseño de Figma (ver referencia en el correo de la prueba)

---

## 1. Objetivo

Implementar una interfaz funcional basada en un diseño de Figma que cubra:
- Vista de bienvenida
- Vista de login con lógica de intentos fallidos (CVF) y bloqueo temporal
- Vista de perfil de usuario post-login
- Modal de expiración de sesión por inactividad

El foco de evaluación no es el pixel-perfect del diseño, sino la **lógica de negocio**, la **estructura del proyecto** y las **buenas prácticas**.

---

## 2. Alcance (Scope)

### Dentro del alcance
- Login contra base de datos real (SQL Server / LocalDB)
- Contador de validaciones fallidas (CVF) con bloqueo temporal de 15 minutos
- Layout compartido (Header/Footer) entre vistas
- Vista de perfil con datos mockeados post-login
- Modal de sesión por expirar (inactividad simulada en cliente)

### Fuera del alcance (explícitamente mockeado)
- Registro de usuarios (signup) — no existe en el diseño
- Envío real de correos — solo se deja comentado el punto de integración
- Menú lateral funcional — solo HTML estático
- Edición real de campos con ícono de lápiz — solo visual
- Combo box de tipo de teléfono secundario — mock sin opciones reales

---

## 3. Stack Tecnológico

| Capa | Tecnología |
|---|---|
| Framework | ASP.NET Core MVC |
| Front-end | Bootstrap 5 (CDN) |
| Base de datos | SQL Server LocalDB |
| ORM | Entity Framework Core |
| Autenticación | Custom (sin ASP.NET Identity) |
| Sesión de inactividad | JavaScript (setTimeout/setInterval) |

**Justificación de decisiones:**
- No se usa ASP.NET Identity porque la lógica de bloqueo por CVF es un requerimiento de negocio custom que Identity no cubre de forma nativa sin sobreingeniería.
- LocalDB evita depender de una instancia completa de SQL Server, ideal para ejecución local.

---

## 4. Requisitos Funcionales (derivados del análisis del Figma)

| ID | Requisito | Prioridad |
|---|---|---|
| RF-01 | Layout con Header y Footer visibles en todas las vistas | MUST |
| RF-02 | Header cambia contenido según estado de sesión (mock) | SHOULD |
| RF-03 | Vista de bienvenida con card, imagen, título, 2 párrafos y botón de login | MUST |
| RF-04 | Vista de login con selector DNI/CE, usuario, contraseña (con toggle mostrar/ocultar) | MUST |
| RF-05 | Botón "Ingresar" deshabilitado hasta llenar los campos | SHOULD |
| RF-06 | Validar si la cuenta está bloqueada antes de validar credenciales | MUST |
| RF-07 | Validar credenciales contra la base de datos | MUST |
| RF-08 | Si las credenciales son inválidas, incrementar CVF | MUST |
| RF-09 | CVF entre 1 y 4: mostrar mensaje de error y resaltar campos en rojo | MUST |
| RF-10 | CVF llega a 5: bloquear cuenta por 15 minutos y mostrar card de bloqueo | MUST |
| RF-11 | Al bloquear, dejar comentario de integración de envío de correo (N2) | SHOULD |
| RF-12 | Login exitoso redirige a vista de Perfil | MUST |
| RF-13 | Vista de Perfil muestra datos de sesión en Header + card con datos mockeados | MUST |
| RF-14 | Modal de expiración de sesión tras 20 minutos de inactividad, con cuenta regresiva de 49s | SHOULD |
| RF-15 | Si no se extiende la sesión, redirige a login con notificación de expiración | SHOULD |

---

## 5. Modelo de Datos

```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    DocumentType VARCHAR(5) NOT NULL,      -- 'DNI' or 'CE'
    DocumentNumber VARCHAR(20) NOT NULL,
    Username VARCHAR(50) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FailedAttemptsCount INT NOT NULL DEFAULT 0,
    IsLocked BIT NOT NULL DEFAULT 0,
    LockedAt DATETIME NULL,
    FullName VARCHAR(100),
    Position VARCHAR(100),
    Entity VARCHAR(100),
    Status VARCHAR(20)
);
```

**Notas:**
- `FailedAttemptsCount` corresponde al CVF (Contador de Validaciones Fallidas) del diagrama.
- `IsLocked` + `LockedAt` permiten calcular si ya pasaron los 15 minutos de bloqueo sin necesidad de un job en segundo plano.

---

## 6. Diagrama de Flujo → Lógica de Negocio

Traducción directa del diagrama de Figma a reglas de `AuthService`:

```
¿Cuenta bloqueada?
 ├─ Sí → ¿Ya pasaron 15 min desde LockedAt?
 │        ├─ Sí → desbloquear, resetear CVF, continuar validación
 │        └─ No → retornar AccountLocked
 └─ No → continuar

¿Credenciales válidas?
 ├─ Sí → retornar Success
 └─ No → incrementar FailedAttemptsCount
          ├─ count < 5 → retornar InvalidCredentials
          └─ count == 5 → bloquear cuenta, setear LockedAt, retornar AccountJustLocked
```

---

## 7. Arquitectura y Patrones de Diseño

| Patrón | Dónde se aplica | Motivo |
|---|---|---|
| MVC | Estructura general del proyecto | Separación de responsabilidades nativa de ASP.NET Core |
| Repository Pattern | `IUserRepository` / `UserRepository` | Aísla el acceso a datos de la lógica de negocio |
| Service Layer | `IAuthService` / `AuthService` | Concentra la lógica del diagrama de flujo fuera del Controller |
| Result Pattern | `LoginResult` + `LoginStatus` enum | Evita usar excepciones para flujo de control esperado |
| Dependency Injection | Registro en `Program.cs` | Desacopla capas, facilita testing |
| DTO / ViewModel | `LoginViewModel`, `ProfileViewModel` | Evita exponer entidades de base de datos a las vistas |

---

## 8. Estructura del Proyecto

```
/Controllers
    HomeController.cs
    AuthController.cs
    ProfileController.cs
/Models
    /Entities
        User.cs
    /ViewModels
        LoginViewModel.cs
        ProfileViewModel.cs
/Services
    IAuthService.cs
    AuthService.cs
/Repositories
    IUserRepository.cs
    UserRepository.cs
/Data
    AppDbContext.cs
/Views
    /Shared/_Layout.cshtml
    /Home/Welcome.cshtml
    /Auth/Login.cshtml
    /Profile/Index.cshtml
/wwwroot
SDD.md
README.md
```

---

## 9. Contratos de Interfaz (Definition of Done por componente)

```csharp
public enum LoginStatus {
    Success,
    InvalidCredentials,
    AccountLocked,
    AccountJustLocked
}

public class LoginResult {
    public LoginStatus Status { get; set; }
    public User? User { get; set; }
}

public interface IAuthService {
    Task<LoginResult> LoginAsync(string documentType, string documentNumber, string password);
}

public interface IUserRepository {
    Task<User?> GetByDocumentAsync(string documentType, string documentNumber);
    Task UpdateAsync(User user);
}
```

---

## 10. Fuera de alcance / Supuestos

- No hay opción de registro (signup) en el diseño analizado, por lo tanto no se implementa.
- Los datos de perfil (nombres, cargo, entidad, etc.) son mockeados y no se editan realmente en base de datos.
- El combo de teléfono secundario y el ícono de edición (lápiz) quedan como elementos visuales sin funcionalidad.
- El envío de correo de notificación de bloqueo se deja como comentario `// TODO: send lock notification email (N2)` en el código, sin integración real de SMTP.
- La expiración de sesión por 20 minutos de inactividad se simula en cliente (JavaScript) por restricciones de tiempo, en vez de un manejo de sesión real en servidor.

---

## 11. Plan de Ejecución (10 horas)

| Hora | Tarea |
|---|---|
| 1 | Setup del proyecto, conexión a LocalDB, migración inicial, seed de usuarios |
| 2-3 | Layout (Header/Footer) + Vista de Login con Bootstrap |
| 3-4 | Lógica de validación de credenciales |
| 4-5 | Lógica de CVF + bloqueo temporal + mensajes en rojo |
| 5-6 | Vista de Bienvenida + navegación entre vistas |
| 6-7 | Vista de Perfil (3 columnas, datos mockeados) |
| 7-8 | Modal de expiración de sesión con JavaScript |
| 8-9 | Pruebas manuales de todos los flujos + ajustes |
| 9-10 | README + grabación de video + push a GitHub + envío de correo |
