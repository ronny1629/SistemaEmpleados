using BackEndApi.DTOs.Models;

namespace BackEndApi.Services.Implementacion
{
    public interface IEmpleadoService
    {
        Task<List<Empleado>> GetList();
        Task<Empleado> GetById(int idEmpleado);
        Task<Empleado> AddEmpleado(Empleado modelo);
        Task<bool> UpdateEmpleado(Empleado modelo);
        Task<bool> DeleteEmpleado(Empleado modelo);
    }
}
