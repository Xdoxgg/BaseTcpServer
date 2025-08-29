using System.Configuration;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BaseServer;

public class Program
{
    private static int _port;

    private static void Setup()
    {
        var port = ConfigurationManager.AppSettings["port"];
        _port = port == null ? 5000 : Convert.ToInt32(port);
    }


    static async Task Main()
    {
        Setup();
        TcpListener listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();
        Console.WriteLine($"Сервер запущен. Ожидание подключений на порту {_port}...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Клиент подключился "+client.Available);

            _ = HandleClientAsync(client); // Обработка клиента асинхронно
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        {
            NetworkStream stream = client.GetStream();
            var buffer = new byte[1024];
            var receivedData = new StringBuilder();

            try
            {
                int bytesRead;
                // Читаем данные, пока клиент не закроет соединение или не пришлёт полный текст
                do
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        receivedData.Append(chunk);

                        // if (chunk.Contains("\n")) break;
                    }
                }
                while (bytesRead > 0 && stream.DataAvailable);

                string receivedText = receivedData.ToString();
                Console.WriteLine($"Получено от клиента: {receivedText}");

                string responseText = $"fuck you, with: {receivedText}";
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);

                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("Ответ отправлен клиенту.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с клиентом: {ex.Message}");
            }
        }
    }
}