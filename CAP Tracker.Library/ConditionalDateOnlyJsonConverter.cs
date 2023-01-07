using System.Text.Json;
using System.Text.Json.Serialization;

namespace CAP_Tracker.Library;

public class ConditionalDateOnlyJsonConverter : JsonConverter<ConditionallyRequired<DateOnly>>
{
    public override ConditionallyRequired<DateOnly> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.ToConditionalDateOnly();
    }

    public override void Write(Utf8JsonWriter writer, ConditionallyRequired<DateOnly> dateTimeValue, JsonSerializerOptions options)
    {
		writer.WriteStringValue(dateTimeValue.ToString());
    }
}
