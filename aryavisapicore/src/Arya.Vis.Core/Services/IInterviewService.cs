using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Entities;

namespace Arya.Vis.Core.Services
{
    public interface IInterviewService
    {
        Interview GetInterview(Guid interviewGuid);
        Task<Interview> GetInterviewAsync(Guid interviewGuid);
        Task<IEnumerable<Interview>>  GetInterviewAsync();
         Task<Interview> CreateAsync(Interview interview);
    }
}