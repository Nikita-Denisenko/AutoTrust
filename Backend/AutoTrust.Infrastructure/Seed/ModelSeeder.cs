using AutoTrust.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTrust.Infrastructure.Seed
{
    public static class ModelSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var modelService = scope.ServiceProvider.GetRequiredService<IModelService>();

            var json = await File.ReadAllTextAsync("Seed/SeedData/models.json");
            await modelService.LoadModelsAsync(json, CancellationToken.None);
        }
    }
}