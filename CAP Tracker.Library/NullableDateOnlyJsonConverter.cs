using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CAP_Tracker.Library;
public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
	public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.GetString()!.ToDateOnly();
	}

	public override void Write(Utf8JsonWriter writer, DateOnly? dateTimeValue, JsonSerializerOptions options)
	{
		writer.WriteStringValue(dateTimeValue!.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
	}
}