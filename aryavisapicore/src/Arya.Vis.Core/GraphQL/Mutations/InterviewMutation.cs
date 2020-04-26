using Arya.Vis.Core.Services;
using Arya.Vis.Core.GraphQL.GraphQLType;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Arya.Vis.Core.Entities;

namespace Arya.Vis.Core.GraphQL.GraphQLQueries
{
    public class InterviewMutation : ObjectGraphType<object>
    {
        public InterviewMutation(IInterviewService interviews)
        {
            Name = "Mutation";

            Field<InterviewType>(
                name: "createInterview",
                arguments: new QueryArguments(new QueryArgument<InterviewCreateInputType> { Name = "interview", Description = "Provide Interview object for creation." }),

                resolve: context =>
                {
                    var interviewInput = context.GetArgument<Interview>("interview");
                    //interviewInput
                    return interviews.CreateAsync(interviewInput);
                });
        }
    }
}