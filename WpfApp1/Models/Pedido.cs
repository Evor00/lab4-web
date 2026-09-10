using System;

namespace WpfApp1.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int? ClienteID { get; set; }
        public string? Cliente { get; set; }
        public int? EmpleadoID { get; set; }
        public string? Empleado { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Today;
        public DateTime? FechaRequerida { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int? TransportistaID { get; set; }
        public string? Transportista { get; set; }
        public string? Destinatario { get; set; }
        public string? CiudadDestino { get; set; }
        public string? PaisDestino { get; set; }
    }
}
