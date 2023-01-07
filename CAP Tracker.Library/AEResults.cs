namespace CAP_Tracker.Library;

public class AEResults : ConditionallyRequired<DateOnly>
{
    public AEResults(string? date, string? score, string? module)
    {
        var conditional = date.ToConditionalDateOnly();
        Required = conditional.Required;
        HasValue = conditional.HasValue;
        Value = conditional.Value;
        Score = Required ? score.ToInt32() : null;
        Module = Required ? module : null;
    }
    public int? Score { get; set; }
    public string? Module { get; set; }

    public override string? ToString()
    {
        return this switch
        {
            { Required: true, HasValue: true } => $"{Value} - {Score} ({Module})",
            { Required: true, HasValue: false } => "None",
            _ => "N/A"
        };
    }
}
