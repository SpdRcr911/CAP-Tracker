namespace CAP_Tracker.Library;
public static class ExtensionMethods
{
	public static DateOnly? ToDateOnly(this string? value)
	{
		if (value != null && DateOnly.TryParse(value!, out var result))
		{
			return new DateOnly?(result);
		};
		return new DateOnly?();
	}
	public static int? ToInt32(this string? value)
	{
		if (value != null && Int32.TryParse(value!, out var result))
		{
			return new int?(result);
		}
		return new int?();
	}
	public static bool? ToBoolean(this string? value)
	{
		if (value != null && Boolean.TryParse(value!, out var result))
		{
			return new Boolean?(result);
		};
		return new Boolean?();
	}
    public static ConditionallyRequired<DateOnly> ToConditionalDateOnly(this string? value)
    {
        if (value != null)
        {
            if (value == "None")
            {
                return new ConditionallyRequired<DateOnly>
                {
                    Required = true,
                    HasValue = false
                };
            }
            else if (value == "N/A")
            {
                return new ConditionallyRequired<DateOnly>
                {
                    Required = false,
                    HasValue = false
                };
            }
            else if (DateOnly.TryParse(value!, out var result))
            {
                return new ConditionallyRequired<DateOnly>
                {
                    Required = true,
                    HasValue = true,
                    Value = result
                };
            }
        }
        return new ConditionallyRequired<DateOnly>();
    }
}