using Unity.Plastic.Newtonsoft.Json;

namespace Assets.Scripts.Services.JsonServices
{

    [Service(typeof(IJsonService))]
    public class JsonService : IJsonService
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}