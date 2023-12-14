using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tgs.VipLevelCommand.IntegrationTest;

internal class GameWagersClientServiceApplication(Action<IWebHostBuilder>? webHostConfigure = null) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => webHostConfigure?.Invoke(builder);
}
