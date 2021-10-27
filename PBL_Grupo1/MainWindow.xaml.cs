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
    public delegate void delegadoPintar(string tag);
    public delegate void delegadoDespintar(string tag);
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private delegadoMensajes delegadoImprimeMensajes;
        private delegadoProcesar delegadoProcesarMensaje;
        private delegadoPintar delegadoPintar;
        private delegadoDespintar delegadoDespintar;

        private GestionDatosEstacion1 gestorEstacion1;
        private Cliente client = null;
        private bool connected = false;
        public MainWindow()
        {
            InitializeComponent();
            delegadoImprimeMensajes = new delegadoMensajes(muestraDatosRecibidos);
            delegadoPintar = new delegadoPintar(pintaDatosRecibidos);
            delegadoDespintar = new delegadoDespintar(despintaDatosRecibidos);

            gestorEstacion1 = new GestionDatosEstacion1(delegadoImprimeMensajes, delegadoPintar, delegadoDespintar);

            delegadoProcesarMensaje = new delegadoProcesar(gestorEstacion1.procesar);

        }

        private void despintaDatosRecibidos(string tag)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(delegadoDespintar, new Object[1] { tag });
            }
            else
            {
                despinta(tag);
            }
            return;
        }

        private void despinta(string tag)
        {
            if (tag == "marcha")
            {
                luz_marcha.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4F4F5"));
            }
        }

        private void pintaDatosRecibidos(string tag)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(delegadoPintar, new Object[1] { tag });
            }
            else
            {
                pinta(tag);
            }
            return;
        }

        private void pinta(string tag)
        {
            if(tag == "marcha")
            {
                luz_marcha.Fill = new SolidColorBrush(Colors.Green);
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
                }
                else client = null;
            }
            else
            {
                if (client != null) client.closeClient();

                client = null;
                connected = false;

                btn_Conectar.Content = "Conectar";
            }
            
            return;
        }

        private void resetColores()
        {

        }

        /**
         * Metodo auxiliar para comprobar que el envío de datos se produce
         * 
        **/
        //private void hello()
        //{
        //    byte[] response = new byte[100];

        //    int res;

        //    txtB_Hex.Text = "";
        //    if (client != null && connected)
        //    {
        //        res = client.receiveData(response, response.Length);

        //        if (res > 0)
        //        {
        //            txtB_Hex.Clear();
        //            for (int i = 0; i < res; i++)
        //            {
        //                txtB_Hex.AppendText(response[i].ToString("X2") + "h ");
        //            }
        //            txtB_Hex.AppendText("\n");
        //        }
        //        else if (res == 0)
        //        {
        //            MessageBox.Show("No data received");
        //        }
        //        else if (res == -1)
        //        {
        //            MessageBox.Show("Server has closed connection", "TCP Client error");
        //            client.closeClient();
        //            client = null;
        //            btn_Conectar.Content = "Conectar";
        //        }
        //        else
        //        {
        //            MessageBox.Show("Unknown error while receiving data", "TCP Client error");
        //        }
        //    }
        //    else
        //        MessageBox.Show("No client connection", "TCP Client error");
        //}
    }
}
