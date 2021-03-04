using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Vehiculo
    {
        public int VEH_CODIGO { get; set; }
        public int REN_CODIGO { get; set; }
        public int VEH_CANT_PASAJEROS { get; set; }
        public string VEH_MODELO { get; set; }
        public string VEH_ESTADO { get; set; }
        public string VEH_DESCRIPCION { get; set; }
    }
}