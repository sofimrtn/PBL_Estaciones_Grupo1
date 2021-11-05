using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PBL_Grupo1
{
    public class GestionDatosEstacion1
    {
        private delegadoMensajes imprimirMensajeRecibido;
        private pintaLuces pintarLuces;
        private List<Ellipse> luces;


        public GestionDatosEstacion1(delegadoMensajes _datos, pintaLuces _delLuces, List<Ellipse> _luces)
        {
            imprimirMensajeRecibido = _datos;
            luces = _luces;
            pintarLuces = _delLuces;
        }

        public void procesar(byte[] datos, int dim)
        {
            BitArray frontCoverOutputs = new BitArray(new byte[] { datos[0] });
            BitArray frontCoverInputs = new BitArray(new byte[] { datos[1] });
            BitArray bitsbasicModuleOutputs = new BitArray(new byte[] { datos[2] });
            BitArray bitsbasicModuleInputs = new BitArray(new byte[] { datos[3] });

            BitArray StopperInputs = new BitArray(new byte[] { datos[4] });

            imprimirMensajeRecibido(new byte[] { datos[2], datos[3] }, 8);

            BitArray module1 = Append(bitsbasicModuleInputs, bitsbasicModuleOutputs);
            BitArray module2 = Append(frontCoverInputs, frontCoverOutputs);
            BitArray aux = Append(module1, module2);
            BitArray estacion1 = Append(aux, StopperInputs);

            for (int i = 0; i < estacion1.Count; i++)
            {
                pintarLuces(estacion1[i], luces[i], estacion1);
            }

        }

        public BitArray Append(BitArray current, BitArray after)
        {
            bool[] bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

    }
}
