using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class OptionInputType : InputObjectType<Option> {
        protected override void Configure(IInputObjectTypeDescriptor<Option> descriptor) {
            descriptor.Field(x => x.Attachment).Type<AttachmentInputType>();
        }
    }
}
