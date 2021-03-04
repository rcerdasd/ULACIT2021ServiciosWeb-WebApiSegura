using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Rentacar
    {
        public int REN_CODIGO { get; set; }
        public string REN_NOMBRE { get; set; }
        public string REN_PAIS { get; set; }
        public string REN_TELEFONO { get; set; }
        public string REN_EMAIL { get; set; }
    }
}