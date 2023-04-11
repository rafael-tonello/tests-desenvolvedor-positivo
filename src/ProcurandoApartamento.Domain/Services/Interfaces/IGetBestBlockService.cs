using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ProcurandoApartamento.Domain.Services.Interfaces;
using ProcurandoApartamento.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ProcurandoApartamento.Domain.Services
{
    public interface IGetBestBlockService
    {

        Task<BlockInfo> getBestBlock(IList<BlockInfo> blocks, List<string> desiredFeatures);
        Task<BlockInfo> getBestBlock(IList<Apartamento> aps, List<string> desiredFeatures);

    }
}
