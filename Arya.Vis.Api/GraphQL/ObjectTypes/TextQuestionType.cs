using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class TextQuestionType : ObjectType<TextQuestion> {
        protected override void Configure(IObjectTypeDescriptor<TextQuestion> descriptor) {
            descriptor.Implements<QuestionInterfaceType>();
        }
    }
}
