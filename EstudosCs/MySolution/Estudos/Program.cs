using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Estudos.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Produto ----------

// Criação / Cadastro
app.MapPost("/produto", async (Produto produto, AppDbContext context) =>
{
    context.Add(produto);
    await context.SaveChangesAsync();
    return Results.Created($"/produto/{produto.Id}", produto);
});

// Leitura // Lista
app.MapGet("/produtos", async (AppDbContext context) =>
{
    var produtos = await context.Produtos.ToListAsync();
    return Results.Ok(produtos);
});

// Produtos por Nome // Listar por Nome
app.MapGet("/produtos/nome/{nome}", async (string nome, AppDbContext context) =>
{
    var produtos = await context.Produtos
                                .Where(p => p.Nome.Contains(nome))
                                .ToListAsync();
    return produtos.Any() ? Results.Ok(produtos) : Results.NotFound("Nenhum produto encontrado.");
});

// Atualizar Produto
app.MapPut("/produtos/{id}", async (int id, Produto produtoAtualizado, AppDbContext context) =>
{
    var produto = await context.Produtos.FindAsync(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }

    produto.Nome = produtoAtualizado.Nome;
    produto.Preco = produtoAtualizado.Preco;
    await context.SaveChangesAsync();
    return Results.Ok("Produto atualizado com sucesso.");
});

// Deletar Produto
app.MapDelete("/produtos/{id}", async (int id, AppDbContext context) =>
{
    var produto = await context.Produtos.FindAsync(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }

    context.Produtos.Remove(produto);
    await context.SaveChangesAsync();
    return Results.Ok("Produto removido com sucesso.");
});