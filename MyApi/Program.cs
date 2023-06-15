using Microsoft.AspNetCore.Mvc;
using MyApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:5137") // replace with your Blazor app's address
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.AddSingleton<BashService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.MapPost("/api/terminal", async ([FromServices]BashService bash,TerminalCommand command) =>
{
    if (command?.Cmd == null) return Results.BadRequest("No command provided");
    var escapedArgs = command.Cmd.Replace("\"", "\\\"");
    var result = await bash.RunCommand(escapedArgs);
    Console.WriteLine(result);
    return Results.Ok(result);
});

app.MapGet("/api/files", ([FromQuery]string directoryPath) =>
{
    if(Directory.Exists(directoryPath))
    {
        try
        {
            var files = Directory.GetFiles(directoryPath, "*.bin");
            return Results.Ok(files);
        }
        catch(Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    else
    {
        return Results.NotFound($"Directory {directoryPath} not found.");
    }
});

app.Run();
