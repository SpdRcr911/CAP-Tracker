using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CAP_Tracker.Library;
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		DateOnly.Parse(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, DateOnly dateTimeValue, JsonSerializerOptions options) =>
		writer.WriteStringValue(dateTimeValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
}