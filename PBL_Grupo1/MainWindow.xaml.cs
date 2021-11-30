using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PBL_Grupo1
{
    public delegate void delegadoMensajes(byte[] datos, int dim);
    public delegate void delegadoProcesar(byte[] datos, int dim);
    public delegate void delegadoPintar(bool bit);

    public delegate void pintaLuces(bool bit, Ellipse e, BitArray bits, int estacion);


    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegadoMensajes delegadoImprimeMensajes;
        private delegadoProcesar delegadoProcesarMensaje;
        private pintaLuces delegadoPintaLuces;

        List<Ellipse> luces;
        List<Ellipse> luces2;

        private GestionDatosEstacion1 gestorEstacion1;
        private Cliente client = null;
        private bool connected = false;

        private BitArray bitsEstacion1;
        private BitArray bitsEstacion2;
        private BitArray bitsEstacion3;

        private _3D_SCADA scada;
        private Estacion2_SCADA scada_2;

        public MainWindow()
        {
            InitializeComponent();

            luces = new List<Ellipse> {luz_marcha, luz_paro, luz_auto, luz_rearme, luz_ind0, luz_seta, luz_paletIzq, luz_paletDer,
                                    luz_oMarchaLuz, luz_oParoLuz, luz_oQ1Luz, luz_oQ2Luz, luz_oCintaDer, luz_oCintaIzq, luz_oCintaSlow, luz_oStopperDown,
                                    elevadorArriba, elevadorAbajo, separadorCerrado, separadorAbierto, almacenVacio,null,paletOK,frontOK,
                                    salSubirElev, salBajarElev,salCerrarSep, salAbrirSep,frenoCilindro, null, null, null,
                                    ind0, ind1, ind2, ind3, null, null, null, stopperDown};

            luces2 = new List<Ellipse> { };

            delegadoImprimeMensajes = new delegadoMensajes(muestraDatosRecibidos);
            delegadoPintaLuces = new pintaLuces(funcionPintaLuces);

            gestorEstacion1 = new GestionDatosEstacion1(delegadoImprimeMensajes, delegadoPintaLuces, luces, luces2);

            delegadoProcesarMensaje = new delegadoProcesar(gestorEstacion1.procesar);

            scada = new _3D_SCADA();
            scada_2 = new Estacion2_SCADA();

        }

        private void funcionPintaLuces(bool bit, Ellipse e, BitArray bits, int estacion)
        {
            if (e == null)
            {
                return;
            }
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(delegadoPintaLuces, new Object[3] { bit, e, bits });
            }
            else
            {
                color(e, bit);
                if (estacion == 1)
                {
                    bitsEstacion1 = bits;
                    scada.setBits(bits);
                }
                else if (estacion == 2)
                {
                    bitsEstacion2 = bits;
                    scada_2.setBits(bits);
                }
                else
                {
                    bitsEstacion3 = bits;
                    //scada.setBits(bits);
                }

            }
            return;
        }

        private void color(Ellipse e, bool bit)
        {
            if (bit)
            {
                e.Fill = new SolidColorBrush(Colors.Green);
            }
            else
            {
                e.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4F4F5"));
            }
        }

        private void muestraDatosRecibidos(byte[] datos, int dim)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(delegadoImprimeMensajes, new Object[2] { datos, dim });
            }
            else
            {
                imprimeMensaje(datos, dim);
            }
            return;
        }

        private void imprimeMensaje(byte[] datos, int dim)
        {
            var time24 = DateTime.Now.ToString("HH:mm:ss");
            txtB_Hex.AppendText(time24 + " -- ");
            for (int i = 0; i < datos.Length; i++)
            {
                txtB_Hex.AppendText(Convert.ToString(datos[i], 2) + " ");
            }
            txtB_Hex.AppendText("\n");

            txtB_Hex.ScrollToEnd();
        }

        private void btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            if (client != null) client.closeClient();
            client = null;
            scada.Close();
            Close();
        }


        private void btn_Conectar_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                client = new Cliente(txtB_IP.Text, Convert.ToInt32(txtB_Port.Text), delegadoProcesarMensaje);
                if (client.conectToServer())
                {
                    btn_Conectar.Content = "Desconectar";

                    connected = true;
                    conectado.Visibility = Visibility.Visible;
                    noConexion.Visibility = Visibility.Hidden;


                }
                else client = null;
            }
            else
            {
                if (client != null) client.closeClient();

                client = null;
                connected = false;

                btn_Conectar.Content = "Conectar";
                conectado.Visibility = Visibility.Hidden;
                noConexion.Visibility = Visibility.Visible;
            }

            return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            scada.Show();
            //scada = new _3D_SCADA();
        }

        private void estacion2_click(object sender, RoutedEventArgs e)
        {
            scada_2.Show();
            //scada_2 = new Estacion2_SCADA();
        }
    }
}
