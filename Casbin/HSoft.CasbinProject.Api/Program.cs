using Casbin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Enforcer>(_ =>
{
    var enforcer = new Enforcer("model.conf", "policy.csv");

    // Opcional: cargar políticas desde base de datos
    // var adapter = new EFAdapter.DbContextAdapter(dbContext);
    // var enforcer = new Enforcer("model.conf", adapter);

    return enforcer;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
