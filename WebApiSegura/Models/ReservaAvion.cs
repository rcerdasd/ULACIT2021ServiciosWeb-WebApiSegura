using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class ReservaAvion
    {
        public int RES_AVI_CODIGO { get; set; }
        public int USU_CODIGO { get; set; }
        public int AVI_CODIGO { get; set; }
        public DateTime RES_AVI_FEC_VUELO { get; set; }
        public decimal RES_AVI_DURACION { get; set; }
        public string RES_AVI_ESCALA { get; set; }

    }
}