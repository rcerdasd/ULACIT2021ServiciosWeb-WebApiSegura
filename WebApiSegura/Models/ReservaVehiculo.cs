using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class ReservaVehiculo
    {
        public int RES_VEH_CODIGO { get; set; }
        public int USU_CODIGO { get; set; }
        public int PAQ_VEH_CODIGO { get; set; }
        public DateTime RES_VEH_FEC_SALIDA { get; set; }
        public DateTime RES_VEH_FEC_REGRESO { get; set; }
    }
}