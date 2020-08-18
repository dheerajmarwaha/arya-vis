using Arya.Vis.Api.GraphQL.EnumTypes;
using Arya.Vis.Api.GraphQL.InputTypes;
using Arya.Vis.Api.GraphQL.Mutations;
using Arya.Vis.Api.GraphQL.Queries;
using HotChocolate;

namespace Arya.Vis.Api.GraphQL.Extensions {
    public static class TypesExtension {
        public static ISchemaBuilder AddTypes(this ISchemaBuilder builder) {
            builder
                .AddType<MediaTypeEnum>()
                .AddType<InterviewType> ()
                .AddType<UserType> ()
                .AddType<MultipleChoiceQuestionType>()
                .AddType<TextQuestionType>()
                .AddType<RangeQuestionType>()
                .AddType<TextAnswerFormatType>()
                .AddType<VideoAnswerFormatType>()
                .AddType<AudioAnswerFormatType>()
                .AddType<RangeOptionInputType>()
                .AddType<AttachmentInputType>()
                .AddType<OptionInputType>()
                .AddType<TextQuestionInputType>()
                .AddType<InterchangeTemplateCreateCommandInputType>()
                .AddType<InterviewRoundCreateCommandInputType>()
                .AddType<InterviewCreateCommandInputType>()
                .AddQueryType<RootQuery>()
                .AddMutationType<Mutation>();
            return builder;
        }
    }
}
