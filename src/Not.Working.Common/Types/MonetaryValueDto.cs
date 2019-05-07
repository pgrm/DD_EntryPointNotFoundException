using System.Diagnostics;

namespace Not.Working.Common.Types
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MonetaryValueDto
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        private string DebuggerDisplay
            => string.Format($"{Amount:N2} {Currency}");
    }
}