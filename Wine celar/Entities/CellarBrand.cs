using System.Text.Json.Serialization;

namespace Wine_celar.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CellarBrand
    {
        Artevino, Liebherr, Avintage, Climadiff, LaSommelière, Haier, Klarstein, ContinentalEdison, Caviss, Vinosphere,Autre
    }
}
