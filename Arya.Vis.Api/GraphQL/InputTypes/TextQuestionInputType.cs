using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class TextQuestionInputType : InputObjectType<TextQuestion> {
        protected override void Configure(IInputObjectTypeDescriptor<TextQuestion> descriptor) {
            descriptor.Field(x => x.CreatedTime).Type<DateTimeType>().Ignore();
            descriptor.Field(x => x.ModifiedTime).Type<DateTimeType>().Ignore();
            descriptor.Field(x => x.Attachments).Type<ListType<AttachmentInputType>>();
        }
    }
}
