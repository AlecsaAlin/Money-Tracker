var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

<<<<<<< HEAD
var movements = new List<MoneyMovement>();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
=======
var sync = new object();
var transactions = new List<Transaction>();
var nextId = 0;
>>>>>>> e89401b3225baeabd9b6a03d39d5b20343a639a2

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

app.MapGet("/summary", () =>
{
    var income = movements.Where(m => m.Type == MoneyMovementType.Income).Sum(m => m.Amount);
    var expenses = movements.Where(m => m.Type == MoneyMovementType.Expense).Sum(m => m.Amount);
    return new Summary(income, expenses, income - expenses);
})
.WithName("GetSummary");

app.Run();

<<<<<<< HEAD
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

enum MoneyMovementType
{
    Income,
    Expense
}

record MoneyMovement(decimal Amount, MoneyMovementType Type);

record Summary(decimal TotalIncome, decimal TotalExpenses, decimal Balance);
=======
record Transaction(int Id, decimal Amount, string Type, string Category);

record CreateTransactionRequest(decimal Amount, string? Type, string? Category);
>>>>>>> e89401b3225baeabd9b6a03d39d5b20343a639a2
