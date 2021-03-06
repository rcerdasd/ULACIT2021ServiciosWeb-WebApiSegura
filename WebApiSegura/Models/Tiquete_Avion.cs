using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Tiquete_Avion
    {
        public int TIQ_AVI_CODIGO {get; set;}
        public int RES_AVI_CODIGO { get; set; }
        public string TIQ_AVI_ORIGEN { get; set; }
        public string TIQ_AVI_DESTINO { get; set; }
        public decimal TIQ_AVI_PRECIO { get; set; }
    }
}