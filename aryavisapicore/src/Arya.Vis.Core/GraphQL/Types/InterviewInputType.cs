using Arya.Vis.Core.Entities;
using GraphQL.Types;

namespace Arya.Vis.Core.GraphQL.GraphQLType
{
    public class InterviewInputType : InputObjectGraphType
    {
        public InterviewInputType()
        {
            Name = "InterviewInput";
            Field<NonNullGraphType<StringGraphType>>("interview_code");
            Field<StringGraphType>("interview_title");
            Field<DateGraphType>("interview_start_date");
            Field<DateGraphType>("interview_end_date");
            Field<IdGraphType>("orgGuid");
            Field<IdGraphType>("interviewStatusGuid");
            Field<IdGraphType>("interviewOwnerGuid");
            Field<DateGraphType>("interviewCreatedDate");
            Field<IdGraphType>("companyGuid");
            Field<StringGraphType>("company_location");
            Field<StringGraphType>("jobPostingUrl");
            Field<StringGraphType>("job_desc");
            Field<BooleanGraphType>("jobSummaryVisible");
            Field<IntGraphType>("publishId");
            Field<StringGraphType>("comments");
            Field<StringGraphType>("emailDesc");
            Field<BooleanGraphType>("sendReminderEmail");
            Field<StringGraphType>("reminderEmailDesc");
            Field<StringGraphType>("smsDesc");
            Field<BooleanGraphType>("sendReminderSms");
            Field<StringGraphType>("reminderSmsDesc");
            Field<StringGraphType>("interviewsharableLink");
            Field<BooleanGraphType>("notifyOnSubmission");           
        }
    }
}