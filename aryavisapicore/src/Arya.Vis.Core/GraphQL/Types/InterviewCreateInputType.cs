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
            Field<NonNullGraphType<StringGraphType>>("interviewTitle");
            Field<NonNullGraphType<DateGraphType>>("interviewStartDate");
            Field<NonNullGraphType<DateGraphType>>("interviewEndDate");
            Field<NonNullGraphType<IdGraphType>>("orgGuid");
            Field<NonNullGraphType<IdGraphType>>("interviewStatusGuid");
            Field<NonNullGraphType<IdGraphType>>("interviewOwnerGuid");
            Field<NonNullGraphType<DateGraphType>>("interviewCreatedDate");
            Field<NonNullGraphType<IdGraphType>>("companyGuid");
            Field<NonNullGraphType<StringGraphType>>("companyLocation");
            Field<NonNullGraphType<StringGraphType>>("jobPostingUrl");
            Field<NonNullGraphType<StringGraphType>>("jobDesc");
            Field<NonNullGraphType<BooleanGraphType>>("jobSummaryVisible");
            Field<NonNullGraphType<IntGraphType>>("publishId");
            Field<NonNullGraphType<StringGraphType>>("comments");
            Field<NonNullGraphType<StringGraphType>>("emailDesc");
            Field<NonNullGraphType<BooleanGraphType>>("sendReminderEmail");
            Field<NonNullGraphType<StringGraphType>>("reminderEmailDesc");
            Field<NonNullGraphType<StringGraphType>>("smsDesc");
            Field<NonNullGraphType<BooleanGraphType>>("sendReminderSms");
            Field<NonNullGraphType<StringGraphType>>("reminderSmsDesc");
            Field<NonNullGraphType<StringGraphType>>("interviewsharableLink");
            Field<NonNullGraphType<BooleanGraphType>>("notifyOnSubmission");           
        }
    }
}