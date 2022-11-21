namespace CAP_Tracker.Library;
public static class ExtensionMethods
{
	public static DateOnly? ToDateOnly(this string value)
	{
		if (DateOnly.TryParse(value!, out var result))
		{
			return new DateOnly?(result);
		};
		return new DateOnly?();
	}
	public static int? ToInt32(this string value)
	{
		if (Int32.TryParse(value, out var result))
		{
			return new int?(result);
		}
		return new int?();
	}
	public static bool? ToBoolean(this string value)
	{
		if (Boolean.TryParse(value, out var result))
		{
			return new Boolean?(result);
		};
		return new Boolean?();
	}
}