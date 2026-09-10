using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly CategoriaDAL _categoriaDAL = new();
        private readonly ProductoDAL _productoDAL = new();
        private readonly ProveedorDAL _proveedorDAL = new();
        private readonly PedidoDAL _pedidoDAL = new();
        private readonly LookupDAL _lookupDAL = new();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtCadenaConexion.Text = Conexion.CadenaConexion;
            
            // Intentar conexión inicial
            ActualizarEstadoConexion();

            // Cargar combos y datos
            CargarLookups();
            CargarProductos();
            CargarCategorias();
            CargarProveedores();
            CargarPedidos();

            // Configurar fechas iniciales del reporte
            dpReporteInicio.SelectedDate = new DateTime(2026, 8, 1);
            dpReporteFin.SelectedDate = new DateTime(2026, 8, 31);
            CargarReporte();

            // Fechas de pedidos por defecto
            dpPedFechaPedido.SelectedDate = DateTime.Today;
            dpPedFechaRequerida.SelectedDate = DateTime.Today.AddDays(7);
        }

        private void ActualizarEstadoConexion()
        {
            bool conectado = Conexion.ProbarConexion(out string mensaje);
            if (conectado)
            {
                pnlBannerConexion.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231)); // verde claro
                pnlBannerConexion.BorderBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
                txtEstadoConexion.Foreground = new SolidColorBrush(Color.FromRgb(22, 101, 52));
                txtEstadoConexion.Text = "🟢 Conectado exitosamente a Microsoft SQL Server (Base de Datos: NeptunoDB).";
            }
            else
            {
                pnlBannerConexion.Background = new SolidColorBrush(Color.FromRgb(254, 243, 199)); // amarillo claro
                pnlBannerConexion.BorderBrush = new SolidColorBrush(Color.FromRgb(245, 158, 11));
                txtEstadoConexion.Foreground = new SolidColorBrush(Color.FromRgb(146, 64, 14));
                txtEstadoConexion.Text = "🟡 Modo Local / Demostración activo (SQL Server no disponible en este equipo. Todos los procedimientos y vistas son 100% operativos).";
            }
        }

        private void CargarLookups()
        {
            try
            {
                var categorias = _categoriaDAL.Listar();
                cboProdCategoria.ItemsSource = categorias;

                var proveedores = _proveedorDAL.Listar();
                cboProdProveedor.ItemsSource = proveedores;

                var clientes = _lookupDAL.ListarClientes();
                cboPedCliente.ItemsSource = clientes;

                var empleados = _lookupDAL.ListarEmpleados();
                cboPedEmpleado.ItemsSource = empleados;

                var transportistas = _lookupDAL.ListarTransportistas();
                cboPedTransportista.ItemsSource = transportistas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar catálogos: {ex.Message}", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #region Configuración de Conexión
        private void BtnConfigConexion_Click(object sender, RoutedEventArgs e)
        {
            pnlConfigConexion.Visibility = pnlConfigConexion.Visibility == Visibility.Visible 
                ? Visibility.Collapsed 
                : Visibility.Visible;
        }

        private void BtnCerrarConfig_Click(object sender, RoutedEventArgs e)
        {
            pnlConfigConexion.Visibility = Visibility.Collapsed;
        }

        private void BtnGuardarCadena_Click(object sender, RoutedEventArgs e)
        {
            Conexion.CadenaConexion = txtCadenaConexion.Text.Trim();
            ActualizarEstadoConexion();
            CargarLookups();
            CargarProductos();
            CargarCategorias();
            CargarProveedores();
            CargarPedidos();
            CargarReporte();
            pnlConfigConexion.Visibility = Visibility.Collapsed;
            MessageBox.Show("Cadena de conexión actualizada y datos recargados.", "Conexión", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnProbarConexion_Click(object sender, RoutedEventArgs e)
        {
            bool conectado = Conexion.ProbarConexion(out string mensaje);
            ActualizarEstadoConexion();
            if (conectado)
            {
                MessageBox.Show(mensaje, "Conexión SQL Server", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarLookups();
                CargarProductos();
                CargarCategorias();
                CargarProveedores();
                CargarPedidos();
                CargarReporte();
            }
            else
            {
                MessageBox.Show(mensaje + "\n\nLa aplicación continuará en modo seguro/demostración.", "Conexión SQL Server", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion

        #region 3 & 9. MANTENIMIENTO DE PRODUCTOS (CRUD)
        private void CargarProductos()
        {
            try
            {
                dgProductos.ItemsSource = null;
                dgProductos.ItemsSource = _productoDAL.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is Producto p)
            {
                txtProdId.Text = p.ProductoID.ToString();
                txtProdNombre.Text = p.NombreProducto;
                cboProdCategoria.SelectedValue = p.CategoriaID;
                cboProdProveedor.SelectedValue = p.ProveedorID;
                txtProdCantidadUnidad.Text = p.CantidadPorUnidad ?? "";
                txtProdPrecio.Text = p.PrecioUnidad.ToString("0.00", CultureInfo.InvariantCulture);
                txtProdExistencia.Text = p.UnidadesEnExistencia.ToString();
                txtProdPedido.Text = p.UnidadesEnPedido.ToString();
                txtProdReorden.Text = p.NivelDeReorden.ToString();
                chkProdDescontinuado.IsChecked = p.Descontinuado;
            }
        }

        private void BtnProdNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormProducto();
            txtProdNombre.Focus();
        }

        private void BtnProdGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProdNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtProdPrecio.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
            {
                MessageBox.Show("Ingrese un precio válido (ejemplo: 25.50).", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            short.TryParse(txtProdExistencia.Text.Trim(), out short stock);
            short.TryParse(txtProdPedido.Text.Trim(), out short pedido);
            short.TryParse(txtProdReorden.Text.Trim(), out short reorden);

            var nuevo = new Producto
            {
                NombreProducto = txtProdNombre.Text.Trim(),
                CategoriaID = cboProdCategoria.SelectedValue != null ? (int)cboProdCategoria.SelectedValue : null,
                ProveedorID = cboProdProveedor.SelectedValue != null ? (int)cboProdProveedor.SelectedValue : null,
                CantidadPorUnidad = txtProdCantidadUnidad.Text.Trim(),
                PrecioUnidad = precio,
                UnidadesEnExistencia = stock,
                UnidadesEnPedido = pedido,
                NivelDeReorden = reorden,
                Descontinuado = chkProdDescontinuado.IsChecked == true
            };

            int idGenerado = _productoDAL.Insertar(nuevo);
            MessageBox.Show($"Producto registrado con éxito (ID: {idGenerado}).", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            CargarProductos();
            LimpiarFormProducto();
        }

        private void BtnProdActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtProdId.Text, out int id))
            {
                MessageBox.Show("Seleccione un producto para actualizar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtProdPrecio.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
            {
                MessageBox.Show("Ingrese un precio válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            short.TryParse(txtProdExistencia.Text.Trim(), out short stock);
            short.TryParse(txtProdPedido.Text.Trim(), out short pedido);
            short.TryParse(txtProdReorden.Text.Trim(), out short reorden);

            var prod = new Producto
            {
                ProductoID = id,
                NombreProducto = txtProdNombre.Text.Trim(),
                CategoriaID = cboProdCategoria.SelectedValue != null ? (int)cboProdCategoria.SelectedValue : null,
                ProveedorID = cboProdProveedor.SelectedValue != null ? (int)cboProdProveedor.SelectedValue : null,
                CantidadPorUnidad = txtProdCantidadUnidad.Text.Trim(),
                PrecioUnidad = precio,
                UnidadesEnExistencia = stock,
                UnidadesEnPedido = pedido,
                NivelDeReorden = reorden,
                Descontinuado = chkProdDescontinuado.IsChecked == true
            };

            bool ok = _productoDAL.Actualizar(prod);
            if (ok)
            {
                MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarProductos();
                LimpiarFormProducto();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnProdEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtProdId.Text, out int id))
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Está seguro de eliminar el producto ID: {id}?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                bool ok = _productoDAL.Eliminar(id);
                if (ok)
                {
                    MessageBox.Show("Producto eliminado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarProductos();
                    LimpiarFormProducto();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnProdLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormProducto();
        }

        private void LimpiarFormProducto()
        {
            txtProdId.Clear();
            txtProdNombre.Clear();
            cboProdCategoria.SelectedIndex = -1;
            cboProdProveedor.SelectedIndex = -1;
            txtProdCantidadUnidad.Clear();
            txtProdPrecio.Clear();
            txtProdExistencia.Clear();
            txtProdPedido.Clear();
            txtProdReorden.Clear();
            chkProdDescontinuado.IsChecked = false;
            dgProductos.SelectedItem = null;
        }
        #endregion

        #region 4 & 10. MANTENIMIENTO DE CATEGORÍAS (CRUD)
        private void CargarCategorias()
        {
            try
            {
                var lista = _categoriaDAL.Listar();
                dgCategorias.ItemsSource = null;
                dgCategorias.ItemsSource = lista;
                cboProdCategoria.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar categorías: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategorias.SelectedItem is Categoria c)
            {
                txtCatId.Text = c.CategoriaID.ToString();
                txtCatNombre.Text = c.NombreCategoria;
                txtCatDescripcion.Text = c.Descripcion ?? "";
            }
        }

        private void BtnCatNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormCategoria();
            txtCatNombre.Focus();
        }

        private void BtnCatGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCatNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cat = new Categoria
            {
                NombreCategoria = txtCatNombre.Text.Trim(),
                Descripcion = txtCatDescripcion.Text.Trim()
            };

            int nuevoId = _categoriaDAL.Insertar(cat);
            MessageBox.Show($"Categoría registrada exitosamente (ID: {nuevoId}).", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            CargarCategorias();
            LimpiarFormCategoria();
        }

        private void BtnCatActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCatId.Text, out int id))
            {
                MessageBox.Show("Seleccione una categoría para actualizar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cat = new Categoria
            {
                CategoriaID = id,
                NombreCategoria = txtCatNombre.Text.Trim(),
                Descripcion = txtCatDescripcion.Text.Trim()
            };

            bool ok = _categoriaDAL.Actualizar(cat);
            if (ok)
            {
                MessageBox.Show("Categoría actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarCategorias();
                LimpiarFormCategoria();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar la categoría.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCatEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCatId.Text, out int id))
            {
                MessageBox.Show("Seleccione una categoría para eliminar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Desea eliminar la categoría ID: {id}?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                bool ok = _categoriaDAL.Eliminar(id);
                if (ok)
                {
                    MessageBox.Show("Categoría eliminada.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarCategorias();
                    LimpiarFormCategoria();
                }
            }
        }

        private void BtnCatLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormCategoria();
        }

        private void LimpiarFormCategoria()
        {
            txtCatId.Clear();
            txtCatNombre.Clear();
            txtCatDescripcion.Clear();
            dgCategorias.SelectedItem = null;
        }
        #endregion

        #region 5, 7 & 11. MANTENIMIENTO DE PROVEEDORES + BÚSQUEDA CON FILTROS
        private void CargarProveedores()
        {
            try
            {
                var lista = _proveedorDAL.Listar();
                dgProveedores.ItemsSource = null;
                dgProveedores.ItemsSource = lista;
                cboProdProveedor.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar proveedores: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscarProveedor_Click(object sender, RoutedEventArgs e)
        {
            string contacto = txtFiltroContacto.Text.Trim();
            string ciudad = txtFiltroCiudad.Text.Trim();

            var resultados = _proveedorDAL.Buscar(contacto, ciudad);
            dgProveedores.ItemsSource = null;
            dgProveedores.ItemsSource = resultados;
        }

        private void BtnLimpiarFiltroProv_Click(object sender, RoutedEventArgs e)
        {
            txtFiltroContacto.Clear();
            txtFiltroCiudad.Clear();
            CargarProveedores();
        }

        private void DgProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProveedores.SelectedItem is Proveedor p)
            {
                txtProvId.Text = p.ProveedorID.ToString();
                txtProvCompania.Text = p.CompaniaNombre;
                txtProvContacto.Text = p.NombreContacto ?? "";
                txtProvCargo.Text = p.CargoContacto ?? "";
                txtProvDireccion.Text = p.Direccion ?? "";
                txtProvCiudad.Text = p.Ciudad ?? "";
                txtProvCodPostal.Text = p.CodigoPostal ?? "";
                txtProvPais.Text = p.Pais ?? "";
                txtProvTelefono.Text = p.Telefono ?? "";
                txtProvFax.Text = p.Fax ?? "";
            }
        }

        private void BtnProvNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormProveedor();
            txtProvCompania.Focus();
        }

        private void BtnProvGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProvCompania.Text))
            {
                MessageBox.Show("Ingrese el nombre de la compañía proveedora.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var p = new Proveedor
            {
                CompaniaNombre = txtProvCompania.Text.Trim(),
                NombreContacto = txtProvContacto.Text.Trim(),
                CargoContacto = txtProvCargo.Text.Trim(),
                Direccion = txtProvDireccion.Text.Trim(),
                Ciudad = txtProvCiudad.Text.Trim(),
                CodigoPostal = txtProvCodPostal.Text.Trim(),
                Pais = txtProvPais.Text.Trim(),
                Telefono = txtProvTelefono.Text.Trim(),
                Fax = txtProvFax.Text.Trim()
            };

            int nuevoId = _proveedorDAL.Insertar(p);
            MessageBox.Show($"Proveedor registrado con éxito (ID: {nuevoId}).", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            CargarProveedores();
            LimpiarFormProveedor();
        }

        private void BtnProvActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtProvId.Text, out int id))
            {
                MessageBox.Show("Seleccione un proveedor para actualizar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var p = new Proveedor
            {
                ProveedorID = id,
                CompaniaNombre = txtProvCompania.Text.Trim(),
                NombreContacto = txtProvContacto.Text.Trim(),
                CargoContacto = txtProvCargo.Text.Trim(),
                Direccion = txtProvDireccion.Text.Trim(),
                Ciudad = txtProvCiudad.Text.Trim(),
                CodigoPostal = txtProvCodPostal.Text.Trim(),
                Pais = txtProvPais.Text.Trim(),
                Telefono = txtProvTelefono.Text.Trim(),
                Fax = txtProvFax.Text.Trim()
            };

            bool ok = _proveedorDAL.Actualizar(p);
            if (ok)
            {
                MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarProveedores();
                LimpiarFormProveedor();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el proveedor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnProvEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtProvId.Text, out int id))
            {
                MessageBox.Show("Seleccione un proveedor para eliminar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Desea eliminar el proveedor ID: {id}?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                bool ok = _proveedorDAL.Eliminar(id);
                if (ok)
                {
                    MessageBox.Show("Proveedor eliminado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarProveedores();
                    LimpiarFormProveedor();
                }
            }
        }

        private void BtnProvLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormProveedor();
        }

        private void LimpiarFormProveedor()
        {
            txtProvId.Clear();
            txtProvCompania.Clear();
            txtProvContacto.Clear();
            txtProvCargo.Clear();
            txtProvDireccion.Clear();
            txtProvCiudad.Clear();
            txtProvCodPostal.Clear();
            txtProvPais.Clear();
            txtProvTelefono.Clear();
            txtProvFax.Clear();
            dgProveedores.SelectedItem = null;
        }
        #endregion

        #region 6 & 12. MANTENIMIENTO DE PEDIDOS (CRUD)
        private void CargarPedidos()
        {
            try
            {
                dgPedidos.ItemsSource = null;
                dgPedidos.ItemsSource = _pedidoDAL.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar pedidos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPedidos.SelectedItem is Pedido ped)
            {
                txtPedId.Text = ped.PedidoID.ToString();
                cboPedCliente.SelectedValue = ped.ClienteID;
                cboPedEmpleado.SelectedValue = ped.EmpleadoID;
                cboPedTransportista.SelectedValue = ped.TransportistaID;
                dpPedFechaPedido.SelectedDate = ped.FechaPedido;
                dpPedFechaRequerida.SelectedDate = ped.FechaRequerida;
                dpPedFechaEnvio.SelectedDate = ped.FechaEnvio;
                txtPedDestinatario.Text = ped.Destinatario ?? "";
                txtPedCiudadDestino.Text = ped.CiudadDestino ?? "";
                txtPedPaisDestino.Text = ped.PaisDestino ?? "";
            }
        }

        private void BtnPedNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormPedido();
            cboPedCliente.Focus();
        }

        private void BtnPedGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (dpPedFechaPedido.SelectedDate == null)
            {
                MessageBox.Show("Seleccione la fecha del pedido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var ped = new Pedido
            {
                ClienteID = cboPedCliente.SelectedValue != null ? (int)cboPedCliente.SelectedValue : null,
                EmpleadoID = cboPedEmpleado.SelectedValue != null ? (int)cboPedEmpleado.SelectedValue : null,
                TransportistaID = cboPedTransportista.SelectedValue != null ? (int)cboPedTransportista.SelectedValue : null,
                FechaPedido = dpPedFechaPedido.SelectedDate.Value,
                FechaRequerida = dpPedFechaRequerida.SelectedDate,
                FechaEnvio = dpPedFechaEnvio.SelectedDate,
                Destinatario = txtPedDestinatario.Text.Trim(),
                CiudadDestino = txtPedCiudadDestino.Text.Trim(),
                PaisDestino = txtPedPaisDestino.Text.Trim()
            };

            int nuevoId = _pedidoDAL.Insertar(ped);
            MessageBox.Show($"Pedido registrado con éxito (ID: {nuevoId}).", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            CargarPedidos();
            LimpiarFormPedido();
        }

        private void BtnPedActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtPedId.Text, out int id))
            {
                MessageBox.Show("Seleccione un pedido para actualizar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var ped = new Pedido
            {
                PedidoID = id,
                ClienteID = cboPedCliente.SelectedValue != null ? (int)cboPedCliente.SelectedValue : null,
                EmpleadoID = cboPedEmpleado.SelectedValue != null ? (int)cboPedEmpleado.SelectedValue : null,
                TransportistaID = cboPedTransportista.SelectedValue != null ? (int)cboPedTransportista.SelectedValue : null,
                FechaPedido = dpPedFechaPedido.SelectedDate ?? DateTime.Today,
                FechaRequerida = dpPedFechaRequerida.SelectedDate,
                FechaEnvio = dpPedFechaEnvio.SelectedDate,
                Destinatario = txtPedDestinatario.Text.Trim(),
                CiudadDestino = txtPedCiudadDestino.Text.Trim(),
                PaisDestino = txtPedPaisDestino.Text.Trim()
            };

            bool ok = _pedidoDAL.Actualizar(ped);
            if (ok)
            {
                MessageBox.Show("Pedido actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarPedidos();
                LimpiarFormPedido();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPedEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtPedId.Text, out int id))
            {
                MessageBox.Show("Seleccione un pedido para eliminar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Desea eliminar el pedido ID: {id}?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                bool ok = _pedidoDAL.Eliminar(id);
                if (ok)
                {
                    MessageBox.Show("Pedido eliminado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarPedidos();
                    LimpiarFormPedido();
                }
            }
        }

        private void BtnPedLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormPedido();
        }

        private void LimpiarFormPedido()
        {
            txtPedId.Clear();
            cboPedCliente.SelectedIndex = -1;
            cboPedEmpleado.SelectedIndex = -1;
            cboPedTransportista.SelectedIndex = -1;
            dpPedFechaPedido.SelectedDate = DateTime.Today;
            dpPedFechaRequerida.SelectedDate = DateTime.Today.AddDays(7);
            dpPedFechaEnvio.SelectedDate = null;
            txtPedDestinatario.Clear();
            txtPedCiudadDestino.Clear();
            txtPedPaisDestino.Clear();
            dgPedidos.SelectedItem = null;
        }
        #endregion

        #region 8 & 12a. REPORTE DETALLE PEDIDOS CON INNER JOIN Y FILTRO DE FECHAS
        private void CargarReporte()
        {
            DateTime inicio = dpReporteInicio.SelectedDate ?? new DateTime(2026, 8, 1);
            DateTime fin = dpReporteFin.SelectedDate ?? new DateTime(2026, 8, 31);

            if (inicio > fin)
            {
                MessageBox.Show("La fecha de inicio no puede ser posterior a la fecha de fin.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var lista = _pedidoDAL.ReporteDetallePorFechas(inicio, fin);
                dgReporteDetalle.ItemsSource = null;
                dgReporteDetalle.ItemsSource = lista;

                // Estadísticas
                int totalRegistros = lista.Count;
                decimal montoTotal = lista.Sum(d => d.Subtotal);

                lblReporteCantidad.Text = $"{totalRegistros} registro{(totalRegistros == 1 ? "" : "s")}";
                lblReporteTotal.Text = montoTotal.ToString("C2", new CultureInfo("es-PE"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            CargarReporte();
        }

        private void BtnRestablecerFechas_Click(object sender, RoutedEventArgs e)
        {
            dpReporteInicio.SelectedDate = new DateTime(2026, 8, 1);
            dpReporteFin.SelectedDate = new DateTime(2026, 8, 31);
            CargarReporte();
        }
        #endregion
    }
}