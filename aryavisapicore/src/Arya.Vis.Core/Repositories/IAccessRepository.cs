using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Repositories
{
    public interface IAccessRepository
    {
        Task<IEnumerable<Feature>> FindAllFeaturesAsync(Guid userGuid);
    }
}
