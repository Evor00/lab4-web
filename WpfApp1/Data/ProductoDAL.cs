using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class ProductoDAL
    {
        public List<Producto> Listar()
        {
            var lista = new List<Producto>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Productos.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarProductos", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(MapearProducto(dr));
                }
                return lista;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return MockData.Productos.ToList();
            }
        }

        public int Insertar(Producto prod)
        {
            if (Conexion.ModoDemostracion)
            {
                int nuevoId = MockData.Productos.Any() ? MockData.Productos.Max(x => x.ProductoID) + 1 : 1;
                prod.ProductoID = nuevoId;
                prod.Categoria = MockData.Categorias.FirstOrDefault(c => c.CategoriaID == prod.CategoriaID)?.NombreCategoria;
                prod.Proveedor = MockData.Proveedores.FirstOrDefault(p => p.ProveedorID == prod.ProveedorID)?.CompaniaNombre;
                MockData.Productos.Add(prod);
                return nuevoId;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_InsertarProducto", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@NombreProducto", prod.NombreProducto);
                cmd.Parameters.AddWithValue("@ProveedorID", (object?)prod.ProveedorID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoriaID", (object?)prod.CategoriaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CantidadPorUnidad", (object?)prod.CantidadPorUnidad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PrecioUnidad", prod.PrecioUnidad);
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", prod.UnidadesEnExistencia);
                cmd.Parameters.AddWithValue("@UnidadesEnPedido", prod.UnidadesEnPedido);
                cmd.Parameters.AddWithValue("@NivelDeReorden", prod.NivelDeReorden);
                cmd.Parameters.AddWithValue("@Descontinuado", prod.Descontinuado);

                var paramNuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramNuevoId);

                conn.Open();
                cmd.ExecuteNonQuery();

                int id = (int)paramNuevoId.Value;
                prod.ProductoID = id;
                return id;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Insertar(prod);
            }
        }

        public bool Actualizar(Producto prod)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Productos.FirstOrDefault(x => x.ProductoID == prod.ProductoID);
                if (item != null)
                {
                    item.NombreProducto = prod.NombreProducto;
                    item.ProveedorID = prod.ProveedorID;
                    item.Proveedor = MockData.Proveedores.FirstOrDefault(p => p.ProveedorID == prod.ProveedorID)?.CompaniaNombre;
                    item.CategoriaID = prod.CategoriaID;
                    item.Categoria = MockData.Categorias.FirstOrDefault(c => c.CategoriaID == prod.CategoriaID)?.NombreCategoria;
                    item.CantidadPorUnidad = prod.CantidadPorUnidad;
                    item.PrecioUnidad = prod.PrecioUnidad;
                    item.UnidadesEnExistencia = prod.UnidadesEnExistencia;
                    item.UnidadesEnPedido = prod.UnidadesEnPedido;
                    item.NivelDeReorden = prod.NivelDeReorden;
                    item.Descontinuado = prod.Descontinuado;
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ActualizarProducto", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductoID", prod.ProductoID);
                cmd.Parameters.AddWithValue("@NombreProducto", prod.NombreProducto);
                cmd.Parameters.AddWithValue("@ProveedorID", (object?)prod.ProveedorID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoriaID", (object?)prod.CategoriaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CantidadPorUnidad", (object?)prod.CantidadPorUnidad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PrecioUnidad", prod.PrecioUnidad);
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", prod.UnidadesEnExistencia);
                cmd.Parameters.AddWithValue("@UnidadesEnPedido", prod.UnidadesEnPedido);
                cmd.Parameters.AddWithValue("@NivelDeReorden", prod.NivelDeReorden);
                cmd.Parameters.AddWithValue("@Descontinuado", prod.Descontinuado);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Actualizar(prod);
            }
        }

        public bool Eliminar(int productoId)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Productos.FirstOrDefault(x => x.ProductoID == productoId);
                if (item != null)
                {
                    MockData.Productos.Remove(item);
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_EliminarProducto", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductoID", productoId);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Eliminar(productoId);
            }
        }

        private static Producto MapearProducto(SqlDataReader dr)
        {
            return new Producto
            {
                ProductoID = dr.GetInt32(dr.GetOrdinal("ProductoID")),
                NombreProducto = dr.GetString(dr.GetOrdinal("NombreProducto")),
                ProveedorID = dr.IsDBNull(dr.GetOrdinal("ProveedorID")) ? null : dr.GetInt32(dr.GetOrdinal("ProveedorID")),
                Proveedor = dr.IsDBNull(dr.GetOrdinal("Proveedor")) ? null : dr.GetString(dr.GetOrdinal("Proveedor")),
                CategoriaID = dr.IsDBNull(dr.GetOrdinal("CategoriaID")) ? null : dr.GetInt32(dr.GetOrdinal("CategoriaID")),
                Categoria = dr.IsDBNull(dr.GetOrdinal("Categoria")) ? null : dr.GetString(dr.GetOrdinal("Categoria")),
                CantidadPorUnidad = dr.IsDBNull(dr.GetOrdinal("CantidadPorUnidad")) ? null : dr.GetString(dr.GetOrdinal("CantidadPorUnidad")),
                PrecioUnidad = dr.GetDecimal(dr.GetOrdinal("PrecioUnidad")),
                UnidadesEnExistencia = dr.GetInt16(dr.GetOrdinal("UnidadesEnExistencia")),
                UnidadesEnPedido = dr.GetInt16(dr.GetOrdinal("UnidadesEnPedido")),
                NivelDeReorden = dr.GetInt16(dr.GetOrdinal("NivelDeReorden")),
                Descontinuado = dr.GetBoolean(dr.GetOrdinal("Descontinuado"))
            };
        }
    }
}
