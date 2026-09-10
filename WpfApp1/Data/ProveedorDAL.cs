using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class ProveedorDAL
    {
        public List<Proveedor> Listar()
        {
            var lista = new List<Proveedor>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Proveedores.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarProveedores", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(MapearProveedor(dr));
                }
                return lista;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return MockData.Proveedores.ToList();
            }
        }

        public List<Proveedor> Buscar(string? nombreContacto, string? ciudad)
        {
            var lista = new List<Proveedor>();

            if (Conexion.ModoDemostracion)
            {
                var query = MockData.Proveedores.AsQueryable();
                if (!string.IsNullOrWhiteSpace(nombreContacto))
                {
                    query = query.Where(p => p.NombreContacto != null && 
                        p.NombreContacto.Contains(nombreContacto, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrWhiteSpace(ciudad))
                {
                    query = query.Where(p => p.Ciudad != null && 
                        p.Ciudad.Contains(ciudad, StringComparison.OrdinalIgnoreCase));
                }
                return query.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_BuscarProveedores", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@NombreContacto", string.IsNullOrWhiteSpace(nombreContacto) ? (object)DBNull.Value : nombreContacto.Trim());
                cmd.Parameters.AddWithValue("@Ciudad", string.IsNullOrWhiteSpace(ciudad) ? (object)DBNull.Value : ciudad.Trim());

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(MapearProveedor(dr));
                }
                return lista;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Buscar(nombreContacto, ciudad);
            }
        }

        public int Insertar(Proveedor p)
        {
            if (Conexion.ModoDemostracion)
            {
                int nuevoId = MockData.Proveedores.Any() ? MockData.Proveedores.Max(x => x.ProveedorID) + 1 : 1;
                p.ProveedorID = nuevoId;
                MockData.Proveedores.Add(p);
                return nuevoId;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_InsertarProveedor", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CompaniaNombre", p.CompaniaNombre);
                cmd.Parameters.AddWithValue("@NombreContacto", (object?)p.NombreContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CargoContacto", (object?)p.CargoContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ciudad", (object?)p.Ciudad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CodigoPostal", (object?)p.CodigoPostal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pais", (object?)p.Pais ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Fax", (object?)p.Fax ?? DBNull.Value);

                var paramNuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramNuevoId);

                conn.Open();
                cmd.ExecuteNonQuery();

                int id = (int)paramNuevoId.Value;
                p.ProveedorID = id;
                return id;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Insertar(p);
            }
        }

        public bool Actualizar(Proveedor p)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Proveedores.FirstOrDefault(x => x.ProveedorID == p.ProveedorID);
                if (item != null)
                {
                    item.CompaniaNombre = p.CompaniaNombre;
                    item.NombreContacto = p.NombreContacto;
                    item.CargoContacto = p.CargoContacto;
                    item.Direccion = p.Direccion;
                    item.Ciudad = p.Ciudad;
                    item.CodigoPostal = p.CodigoPostal;
                    item.Pais = p.Pais;
                    item.Telefono = p.Telefono;
                    item.Fax = p.Fax;
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ActualizarProveedor", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProveedorID", p.ProveedorID);
                cmd.Parameters.AddWithValue("@CompaniaNombre", p.CompaniaNombre);
                cmd.Parameters.AddWithValue("@NombreContacto", (object?)p.NombreContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CargoContacto", (object?)p.CargoContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ciudad", (object?)p.Ciudad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CodigoPostal", (object?)p.CodigoPostal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pais", (object?)p.Pais ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Fax", (object?)p.Fax ?? DBNull.Value);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Actualizar(p);
            }
        }

        public bool Eliminar(int proveedorId)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Proveedores.FirstOrDefault(x => x.ProveedorID == proveedorId);
                if (item != null)
                {
                    MockData.Proveedores.Remove(item);
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_EliminarProveedor", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProveedorID", proveedorId);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Eliminar(proveedorId);
            }
        }

        private static Proveedor MapearProveedor(SqlDataReader dr)
        {
            return new Proveedor
            {
                ProveedorID = dr.GetInt32(dr.GetOrdinal("ProveedorID")),
                CompaniaNombre = dr.GetString(dr.GetOrdinal("CompaniaNombre")),
                NombreContacto = dr.IsDBNull(dr.GetOrdinal("NombreContacto")) ? null : dr.GetString(dr.GetOrdinal("NombreContacto")),
                CargoContacto = dr.IsDBNull(dr.GetOrdinal("CargoContacto")) ? null : dr.GetString(dr.GetOrdinal("CargoContacto")),
                Direccion = dr.IsDBNull(dr.GetOrdinal("Direccion")) ? null : dr.GetString(dr.GetOrdinal("Direccion")),
                Ciudad = dr.IsDBNull(dr.GetOrdinal("Ciudad")) ? null : dr.GetString(dr.GetOrdinal("Ciudad")),
                CodigoPostal = dr.IsDBNull(dr.GetOrdinal("CodigoPostal")) ? null : dr.GetString(dr.GetOrdinal("CodigoPostal")),
                Pais = dr.IsDBNull(dr.GetOrdinal("Pais")) ? null : dr.GetString(dr.GetOrdinal("Pais")),
                Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? null : dr.GetString(dr.GetOrdinal("Telefono")),
                Fax = dr.IsDBNull(dr.GetOrdinal("Fax")) ? null : dr.GetString(dr.GetOrdinal("Fax"))
            };
        }
    }
}
