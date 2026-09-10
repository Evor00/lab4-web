/* ============================================================
   NeptunoDB - SCRIPT COMPLETO (Base de Datos + Tablas + Datos + SP)
   Desarrollo de Aplicaciones Empresariales Avanzado (Semana 04)
   ============================================================ */

IF DB_ID(N'NeptunoDB') IS NULL
BEGIN
    CREATE DATABASE NeptunoDB;
END
GO

USE NeptunoDB;
GO

/* ============================================================
   1. TABLAS PRINCIPALES
   ============================================================ */

IF OBJECT_ID(N'dbo.DetallePedidos', N'U') IS NOT NULL DROP TABLE dbo.DetallePedidos;
IF OBJECT_ID(N'dbo.Pedidos', N'U') IS NOT NULL DROP TABLE dbo.Pedidos;
IF OBJECT_ID(N'dbo.Productos', N'U') IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID(N'dbo.Transportistas', N'U') IS NOT NULL DROP TABLE dbo.Transportistas;
IF OBJECT_ID(N'dbo.Empleados', N'U') IS NOT NULL DROP TABLE dbo.Empleados;
IF OBJECT_ID(N'dbo.Clientes', N'U') IS NOT NULL DROP TABLE dbo.Clientes;
IF OBJECT_ID(N'dbo.Proveedores', N'U') IS NOT NULL DROP TABLE dbo.Proveedores;
IF OBJECT_ID(N'dbo.Categorias', N'U') IS NOT NULL DROP TABLE dbo.Categorias;
GO

CREATE TABLE dbo.Categorias (
    CategoriaID     INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria NVARCHAR(30)  NOT NULL,
    Descripcion     NVARCHAR(200) NULL
);
GO

CREATE TABLE dbo.Proveedores (
    ProveedorID     INT IDENTITY(1,1) PRIMARY KEY,
    CompaniaNombre  NVARCHAR(60)  NOT NULL,
    NombreContacto  NVARCHAR(40)  NULL,
    CargoContacto   NVARCHAR(40)  NULL,
    Direccion       NVARCHAR(80)  NULL,
    Ciudad          NVARCHAR(30)  NULL,
    CodigoPostal    NVARCHAR(10)  NULL,
    Pais            NVARCHAR(30)  NULL,
    Telefono        NVARCHAR(24)  NULL,
    Fax             NVARCHAR(24)  NULL
);
GO

CREATE TABLE dbo.Clientes (
    ClienteID       INT IDENTITY(1,1) PRIMARY KEY,
    Empresa         NVARCHAR(60) NOT NULL,
    NombreContacto  NVARCHAR(40) NULL,
    Ciudad          NVARCHAR(30) NULL,
    Pais            NVARCHAR(30) NULL,
    Telefono        NVARCHAR(24) NULL
);
GO

CREATE TABLE dbo.Empleados (
    EmpleadoID        INT IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(20) NOT NULL,
    Apellidos         NVARCHAR(30) NOT NULL,
    Cargo             NVARCHAR(40) NULL,
    FechaNacimiento   DATE NULL,
    FechaContratacion DATE NULL,
    Ciudad            NVARCHAR(30) NULL,
    Pais              NVARCHAR(30) NULL
);
GO

CREATE TABLE dbo.Transportistas (
    TransportistaID INT IDENTITY(1,1) PRIMARY KEY,
    CompaniaNombre  NVARCHAR(60) NOT NULL,
    Telefono        NVARCHAR(24) NULL
);
GO

CREATE TABLE dbo.Productos (
    ProductoID           INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto       NVARCHAR(60)   NOT NULL,
    ProveedorID          INT            NULL,
    CategoriaID          INT            NULL,
    CantidadPorUnidad    NVARCHAR(30)   NULL,
    PrecioUnidad         DECIMAL(10,2)  NOT NULL DEFAULT 0,
    UnidadesEnExistencia SMALLINT       NOT NULL DEFAULT 0,
    UnidadesEnPedido     SMALLINT       NOT NULL DEFAULT 0,
    NivelDeReorden       SMALLINT       NOT NULL DEFAULT 0,
    Descontinuado        BIT            NOT NULL DEFAULT 0,
    CONSTRAINT FK_Productos_Proveedores FOREIGN KEY (ProveedorID) REFERENCES dbo.Proveedores(ProveedorID),
    CONSTRAINT FK_Productos_Categorias  FOREIGN KEY (CategoriaID) REFERENCES dbo.Categorias(CategoriaID)
);
GO

