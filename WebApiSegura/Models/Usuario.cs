using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Usuario
    {
        public int USU_CODIGO { get; set; }
        public string USU_IDENTIFICACION { get; set; }

        public string USU_NOMBRE { get; set; }

        public string USU_PASSWORD { get; set; }
        public string USU_EMAIL { get; set; }
        public string USU_ESTADO { get; set; }
        public string USU_FECH_NAC { get; set; }
        public string USU_TELEFONO { get; set; }

    }
}