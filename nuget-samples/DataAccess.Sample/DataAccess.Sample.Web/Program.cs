using DataAccess.Repository;
using DataAccess.Sample.Data.DatabaseContexts;
using DataAccess.Sample.Data.Queries;
using DataAccess.Sample.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// set up db context
builder.Services.AddDbContext<MovieContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

// set up services
builder.Services.AddScoped<RepositoryFactory<MovieContext>>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// set up HotChocolate
builder.Services.AddGraphQLServer()
    .AddQueryType(q => q.Name("Query"))
    .AddTypeExtension<MovieQuery>()
    //.AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.UseCors(c =>
{
    c.WithOrigins("http://localhost:4200");
    c.AllowAnyHeader();
});

app.Run();