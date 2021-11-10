namespace Application.Data;

public static class MigrationsApplier
{
    public static void ApplyMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var guitarsContext = scope.ServiceProvider.GetService<GuitarsContext>();
        if (guitarsContext.Database.IsMySql())
        {
            guitarsContext.Database.Migrate();
        }
    }
}