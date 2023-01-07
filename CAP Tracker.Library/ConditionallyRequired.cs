using System.Globalization;

namespace CAP_Tracker.Library
{
    public class ConditionallyRequired<T> where T : struct
    {
        public bool Required { get; set; }
        public bool HasValue { get; set; }
        public T? Value { get; set; } = default;
        public override string? ToString()
        {
            if (Required)
            {
                if (HasValue)
                {
                    return Value!.ToString();
                }
                else
                {
                    return "None";
                }
            }
            else
            {
                return "N/A";
            }

            //return Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
