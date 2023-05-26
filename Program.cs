using FluentValidation;
using Sabadell_JV_C2C_VtosLej_Endpoint.DataAcces;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Validation;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Agregar el validador para Data
builder.Services.AddSingleton<IValidator<Data>, DataValidator>();
builder.Services.AddSingleton<ILeadDataAcces, LeadDataAcces>();
builder.Services.AddSingleton<ILeadLogic, LeadLogic>();
builder.Services.AddSingleton<IDataBaseConnectionFactory, DataBaseConnectionFactory>();



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
