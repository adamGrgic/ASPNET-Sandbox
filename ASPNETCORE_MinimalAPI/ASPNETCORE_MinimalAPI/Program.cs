using ASPNETCORE_MinimalAPI;
using Microsoft.EntityFrameworkCore;

// ASP.NET CORE 7 Demo API Project
// 



// Build application by creating a builder object 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


// TODO: add these routes to modules (??)
// use attribute routing with controllers instead:
// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-7.0


// create route group 
var todoItems = app.MapGroup("/todoitems");

// TODO: how is the database inserted into the params? 
// ChatGPT: The TodoDb is registered with the DI container in the startup
// configuration with AddDbContext<TodoDb>. When ASP.NET Core processes a
// request that hits an endpoint needing TodoDb, it automatically injects
// an instance of TodoDb into the method.

// same get route
todoItems.MapGet("/", async (TodoDb db) =>
    await db.Todos.ToListAsync());


// another sample get route with a child route
todoItems.MapGet("/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());


// get route with param
todoItems.MapGet("/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

// sample post route 
todoItems.MapPost("/", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

todoItems.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

todoItems.MapDelete("/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();