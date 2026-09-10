namespace WpfApp1.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string? NombreContacto { get; set; }

        public override string ToString() => Empresa;
    }

    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Cargo { get; set; }

        public override string ToString() => NombreCompleto;
    }

    public class Transportista
    {
        public int TransportistaID { get; set; }
        public string CompaniaNombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }

        public override string ToString() => CompaniaNombre;
    }
}
