using Arya.Vis.Core.Entities;
using GraphQL.Types;

namespace Arya.Vis.Core.GraphQL.GraphQLType
{
    public class InterviewCreateInputType : InputObjectGraphType
    {
        public InterviewCreateInputType()
        {
            Name = "InterviewInput";
            Field<NonNullGraphType<StringGraphType>>("interviewCode");
            Field<StringGraphType>("interviewTitle");
            Field<DateGraphType>("interviewStartDate");
            Field<DateGraphType>("interviewEndDate");
            Field<IdGraphType>("orgGuid");
            Field<IdGraphType>("interviewStatusGuid");
            Field<IdGraphType>("interviewOwnerGuid");
            Field<DateGraphType>("interviewCreatedDate");
            Field<IdGraphType>("companyGuid");
            Field<StringGraphType>("companyLocation");
            Field<StringGraphType>("jobPostingUrl");
            Field<StringGraphType>("jobDesc");
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