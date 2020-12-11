using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
namespace TcpGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 9999;
            TcpListener server = null;
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");

            try
            {
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                server.Start();
                Console.WriteLine("Ожидание подключений...");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                    Console.WriteLine("Подключен");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }
    }
}
