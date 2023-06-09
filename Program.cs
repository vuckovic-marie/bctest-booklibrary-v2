using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddTransient<IBookService, BookService>();
    services.AddSingleton<IAppStart, AppStart>();

}).Build();

var app = _host.Services.GetRequiredService<IAppStart>();
app.Run();