CREATE TABLE dbo.Pedidos (
    PedidoID        INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID       INT NULL,
    EmpleadoID      INT NULL,
    FechaPedido     DATE NOT NULL,
    FechaRequerida  DATE NULL,
    FechaEnvio      DATE NULL,
    TransportistaID INT NULL,
    Destinatario    NVARCHAR(60) NULL,
    CiudadDestino   NVARCHAR(30) NULL,
    PaisDestino     NVARCHAR(30) NULL,
    CONSTRAINT FK_Pedidos_Clientes       FOREIGN KEY (ClienteID)       REFERENCES dbo.Clientes(ClienteID),
    CONSTRAINT FK_Pedidos_Empleados      FOREIGN KEY (EmpleadoID)      REFERENCES dbo.Empleados(EmpleadoID),
    CONSTRAINT FK_Pedidos_Transportistas FOREIGN KEY (TransportistaID) REFERENCES dbo.Transportistas(TransportistaID)
);
GO

CREATE TABLE dbo.DetallePedidos (
    PedidoID     INT NOT NULL,
    ProductoID   INT NOT NULL,
    PrecioUnidad DECIMAL(10,2) NOT NULL,
    Cantidad     SMALLINT NOT NULL DEFAULT 1,
    Descuento    DECIMAL(4,2) NOT NULL DEFAULT 0,
    CONSTRAINT PK_DetallePedidos PRIMARY KEY (PedidoID, ProductoID),
    CONSTRAINT FK_DetallePedidos_Pedidos   FOREIGN KEY (PedidoID)   REFERENCES dbo.Pedidos(PedidoID),
    CONSTRAINT FK_DetallePedidos_Productos FOREIGN KEY (ProductoID) REFERENCES dbo.Productos(ProductoID)
);
GO

/* ============================================================
   2. DATOS DE PRUEBA
   ============================================================ */

INSERT INTO dbo.Categorias (NombreCategoria, Descripcion) VALUES
(N'Bebidas',            N'Refrescos, cafés, tés, cervezas y otras bebidas'),
(N'Condimentos',        N'Salsas, especias y aderezos'),
(N'Confituras',         N'Mermeladas, dulces y postres'),
(N'Lácteos',            N'Quesos y otros productos lácteos'),
(N'Carnes y Embutidos', N'Carnes preparadas y embutidos');
GO

INSERT INTO dbo.Proveedores (CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax) VALUES
(N'Lácteos García S.A.',       N'Ana García',        N'Gerente de Ventas',            N'Av. Los Álamos 245', N'Lima',      N'15024', N'Perú', N'511-4567890', N'511-4567891'),
(N'Bebidas del Sur Ltda.',     N'Carlos Ramírez',    N'Jefe Comercial',               N'Jr. Comercio 890',   N'Arequipa',  N'04001', N'Perú', N'054-223344', N'054-223345'),
(N'Embutidos La Preferida',    N'María Torres',      N'Coordinadora de Distribución', N'Calle Las Flores 120', N'Trujillo', N'13001', N'Perú', N'044-556677', N'044-556678'),
(N'Condimentos Andinos SAC',   N'Jorge Quispe',      N'Gerente General',              N'Av. Industrial 500', N'Cusco',     N'08001', N'Perú', N'084-778899', N'084-778900'),
(N'Dulces del Valle E.I.R.L.', N'Lucía Fernández',   N'Encargada de Ventas',          N'Jr. San Martín 77',  N'Chiclayo',  N'14001', N'Perú', N'074-991122', N'074-991123');
GO

INSERT INTO dbo.Clientes (Empresa, NombreContacto, Ciudad, Pais, Telefono) VALUES
(N'Comercial Andina SAC',        N'Pedro Salazar',           N'Lima',     N'Perú', N'511-2345678'),
(N'Supermercados del Norte',     N'Rosa Medina',             N'Trujillo', N'Perú', N'044-334455'),
(N'Distribuidora Sureña EIRL',   N'Luis Chávez',             N'Arequipa', N'Perú', N'054-667788'),
(N'Minimarket Central',          N'Elena Rojas',             N'Cusco',    N'Perú', N'084-112233'),
(N'Tiendas Express SAC',         N'Miguel Ángel Paredes',    N'Chiclayo', N'Perú', N'074-445566');
GO

