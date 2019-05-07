using AutoMapper;

namespace Not.Working.Common.Interfaces
{
    public interface IAutoMapperConfiguration
    {
        void ConfigureAutoMapper(IMapperConfigurationExpression configuration);
    }
}