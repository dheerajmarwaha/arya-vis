using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Repositories;
using System.Linq;

namespace Arya.Vis.Repository
{
    public class InterviewRepository : IInterviewRepository
    {
         private IList<Interview> _interviews;
        public InterviewRepository()
        {
           _interviews = new List<Interview>();
        }
        public Task<Interview> GetInterviewAsync(Guid interviewGuid)
        {
             return Task.FromResult(_interviews.Single(i => Equals(i.InterviewGuid, interviewGuid)));
        }
        public async Task<Interview> CreateAsync(Interview interview) {
            interview.InterviewGuid = Guid.NewGuid();
            _interviews.Add(interview);
            return interview;
            // var command = SqlProvider.CreateCommand(Routines.CreateJob);
            // using(command) {
            //     AddJobQueryParameters(command, job, userId);
            //     return Convert.ToInt32(await SqlProvider.ExecuteScalarAsync(command));
            // }
        }

        public Task<IEnumerable<Interview>> GetInterviewsAsync()
        {
            return Task.FromResult(_interviews.AsEnumerable());
        }
    }
}
