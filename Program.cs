using Microsoft.EntityFrameworkCore;
using WidmyIPS.Data;
using WidmyIPS.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<IPSDb>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/IPSs/", async (IPS i, IPSDb db) =>
{
    db.IPSs.Add(i);
    await db.SaveChangesAsync();
    return Results.Created($"/IPSs/{i.Id}", i);
});

app.MapGet("/IPSs/{id:int}", async (int id, IPSDb db) =>
{
    return await db.IPSs.FindAsync(id)
        is IPS i
        ? Results.Ok(i)
        : Results.NotFound();
});

app.MapGet("/IPSs/", async (IPSDb db) =>
{
    return await db.IPSs.ToListAsync();
});

app.MapPut("/IPSs/{id:int}", async (int id, IPS i, IPSDb db) =>
{
    if (id != i.Id)
    {
        return Results.BadRequest();
    }

    var ips = await db.IPSs.FindAsync(id);

    if (ips is null)
    {
        return Results.NotFound();
    }

    ips.Name = i.Name;
    ips.Address = i.Address;

    await db.SaveChangesAsync();

    return Results.Ok(ips);
});

app.MapDelete("/IPSs/{id:int}", async (int id, IPSDb db) =>
{
    var ips = await db.IPSs.FindAsync(id);

    if (ips is null)
    {
        return Results.NotFound();
    }

    db.IPSs.Remove(ips);
    await db.SaveChangesAsync();

    return Results.NoContent();
});	

app.Run();