using System;
using System.Threading;
using Arya.Vis.Api.DataLoaders;
using Arya.Vis.Core.Entities;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class RootQuery : ObjectType {
        protected override void Configure (IObjectTypeDescriptor descriptor) {
            descriptor.Field ("interview").Type<InterviewType> ().Argument ("id", id => id.Type<NonNullType<IdType>>()).Resolver ((context) => {
                var id = context.Argument<Guid> ("id");
                var interviewDataLoader = context.Service<InterviewDataLoader>();
                var dataLoader = context.BatchDataLoader<Guid, Interview>("interviewByIdBatch", async keys => {
                    return await interviewDataLoader.GetBatchInterviews(keys);
                });
                return dataLoader.LoadAsync(id, default(CancellationToken));
            });
        }
    }
}
