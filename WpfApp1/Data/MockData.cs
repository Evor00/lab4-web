using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public static class MockData
    {
        public static List<Categoria> Categorias { get; } = new()
        {
            new Categoria { CategoriaID = 1, NombreCategoria = "Bebidas", Descripcion = "Refrescos, cafés, tés, cervezas y otras bebidas" },
            new Categoria { CategoriaID = 2, NombreCategoria = "Condimentos", Descripcion = "Salsas, especias y aderezos" },
            new Categoria { CategoriaID = 3, NombreCategoria = "Confituras", Descripcion = "Mermeladas, dulces y postres" },
            new Categoria { CategoriaID = 4, NombreCategoria = "Lácteos", Descripcion = "Quesos y otros productos lácteos" },
            new Categoria { CategoriaID = 5, NombreCategoria = "Carnes y Embutidos", Descripcion = "Carnes preparadas y embutidos" }
        };

        public static List<Proveedor> Proveedores { get; } = new()
        {
            new Proveedor { ProveedorID = 1, CompaniaNombre = "Lácteos García S.A.", NombreContacto = "Ana García", CargoContacto = "Gerente de Ventas", Direccion = "Av. Los Álamos 245", Ciudad = "Lima", CodigoPostal = "15024", Pais = "Perú", Telefono = "511-4567890", Fax = "511-4567891" },
            new Proveedor { ProveedorID = 2, CompaniaNombre = "Bebidas del Sur Ltda.", NombreContacto = "Carlos Ramírez", CargoContacto = "Jefe Comercial", Direccion = "Jr. Comercio 890", Ciudad = "Arequipa", CodigoPostal = "04001", Pais = "Perú", Telefono = "054-223344", Fax = "054-223345" },
            new Proveedor { ProveedorID = 3, CompaniaNombre = "Embutidos La Preferida", NombreContacto = "María Torres", CargoContacto = "Coordinadora de Distribución", Direccion = "Calle Las Flores 120", Ciudad = "Trujillo", CodigoPostal = "13001", Pais = "Perú", Telefono = "044-556677", Fax = "044-556678" },
            new Proveedor { ProveedorID = 4, CompaniaNombre = "Condimentos Andinos SAC", NombreContacto = "Jorge Quispe", CargoContacto = "Gerente General", Direccion = "Av. Industrial 500", Ciudad = "Cusco", CodigoPostal = "08001", Pais = "Perú", Telefono = "084-778899", Fax = "084-778900" },
            new Proveedor { ProveedorID = 5, CompaniaNombre = "Dulces del Valle E.I.R.L.", NombreContacto = "Lucía Fernández", CargoContacto = "Encargada de Ventas", Direccion = "Jr. San Martín 77", Ciudad = "Chiclayo", CodigoPostal = "14001", Pais = "Perú", Telefono = "074-991122", Fax = "074-991123" }
        };

        public static List<Cliente> Clientes { get; } = new()
        {
            new Cliente { ClienteID = 1, Empresa = "Comercial Andina SAC", NombreContacto = "Pedro Salazar" },
            new Cliente { ClienteID = 2, Empresa = "Supermercados del Norte", NombreContacto = "Rosa Medina" },
            new Cliente { ClienteID = 3, Empresa = "Distribuidora Sureña EIRL", NombreContacto = "Luis Chávez" },
            new Cliente { ClienteID = 4, Empresa = "Minimarket Central", NombreContacto = "Elena Rojas" },
            new Cliente { ClienteID = 5, Empresa = "Tiendas Express SAC", NombreContacto = "Miguel Ángel Paredes" }
        };

        public static List<Empleado> Empleados { get; } = new()
        {
            new Empleado { EmpleadoID = 1, NombreCompleto = "Juan Pérez Gómez", Cargo = "Vendedor" },
            new Empleado { EmpleadoID = 2, NombreCompleto = "María López Díaz", Cargo = "Supervisora de Ventas" },
            new Empleado { EmpleadoID = 3, NombreCompleto = "Carlos Ruiz Mendoza", Cargo = "Vendedor" },
            new Empleado { EmpleadoID = 4, NombreCompleto = "Sofía Vargas Castro", Cargo = "Gerente Regional" },
            new Empleado { EmpleadoID = 5, NombreCompleto = "Diego Fernández Ríos", Cargo = "Vendedor" }
        };

        public static List<Transportista> Transportistas { get; } = new()
        {
            new Transportista { TransportistaID = 1, CompaniaNombre = "Transportes Rápido SAC", Telefono = "511-8889900" },
            new Transportista { TransportistaID = 2, CompaniaNombre = "Envíos Seguros EIRL", Telefono = "511-7776655" },
            new Transportista { TransportistaID = 3, CompaniaNombre = "Logística del Pacífico", Telefono = "054-990011" },
            new Transportista { TransportistaID = 4, CompaniaNombre = "Courier Nacional SA", Telefono = "044-223344" },
            new Transportista { TransportistaID = 5, CompaniaNombre = "TransAndino Express", Telefono = "084-556677" }
        };

        public static List<Producto> Productos { get; } = new()
        {
            new Producto { ProductoID = 1, NombreProducto = "Café Andino Premium", ProveedorID = 2, Proveedor = "Bebidas del Sur Ltda.", CategoriaID = 1, Categoria = "Bebidas", CantidadPorUnidad = "500 g", PrecioUnidad = 45.90m, UnidadesEnExistencia = 120, UnidadesEnPedido = 30, NivelDeReorden = 20, Descontinuado = false },
            new Producto { ProductoID = 2, NombreProducto = "Salsa de Ají Amarillo", ProveedorID = 4, Proveedor = "Condimentos Andinos SAC", CategoriaID = 2, Categoria = "Condimentos", CantidadPorUnidad = "300 ml", PrecioUnidad = 12.50m, UnidadesEnExistencia = 200, UnidadesEnPedido = 50, NivelDeReorden = 30, Descontinuado = false },
            new Producto { ProductoID = 3, NombreProducto = "Mermelada de Aguaymanto", ProveedorID = 5, Proveedor = "Dulces del Valle E.I.R.L.", CategoriaID = 3, Categoria = "Confituras", CantidadPorUnidad = "250 g", PrecioUnidad = 15.00m, UnidadesEnExistencia = 80, UnidadesEnPedido = 20, NivelDeReorden = 15, Descontinuado = false },
            new Producto { ProductoID = 4, NombreProducto = "Queso Fresco Andino", ProveedorID = 1, Proveedor = "Lácteos García S.A.", CategoriaID = 4, Categoria = "Lácteos", CantidadPorUnidad = "1 kg", PrecioUnidad = 22.00m, UnidadesEnExistencia = 60, UnidadesEnPedido = 10, NivelDeReorden = 10, Descontinuado = false },
            new Producto { ProductoID = 5, NombreProducto = "Chorizo Ahumado", ProveedorID = 3, Proveedor = "Embutidos La Preferida", CategoriaID = 5, Categoria = "Carnes y Embutidos", CantidadPorUnidad = "500 g", PrecioUnidad = 18.75m, UnidadesEnExistencia = 90, UnidadesEnPedido = 25, NivelDeReorden = 20, Descontinuado = false }
        };

        public static List<Pedido> Pedidos { get; } = new()
        {
            new Pedido { PedidoID = 1, ClienteID = 1, Cliente = "Comercial Andina SAC", EmpleadoID = 1, Empleado = "Juan Pérez Gómez", FechaPedido = new DateTime(2026, 8, 10), FechaRequerida = new DateTime(2026, 8, 20), FechaEnvio = new DateTime(2026, 8, 15), TransportistaID = 1, Transportista = "Transportes Rápido SAC", Destinatario = "Comercial Andina SAC", CiudadDestino = "Lima", PaisDestino = "Perú" },
            new Pedido { PedidoID = 2, ClienteID = 2, Cliente = "Supermercados del Norte", EmpleadoID = 3, Empleado = "Carlos Ruiz Mendoza", FechaPedido = new DateTime(2026, 8, 12), FechaRequerida = new DateTime(2026, 8, 22), FechaEnvio = new DateTime(2026, 8, 18), TransportistaID = 3, Transportista = "Logística del Pacífico", Destinatario = "Supermercados del Norte", CiudadDestino = "Trujillo", PaisDestino = "Perú" },
            new Pedido { PedidoID = 3, ClienteID = 3, Cliente = "Distribuidora Sureña EIRL", EmpleadoID = 2, Empleado = "María López Díaz", FechaPedido = new DateTime(2026, 8, 15), FechaRequerida = new DateTime(2026, 8, 25), FechaEnvio = null, TransportistaID = 4, Transportista = "Courier Nacional SA", Destinatario = "Distribuidora Sureña EIRL", CiudadDestino = "Arequipa", PaisDestino = "Perú" },
            new Pedido { PedidoID = 4, ClienteID = 4, Cliente = "Minimarket Central", EmpleadoID = 4, Empleado = "Sofía Vargas Castro", FechaPedido = new DateTime(2026, 8, 20), FechaRequerida = new DateTime(2026, 8, 30), FechaEnvio = new DateTime(2026, 8, 26), TransportistaID = 5, Transportista = "TransAndino Express", Destinatario = "Minimarket Central", CiudadDestino = "Cusco", PaisDestino = "Perú" },
            new Pedido { PedidoID = 5, ClienteID = 5, Cliente = "Tiendas Express SAC", EmpleadoID = 5, Empleado = "Diego Fernández Ríos", FechaPedido = new DateTime(2026, 8, 22), FechaRequerida = new DateTime(2026, 9, 1), FechaEnvio = new DateTime(2026, 8, 28), TransportistaID = 2, Transportista = "Envíos Seguros EIRL", Destinatario = "Tiendas Express SAC", CiudadDestino = "Chiclayo", PaisDestino = "Perú" }
        };

        public static List<DetallePedidoReporte> DetallesReporte { get; } = new()
        {
            new DetallePedidoReporte { PedidoID = 1, FechaPedido = new DateTime(2026, 8, 10), Destinatario = "Comercial Andina SAC", CiudadDestino = "Lima", ProductoID = 1, NombreProducto = "Café Andino Premium", PrecioUnidad = 45.90m, Cantidad = 10, Descuento = 0.00m, Subtotal = 459.00m },
            new DetallePedidoReporte { PedidoID = 2, FechaPedido = new DateTime(2026, 8, 12), Destinatario = "Supermercados del Norte", CiudadDestino = "Trujillo", ProductoID = 2, NombreProducto = "Salsa de Ají Amarillo", PrecioUnidad = 12.50m, Cantidad = 25, Descuento = 0.05m, Subtotal = 296.88m },
            new DetallePedidoReporte { PedidoID = 3, FechaPedido = new DateTime(2026, 8, 15), Destinatario = "Distribuidora Sureña EIRL", CiudadDestino = "Arequipa", ProductoID = 3, NombreProducto = "Mermelada de Aguaymanto", PrecioUnidad = 15.00m, Cantidad = 15, Descuento = 0.00m, Subtotal = 225.00m },
            new DetallePedidoReporte { PedidoID = 4, FechaPedido = new DateTime(2026, 8, 20), Destinatario = "Minimarket Central", CiudadDestino = "Cusco", ProductoID = 4, NombreProducto = "Queso Fresco Andino", PrecioUnidad = 22.00m, Cantidad = 8, Descuento = 0.10m, Subtotal = 158.40m },
            new DetallePedidoReporte { PedidoID = 5, FechaPedido = new DateTime(2026, 8, 22), Destinatario = "Tiendas Express SAC", CiudadDestino = "Chiclayo", ProductoID = 5, NombreProducto = "Chorizo Ahumado", PrecioUnidad = 18.75m, Cantidad = 12, Descuento = 0.00m, Subtotal = 225.00m }
        };
    }
}