INSERT INTO dbo.Empleados (Nombre, Apellidos, Cargo, FechaNacimiento, FechaContratacion, Ciudad, Pais) VALUES
(N'Juan',   N'Pérez Gómez',     N'Vendedor',              '1990-05-12', '2020-01-15', N'Lima',     N'Perú'),
(N'María',  N'López Díaz',      N'Supervisora de Ventas', '1988-09-23', '2018-03-01', N'Lima',     N'Perú'),
(N'Carlos', N'Ruiz Mendoza',    N'Vendedor',              '1992-02-17', '2021-06-10', N'Arequipa', N'Perú'),
(N'Sofía',  N'Vargas Castro',   N'Gerente Regional',      '1985-11-30', '2015-08-20', N'Trujillo', N'Perú'),
(N'Diego',  N'Fernández Ríos',  N'Vendedor',              '1995-07-08', '2022-02-01', N'Cusco',    N'Perú');
GO

INSERT INTO dbo.Transportistas (CompaniaNombre, Telefono) VALUES
(N'Transportes Rápido SAC',   N'511-8889900'),
(N'Envíos Seguros EIRL',      N'511-7776655'),
(N'Logística del Pacífico',   N'054-990011'),
(N'Courier Nacional SA',      N'044-223344'),
(N'TransAndino Express',      N'084-556677');
GO

INSERT INTO dbo.Productos (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad, UnidadesEnExistencia, UnidadesEnPedido, NivelDeReorden, Descontinuado) VALUES
(N'Café Andino Premium',    2, 1, N'500 g',  45.90, 120, 30, 20, 0),
(N'Salsa de Ají Amarillo',  4, 2, N'300 ml', 12.50, 200, 50, 30, 0),
(N'Mermelada de Aguaymanto',5, 3, N'250 g',  15.00,  80, 20, 15, 0),
(N'Queso Fresco Andino',    1, 4, N'1 kg',   22.00,  60, 10, 10, 0),
(N'Chorizo Ahumado',        3, 5, N'500 g',  18.75,  90, 25, 20, 0);
GO

INSERT INTO dbo.Pedidos (ClienteID, EmpleadoID, FechaPedido, FechaRequerida, FechaEnvio, TransportistaID, Destinatario, CiudadDestino, PaisDestino) VALUES
(1, 1, '2026-08-10', '2026-08-20', '2026-08-15', 1, N'Comercial Andina SAC',      N'Lima',     N'Perú'),
(2, 3, '2026-08-12', '2026-08-22', '2026-08-18', 3, N'Supermercados del Norte',   N'Trujillo', N'Perú'),
(3, 2, '2026-08-15', '2026-08-25', NULL,         4, N'Distribuidora Sureña EIRL', N'Arequipa', N'Perú'),
(4, 4, '2026-08-20', '2026-08-30', '2026-08-26', 5, N'Minimarket Central',        N'Cusco',    N'Perú'),
(5, 5, '2026-08-22', '2026-09-01', '2026-08-28', 2, N'Tiendas Express SAC',       N'Chiclayo', N'Perú');
GO

INSERT INTO dbo.DetallePedidos (PedidoID, ProductoID, PrecioUnidad, Cantidad, Descuento) VALUES
(1, 1, 45.90, 10, 0.00),
(2, 2, 12.50, 25, 0.05),
(3, 3, 15.00, 15, 0.00),
(4, 4, 22.00,  8, 0.10),
(5, 5, 18.75, 12, 0.00);
GO

/* ============================================================
   3. PROCEDIMIENTOS ALMACENADOS
   ============================================================ */

