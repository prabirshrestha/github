using System;

namespace Github
{
    public class JsonSerializer
    {
        private static readonly JsonSerializer _instance = new JsonSerializer();

        public static IJsonSerializer Current
        {
            get
            {
                return _instance.InnerCurrent;
            }
        }

        public static void SetJsonSerializer(IJsonSerializer jsonSerializer)
        {
            _instance.InnerSetApplication(jsonSerializer);
        }

        public static void SetJsonSerializer(Func<IJsonSerializer> getJsonSerializer)
        {
            _instance.InnerSetApplication(getJsonSerializer);
        }

        private IJsonSerializer _current = new SimpleJsonSerializer();

        public IJsonSerializer InnerCurrent
        {
            get
            {
                return _current;
            }
        }

        public void InnerSetApplication(IJsonSerializer jsonSerializer)
        {
            _current = jsonSerializer ?? new SimpleJsonSerializer();
        }

        public void InnerSetApplication(Func<IJsonSerializer> getJsonSerializer)
        {
            InnerSetApplication(getJsonSerializer());
        }

        private class SimpleJsonSerializer : IJsonSerializer
        {
            public string SerializeObject(object obj)
            {
                return SimpleJson.SimpleJson.SerializeObject(obj);
            }

            public object DeserializeObject(string json, Type type)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json, type);
            }

            public T DeserializeObject<T>(string json)
            {
                return SimpleJson.SimpleJson.DeserializeObject<T>(json);
            }

            public object DeserializeObject(string json)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json);
            }
        }
    }
}