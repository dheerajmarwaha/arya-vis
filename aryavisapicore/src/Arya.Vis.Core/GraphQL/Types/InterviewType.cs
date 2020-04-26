using Arya.Vis.Core.Entities;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Arya.Vis.Core.GraphQL.GraphQLType
{
    public class InterviewType : ObjectGraphType<Interview>
    {
    public InterviewType(IDataLoaderContextAccessor dataLoaderAccessor)
    {
        Field(i => i.InterviewGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewCode, nullable: true);
        Field(i => i.InterviewTitle, nullable: true);
        Field(i => i.InterviewStartDate, nullable: true);
        Field(i => i.InterviewEndDate, nullable: true);
        Field(i => i.OrgGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewStatusGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewOwnerGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.InterviewCreatedDate, nullable: true);
        Field(i => i.CompanyGuid, nullable: true, type: typeof(IdGraphType)).Description("Company indicates an organization for which recruiter is organizing an interview");
        Field(i => i.CompanyLocation, nullable: true);
        Field(i => i.JobPostingUrl, nullable: true);
        Field(i => i.JobDesc, nullable: true);
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
        Field(i => i.CreatedByGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.CreatedDate, nullable: true);
        Field(i => i.modifiedByGuid, nullable: true, type: typeof(IdGraphType));
        Field(i => i.modifiedDate, nullable: true);
    }    
    }
}