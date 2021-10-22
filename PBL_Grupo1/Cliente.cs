using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PBL_Grupo1
{
    public class Cliente
    {
        private Socket clientTCP = null;
        private IPEndPoint ipep = null;
        private bool connected;
        private delegadoProcesar procesar;

        private Thread hiloCliente;

        public Cliente(String ip, Int32 port, delegadoProcesar delegadoProcesarMensaje)
        {
            try
            {
                procesar = delegadoProcesarMensaje;
                IPAddress direction = IPAddress.Parse(ip);
                ipep = new IPEndPoint(direction, port);

                clientTCP = new Socket(direction.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                clientTCP.SendTimeout = 500;
                clientTCP.ReceiveTimeout = 1000;

                hiloCliente = new Thread(new ThreadStart(this.rutina));
                hiloCliente.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.ToString(), "TCP client ERROR");
                throw;
            }
        }

        public bool conectToServer()
        {
            try
            {
                clientTCP.Connect(ipep);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    MessageBox.Show("Error while connecting to server: please turn on server before connecting client. IP= " + ipep.Address.ToString() + "Port= " + ipep.Port.ToString());
                }
                else
                {
                    MessageBox.Show("Error while connecting to server: " + ex.ToString() + "Client error.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR conneting so server: " + ex.ToString() + "Client error.");
                throw;
            }
            finally
            {
                connected = (clientTCP != null && clientTCP.Connected) ? true : false;

            }
            return connected;
        }

        public void closeClient()
        {
            if (clientTCP != null && connected)
            {
                clientTCP.Close();
            }
            clientTCP = null;
            connected = false;
            return;
        }

        public int sendData(byte[] data, int dim)
        {
            try
            {
                if (connected)
                {
                    int res = clientTCP.Send(data, dim, SocketFlags.None);
                    if (res == dim) return res;
                    else if (res == 0)
                    {
                        clientTCP = null;
                        connected = false;
                        return -1;
                    }
                    else return -2;
                }
                else return -1;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut) return 0;
                else if (ex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    clientTCP = null;
                    connected = false;
                    return -1;
                }
                else return -2;
            }
            catch (Exception)
            {
                return -2;
            }
        }

        public int receiveData(byte[] data, int dimMax)
        {
            try
            {
                if (connected)
                {
                    int res = clientTCP.Receive(data, dimMax, SocketFlags.None);
                    if (res > 0) return res;
                    else if (res == 0)
                    {
                        clientTCP = null;
                        connected = false;
                        return -1;
                    }
                    else return -2;
                }
                else return -1;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut) return 0;
                else if (ex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    clientTCP = null;
                    connected = false;
                    return -1;
                }
                else return -2;
            }
            catch (Exception)
            {
                return -2;
            }
        }

        private void rutina()
        {
            try
            {
                byte[] data = new byte[100];
                byte[] res;
                int dimRes;

                while (connected)
                {
                    int aux = this.receiveData(data, data.Length);

                    if (aux > 0)
                    {
                        procesar(data, aux);
                    }
                    else
                    {
                        //TODO: qué hacer aquí
                    }
                    
                }
            }
            catch (SocketException ex)
            {


            }


        }
    }
}