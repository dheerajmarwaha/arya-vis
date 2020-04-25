using Arya.Vis.Core.GraphQL.GraphQLQueries;
using GraphQL;
using GraphQL.Types;

namespace Arya.Vis.Core.GraphQLSchema
{
    public class InterviewSchema : Schema
    {
        public InterviewSchema(InterviewQuery query, IDependencyResolver resolver)
        {
            Query = query;
            DependencyResolver = resolver;
        }
    }
}