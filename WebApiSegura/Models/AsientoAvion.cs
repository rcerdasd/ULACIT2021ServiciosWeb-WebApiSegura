using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class AsientoAvion
    {
        public int ASI_AVI_CODIGO { get; set; }
        public int AVI_CODIGO { get; set; }
        public int ASI_AVI_NUMERO { get; set; }
        public string ASI_AVI_POSICION { get; set; }
        public string ASI_AVI_CLASE { get; set; }
        public decimal ASI_AVI_PRECIO { get; set; }

    }
}