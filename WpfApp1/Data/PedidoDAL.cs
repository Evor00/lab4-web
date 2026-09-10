using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class PedidoDAL
    {
        public List<Pedido> Listar()
        {
            var lista = new List<Pedido>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Pedidos.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarPedidos", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(MapearPedido(dr));
                }
                return lista;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return MockData.Pedidos.ToList();
            }
        }

        public int Insertar(Pedido ped)
        {
            if (Conexion.ModoDemostracion)
            {
                int nuevoId = MockData.Pedidos.Any() ? MockData.Pedidos.Max(x => x.PedidoID) + 1 : 1;
                ped.PedidoID = nuevoId;
                ped.Cliente = MockData.Clientes.FirstOrDefault(c => c.ClienteID == ped.ClienteID)?.Empresa;
                ped.Empleado = MockData.Empleados.FirstOrDefault(e => e.EmpleadoID == ped.EmpleadoID)?.NombreCompleto;
                ped.Transportista = MockData.Transportistas.FirstOrDefault(t => t.TransportistaID == ped.TransportistaID)?.CompaniaNombre;
                MockData.Pedidos.Add(ped);
                return nuevoId;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_InsertarPedido", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ClienteID", (object?)ped.ClienteID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmpleadoID", (object?)ped.EmpleadoID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaPedido", ped.FechaPedido);
                cmd.Parameters.AddWithValue("@FechaRequerida", (object?)ped.FechaRequerida ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaEnvio", (object?)ped.FechaEnvio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TransportistaID", (object?)ped.TransportistaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Destinatario", (object?)ped.Destinatario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CiudadDestino", (object?)ped.CiudadDestino ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PaisDestino", (object?)ped.PaisDestino ?? DBNull.Value);

                var paramNuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramNuevoId);

                conn.Open();
                cmd.ExecuteNonQuery();

                int id = (int)paramNuevoId.Value;
                ped.PedidoID = id;
                return id;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Insertar(ped);
            }
        }

        public bool Actualizar(Pedido ped)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Pedidos.FirstOrDefault(x => x.PedidoID == ped.PedidoID);
                if (item != null)
                {
                    item.ClienteID = ped.ClienteID;
                    item.Cliente = MockData.Clientes.FirstOrDefault(c => c.ClienteID == ped.ClienteID)?.Empresa;
                    item.EmpleadoID = ped.EmpleadoID;
                    item.Empleado = MockData.Empleados.FirstOrDefault(e => e.EmpleadoID == ped.EmpleadoID)?.NombreCompleto;
                    item.FechaPedido = ped.FechaPedido;
                    item.FechaRequerida = ped.FechaRequerida;
                    item.FechaEnvio = ped.FechaEnvio;
                    item.TransportistaID = ped.TransportistaID;
                    item.Transportista = MockData.Transportistas.FirstOrDefault(t => t.TransportistaID == ped.TransportistaID)?.CompaniaNombre;
                    item.Destinatario = ped.Destinatario;
                    item.CiudadDestino = ped.CiudadDestino;
                    item.PaisDestino = ped.PaisDestino;
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ActualizarPedido", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PedidoID", ped.PedidoID);
                cmd.Parameters.AddWithValue("@ClienteID", (object?)ped.ClienteID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmpleadoID", (object?)ped.EmpleadoID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaPedido", ped.FechaPedido);
                cmd.Parameters.AddWithValue("@FechaRequerida", (object?)ped.FechaRequerida ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaEnvio", (object?)ped.FechaEnvio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TransportistaID", (object?)ped.TransportistaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Destinatario", (object?)ped.Destinatario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CiudadDestino", (object?)ped.CiudadDestino ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PaisDestino", (object?)ped.PaisDestino ?? DBNull.Value);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Actualizar(ped);
            }
        }

        public bool Eliminar(int pedidoId)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Pedidos.FirstOrDefault(x => x.PedidoID == pedidoId);
                if (item != null)
                {
                    MockData.Pedidos.Remove(item);
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_EliminarPedido", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PedidoID", pedidoId);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Eliminar(pedidoId);
            }
        }

        public List<DetallePedidoReporte> ReporteDetallePorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<DetallePedidoReporte>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.DetallesReporte
                    .Where(d => d.FechaPedido.Date >= fechaInicio.Date && d.FechaPedido.Date <= fechaFin.Date)
                    .OrderByDescending(d => d.FechaPedido)
                    .ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ReporteDetallePedidosPorFechas", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new DetallePedidoReporte
                    {
                        PedidoID = dr.GetInt32(dr.GetOrdinal("PedidoID")),
                        FechaPedido = dr.GetDateTime(dr.GetOrdinal("FechaPedido")),
                        Destinatario = dr.IsDBNull(dr.GetOrdinal("Destinatario")) ? null : dr.GetString(dr.GetOrdinal("Destinatario")),
                        CiudadDestino = dr.IsDBNull(dr.GetOrdinal("CiudadDestino")) ? null : dr.GetString(dr.GetOrdinal("CiudadDestino")),
                        ProductoID = dr.GetInt32(dr.GetOrdinal("ProductoID")),
                        NombreProducto = dr.GetString(dr.GetOrdinal("NombreProducto")),
                        PrecioUnidad = dr.GetDecimal(dr.GetOrdinal("PrecioUnidad")),
                        Cantidad = dr.GetInt16(dr.GetOrdinal("Cantidad")),
                        Descuento = dr.GetDecimal(dr.GetOrdinal("Descuento")),
                        Subtotal = dr.GetDecimal(dr.GetOrdinal("Subtotal"))
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return ReporteDetallePorFechas(fechaInicio, fechaFin);
            }
        }

        private static Pedido MapearPedido(SqlDataReader dr)
        {
            return new Pedido
            {
                PedidoID = dr.GetInt32(dr.GetOrdinal("PedidoID")),
                ClienteID = dr.IsDBNull(dr.GetOrdinal("ClienteID")) ? null : dr.GetInt32(dr.GetOrdinal("ClienteID")),
                Cliente = dr.IsDBNull(dr.GetOrdinal("Cliente")) ? null : dr.GetString(dr.GetOrdinal("Cliente")),
                EmpleadoID = dr.IsDBNull(dr.GetOrdinal("EmpleadoID")) ? null : dr.GetInt32(dr.GetOrdinal("EmpleadoID")),
                Empleado = dr.IsDBNull(dr.GetOrdinal("Empleado")) ? null : dr.GetString(dr.GetOrdinal("Empleado")),
                FechaPedido = dr.GetDateTime(dr.GetOrdinal("FechaPedido")),
                FechaRequerida = dr.IsDBNull(dr.GetOrdinal("FechaRequerida")) ? null : dr.GetDateTime(dr.GetOrdinal("FechaRequerida")),
                FechaEnvio = dr.IsDBNull(dr.GetOrdinal("FechaEnvio")) ? null : dr.GetDateTime(dr.GetOrdinal("FechaEnvio")),
                TransportistaID = dr.IsDBNull(dr.GetOrdinal("TransportistaID")) ? null : dr.GetInt32(dr.GetOrdinal("TransportistaID")),
                Transportista = dr.IsDBNull(dr.GetOrdinal("Transportista")) ? null : dr.GetString(dr.GetOrdinal("Transportista")),
                Destinatario = dr.IsDBNull(dr.GetOrdinal("Destinatario")) ? null : dr.GetString(dr.GetOrdinal("Destinatario")),
                CiudadDestino = dr.IsDBNull(dr.GetOrdinal("CiudadDestino")) ? null : dr.GetString(dr.GetOrdinal("CiudadDestino")),
                PaisDestino = dr.IsDBNull(dr.GetOrdinal("PaisDestino")) ? null : dr.GetString(dr.GetOrdinal("PaisDestino"))
            };
        }
    }
}
