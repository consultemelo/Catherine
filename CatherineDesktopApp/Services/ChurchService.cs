using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CatherineDesktopApp.Models;
using CatherineDesktopApp.Data;
using CatherineDesktopApp.Shared;

namespace LitheFrontDeskApp.Services
{
    public interface IChurchService
    {
        Task<IEnumerable<Church>> GetChurchesAsync(bool includeDeleted = false, bool defaultOnly = false);
        Task<Church> GetChurchByIdAsync(int id);
        Task<Church> AddChurchAsync(Church church);
        Task<Church> UpdateChurchAsync(Church church);
        Task<Church> DeleteChurchAsync(Church church);
        Task<Church> RestoreChurchAsync(Church church);
    }

    public class ChurchService : IChurchService
    {
        private readonly IRepository<Church> _churchRepository;

        public ChurchService(IRepository<Church> churchRepository)
        {
            _churchRepository = churchRepository;
        }

        public async Task<IEnumerable<Church>> GetChurchesAsync(bool includeDeleted = false, bool defaultOnly = false)
        {
            IQuerySpecification<Church> spec = new QuerySpecification<Church>();

            if (!includeDeleted)
                spec.SetFilter(p => p.IsDeleted == false);

            return await _churchRepository.SelectAsync(spec);
        }

        public async Task<Church> AddChurchAsync(Church church)
        {
            return await _churchRepository.CreateAsync(church);
        }

        public async Task<Church> UpdateChurchAsync(Church church)
        {
            return await _churchRepository.UpdateAsync(church);
        }

        public async Task<Church> GetChurchByIdAsync(int id)
        {
            return await _churchRepository.SelectByIdAsync(id);
        }

        public async Task<Church> DeleteChurchAsync(Church church)
        {
            church.IsDeleted = true;
            return await UpdateChurchAsync(church);
        }

        public async Task<Church> RestoreChurchAsync(Church church)
        {
            church.IsDeleted = false;
            return await UpdateChurchAsync(church);
        }
    }
}
