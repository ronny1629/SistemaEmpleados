using BackEndApi.DTOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Services.Implementacion
{
    public class DepartamentoService: IDepartamentoService
    {
        private DbempleadoContext _dbempleado;
        public DepartamentoService(DbempleadoContext dbempleado)
        {
            _dbempleado = dbempleado;
        }

        public async Task<List<Departamento>> GetList()
        {
            try
            {
                List<Departamento> lista = new List<Departamento>();
                lista = await _dbempleado.Departamentos.ToListAsync();

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
