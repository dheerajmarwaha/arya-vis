using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class AudioAnswerFormatType : ObjectType<AudioAnswerFormat> {
        protected override void Configure(IObjectTypeDescriptor<AudioAnswerFormat> descriptor) {
            descriptor.Implements<AnswerFormatType>();
        }
    }
}
