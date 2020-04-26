using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Entities;

namespace Arya.Vis.Core.Repositories
{
    public interface IInterviewRepository
    {
        Task<IEnumerable<Interview>> GetInterviewsAsync();
        Task<Interview> GetInterviewAsync(Guid interviewGuid);
        Task<Interview> CreateAsync(Interview interview);
    }
}