-- 3. CRUD PRODUCTOS
IF OBJECT_ID(N'dbo.sp_ListarProductos', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarProductos;
GO
CREATE PROCEDURE dbo.sp_ListarProductos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.ProductoID,
        p.NombreProducto,
        p.ProveedorID,
        ISNULL(pr.CompaniaNombre, N'(Sin Proveedor)') AS Proveedor,
        p.CategoriaID,
        ISNULL(c.NombreCategoria, N'(Sin Categoría)') AS Categoria,
        p.CantidadPorUnidad,
        p.PrecioUnidad,
        p.UnidadesEnExistencia,
        p.UnidadesEnPedido,
        p.NivelDeReorden,
        p.Descontinuado
    FROM dbo.Productos p
    LEFT JOIN dbo.Proveedores pr ON p.ProveedorID = pr.ProveedorID
    LEFT JOIN dbo.Categorias c ON p.CategoriaID = c.CategoriaID
    ORDER BY p.ProductoID ASC;
END
GO

IF OBJECT_ID(N'dbo.sp_InsertarProducto', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_InsertarProducto;
GO
CREATE PROCEDURE dbo.sp_InsertarProducto
    @NombreProducto      NVARCHAR(60),
    @ProveedorID         INT = NULL,
    @CategoriaID         INT = NULL,
    @CantidadPorUnidad   NVARCHAR(30) = NULL,
    @PrecioUnidad        DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT = 0,
    @UnidadesEnPedido    SMALLINT = 0,
    @NivelDeReorden      SMALLINT = 0,
    @Descontinuado       BIT = 0,
    @NuevoID             INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Productos (
        NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad,
        PrecioUnidad, UnidadesEnExistencia, UnidadesEnPedido, NivelDeReorden, Descontinuado
    )
    VALUES (
        @NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad,
        @PrecioUnidad, @UnidadesEnExistencia, @UnidadesEnPedido, @NivelDeReorden, @Descontinuado
    );
    SET @NuevoID = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID(N'dbo.sp_ActualizarProducto', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActualizarProducto;
GO
CREATE PROCEDURE dbo.sp_ActualizarProducto
    @ProductoID          INT,
    @NombreProducto      NVARCHAR(60),
    @ProveedorID         INT = NULL,
    @CategoriaID         INT = NULL,
    @CantidadPorUnidad   NVARCHAR(30) = NULL,
    @PrecioUnidad        DECIMAL(10,2),
    @UnidadesEnExistencia SMALLINT,
    @UnidadesEnPedido    SMALLINT,
    @NivelDeReorden      SMALLINT,
    @Descontinuado       BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos
    SET 
        NombreProducto       = @NombreProducto,
        ProveedorID          = @ProveedorID,
        CategoriaID          = @CategoriaID,
        CantidadPorUnidad    = @CantidadPorUnidad,
        PrecioUnidad         = @PrecioUnidad,
        UnidadesEnExistencia = @UnidadesEnExistencia,
        UnidadesEnPedido     = @UnidadesEnPedido,
        NivelDeReorden       = @NivelDeReorden,
        Descontinuado        = @Descontinuado
    WHERE ProductoID = @ProductoID;
END
GO

IF OBJECT_ID(N'dbo.sp_EliminarProducto', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarProducto;
GO
CREATE PROCEDURE dbo.sp_EliminarProducto
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.DetallePedidos WHERE ProductoID = @ProductoID;
    DELETE FROM dbo.Productos WHERE ProductoID = @ProductoID;
END
GO

-- 4. CRUD CATEGORÍAS
IF OBJECT_ID(N'dbo.sp_ListarCategorias', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarCategorias;
GO
CREATE PROCEDURE dbo.sp_ListarCategorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion
    FROM dbo.Categorias
    ORDER BY CategoriaID ASC;
END
GO

IF OBJECT_ID(N'dbo.sp_InsertarCategoria', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_InsertarCategoria;
GO
CREATE PROCEDURE dbo.sp_InsertarCategoria
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL,
    @NuevoID         INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Categorias (NombreCategoria, Descripcion)
    VALUES (@NombreCategoria, @Descripcion);
    SET @NuevoID = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID(N'dbo.sp_ActualizarCategoria', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActualizarCategoria;
GO
CREATE PROCEDURE dbo.sp_ActualizarCategoria
    @CategoriaID     INT,
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Categorias
    SET 
        NombreCategoria = @NombreCategoria,
        Descripcion     = @Descripcion
    WHERE CategoriaID = @CategoriaID;
END
GO

IF OBJECT_ID(N'dbo.sp_EliminarCategoria', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarCategoria;
GO
CREATE PROCEDURE dbo.sp_EliminarCategoria
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos SET CategoriaID = NULL WHERE CategoriaID = @CategoriaID;
    DELETE FROM dbo.Categorias WHERE CategoriaID = @CategoriaID;
END
GO

-- 5. CRUD PROVEEDORES
IF OBJECT_ID(N'dbo.sp_ListarProveedores', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarProveedores;
GO
CREATE PROCEDURE dbo.sp_ListarProveedores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
        Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    ORDER BY ProveedorID ASC;
END
GO

IF OBJECT_ID(N'dbo.sp_InsertarProveedor', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_InsertarProveedor;
GO
CREATE PROCEDURE dbo.sp_InsertarProveedor
    @CompaniaNombre  NVARCHAR(60),
    @NombreContacto  NVARCHAR(40) = NULL,
    @CargoContacto   NVARCHAR(40) = NULL,
    @Direccion       NVARCHAR(80) = NULL,
    @Ciudad          NVARCHAR(30) = NULL,
    @CodigoPostal    NVARCHAR(10) = NULL,
    @Pais            NVARCHAR(30) = NULL,
    @Telefono        NVARCHAR(24) = NULL,
    @Fax             NVARCHAR(24) = NULL,
    @NuevoID         INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Proveedores (
        CompaniaNombre, NombreContacto, CargoContacto,
        Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    )
    VALUES (
        @CompaniaNombre, @NombreContacto, @CargoContacto,
        @Direccion, @Ciudad, @CodigoPostal, @Pais, @Telefono, @Fax
    );
    SET @NuevoID = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID(N'dbo.sp_ActualizarProveedor', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActualizarProveedor;
GO
CREATE PROCEDURE dbo.sp_ActualizarProveedor
    @ProveedorID     INT,
    @CompaniaNombre  NVARCHAR(60),
    @NombreContacto  NVARCHAR(40) = NULL,
    @CargoContacto   NVARCHAR(40) = NULL,
    @Direccion       NVARCHAR(80) = NULL,
    @Ciudad          NVARCHAR(30) = NULL,
    @CodigoPostal    NVARCHAR(10) = NULL,
    @Pais            NVARCHAR(30) = NULL,
    @Telefono        NVARCHAR(24) = NULL,
    @Fax             NVARCHAR(24) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Proveedores
    SET 
        CompaniaNombre = @CompaniaNombre,
        NombreContacto = @NombreContacto,
        CargoContacto  = @CargoContacto,
        Direccion      = @Direccion,
        Ciudad         = @Ciudad,
        CodigoPostal   = @CodigoPostal,
        Pais           = @Pais,
        Telefono       = @Telefono,
        Fax            = @Fax
    WHERE ProveedorID = @ProveedorID;
END
GO

IF OBJECT_ID(N'dbo.sp_EliminarProveedor', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarProveedor;
GO
CREATE PROCEDURE dbo.sp_EliminarProveedor
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos SET ProveedorID = NULL WHERE ProveedorID = @ProveedorID;
    DELETE FROM dbo.Proveedores WHERE ProveedorID = @ProveedorID;
END
GO

-- 6. CRUD PEDIDOS
IF OBJECT_ID(N'dbo.sp_ListarPedidos', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarPedidos;
GO
CREATE PROCEDURE dbo.sp_ListarPedidos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.PedidoID,
        p.ClienteID,
        ISNULL(c.Empresa, N'(Sin Cliente)') AS Cliente,
        p.EmpleadoID,
        ISNULL(e.Nombre + ' ' + e.Apellidos, N'(Sin Empleado)') AS Empleado,
        p.FechaPedido,
        p.FechaRequerida,
        p.FechaEnvio,
        p.TransportistaID,
        ISNULL(t.CompaniaNombre, N'(Sin Transportista)') AS Transportista,
        p.Destinatario,
        p.CiudadDestino,
        p.PaisDestino
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes c ON p.ClienteID = c.ClienteID
    LEFT JOIN dbo.Empleados e ON p.EmpleadoID = e.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON p.TransportistaID = t.TransportistaID
    ORDER BY p.PedidoID DESC;
END
GO

IF OBJECT_ID(N'dbo.sp_InsertarPedido', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_InsertarPedido;
GO
CREATE PROCEDURE dbo.sp_InsertarPedido
    @ClienteID       INT = NULL,
    @EmpleadoID      INT = NULL,
    @FechaPedido     DATE,
    @FechaRequerida  DATE = NULL,
    @FechaEnvio      DATE = NULL,
    @TransportistaID INT = NULL,
    @Destinatario    NVARCHAR(60) = NULL,
    @CiudadDestino   NVARCHAR(30) = NULL,
    @PaisDestino     NVARCHAR(30) = NULL,
    @NuevoID         INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Pedidos (
        ClienteID, EmpleadoID, FechaPedido, FechaRequerida, FechaEnvio,
        TransportistaID, Destinatario, CiudadDestino, PaisDestino
    )
    VALUES (
        @ClienteID, @EmpleadoID, @FechaPedido, @FechaRequerida, @FechaEnvio,
        @TransportistaID, @Destinatario, @CiudadDestino, @PaisDestino
    );
    SET @NuevoID = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID(N'dbo.sp_ActualizarPedido', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActualizarPedido;
GO
CREATE PROCEDURE dbo.sp_ActualizarPedido
    @PedidoID        INT,
    @ClienteID       INT = NULL,
    @EmpleadoID      INT = NULL,
    @FechaPedido     DATE,
    @FechaRequerida  DATE = NULL,
    @FechaEnvio      DATE = NULL,
    @TransportistaID INT = NULL,
    @Destinatario    NVARCHAR(60) = NULL,
    @CiudadDestino   NVARCHAR(30) = NULL,
    @PaisDestino     NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Pedidos
    SET 
        ClienteID       = @ClienteID,
        EmpleadoID      = @EmpleadoID,
        FechaPedido     = @FechaPedido,
        FechaRequerida  = @FechaRequerida,
        FechaEnvio      = @FechaEnvio,
        TransportistaID = @TransportistaID,
        Destinatario    = @Destinatario,
        CiudadDestino   = @CiudadDestino,
        PaisDestino     = @PaisDestino
    WHERE PedidoID = @PedidoID;
END
GO

IF OBJECT_ID(N'dbo.sp_EliminarPedido', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarPedido;
GO
CREATE PROCEDURE dbo.sp_EliminarPedido
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.DetallePedidos WHERE PedidoID = @PedidoID;
    DELETE FROM dbo.Pedidos WHERE PedidoID = @PedidoID;
END
GO

-- 7. BÚSQUEDA DE PROVEEDORES POR NOMBRE DE CONTACTO Y CIUDAD
IF OBJECT_ID(N'dbo.sp_BuscarProveedores', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_BuscarProveedores;
GO
CREATE PROCEDURE dbo.sp_BuscarProveedores
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad         NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
        Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE 
        (@NombreContacto IS NULL OR LTRIM(RTRIM(@NombreContacto)) = '' OR NombreContacto LIKE '%' + @NombreContacto + '%')
        AND
        (@Ciudad IS NULL OR LTRIM(RTRIM(@Ciudad)) = '' OR Ciudad LIKE '%' + @Ciudad + '%')
    ORDER BY ProveedorID ASC;
END
GO

-- 8. REPORTE DETALLES DE PEDIDOS CON INNER JOIN Y FILTRO DE FECHAS
IF OBJECT_ID(N'dbo.sp_ReporteDetallePedidosPorFechas', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ReporteDetallePedidosPorFechas;
GO
CREATE PROCEDURE dbo.sp_ReporteDetallePedidosPorFechas
    @FechaInicio DATE,
    @FechaFin    DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        dp.PedidoID,
        p.FechaPedido,
        p.Destinatario,
        p.CiudadDestino,
        dp.ProductoID,
        pr.NombreProducto,
        dp.PrecioUnidad,
        dp.Cantidad,
        dp.Descuento,
        CAST(ROUND((dp.PrecioUnidad * dp.Cantidad) * (1.0 - dp.Descuento), 2) AS DECIMAL(10,2)) AS Subtotal
    FROM dbo.DetallePedidos dp
    INNER JOIN dbo.Pedidos p ON dp.PedidoID = p.PedidoID
    INNER JOIN dbo.Productos pr ON dp.ProductoID = pr.ProductoID
    WHERE p.FechaPedido BETWEEN @FechaInicio AND @FechaFin
    ORDER BY p.FechaPedido DESC, dp.PedidoID DESC, dp.ProductoID ASC;
END
GO

-- AUXILIARES PARA COMBOS
IF OBJECT_ID(N'dbo.sp_ListarClientes', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarClientes;
GO
CREATE PROCEDURE dbo.sp_ListarClientes
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ClienteID, Empresa, NombreContacto FROM dbo.Clientes ORDER BY Empresa ASC;
END
GO

IF OBJECT_ID(N'dbo.sp_ListarEmpleados', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarEmpleados;
GO
CREATE PROCEDURE dbo.sp_ListarEmpleados
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmpleadoID, (Nombre + ' ' + Apellidos) AS NombreCompleto, Cargo FROM dbo.Empleados ORDER BY NombreCompleto ASC;
END
GO

IF OBJECT_ID(N'dbo.sp_ListarTransportistas', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarTransportistas;
GO
CREATE PROCEDURE dbo.sp_ListarTransportistas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TransportistaID, CompaniaNombre, Telefono FROM dbo.Transportistas ORDER BY CompaniaNombre ASC;
END
GO
