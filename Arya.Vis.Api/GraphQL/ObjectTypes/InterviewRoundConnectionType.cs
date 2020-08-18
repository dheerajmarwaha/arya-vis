using Arya.Vis.Core.ViewModels;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class InterviewRoundConnectionType : ObjectType<InterviewRoundConnection> {
        protected override void Configure(IObjectTypeDescriptor<InterviewRoundConnection> descriptor){
            descriptor.Field(x => x.InterviewRounds).Name("interviewRoundEdges").Type<ListType<InterviewRoundType>>();
        }
    }
}
