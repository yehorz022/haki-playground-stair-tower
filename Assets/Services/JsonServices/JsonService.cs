using Newtonsoft.Json;

namespace Assets.Services.Json
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