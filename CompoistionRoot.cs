public static class CompositionRoot
{
    public static void AddCompositionRoot(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IGamesContainer, GamesContainer>();
    }
}