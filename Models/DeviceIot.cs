using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotDevicesSimulator.Models
{
    public class DeviceIot
    {
        // Identificador físico único (simulado)
        public string MacAddress { get; set; }

        public string Nombre { get; set; }
        public string Tipo { get; set; } // Ej: "SensorTemperatura", "CamaraSeguridad"

        // Estado actual (Telemetría)
        public string Status { get; set; } = "Apagado";

        // Aquí guardaremos los ajustes que vienen del Frontend
        public object Configuracion { get; set; }
    }
}
