using Not.Working.Common.Interfaces;
using Not.Working.Common.Types;
using AutoMapper;

namespace Not.Working.Api.Models
{
    public class MonetaryValueModel : IAutoMapperConfiguration
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public void ConfigureAutoMapper(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<MonetaryValueModel, MonetaryValueDto>();
        }
    }
}