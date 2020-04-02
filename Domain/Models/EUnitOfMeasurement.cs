using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EUnitOfMeasurement
    {
        [EnumMember(Value="UN")]
        Unity = 1,

        [EnumMember(Value = "MG")]
        Milligram = 2,

        [EnumMember(Value = "G")]
        Gram = 3,

        [EnumMember(Value = "KG")]
        Kilogram = 4,

        [EnumMember(Value = "L")]
        Liter = 5
    }
}