## Usage (`[FeatureGate]` attribute example)

1. In your ASP.NET Core project include `Bodatero.FeatureManagementMinimalAPI, Microsoft.FeatureManagement
and Microsoft.FeatureManagement.Mvc` namespaces
2. Add services to the `IServiceCollection`:
```cs
builder.Services.AddFeatureManagement();;
builder.Services.AddFeatureGateForMinimalAPI();
```
3. Add a middleware with the `UseMinimalApiFeatureGate()` method
4. Apply the `[FeatureGate]` attribute on an endpoint: 
```cs
app.MapGet("api/items/{take:int}", 
[FeatureGate("GetItemsTakeFeature")] async (IMediator mediator, int take) =>
{
    return Results.Ok(await mediator.Send(new TakeItemsQuery() { Take = take }));
});
```
5. Specify your features in the configuration file or in the environment variables for example

### Please support me with a donation on [Patreon](https://www.patreon.com/verbro/membership)
