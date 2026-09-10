/* ============================================================
   NeptunoDB - Procedimientos Almacenados
   Desarrollo de Aplicaciones Empresariales Avanzado (Semana 04)
   Motor: Microsoft SQL Server (T-SQL)
   ============================================================ */

USE NeptunoDB;
GO

/* ============================================================
   3. CRUD DE PRODUCTOS
   ============================================================ */

-- 3.1 Listar Productos
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

-- 3.2 Insertar Producto
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
        NombreProducto,
        ProveedorID,
        CategoriaID,
        CantidadPorUnidad,
        PrecioUnidad,
        UnidadesEnExistencia,
        UnidadesEnPedido,
        NivelDeReorden,
        Descontinuado
    )
    VALUES (
        @NombreProducto,
        @ProveedorID,
        @CategoriaID,
        @CantidadPorUnidad,
        @PrecioUnidad,
        @UnidadesEnExistencia,
        @UnidadesEnPedido,
        @NivelDeReorden,
        @Descontinuado
    );

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

-- 3.3 Actualizar Producto
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

-- 3.4 Eliminar Producto
IF OBJECT_ID(N'dbo.sp_EliminarProducto', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarProducto;
GO
CREATE PROCEDURE dbo.sp_EliminarProducto
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Se eliminan primero las referencias en DetallePedidos si existieran
    DELETE FROM dbo.DetallePedidos WHERE ProductoID = @ProductoID;
    DELETE FROM dbo.Productos WHERE ProductoID = @ProductoID;
END
GO


/* ============================================================
   4. CRUD DE CATEGORÍAS
   ============================================================ */

-- 4.1 Listar Categorías
IF OBJECT_ID(N'dbo.sp_ListarCategorias', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarCategorias;
GO
CREATE PROCEDURE dbo.sp_ListarCategorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CategoriaID,
        NombreCategoria,
        Descripcion
    FROM dbo.Categorias
    ORDER BY CategoriaID ASC;
END
GO

-- 4.2 Insertar Categoría
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

-- 4.3 Actualizar Categoría
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

-- 4.4 Eliminar Categoría
IF OBJECT_ID(N'dbo.sp_EliminarCategoria', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarCategoria;
GO
CREATE PROCEDURE dbo.sp_EliminarCategoria
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Desvincular productos asociados antes de eliminar
    UPDATE dbo.Productos SET CategoriaID = NULL WHERE CategoriaID = @CategoriaID;
    DELETE FROM dbo.Categorias WHERE CategoriaID = @CategoriaID;
END
GO


/* ============================================================
   5. CRUD DE PROVEEDORES
   ============================================================ */

-- 5.1 Listar Proveedores
IF OBJECT_ID(N'dbo.sp_ListarProveedores', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ListarProveedores;
GO
CREATE PROCEDURE dbo.sp_ListarProveedores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ProveedorID,
        CompaniaNombre,
        NombreContacto,
        CargoContacto,
        Direccion,
        Ciudad,
        CodigoPostal,
        Pais,
        Telefono,
        Fax
    FROM dbo.Proveedores
    ORDER BY ProveedorID ASC;
END
GO

-- 5.2 Insertar Proveedor
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
        CompaniaNombre,
        NombreContacto,
        CargoContacto,
        Direccion,
        Ciudad,
        CodigoPostal,
        Pais,
        Telefono,
        Fax
    )
    VALUES (
        @CompaniaNombre,
        @NombreContacto,
        @CargoContacto,
        @Direccion,
        @Ciudad,
        @CodigoPostal,
        @Pais,
        @Telefono,
        @Fax
    );

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

-- 5.3 Actualizar Proveedor
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

-- 5.4 Eliminar Proveedor
IF OBJECT_ID(N'dbo.sp_EliminarProveedor', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarProveedor;
GO
CREATE PROCEDURE dbo.sp_EliminarProveedor
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Desvincular productos asociados antes de eliminar
    UPDATE dbo.Productos SET ProveedorID = NULL WHERE ProveedorID = @ProveedorID;
    DELETE FROM dbo.Proveedores WHERE ProveedorID = @ProveedorID;
END
GO


/* ============================================================
   6. CRUD DE PEDIDOS
   ============================================================ */

-- 6.1 Listar Pedidos
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

-- 6.2 Insertar Pedido
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
        ClienteID,
        EmpleadoID,
        FechaPedido,
        FechaRequerida,
        FechaEnvio,
        TransportistaID,
        Destinatario,
        CiudadDestino,
        PaisDestino
    )
    VALUES (
        @ClienteID,
        @EmpleadoID,
        @FechaPedido,
        @FechaRequerida,
        @FechaEnvio,
        @TransportistaID,
        @Destinatario,
        @CiudadDestino,
        @PaisDestino
    );

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

-- 6.3 Actualizar Pedido
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

-- 6.4 Eliminar Pedido
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


/* ============================================================
   7. LISTADO DE PROVEEDORES BUSCANDO POR NOMBRECONTACTO Y CIUDAD
   ============================================================ */

IF OBJECT_ID(N'dbo.sp_BuscarProveedores', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_BuscarProveedores;
GO
CREATE PROCEDURE dbo.sp_BuscarProveedores
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad         NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ProveedorID,
        CompaniaNombre,
        NombreContacto,
        CargoContacto,
        Direccion,
        Ciudad,
        CodigoPostal,
        Pais,
        Telefono,
        Fax
    FROM dbo.Proveedores
    WHERE 
        (@NombreContacto IS NULL OR LTRIM(RTRIM(@NombreContacto)) = '' OR NombreContacto LIKE '%' + @NombreContacto + '%')
        AND
        (@Ciudad IS NULL OR LTRIM(RTRIM(@Ciudad)) = '' OR Ciudad LIKE '%' + @Ciudad + '%')
    ORDER BY ProveedorID ASC;
END
GO


/* ============================================================
   8. LISTADO DE DETALLES DE PEDIDOS CON INNER JOIN A PEDIDOS,
      FILTRANDO POR UN INTERVALO DE FECHAS
   ============================================================ */

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


/* ============================================================
   PROCEDIMIENTOS AUXILIARES (Para llenar ComboBoxes en la UI)
   ============================================================ */

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
