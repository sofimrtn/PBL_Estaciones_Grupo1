using System;
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
    public delegate void delegadoPintar(bool tag);


    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegadoMensajes delegadoImprimeMensajes;
        private delegadoProcesar delegadoProcesarMensaje;


        private delegadoPintar delPintarMarcha;
        private delegadoPintar delPintarParo;
        private delegadoPintar delPintarRearme;
        private delegadoPintar delPintaAuto;

                private List<Delegate> listPintar;

        private GestionDatosEstacion1 gestorEstacion1;
        private Cliente client = null;
        private bool connected = false;
        public MainWindow()
        {
            InitializeComponent();
            delegadoImprimeMensajes = new delegadoMensajes(muestraDatosRecibidos);
            delPintarMarcha = new delegadoPintar(pintaMarcha);
            delPintarParo = new delegadoPintar(pintaParo);
            delPintarRearme = new delegadoPintar(pintaRearme);
            delPintaAuto = new delegadoPintar(pintaMarcha); //CHECKKK

            listPintar = new List<Delegate> {delPintarMarcha, delPintarParo, delPintaAuto, delPintarRearme};

            gestorEstacion1 = new GestionDatosEstacion1(delegadoImprimeMensajes, listPintar);

            delegadoProcesarMensaje = new delegadoProcesar(gestorEstacion1.procesar);

        }

        private bool checkDelegate(Delegate del, bool bit)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(del, new Object[1] { bit });
                return false;
            } else
            {
                return true;
            }
        }

        private void pintaMarcha(bool bit)
        {
            if(checkDelegate(delPintarMarcha, bit))
            {
                if (bit)
                {
                    luz_marcha.Fill = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    luz_marcha.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4F4F5"));
                }
            }
                    
        }

        private void pintaParo(bool bit)
        {
            if(checkDelegate(delPintarParo, bit))
            {
                if (bit)
                {
                    luz_paro.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    luz_paro.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4F4F5"));
                }
            }
            
        }

        private void pintaRearme(bool bit)
        {
            if (checkDelegate(delPintarRearme, bit))
            {
                if (bit)
                {
                    luz_rearme.Fill = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    luz_rearme.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4F4F5"));
                }
            } 
        }

        private void muestraDatosRecibidos(byte[] datos, int dim)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(delegadoImprimeMensajes, new Object[2] {datos,dim });
            } else
            {
                imprimeMensaje(datos, dim);
            }
            return;
        }

        private void imprimeMensaje(byte[] datos, int dim)
        {
            var time24 = DateTime.Now.ToString("HH:mm:ss");
            txtB_Hex.AppendText(time24 + " ---- ");
            for (int i = 0; i < datos.Length; i++)
            {
                txtB_Hex.AppendText(Convert.ToString(datos[i],2) + " " );
            }
            txtB_Hex.AppendText("\n");

            txtB_Hex.ScrollToEnd();
        }

        private void btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            if (client != null) client.closeClient();
            client = null;
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
    }
}
