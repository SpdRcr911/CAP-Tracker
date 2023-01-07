namespace CAP_Tracker.Library
{
    public class ConditionallyRequired<T> where T : struct
    {
        public bool Required { get; set; }
        public bool HasValue { get; set; }
        public T? Value { get; set; } = default;
        public override string? ToString()
        {
            return this switch
            {
                { Required: true, HasValue: true } => Value!.ToString(),
                { Required: true, HasValue: false } => "None",
                _ => "N/A"
            };
        }
    }
}
