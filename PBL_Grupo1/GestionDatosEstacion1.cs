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
    class GestionDatosEstacion1
    {
        private delegadoMensajes imprimirMensajeRecibido;
        private delegadoPintar pinta;
        private delegadoDespintar despinta;

        public GestionDatosEstacion1(delegadoMensajes _datos, delegadoPintar _tag, delegadoDespintar _tag2)
        {
            imprimirMensajeRecibido = _datos;
            pinta = _tag;
            despinta = _tag2;
        }

        public void procesar(byte[] datos, int dim)
        {
            byte basicModuleOutputs = datos[2];
            byte basicModuleInputs = datos[3];

            gestionBasicModule(basicModuleOutputs, basicModuleInputs);

        }

        private void gestionBasicModule(byte basicModuleOutputs, byte basicModuleInputs)
        {
            BitArray bitsbasicModuleInputs = new BitArray(new byte[] { basicModuleInputs });
            imprimirMensajeRecibido(new byte[] { basicModuleInputs, basicModuleOutputs }, bitsbasicModuleInputs.Length);

            bool bitMarcha = bitsbasicModuleInputs[0];
            bool bitParo = bitsbasicModuleInputs[1];
            bool bitRearme = bitsbasicModuleInputs[3];

            if (bitMarcha.Equals(true))
            {
                pinta("marcha");
            }else {
                despinta("marcha");
            }

            if (bitParo)
            {
                pinta("paro");
            }
            else
            {
                despinta("paro");
            }

            if (bitRearme)
            {
                pinta("rearme");
            }
            else
            {
                despinta("rearme");
            }

        }
    }
}
