using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GrupoJ.Controllers
{
    [EnableCors (origins:"*", headers:"*", methods:"*")]
    public class TiquetesController : ApiController
    {
        private GrupoJEntities bd = new GrupoJEntities();

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Tickets/NuevoTique")]
        public Respuesta NuevoTiquete([FromBody] Tiquetes_prof Tk)
        {
            Respuesta R = new Respuesta();
            try
            {
                Tk.Fecha = DateTime.Now;
                Tk.Estado = "PEN";
                bd.Tiquetes_prof.Add(Tk);
                bd.SaveChanges();
                R.Codigo = Tk.Codigo_tiquete;
                R.Mensaje = "Tiquete agregado exitosamente, #"+ Tk.Codigo_tiquete.ToString();
            }
            catch (Exception Er)
            {
                R.Codigo = -1;
                R.Mensaje = "Error: " + Er.Message;
            }
            return R;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Tickets/ListaClientes")]
        public object ListaClientes() {


            var Cls = from Cl in bd.Clientes.Where(d=> String.IsNullOrEmpty( d.nombre)  != true  ) select new { Cl.codigo_cliente, Cl.nombre };
            return Cls;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Tickets/ListaUsuarios")]
        public object ListaUsuarios()
        {
            return  from Us in bd.Usuarios.Where(d=>String.IsNullOrEmpty(d.Nombre) != true) select new { Us.Usuario, Us.Nombre };
            
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Tickets/CasosPendientes")]
        public object CasosPendientes(DateTime Desde, DateTime Hasta)
        {
            return  bd.Reporte_Estados(Desde, Hasta) ;

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Tickets/ListaClientesExtensa")]
        public List<Respuesta> ListaClientesExtensa()
        {
            var Clientes = bd.Clientes.ToList();
            List<Respuesta> R = new List<Respuesta>();

            foreach (var C in Clientes)
            {
                R.Add(new Respuesta { Codigo = C.codigo_cliente, Mensaje = C.nombre });
            }
            return R;
        }
    }

    

    public class Respuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
    }
}
