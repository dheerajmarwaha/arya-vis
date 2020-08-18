using Arya.Vis.Api.Commands;
using Arya.Vis.Api.GraphQL.EnumTypes;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class InterviewCreateCommandInputType : InputObjectType<InterviewCreateCommand> {

        protected override void Configure(IInputObjectTypeDescriptor<InterviewCreateCommand> descriptor) {
            descriptor.Field(x => x.Status).Type<InterviewStatusType>();
            descriptor.Field(x => x.Rounds).Type<ListType<InterviewRoundCreateCommandInputType>>();
            descriptor.Field(x => x.Context).Type<InterviewContextInputType>();
            descriptor.Field(x => x.RequiredCandidateInfo).Type<RequiredCandidateInfoInputType>();
        }
    }
}
