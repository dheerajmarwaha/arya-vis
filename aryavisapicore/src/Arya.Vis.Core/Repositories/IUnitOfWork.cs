using System;
using System.Data;
using System.Threading.Tasks;

/* @author Mouli Kalakota */

namespace Arya.Vis.Core.Repositories
{
   public interface IUnitOfWork : IDisposable {
        Task StartAsync();
        Task StartAsync(IsolationLevel isolationLevel);
        Task<int> CompleteAsync();
        void Rollback();
    }
}