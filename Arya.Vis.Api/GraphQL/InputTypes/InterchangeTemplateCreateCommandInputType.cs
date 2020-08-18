using Arya.Vis.Api.Commands;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class InterchangeTemplateCreateCommandInputType : InputObjectType<InterchangeTemplateCreateCommand> {
        protected override void Configure(IInputObjectTypeDescriptor<InterchangeTemplateCreateCommand> descriptor) {
            descriptor.Field(x => x.TextQuestion).Type<TextQuestionInputType>();
            descriptor.Field(x => x.RangeQuestion).Type<RangeQuestionInputType>();
            descriptor.Field(x => x.MultipleChoiceQuestion).Type<MultipleChoiceQuestionInputType>();
            descriptor.Field(x => x.TextAnswerFormat).Type<TextAnswerFormatInputType>();
            descriptor.Field(x => x.VideoAnswerFormat).Type<VideoAnswerFormatInputType>();
            descriptor.Field(x => x.AudioAnswerFormat).Type<AudioAnswerFormatInputType>();
        }
    }
}
