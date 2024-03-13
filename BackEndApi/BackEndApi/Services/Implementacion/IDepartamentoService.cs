using BackEndApi.DTOs.Models;

namespace BackEndApi.Services.Implementacion
{
    public interface IDepartamentoService
    {
        Task<List<Departamento>> GetList();
    }
}
