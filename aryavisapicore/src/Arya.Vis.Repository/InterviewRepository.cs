using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Repositories;
using System.Linq;
using Arya.Storage;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using Arya.Vis.Core.Utils;

namespace Arya.Vis.Repository
{
    public class InterviewRepository : BaseRepository, IInterviewRepository
    {
        private readonly ILogger<InterviewRepository> _logger;

        public InterviewRepository(ISqlProvider sqlProvider, ILogger<InterviewRepository> logger) :
            base(sqlProvider)
        {
            _logger = logger;
        }
        public async Task<Interview> CreateAsync(Interview interview)
        {
            interview.interview_guid  = Guid.NewGuid();
            var command = SqlProvider.CreateCommand(Routines.CreateInterview);

            try
            {
                await SqlProvider.OpenConnectionAsync();
            }
            catch { }
            using (command)
            {
                AddInterviewQueryParameters(command, interview);
                await SqlProvider.ExecuteScalarAsync(command);
                return interview;
            }
        }

        public async Task<Interview> GetInterviewAsync(Guid interviewGuid)
        {
            ValidationUtil.NotEmptyGuid(interviewGuid, nameof(interviewGuid));
            try
            {
                await SqlProvider.OpenConnectionAsync();
            }
            catch { }
            var command = SqlProvider.CreateCommand(Routines.GetInterview);
            _logger.LogInformation("InterviewRepository : Getting details of Interview {@InterviewGuid}", interviewGuid);
            using (command)
            {
                SqlProvider.AddParameterWithValue(command, "v_interview_guid", interviewGuid);
                Interview interview = ModelUtils<Interview>.CreateObject(await SqlProvider.ExecuteCommandAsync(command));
                return interview;
                //return await ReadInterviewAsync(await SqlProvider.ExecuteReaderAsync(command));
            }
        }

        public async Task<IEnumerable<Interview>> GetInterviewsAsync()
        {
            try
            {
                await SqlProvider.OpenConnectionAsync();
            }
            catch { }
            var command = SqlProvider.CreateCommand(Routines.GetInterview);
            _logger.LogInformation("InterviewRepository : Getting details of all Interviews");
            using (command)
            {
                SqlProvider.AddParameterWithValue(command, "v_interview_guid", null);
                List<Interview> lstInterviews = ModelUtils<Interview>.CreateObjects(await SqlProvider.ExecuteCommandAsync(command));

                return lstInterviews;
                //return await ReadInterviewsAsync(await SqlProvider.ExecuteReaderAsync(command));
            }
        }

