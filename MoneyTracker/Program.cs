var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var sync = new object();
var transactions = new List<Transaction>();
var nextId = 0;

app.MapGet("/transactions", () =>
{
    lock (sync)
        return Results.Ok(transactions.ToList());
});

app.MapPost("/transactions", (CreateTransactionRequest body) =>
{
    if (string.IsNullOrWhiteSpace(body.Type))
        return Results.BadRequest("Type is required.");

    var type = body.Type.Trim().ToLowerInvariant();
    if (type is not ("income" or "expense"))
        return Results.BadRequest("Type must be 'income' or 'expense'.");

    if (body.Amount <= 0)
        return Results.BadRequest("Amount must be greater than zero.");

    var category = string.IsNullOrWhiteSpace(body.Category) ? "" : body.Category.Trim();

    var id = Interlocked.Increment(ref nextId);
    var tx = new Transaction(id, body.Amount, type, category);

    lock (sync)
        transactions.Add(tx);

    return Results.Created($"/transactions/{id}", tx);
});

app.MapDelete("/transactions/{id:int}", (int id) =>
{
    lock (sync)
    {
        var removed = transactions.RemoveAll(t => t.Id == id);
        return removed > 0 ? Results.NoContent() : Results.NotFound();
    }
});

app.Run();

record Transaction(int Id, decimal Amount, string Type, string Category);

record CreateTransactionRequest(decimal Amount, string? Type, string? Category);
