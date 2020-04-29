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

            FieldAsync<InterviewType>(
                name: "createInterview",

                arguments: new QueryArguments(
                    new QueryArgument<InterviewInputType> { Name = "interview", Description = "Provide Interview object for creation." }),

                resolve: async context =>
                {
                    var interviewInput = context.GetArgument<Interview>("interview");
                    return  await context.TryAsyncResolve( 
                        async c=> await interviews.CreateAsync(interviewInput));
                });
        }
    }
}