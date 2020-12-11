using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
namespace TcpGame
{
   public class ClientObject
    {
        public TcpClient client;
        int server_number;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                //Генерация числа
                Random r = new Random();
                server_number = r.Next(1, 10);
                Debug.WriteLine(server_number);

                //Получение клиента
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);

                    //Обработка сообщения
                    string[] client_message = message.Split(':');
                    int my_num; //число клиента

                    try
                    {
                        my_num = int.Parse(client_message[1]); //корректность строки
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        message = message.Substring(message.IndexOf(':') + 1).Trim();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                        continue; 
                    }

                    if (my_num == server_number)
                    {
                        message = "correct";
                        message = message.Substring(message.IndexOf(':') + 1).Trim();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                        break;
                    }
                    else if (my_num < server_number)
                    {
                        message = "more";
                        message = message.Substring(message.IndexOf(':') + 1).Trim();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                    else if (my_num > server_number)
                    {
                        message = "less";
                        message = message.Substring(message.IndexOf(':') + 1).Trim();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        message = "incorrect input";
                        message = message.Substring(message.IndexOf(':') + 1).Trim();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string message = ex.Message;
                message = message.Substring(message.IndexOf(':') + 1).Trim();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
