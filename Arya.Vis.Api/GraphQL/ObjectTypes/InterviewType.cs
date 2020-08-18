using System.Collections.Generic;
using System.Linq;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.ViewModels;
using HotChocolate.Types;

namespace Arya.Vis.Api.GraphQL.Queries {
    public class InterviewType : ObjectType<Interview> {
        protected override void Configure (IObjectTypeDescriptor<Interview> descriptor) {
            descriptor.Field(x => x.Rounds).Ignore();
            descriptor.Field (x => x.CreatedBy).Ignore();
            descriptor.Field (x => x.ModifiedBy).Ignore ();
            descriptor.Field(x => x.Panelists).Ignore();
            descriptor.Field(x => x.SharedWith).Ignore();
            descriptor.Field(x => x.AssignedTo).Ignore();

            descriptor.Field("interviewRoundConnection")
                .Type<InterviewRoundConnectionType>()
                .Argument("from", from => from.Type<IntType>())
                .Argument("size", size => size.Type<IntType>())
                .Resolver(context => {
                    var rounds = context.Parent<Interview>().Rounds?.ToList() ?? new List<InterviewRound>(0);
                    var from = context.Argument<int>("from");
                    var size = context.Argument<int>("size");
                    return new InterviewRoundConnection {
                        TotalCount = rounds.Count,
                        InterviewRounds = rounds.Skip(from).Take(size)
                    };
            });
        }
    }
}
