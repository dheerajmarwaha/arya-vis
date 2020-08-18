using Arya.Vis.Api.GraphQL.EnumTypes;
using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class AttachmentInputType : InputObjectType<Attachment> {
        protected override void Configure(IInputObjectTypeDescriptor<Attachment> descriptor) {
            descriptor.Field(x => x.MediaType).Type<MediaTypeEnum>();
        }
    }
}
