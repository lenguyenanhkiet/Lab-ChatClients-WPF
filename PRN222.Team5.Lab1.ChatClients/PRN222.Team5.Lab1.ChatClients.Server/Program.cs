using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Chat Server đang chạy trên port 5000 (Quyền Administrator)...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                Console.WriteLine("Có một Client mới kết nối!");
                _ = HandleClientAsync(client);
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Nhận được: {message}");

                    BroadcastMessage(message, client);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Client đã ngắt kết nối đột ngột.");
            }
            finally
            {
                clients.Remove(client);
                client.Close();
            }
        }

        static void BroadcastMessage(string message, TcpClient senderClient)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                if (client != senderClient)
                {
                    client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
    }
}