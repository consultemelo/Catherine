using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using CatherineDesktopApp.Models;
using CatherineDesktopApp.Data;
using CatherineDesktopApp.Shared;

namespace LitheFrontDeskApp.Services
{
    public interface IPriestService
    {
        Task<IEnumerable<Priest>> GetPriestsAsync(bool includeDeleted = false);
        Task<Priest> GetPriestByIdAsync(int id);
        Task<Priest> AddPriestAsync(Priest priest);
        Task<Priest> UpdatePriestAsync(Priest priest);
        Task<Priest> DeletePriestAsync(Priest priest);
        Task<Priest> RestorePriestAsync(Priest priest);

        ReadOnlyCollection<EnumDisplay<PriestTitle>> PriestTitles { get; }
        ReadOnlyCollection<EnumDisplay<PriestPosition>> PriestPositions { get; }
    }

    public class PriestService : IPriestService
    {
        private readonly IRepository<Priest> _priestRepository;

        public PriestService(IRepository<Priest> priestRepository)
        {
            _priestRepository = priestRepository;
        }

        public async Task<IEnumerable<Priest>> GetPriestsAsync(bool includeDeleted = false)
        {
            IQuerySpecification<Priest> spec = new QuerySpecification<Priest>();

            if (!includeDeleted)
                spec.SetFilter(p => p.IsDeleted == includeDeleted);

            return await _priestRepository.SelectAsync(spec);
        }

        public async Task<Priest> GetPriestByIdAsync(int id)
        {
            return await _priestRepository.SelectByIdAsync(id);
        }

        public async Task<Priest> AddPriestAsync(Priest priest)
        {
            return await _priestRepository.CreateAsync(priest);
        }

        public async Task<Priest> UpdatePriestAsync(Priest priest)
        {
            return await _priestRepository.UpdateAsync(priest);
        }

        public async Task<Priest> DeletePriestAsync(Priest priest)
        {
            priest.IsDeleted = true;
            return await _priestRepository.UpdateAsync(priest);
        }

        public async Task<Priest> RestorePriestAsync(Priest priest)
        {
            priest.IsDeleted = false;
            return await _priestRepository.UpdateAsync(priest);
        }


        #region Enum Displays       
        private readonly ReadOnlyCollection<EnumDisplay<PriestTitle>> _priestTitles = EnumExtensions.GenerateEnumDisplays<PriestTitle>();
        public ReadOnlyCollection<EnumDisplay<PriestTitle>> PriestTitles => _priestTitles;

        private readonly ReadOnlyCollection<EnumDisplay<PriestPosition>> _priestPositions = EnumExtensions.GenerateEnumDisplays<PriestPosition>();
        public ReadOnlyCollection<EnumDisplay<PriestPosition>> PriestPositions => _priestPositions;
        #endregion
    }
}
