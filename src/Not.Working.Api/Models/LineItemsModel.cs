using System.Collections.Generic;
using Not.Working.Common.Interfaces;
using Not.Working.Rendering.Models;
using AutoMapper;

namespace Not.Working.Api.Models
{
    public class LineItemsModel : IAutoMapperConfiguration
    {
        public List<LineItemModel> LineItems { get; set; }

        public MonetaryValueModel SubTotal { get; set; }

        public void ConfigureAutoMapper(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<LineItemsModel, LineItemsDto>();
        }
    }
}