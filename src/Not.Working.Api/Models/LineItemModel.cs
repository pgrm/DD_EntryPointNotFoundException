using System;
using Not.Working.Common.Extensions;
using Not.Working.Common.Interfaces;
using Not.Working.Rendering.Models;
using AutoMapper;

namespace Not.Working.Api.Models
{
    public class LineItemModel : IAutoMapperConfiguration
    {
        /// <summary>
        /// The date on which this item is delivered
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The description of the item to be delivered
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price for this item
        /// </summary>
        public MonetaryValueModel Price { get; set; }

        public bool IsNoShowFee { get; set; }

        public void ConfigureAutoMapper(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<LineItemModel, LineItemDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFromSafely(src => src.Date));
        }
    }
}