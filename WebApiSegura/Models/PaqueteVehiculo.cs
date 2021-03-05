using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class PaqueteVehiculo
    {
        public int PAQ_VEH_CODIGO { get; set; }
        public int VEH_CODIGO { get; set; }
        public string PAQ_SEGURO { get; set; }
        public string PAQ_BICICLETA { get; set; }
        public string PAQ_DESCRIPCION { get; set; }
    }
}