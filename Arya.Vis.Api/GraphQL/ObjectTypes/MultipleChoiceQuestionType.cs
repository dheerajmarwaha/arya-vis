using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class MultipleChoiceQuestionType : ObjectType<MultipleChoiceQuestion> {
        protected override void Configure(IObjectTypeDescriptor<MultipleChoiceQuestion> descriptor) {
            descriptor.Implements<QuestionInterfaceType>();
        }
    }
}
