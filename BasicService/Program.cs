using BasicService.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System;
using BasicService.Validation;
using BasicService.Models;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();


    /* Adding all the validations here */
{
    builder.Services.AddTransient<IValidator<Contact>, ContactValidator>();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



/*      Implemented In memory Database
builder.Services.AddDbContext<ContactsAPIDbContext>(options => options.UseInMemoryDatabase("ContactsDb"));
*/


// Add-Migration "Initial Migration"   :-  Creating all the necessary Class for creating table on database
// Update-Database   :- To create Database on the given Connection String
builder.Services.AddDbContext<ContactsAPIDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsAPIConnectionString")));




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
