using Newtonsoft.Json;

public sealed class JsonSerializerService
{
    private readonly JsonSerializerSettings settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };

    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, settings);
    }

    public T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}