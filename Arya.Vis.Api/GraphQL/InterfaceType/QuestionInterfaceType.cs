using Arya.Vis.Core.Services;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class QuestionInterfaceType : InterfaceType<IQuestion> {
        protected override void Configure(IInterfaceTypeDescriptor<IQuestion> descriptor) {
            descriptor.Field(x => x.CreatedBy).Ignore();
            descriptor.Field(x => x.CreatedTime).Ignore();
            descriptor.Field(x => x.ModifiedBy).Ignore();
            descriptor.Field(x => x.ModifiedTime).Ignore();
        }
    }
}
