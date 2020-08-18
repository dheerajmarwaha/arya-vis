using Arya.Vis.Core.Entities;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.InputTypes {
    public class InterviewContextInputType : InputObjectType<InterviewContext> {
        protected override void Configure(IInputObjectTypeDescriptor<InterviewContext> descriptor) {
            descriptor.Field(x => x.CreatedTime).Ignore();
            descriptor.Field(x => x.CreatedBy).Ignore();
            descriptor.Field(x => x.ModifiedTime).Ignore();
            descriptor.Field(x => x.ModifiedBy).Ignore();
            descriptor.Field(x => x.Experience).Type<ExperienceInputType>();
            descriptor.Field(x => x.Company).Type<CompanyInputType>();
            descriptor.Field(x => x.Location).Type<LocationInputType>();
        }
    }
}
