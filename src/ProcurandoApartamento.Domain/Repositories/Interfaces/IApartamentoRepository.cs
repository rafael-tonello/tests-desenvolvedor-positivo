using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcurandoApartamento.Domain.Repositories.Interfaces
{
    public interface IApartamentoRepository : IGenericRepository<Apartamento, long>
    {
        Task<IList<Apartamento>> getDisponibleApts();
    }
}
