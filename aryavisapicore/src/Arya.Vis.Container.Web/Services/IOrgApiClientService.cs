using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Services
{
    public interface IOrgApiClientService
    {
        Task<Organization> GetAsync(string clientId);
    }
}
