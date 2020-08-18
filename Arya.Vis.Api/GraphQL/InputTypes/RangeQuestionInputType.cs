using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class RangeQuestionInputType : InputObjectType<RangeQuestion> {
        protected override void Configure(IInputObjectTypeDescriptor<RangeQuestion> descriptor) {
            descriptor.Field(x => x.CreatedTime).Type<DateTimeType>().Ignore();
            descriptor.Field(x => x.ModifiedTime).Type<DateTimeType>().Ignore();
            descriptor.Field(x => x.Attachments).Type<ListType<AttachmentInputType>>();
            descriptor.Field(x => x.Min).Type<RangeOptionInputType>();
            descriptor.Field(x => x.Max).Type<RangeOptionInputType>();
        }
    }
}
