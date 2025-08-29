using System.Text.Json;

namespace BaseServer;

public class RequestParser
{
    public static T? PrintType<T>(string message)
    {
        return JsonSerializer.Deserialize<T>(message);
    }
    
    public static string? CreatJson<T>(T obj)
    {
        return JsonSerializer.Serialize(obj);
    }
    
    
}