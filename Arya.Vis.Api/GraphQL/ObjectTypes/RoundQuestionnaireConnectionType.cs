using Arya.Vis.Core.ViewModels;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.ObjectTypes {
    public class RoundQuestionnaireConnectionType : ObjectType<RoundQuestionnaireConnection> {
        protected override void Configure(IObjectTypeDescriptor<RoundQuestionnaireConnection> descriptor) {
            descriptor.Field(x => x.Questionnaire).Name("roundQuestionnaireEdges");
        }
    }
}
