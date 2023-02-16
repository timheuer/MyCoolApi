using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace MyCoolApi.Tests;

class MyCoolApiApp : WebApplicationFactory<Program> {
    protected override IHost CreateHost(IHostBuilder builder) {
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}