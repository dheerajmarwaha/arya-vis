using Arya.Vis.Core.GraphQL.GraphQLQueries;
using GraphQL;
using GraphQL.Types;

namespace Arya.Vis.Core.GraphQLSchema
{
    public class InterviewSchema : Schema
    {
        public InterviewSchema(InterviewQuery query, InterviewMutation mutation, IDependencyResolver resolver)
        {
            Query = query;
            Mutation = mutation;
            DependencyResolver = resolver;
        }
    }
}