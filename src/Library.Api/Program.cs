using Library.Api.Data;
using Library.Api.Features.Authors;
using Library.Api.Shared.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION")
	?? throw new ArgumentNullException(nameof(connectionString));

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>(
	_ => new SqlConnectionFactory(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(assembly);
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware();

app.UseHttpsRedirection();

app.MapCarter();

app.Run();
