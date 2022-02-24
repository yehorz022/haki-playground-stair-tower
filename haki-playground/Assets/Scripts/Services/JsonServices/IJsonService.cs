namespace Assets.Scripts.Services.JsonServices
{
    public interface IJsonService
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
    }
}