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
        return value switch
        {
            "None"  => new ConditionallyRequired<DateOnly> { Required = true, HasValue = false },
            "N/A" => new ConditionallyRequired<DateOnly> { Required = false, HasValue = false },
            _ when DateOnly.TryParse(value!, out var result) => new ConditionallyRequired<DateOnly> { Required = true, HasValue = true, Value = result },
            _ => new ConditionallyRequired<DateOnly>()
        };
    }
}
