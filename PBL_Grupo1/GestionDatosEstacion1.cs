using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL_Grupo1
{
    class GestionDatosEstacion1
    {
        private delegadoMensajes imprimirMensajeRecibido;

        public GestionDatosEstacion1(delegadoMensajes _datos)
        {
            imprimirMensajeRecibido = _datos;

        }

        public void procesar(byte[] datos, int dim)
        {
            imprimirMensajeRecibido(datos, dim);
        }
    }
}
