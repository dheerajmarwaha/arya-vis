using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Entities;

namespace Arya.Vis.Core.Services
{
    public interface IInterviewService
    {
        Interview GetJob(Guid interviewGuid);
        Task<Interview> GetJobAsync(Guid interviewGuid);
        Task<IEnumerable<Interview>>  GetJobsAsync();
         Task<Interview> CreateAsync(Interview interview);
    }
}