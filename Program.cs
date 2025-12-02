using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Application.Services;
using APIUsuarios.Infrastructure.Persistence;
using APIUsuarios.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioCreateDtoValidator>();

var app = builder.Build();

app.MapGet("/usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    return Results.Ok(await service.ListarAsync(ct));
});

app.MapGet("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    return usuario != null ? Results.Ok(usuario) : Results.NotFound();
});

app.MapPost("/usuarios", async (
    UsuarioCreateDto dto,
    IUsuarioService service,
    CancellationToken ct,
    IValidator<UsuarioCreateDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    if (await service.EmailJaCadastradoAsync(dto.Email, ct))
    {
        return Results.Conflict(new { mensagem = "Email já cadastrado" });
    }

    var usuario = await service.CriarAsync(dto, ct);
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
});

app.MapPut("/usuarios/{id}", async (
    int id,
    UsuarioUpdateDto dto,
    IUsuarioService service,
    CancellationToken ct,
    IValidator<UsuarioUpdateDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var usuarioExistente = await service.ObterAsync(id, ct);
    if (usuarioExistente == null)
    {
        return Results.NotFound();
    }

    var serviceCast = (UsuarioService)service;
    if (await serviceCast.EmailJaCadastradoParaOutroAsync(dto.Email, id, ct))
    {
        return Results.Conflict(new { mensagem = "Email já cadastrado para outro usuário" });
    }

    var usuario = await service.AtualizarAsync(id, dto, ct);
    return Results.Ok(usuario);
});

app.MapDelete("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    if (usuario == null) return Results.NotFound();
    await service.RemoverAsync(id, ct);
    return Results.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
