using BackEndApi.DTOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Services.Implementacion
{
    public class EmpleadoService: IEmpleadoService
    {
        private readonly DbempleadoContext _dbempleado;
        public EmpleadoService(DbempleadoContext dbempleado)
        {
                _dbempleado = dbempleado;
        }

        public async Task<List<Empleado>> GetList()
        {
            try
            {
                List<Empleado> list = new List<Empleado>();
                list = await _dbempleado.Empleados.Include(dpt => dpt.IdDepartamentoNavigation).ToListAsync();
                return list;

            }
            catch ( Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Empleado> GetById(int idEmpleado)
        {
            try
            {
                Empleado? encontrado = new Empleado();

                encontrado = await _dbempleado.Empleados.Include(dpt => dpt.IdDepartamentoNavigation)
                             .Where(e => e.IdEmpleado == idEmpleado).FirstOrDefaultAsync();

                return encontrado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Empleado> AddEmpleado(Empleado modelo)
        {
            try
            {
                _dbempleado.Empleados.Add(modelo);
                await _dbempleado.SaveChangesAsync();
                return modelo;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdateEmpleado(Empleado modelo)
        {
            try
            {
                _dbempleado.Empleados.Update(modelo);
                await _dbempleado.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteEmpleado(Empleado modelo)
        {
            try
            {
                _dbempleado.Remove(modelo);
                await _dbempleado.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }
}
