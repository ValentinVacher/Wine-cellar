using System.Text.Json.Serialization;

namespace Wine_celar.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CellarType
    {
        Vieillissement, Conservation, Service, MultiTempérature, Professionnelle
    }
}
