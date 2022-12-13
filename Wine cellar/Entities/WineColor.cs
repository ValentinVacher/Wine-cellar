using System.Text.Json.Serialization;

namespace Wine_cellar.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WineColor
    {
        Rouge, Rosé, Blanc, Vert
    }
}
