using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arya.Utils;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.Services;

namespace Arya.Vis.Api.DataLoaders {
    public class InterviewDataLoader {
        private readonly IInterviewService _interviewService;
        public InterviewDataLoader(IInterviewService interviewService) {
            _interviewService = interviewService;
        }

        public async Task<IReadOnlyDictionary<Guid, Interview>> GetBatchInterviews(IReadOnlyList<Guid> ids) {
            ValidationUtil.NotEmptyCollection<Guid>(ids?.ToList());
            var query = new InterviewSearchQuery {
                Ids = ids
            };
            var interviewSearchResult = await _interviewService.SearchAsync(query);
            return interviewSearchResult?.Interviews?.ToDictionary(x => x.Id) ?? new Dictionary<Guid, Interview>(0);
        }
    }
}
