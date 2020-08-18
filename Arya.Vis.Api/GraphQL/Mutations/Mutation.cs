using System.Threading.Tasks;
using Arya.Vis.Api.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Services;
using Microsoft.Extensions.Logging;

namespace Arya.Vis.Api.GraphQL.Mutations {
    public class Mutation {
        private readonly IInterviewService _interviewService;
        private readonly ILogger<Mutation> _logger;

        public Mutation(IInterviewService interviewService, ILogger<Mutation> logger) {
            _interviewService = interviewService;
            _logger = logger;
        }

        public async Task<Interview> CreateInterview(InterviewCreateCommand interviewCreateCommand) {
            _logger.LogInformation("Creating interview with command {@InterviewCreateCommand}", interviewCreateCommand);
            var interview = InterviewCreateCommand.MapToInterview(interviewCreateCommand);
            return await _interviewService.CreateAsync(interview);
        }
    }
}
