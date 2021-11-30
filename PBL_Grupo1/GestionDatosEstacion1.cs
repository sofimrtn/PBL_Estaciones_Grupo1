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
        private List<Ellipse> lucesEstacion1;
        private List<Ellipse> lucesEstacion2;


        public GestionDatosEstacion1(delegadoMensajes _datos, pintaLuces _delLuces, List<Ellipse> _luces, List<Ellipse> _luces2)
        {
            imprimirMensajeRecibido = _datos;
            lucesEstacion1 = _luces;
            lucesEstacion2 = _luces2;
            pintarLuces = _delLuces;
        }

        public void procesar(byte[] datos, int dim)
        {
            Estacion1(datos);
            Estacion2(datos);

        }

        private void Estacion1(byte[] datos)
        {
            BitArray frontCoverOutputs = new BitArray(new byte[] { datos[0] });
            BitArray frontCoverInputs = new BitArray(new byte[] { datos[1] });
            BitArray bitsbasicModuleOutputs = new BitArray(new byte[] { datos[2] });
            BitArray bitsbasicModuleInputs = new BitArray(new byte[] { datos[3] });

            BitArray StopperInputs = new BitArray(new byte[] { datos[5] });

            imprimirMensajeRecibido(new byte[] { datos[2], datos[3] }, 8);

            BitArray module1 = Append(bitsbasicModuleInputs, bitsbasicModuleOutputs);
            BitArray module2 = Append(frontCoverInputs, frontCoverOutputs);
            BitArray aux = Append(module1, module2);
            BitArray estacion1 = Append(aux, StopperInputs);

            for (int i = 0; i < estacion1.Count; i++)
            {
                pintarLuces(estacion1[i], lucesEstacion1[i], estacion1, 1);
            }
        }


        private void Estacion2(byte[] datos)
        {
            //TODO
            BitArray bitsbasicModule_2_Outputs = new BitArray(new byte[] { datos[6] });
            BitArray bitsbasicModule_2_Inputs = new BitArray(new byte[] { datos[7] });

            BitArray measuringOutputs = new BitArray(new byte[] { datos[8] });
            BitArray measuringInputs = new BitArray(new byte[] { datos[9] });

            BitArray StopperInputs = new BitArray(new byte[] { datos[11] });

            BitArray module1_2 = Append(bitsbasicModule_2_Inputs, bitsbasicModule_2_Outputs);
            BitArray module2_2 = Append(measuringInputs, measuringOutputs);
            BitArray aux = Append(module1_2, module2_2);

            BitArray estacion2 = Append(aux, StopperInputs);

            for (int i = 0; i < estacion2.Count; i++)
            {
                pintarLuces(estacion2[i], lucesEstacion2[i], estacion2, 2);
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
