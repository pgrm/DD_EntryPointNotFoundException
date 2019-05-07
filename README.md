# Reproduce EntryPointNotFoundException with DataDog and Automapper

## Remarks
Project to reproduce exception when Datadog trace is turned on `DD_TRACE_ENABLED=true` and using Automapper

## Discoveries

Mappings that include collections are causing the `EntryPointNotFoundException`

### Files causing the problem

1. `LineItemsModel.cs`
2. `LineItemsDto.cs`

### Steps to reproduce
1. Run `dotnet publish -c Release .` In `src/Not.Working.Api`
2. Copy over the `Dockerfile` into the generated publish folder under `src/Not.Working.Api/bin/Release/netcoreapp2.2/publish`
3. In the publish folder run `docker build -t notworkingapp .` to create a docker image
4. Run your docker image either from a docker compose file `docker-compose up -d` or `docker run -d -p 8080:80 --name myapp notworkingapp`
5. Check the logs of your container and it should show the `EntryPointNotFoundException`

### Worth to mention
1. By commenting out or ignoring the mapping, you get an `InvalidOperationException` which is expected as we trimmed down the whole project just to showcase the `EntryPointNotFoundException` and we do not expect this project to properly run/execute.
2. For files `LineItemsModel.cs` and `LineItemsDto.cs` changing the list to an array does not cause the exception. So `List<LineItemModel>` = broken, but `LineItemModel[]` works.
3. Ignoring the property causing the issue also works, for example changing the mapping in `LineItemsModel.cs` to ```configuration.CreateMap<LineItemsModel, LineItemsDto>()
                .ForMember(dest => dest.LineItems, opt => opt.Ignore());```
4. All the collections go through the same code in automapper here: [CollectionMapperExpressionFactory.cs](https://github.com/AutoMapper/AutoMapper/blob/4220230d60cecaa008ccf98d1612fbbb363d37c4/src/AutoMapper/Mappers/Internal/CollectionMapperExpressionFactory.cs#L65) where they subsequently break.

### Stacktrace
```Unhandled Exception: System.EntryPointNotFoundException: Entry point was not found.
   at AutoMapper.IMemberMap.get_UseDestinationValue()
   at AutoMapper.Mappers.Internal.CollectionMapperExpressionFactory.<MapCollectionExpression>g__UseDestinationValue|1_0(<>c__DisplayClass1_0& )
   at AutoMapper.Mappers.Internal.CollectionMapperExpressionFactory.MapCollectionExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap, IMemberMap memberMap, Expression sourceExpression, Expression destExpression, Expression contextExpression, Type ifInterfaceType, MapItem mapItem)
   at AutoMapper.Mappers.CollectionMapper.MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap, IMemberMap memberMap, Expression sourceExpression, Expression destExpression, Expression contextExpression)
   at AutoMapper.Execution.ExpressionBuilder.MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap, TypePair typePair, Expression sourceParameter, Expression contextParameter, IMemberMap propertyMap, Expression destinationParameter)
   at AutoMapper.Execution.TypeMapPlanBuilder.CreatePropertyMapFunc(IMemberMap memberMap, Expression destination, MemberInfo destinationMember)
   at AutoMapper.Execution.TypeMapPlanBuilder.TryPropertyMap(PropertyMap propertyMap)
   at AutoMapper.Execution.TypeMapPlanBuilder.CreateAssignmentFunc(Expression destinationFunc)
   at AutoMapper.Execution.TypeMapPlanBuilder.CreateMapperLambda(HashSet`1 typeMapsPath)
   at AutoMapper.TypeMap.CreateMapperLambda(IConfigurationProvider configurationProvider, HashSet`1 typeMapsPath)
   at AutoMapper.TypeMap.Seal(IConfigurationProvider configurationProvider)
   at AutoMapper.MapperConfiguration.Seal()
   at AutoMapper.MapperConfiguration..ctor(MapperConfigurationExpression configurationExpression)
   at AutoMapper.Mapper.Initialize(Action`1 config)
   at Not.Working.Api.Startup.ConfigureServices(IServiceCollection services) in /Users/randy/Projects/rendering/src/Not.Working.Api/Startup.cs:line 33
--- End of stack trace from previous location where exception was thrown ---
   at Microsoft.AspNetCore.Hosting.ConventionBasedStartup.ConfigureServices(IServiceCollection services)
   at Microsoft.AspNetCore.Hosting.Internal.WebHost.EnsureApplicationServices()
   at Microsoft.AspNetCore.Hosting.Internal.WebHost.Initialize()
   at Microsoft.AspNetCore.Hosting.WebHostBuilder.Build()
   at Not.Working.Api.Program.Main(String[] args) in /Users/randy/Projects/rendering/src/Not.Working.Api/Program.cs:line 11```
