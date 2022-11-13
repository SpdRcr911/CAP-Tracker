namespace CAP_Tracker.Library;
public static class StringHelper
{
	public static DateOnly? ToDateOnly(this string value)
	{
		if (DateOnly.TryParse(value!, out var result))
		{
			return new DateOnly?(result);
		};
		return new DateOnly?();
	}
}