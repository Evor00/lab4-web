using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class CategoriaDAL
    {
        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Categorias.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarCategorias", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Categoria
                    {
                        CategoriaID = dr.GetInt32(dr.GetOrdinal("CategoriaID")),
                        NombreCategoria = dr.GetString(dr.GetOrdinal("NombreCategoria")),
                        Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? null : dr.GetString(dr.GetOrdinal("Descripcion"))
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return MockData.Categorias.ToList();
            }
        }

        public int Insertar(Categoria cat)
        {
            if (Conexion.ModoDemostracion)
            {
                int nuevoId = MockData.Categorias.Any() ? MockData.Categorias.Max(c => c.CategoriaID) + 1 : 1;
                cat.CategoriaID = nuevoId;
                MockData.Categorias.Add(cat);
                return nuevoId;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_InsertarCategoria", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@NombreCategoria", cat.NombreCategoria);
                cmd.Parameters.AddWithValue("@Descripcion", (object?)cat.Descripcion ?? DBNull.Value);

                var paramNuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramNuevoId);

                conn.Open();
                cmd.ExecuteNonQuery();

                int id = (int)paramNuevoId.Value;
                cat.CategoriaID = id;
                return id;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Insertar(cat);
            }
        }

        public bool Actualizar(Categoria cat)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Categorias.FirstOrDefault(c => c.CategoriaID == cat.CategoriaID);
                if (item != null)
                {
                    item.NombreCategoria = cat.NombreCategoria;
                    item.Descripcion = cat.Descripcion;
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ActualizarCategoria", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CategoriaID", cat.CategoriaID);
                cmd.Parameters.AddWithValue("@NombreCategoria", cat.NombreCategoria);
                cmd.Parameters.AddWithValue("@Descripcion", (object?)cat.Descripcion ?? DBNull.Value);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Actualizar(cat);
            }
        }

        public bool Eliminar(int categoriaId)
        {
            if (Conexion.ModoDemostracion)
            {
                var item = MockData.Categorias.FirstOrDefault(c => c.CategoriaID == categoriaId);
                if (item != null)
                {
                    MockData.Categorias.Remove(item);
                    return true;
                }
                return false;
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_EliminarCategoria", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CategoriaID", categoriaId);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
            catch (Exception)
            {
                Conexion.ModoDemostracion = true;
                return Eliminar(categoriaId);
            }
        }
    }
}
