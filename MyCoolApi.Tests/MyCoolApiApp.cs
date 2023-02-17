using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace MyCoolApi.Tests;

class MyCoolApiApp : WebApplicationFactory<Program> {
    protected override IHost CreateHost(IHostBuilder builder) {
        
        // without this line below, certain remote test scenarios will fail
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        return base.CreateHost(builder);
    }
}