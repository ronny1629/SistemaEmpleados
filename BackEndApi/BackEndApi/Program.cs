using BackEndApi.Services.Implementacion;
using BackEndApi.Utilidades;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BackEndApi.DTOs;
using BackEndApi.DTOs.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbempleadoContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors(options => 

{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
  
    });
});



var app = builder.Build();


#region PETICIONES API REST
app.MapGet("/departamento/lista", async (
    
        IDepartamentoService _departamentoServicio,
        IMapper _mapper
    ) => {

        var listaDepartamento = await _departamentoServicio.GetList();
        var listaDepertamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

        if (listaDepertamentoDTO.Count > 0)
        {
            return Results.Ok(listaDepertamentoDTO);
        }
        else
        {
            return Results.NotFound();
        }
    });

app.MapGet("/empleado/lista", async (

        IEmpleadoService _empleadoServicio,
        IMapper _mapper
     ) => {

        var listaEmpleado = await _empleadoServicio.GetList();
        var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

        if (listaEmpleadoDTO.Count > 0)
        {
           return Results.Ok(listaEmpleadoDTO);
        }
        else
        {
           return Results.NotFound();
        }
});



app.MapPost("/empleado/guardar", async (
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoServicio,
     IMapper _mapper
     ) =>{
     
    var _empleado = _mapper.Map<Empleado>(modelo);
    var _empleadoCreado = await _empleadoServicio.AddEmpleado(_empleado);

         if (_empleadoCreado.IdEmpleado != 0)
         {
             return Results.Ok(_mapper.Map<EmpleadoDTO>(_empleadoCreado));
         }
         else
         {
             return Results.StatusCode(StatusCodes.Status500InternalServerError);
         }
});


app.MapPut("/empleado/actualizar/{idEmpleado}", async (
       int idEmpleado,
       EmpleadoDTO modelo,
       IEmpleadoService _empleadoServicio,
       IMapper _mapper
    ) => {

        var _encontrado = await _empleadoServicio.GetById(idEmpleado);

        if (_encontrado is null) return Results.NotFound();
      
        
        var _empleado = _mapper.Map<Empleado>(modelo);


        _encontrado.NombreCompleto = _empleado.NombreCompleto;
        _encontrado.IdDepartamento = _empleado.IdDepartamento;
        _encontrado.Sueldo = _empleado.Sueldo;
        _encontrado.FechaContrato = _empleado.FechaContrato;

        var respuesta = await _empleadoServicio.UpdateEmpleado(_encontrado);

        if (respuesta)
        {
            return Results.Ok(_mapper.Map<EmpleadoDTO>(_encontrado));
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

        }
    });


app.MapDelete("/empleado/eliminar/{idEmpleado}", async (
       int idEmpleado,
       IEmpleadoService _empleadoServicio

    ) => {

        var _encontrado = await _empleadoServicio.GetById(idEmpleado);

        if (_encontrado is null) return Results.NotFound();

        var respuesta = await _empleadoServicio.DeleteEmpleado(_encontrado);

        if (respuesta)
        {
            return Results.Ok();
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

        }
    });




#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.Run();