using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class VideoAnswerFormatType : ObjectType<VideoAnswerFormat> {
        protected override void Configure(IObjectTypeDescriptor<VideoAnswerFormat> descriptor) {
            descriptor.Implements<AnswerFormatType>();
        }
    }
}