        private async Task<Interview> ReadInterviewAsync(DbDataReader reader)
        {

            using (reader)
            {

                if (await SqlProvider.ReadAsync(reader))
                {
                    var interview = new Interview
                    {
                        interview_guid  = await SqlProvider.GetFieldValueAsync<Guid>(reader, "interview_guid"),
                        interview_code  = await SqlProvider.GetFieldValueAsync<string>(reader, "interview_code"),
                        interview_title  = await SqlProvider.GetFieldValueAsync<string>(reader, "interview_title"),
                        job_desc  = await SqlProvider.GetFieldValueAsync<string>(reader, "job_desc"),
                        company_location  = await SqlProvider.GetFieldValueAsync<string>(reader, "company_location"),
                        InterviewStatusGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "interview_status_guid"),
                        InterviewOwnerGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "interview_owner_guid"),
                        interview_start_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "interview_start_date")),
                        interview_end_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "interview_end_date")),
                        created_by_guid  = await SqlProvider.GetFieldValueAsync<Guid>(reader, "created_by_guid"),
                        created_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "created_date")),
                        modified_by_guid  = await SqlProvider.GetFieldValueAsync<Guid>(reader, "modified_by_guid"),
                        modified_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "modified_date")),
                    };
                    return interview;
                }
            }
            return null;
        }
        private async Task<IEnumerable<Interview>> ReadInterviewsAsync(DbDataReader reader)
        {
            IList<Interview> lstInterviews = new List<Interview>();
            using (reader)
            {
                //await SqlProvider.NextResultAsync(reader);
                while (await SqlProvider.ReadAsync(reader))
                {
                    var interview = new Interview
                    {
                        interview_guid  = await SqlProvider.GetFieldValueAsync<Guid>(reader, "interview_guid"),
                        interview_code  = await SqlProvider.GetFieldValueAsync<string>(reader, "interview_code"),
                        interview_title  = await SqlProvider.GetFieldValueAsync<string>(reader, "interview_title"),
                        job_desc  = await SqlProvider.GetFieldValueAsync<string>(reader, "job_desc"),
                        company_location  = await SqlProvider.GetFieldValueAsync<string>(reader, "company_location"),
                        InterviewStatusGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "interview_status_guid"),
                        InterviewOwnerGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "interview_owner_guid"),
                        interview_start_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "interview_start_date")),
                        interview_end_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "interview_end_date")),
                        created_by_guid  = await SqlProvider.GetFieldValueAsync<Guid>(reader, "created_by_guid"),
                        created_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "created_date")),
                        modified_by_guid  = await SqlProvider.GetFieldValueAsync<Guid>(reader, "modified_by_guid"),
                        modified_date  = Convert.ToDateTime(await SqlProvider.GetFieldValueAsync<DateTime>(reader, "modified_date")),
                    };
                    lstInterviews.Add(interview);
                    //await SqlProvider.NextResultAsync(reader);
                }
            }
            return lstInterviews;
        }
        private void AddInterviewQueryParameters(DbCommand command, Interview interview)
        {
            SqlProvider.AddParameterWithValue(command, "v_interview_guid", interview.interview_guid );
            SqlProvider.AddParameterWithValue(command, "v_interview_code", interview.interview_code );
            SqlProvider.AddParameterWithValue(command, "v_interview_title", interview.interview_title );
            SqlProvider.AddParameterWithValue(command, "v_interview_start_date", interview.interview_start_date );
            SqlProvider.AddParameterWithValue(command, "v_interview_end_date", interview.interview_end_date );
            SqlProvider.AddParameterWithValue(command, "v_org_guid", interview.OrgGuid);
            SqlProvider.AddParameterWithValue(command, "v_interview_status_guid", interview.InterviewStatusGuid);
            SqlProvider.AddParameterWithValue(command, "v_interview_owner_guid", interview.InterviewOwnerGuid);
            SqlProvider.AddParameterWithValue(command, "v_interview_created_date", interview.InterviewCreatedDate);
            SqlProvider.AddParameterWithValue(command, "v_job_title", interview.interview_title );
            SqlProvider.AddParameterWithValue(command, "v_company_guid", interview.CompanyGuid);
            SqlProvider.AddParameterWithValue(command, "v_company_location", interview.company_location );
            SqlProvider.AddParameterWithValue(command, "v_job_posting_url", interview.JobPostingUrl);
            SqlProvider.AddParameterWithValue(command, "v_job_desc", interview.job_desc );
            SqlProvider.AddParameterWithValue(command, "v_job_summary_visible", interview.JobSummaryVisible);
            SqlProvider.AddParameterWithValue(command, "v_publish_id", interview.PublishId);
            SqlProvider.AddParameterWithValue(command, "v_comments", interview.Comments);
            SqlProvider.AddParameterWithValue(command, "v_email_desc", interview.EmailDesc);
            SqlProvider.AddParameterWithValue(command, "v_send_reminder_email", interview.SendReminderEmail);
            SqlProvider.AddParameterWithValue(command, "v_reminder_email_desc", interview.ReminderEmailDesc);
            SqlProvider.AddParameterWithValue(command, "v_sms_desc", interview.SmsDesc);
            SqlProvider.AddParameterWithValue(command, "v_send_reminder_sms", interview.SendReminderSms);
            SqlProvider.AddParameterWithValue(command, "v_reminder_sms_desc", interview.ReminderSmsDesc);
            SqlProvider.AddParameterWithValue(command, "v_interview_sharable_link", interview.InterviewsharableLink);
            SqlProvider.AddParameterWithValue(command, "v_notify_on_submission", interview.NotifyOnSubmission);
            SqlProvider.AddParameterWithValue(command, "v_send_notification_to", interview.SendNotificationTo);
            SqlProvider.AddParameterWithValue(command, "v_created_by_guid", interview.created_by_guid );
            SqlProvider.AddParameterWithValue(command, "v_created_date", DateTime.Now);
            SqlProvider.AddParameterWithValue(command, "v_modified_by_guid", interview.modified_by_guid );
            SqlProvider.AddParameterWithValue(command, "v_modified_date", DateTime.Now);
        }

        /* In memory implementation
        private IList<Interview> _interviews;
        private readonly ILogger<InterviewRepository> _logger;

        public InterviewRepository(ISqlProvider sqlProvider, ILogger<InterviewRepository> logger) :
            base(sqlProvider)
        {
            _logger = logger;
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
            // var command = SqlProvider.CreateCommand(Routines.CreateInterview);
            // using(command) {
            //     AddInterviewQueryParameters(command, Interview, userId);
            //     return Convert.ToInt32(await SqlProvider.ExecuteScalarAsync(command));
            // }
        }

        public Task<IEnumerable<Interview>> GetInterviewsAsync()
        {
            return Task.FromResult(_interviews.AsEnumerable());
        }
        */

    }
}
