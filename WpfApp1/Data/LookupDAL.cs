using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class LookupDAL
    {
        public List<Cliente> ListarClientes()
        {
            var lista = new List<Cliente>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Clientes.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarClientes", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Cliente
                    {
                        ClienteID = dr.GetInt32(dr.GetOrdinal("ClienteID")),
                        Empresa = dr.GetString(dr.GetOrdinal("Empresa")),
                        NombreContacto = dr.IsDBNull(dr.GetOrdinal("NombreContacto")) ? null : dr.GetString(dr.GetOrdinal("NombreContacto"))
                    });
                }
                return lista;
            }
            catch
            {
                Conexion.ModoDemostracion = true;
                return MockData.Clientes.ToList();
            }
        }

        public List<Empleado> ListarEmpleados()
        {
            var lista = new List<Empleado>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Empleados.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarEmpleados", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Empleado
                    {
                        EmpleadoID = dr.GetInt32(dr.GetOrdinal("EmpleadoID")),
                        NombreCompleto = dr.GetString(dr.GetOrdinal("NombreCompleto")),
                        Cargo = dr.IsDBNull(dr.GetOrdinal("Cargo")) ? null : dr.GetString(dr.GetOrdinal("Cargo"))
                    });
                }
                return lista;
            }
            catch
            {
                Conexion.ModoDemostracion = true;
                return MockData.Empleados.ToList();
            }
        }

        public List<Transportista> ListarTransportistas()
        {
            var lista = new List<Transportista>();

            if (Conexion.ModoDemostracion)
            {
                return MockData.Transportistas.ToList();
            }

            try
            {
                using var conn = Conexion.ObtenerConexion();
                using var cmd = new SqlCommand("dbo.sp_ListarTransportistas", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Transportista
                    {
                        TransportistaID = dr.GetInt32(dr.GetOrdinal("TransportistaID")),
                        CompaniaNombre = dr.GetString(dr.GetOrdinal("CompaniaNombre")),
                        Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? null : dr.GetString(dr.GetOrdinal("Telefono"))
                    });
                }
                return lista;
            }
            catch
            {
                Conexion.ModoDemostracion = true;
                return MockData.Transportistas.ToList();
            }
        }
    }
}
