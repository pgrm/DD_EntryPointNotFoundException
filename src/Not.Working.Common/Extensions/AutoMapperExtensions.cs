using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Not.Working.Common.Interfaces;
using AutoMapper;

namespace Not.Working.Common.Extensions
{
    public static class AutomapperExtensions
    {
        public static void ConfigureAutoMapperForApplication(this IMapperConfigurationExpression configuration) =>
           configuration.ConfigureAutoMapperForApplication(Assembly.GetEntryAssembly());

        public static void ConfigureAutoMapperForApplication(this IMapperConfigurationExpression configuration, Assembly entryAssembly)
        {
            IEnumerable<Type> allTypes = entryAssembly
                .GetReferencedAssembliesRecursively()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.GetTypeInfo().IsAbstract);

            foreach (Type type in allTypes)
            {
                TryConfigureAutoMapperForType(configuration, type);
            }
        }

        /// <summary>
        /// The same as <see cref="IMemberConfigurationExpression.MapFrom(Type)"/> but also checks that source and destination is of the same type.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="mappingOption"></param>
        /// <param name="sourceMember"></param>
        public static void MapFromSafely<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> mappingOption,
            Expression<Func<TSource, TMember>> sourceMember) => mappingOption.MapFrom(sourceMember);

        private static void TryConfigureAutoMapperForType(IMapperConfigurationExpression configuration, Type type)
        {
            if (typeof(IAutoMapperConfiguration).GetTypeInfo().IsAssignableFrom(type))
            {
                ((IAutoMapperConfiguration)Activator.CreateInstance(type, true)).ConfigureAutoMapper(configuration);
            }
        }
    }
}