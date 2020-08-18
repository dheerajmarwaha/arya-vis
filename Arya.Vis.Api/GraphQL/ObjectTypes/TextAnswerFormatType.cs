using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class TextAnswerFormatType : ObjectType<TextAnswerFormat> {
        protected override void Configure(IObjectTypeDescriptor<TextAnswerFormat> descriptor) {
            descriptor.Implements<AnswerFormatType>();
        }
    }
}
