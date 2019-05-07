using System;
using Not.Working.Common.Types;

namespace Not.Working.Rendering.Models
{
    public class LineItemDto
    {
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public MonetaryValueDto Price { get; set; }

        public bool IsNoShowFee { get; set; }
    }
}