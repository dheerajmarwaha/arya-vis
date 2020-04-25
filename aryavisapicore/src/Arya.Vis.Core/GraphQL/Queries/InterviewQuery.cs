using Arya.Vis.Core.Services;
using Arya.Vis.Core.GraphQL.GraphQLType;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Arya.Vis.Core.GraphQL.GraphQLQueries
{
    public class InterviewQuery : ObjectGraphType<object>
    {
        public InterviewQuery(IInterviewService interviews)
        {
            Name="Query";
            Field<ListGraphType<InterviewType>>(
                name:"interviews",
                resolve: context => interviews.GetJobsAsync()
            );
        }
    }
}