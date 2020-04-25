using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;

namespace Arya.Vis.Core.ServicesImpl
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepositry;

        public InterviewService(IInterviewRepository interviewRepositry)
        {
            _interviewRepositry = interviewRepositry;
        }

         public Interview GetJob(Guid interviewGuid)
        {
            return GetJobAsync(interviewGuid).Result;
        }

        public Task<Interview> GetJobAsync(Guid interviewGuid)
        {
            return _interviewRepositry.GetInterviewAsync(interviewGuid);
        }
        public Task<IEnumerable<Interview>> GetJobsAsync()
        {
            return _interviewRepositry.GetInterviewsAsync();
        }
        public Task<Interview> CreateAsync(Interview interview)
        {
            return _interviewRepositry.CreateAsync(interview);
        }

      

        

        
    }
}