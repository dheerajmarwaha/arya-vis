using System.Collections.Generic;
using System.Linq;
using Arya.Vis.Api.GraphQL.ObjectTypes;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.ViewModels;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class InterviewRoundType : ObjectType<InterviewRound> {
        protected override void Configure(IObjectTypeDescriptor<InterviewRound> descriptor) {
            descriptor.Field(x => x.Questionnaire).Ignore();
            descriptor.Field(x => x.Panelists).Ignore();

            descriptor.Field("roundQuestionnaireConnection")
            .Type<RoundQuestionnaireConnectionType>()
            .Argument("from", from => from.Type<IntType>())
            .Argument("size", size => size.Type<IntType>())
            .Resolver(context => {
                var questionnaire = context.Parent<InterviewRound>().Questionnaire?.ToList() ?? new List<InterchangeTemplate>(0);
                var from = context.Argument<int>("from");
                var size = context.Argument<int>("size");
                return new RoundQuestionnaireConnection {
                    TotalCount = questionnaire.Count,
                    Questionnaire = questionnaire.Skip(from).Take(size)
                };
            });
        }
    }
}
