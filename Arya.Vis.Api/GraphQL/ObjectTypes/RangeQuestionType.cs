using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class RangeQuestionType : ObjectType<RangeQuestion> {
        protected override void Configure(IObjectTypeDescriptor<RangeQuestion> descriptor) {
            descriptor.Implements<QuestionInterfaceType>();
        }
    }
}
