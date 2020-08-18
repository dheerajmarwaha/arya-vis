using Arya.Vis.Api.Commands;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class InterviewRoundCreateCommandInputType : InputObjectType<InterviewRoundCreateCommand> {
        protected override void Configure(IInputObjectTypeDescriptor<InterviewRoundCreateCommand> descriptor) {
            descriptor.Field(x => x.Questionnaire).Type<ListType<InterchangeTemplateCreateCommandInputType>>();
        }
    }
}
