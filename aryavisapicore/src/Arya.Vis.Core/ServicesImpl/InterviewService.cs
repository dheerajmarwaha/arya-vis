using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arya.Exceptions;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Microsoft.Extensions.Logging;

namespace Arya.Vis.Core.ServicesImpl
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepositry;
        private readonly ILogger<InterviewService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public InterviewService(ILogger<InterviewService> logger,
                                IUnitOfWork unitOfWork,
                                IInterviewRepository interviewRepositry)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _interviewRepositry = interviewRepositry;
        }

        public  Interview GetInterview(Guid interviewGuid)
        {
            return GetInterviewAsync(interviewGuid).Result;
        }

        public async Task<Interview> GetInterviewAsync(Guid interviewGuid)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var interview = await _interviewRepositry.GetInterviewAsync(interviewGuid);
                    await _unitOfWork.CompleteAsync();
                    return interview;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting interview with guid {interviewGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
        public async Task<IEnumerable<Interview>> GetInterviewsAsync()
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var lstInterviews = await _interviewRepositry.GetInterviewsAsync();
                    await _unitOfWork.CompleteAsync();
                    return lstInterviews;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting all interviews. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
        public async Task<Interview> CreateAsync(Interview interview)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var result = await _interviewRepositry.CreateAsync(interview);
                    await _unitOfWork.CompleteAsync();
                    return result;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting creating interview for org {interview.OrgGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}