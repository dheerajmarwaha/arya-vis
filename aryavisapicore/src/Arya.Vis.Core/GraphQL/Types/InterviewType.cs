using Arya.Vis.Core.Entities;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Arya.Vis.Core.GraphQL.GraphQLType
{
    public class InterviewType : ObjectGraphType<Interview>
    {
    public InterviewType(IDataLoaderContextAccessor dataLoaderAccessor)
    {
        Field(i => i.interview_guid , nullable: true, type: typeof(IdGraphType));
        Field(i => i.interview_code , nullable: true);
        Field(i => i.interview_title , nullable: true);
        Field(i => i.interview_start_date , nullable: true);
        Field(i => i.interview_end_date , nullable: true);
        Field(i => i.OrgGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewStatusGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewOwnerGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewCreatedDate, nullable: true);
        Field(i => i.CompanyGuid, nullable: true, type: typeof(IdGraphType)).Description("Company indicates an organization for which recruiter is organizing an interview");
        Field(i => i.company_location , nullable: true);
        Field(i => i.JobPostingUrl, nullable: true);
        Field(i => i.job_desc , nullable: true);
        Field(i => i.JobSummaryVisible, nullable: true);
        Field(i => i.PublishId, nullable: true);
        Field(i => i.Comments, nullable: true);
        Field(i => i.EmailDesc, nullable: true);
        Field(i => i.SendReminderEmail, nullable: true);
        Field(i => i.ReminderEmailDesc, nullable: true);
        Field(i => i.SmsDesc, nullable: true);
        Field(i => i.SendReminderSms, nullable: true);
        Field(i => i.ReminderSmsDesc, nullable: true);
        Field(i => i.InterviewsharableLink, nullable: true);
        Field(i => i.NotifyOnSubmission, nullable: true);
        Field(i => i.SendNotificationTo, nullable: true);
        Field(i => i.created_by_guid , nullable: true, type: typeof(IdGraphType));
        Field(i => i.created_date , nullable: true);
        Field(i => i.modified_by_guid , nullable: true, type: typeof(IdGraphType));
        Field(i => i.modified_date , nullable: true);
    }    
    }
}