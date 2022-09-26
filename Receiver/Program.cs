using Microsoft.Extensions.DependencyInjection;
using Receiver.Services;
using Receiver.Services.Connection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IReceiveMsgService, MsgReceiverService>();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ReceiveMessages}/{action=MessageReceiver}/{id?}");
app.Run();
