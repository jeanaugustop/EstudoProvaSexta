using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Estudos.Model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Produto ----------

// Criação / Cadastro
app.MapPost("/produto", ([FromBody]Produto produto, [FromServices] AppDbContext context) =>
{
    //Validar se a Empresa Existe
    Empresa? empresa =
        context.Empresas.Find(produto.EmpresaId);

    if (empresa is null)
        return Results.NotFound("Empresa nao encontrada");

    produto.Empresa = empresa;

    context.Add(produto);
    context.SaveChanges();
    return Results.Created($"/produto/{produto.Id}", produto);
});

// Leitura // Lista
app.MapGet("/produtos", ([FromServices] AppDbContext context) =>
{
    var produtos = context.Produtos.ToList();
    return Results.Ok(produtos);
});

// Produtos por Nome // Listar por Nome
app.MapGet("/produtos/nome/{nome}", (string nome, AppDbContext context) =>
{
    var produtos = context.Produtos
                            .Where(p => p.Nome != null && p.Nome.Contains(nome))
                            .ToList();
    return produtos.Any() ? Results.Ok(produtos) : Results.NotFound("Nenhum produto encontrado.");
});

// Atualizar Produto
app.MapPut("/produtos/{id}", ([FromRoute]int id, [FromBody] Produto produtoAtualizado, [FromServices] AppDbContext context) =>
{
    var produto = context.Produtos.Find(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }

    produto.Nome = produtoAtualizado.Nome;
    produto.Preco = produtoAtualizado.Preco;
    context.SaveChanges();
    return Results.Ok("Produto atualizado com sucesso.");
});

// Deletar Produto
app.MapDelete("/produtos/{id}", (int id, AppDbContext context) =>
{
    var produto = context.Produtos.Find(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }

    context.Produtos.Remove(produto);
    context.SaveChanges();
    return Results.Ok("Produto removido com sucesso.");
});

//Empresa ----------------

app.MapPost("/empresa", ([FromBody]Empresa empresa, [FromServices] AppDbContext context) =>
{
    context.Add(empresa);
    context.SaveChanges();
    return Results.Created($"/empresa/{empresa.Id}", empresa);
});

// Leitura // Lista
app.MapGet("/empresas", ([FromServices] AppDbContext context) =>
{
    var produtos = context.Produtos.ToList();
    return Results.Ok(produtos);
});