using System.Collections.Generic;
using Not.Working.Common.Types;

namespace Not.Working.Rendering.Models
{
    public class LineItemsDto
    {
        public List<LineItemDto> LineItems { get; set; }

        public MonetaryValueDto SubTotal { get; set; }
    }
}
