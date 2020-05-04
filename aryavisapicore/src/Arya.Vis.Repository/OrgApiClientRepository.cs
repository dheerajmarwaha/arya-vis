using Arya.Storage;
using Arya.Vis.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Repository
{
    public class OrgApiClientRepository : BaseRepository, IOrgApiClientRepository
    {
        public OrgApiClientRepository(ISqlProvider sqlProvider) : base(sqlProvider) { }

        public async Task<Guid?> GetOrgGuidAsync(string clientId)
        {
            var command = SqlProvider.CreateCommand("v3_get_organization_guid_by_client_id");
            SqlProvider.AddParameterWithValue(command, "vClientId", clientId);
            using (command)
            {
                var result = await SqlProvider.ExecuteScalarAsync(command);
                if (result == null)
                {
                    return null;
                }
                return Guid.Parse(result.ToString());
            }
        }
    }
}
