using Arya.Vis.Core.Services;
using Arya.Vis.Core.GraphQL.GraphQLType;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Security.Claims;

namespace Arya.Vis.Core.GraphQL.GraphQLQueries
{
    public class InterviewQuery : ObjectGraphType<object>
    {
        public InterviewQuery(IInterviewService interviews)
        {
            Name = "Query";

            Field<InterviewType>(
                name: "interview",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "interviewGuid", Description = "The Guid of the Interview." }),

                resolve: context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var interviewGuid = context.GetArgument<Guid>("interviewGuid");
                    return interviews.GetInterviewAsync(interviewGuid);
                });

            Field<ListGraphType<InterviewType>>(
                name: "interviews",
                resolve: context => interviews.GetInterviewsAsync()
            );
        }
    }
}