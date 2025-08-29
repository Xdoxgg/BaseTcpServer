using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BaseClient
{
    class TcpClientExample
    {
        static async Task Main()
        {
            string server = "26.105.234.194"; // адрес сервера
            int port = 5000;             // порт (тот же, что у сервера)

            try
            {
                using TcpClient client = new TcpClient();
                Console.WriteLine("Подключение к серверу...");
                await client.ConnectAsync(server, port);
                Console.WriteLine("Подключено!");

                using NetworkStream stream = client.GetStream();

                // Отправка сообщения
                string? message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Отправлено: {message}");

                // Чтение ответа
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Ответ от сервера: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}