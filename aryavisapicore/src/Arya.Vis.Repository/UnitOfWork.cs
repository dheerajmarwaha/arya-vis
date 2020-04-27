using Arya.Storage;
using Arya.Storage.MySql;
using Arya.Vis.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;

namespace Arya.Vis.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlProvider SqlProvider;
        private readonly ILogger<UnitOfWork> _logger;
        private int Connections;
        private bool _isRolledBack = false;

        public UnitOfWork(ISqlProvider sqlProvider, ILogger<UnitOfWork> logger)
        {
            SqlProvider = sqlProvider;
            Connections = 0;
            _logger = logger;
        }

        public Task<int> CompleteAsync()
        {
            // _logger.LogInformation("Request to complete unit of work started. Current connections: {@Connections}", Connections);
            if (Connections == 1)
            {
                SqlProvider.CommitTransaction();
                // _logger.LogInformation("Unit of work is completed. Current connections: {@Connections}", Connections);
                // } else {
                //     _logger.LogInformation("Unit of work is completed without commiting transaction. Current connections: {@Connections}", Connections);
            }
            return Task.FromResult(0);
        }

        public void Rollback()
        {
            // _logger.LogInformation("Request to rollback unit of work started. Current connections: {@Connections}", Connections);
            if (!_isRolledBack)
            {
                SqlProvider.RollbackTransaction();
                _isRolledBack = true;
            }
            // _logger.LogInformation("Unit of work is rolled back. Current connections: {@Connections}", Connections);
        }

        public void Dispose()
        {
            // _logger.LogInformation("Request to dispose unit of work started. Current connections: {@Connections}", Connections);
            if (Connections == 1)
            {
                SqlProvider.CloseTransaction();
                // _logger.LogInformation("Unit of work is disposed. Current connections: {@Connections}", Connections);
                // } else {
                // _logger.LogInformation("Disposing Unit of work skipped. Current connections: {@Connections}", Connections);
                SqlProvider.Dispose();
            }
            if (Connections != 0)
            {
                Connections -= 1;
            }
            // _logger.LogInformation("Active connections in Unit of Work after dispose request: {@Connections}", Connections);
        }

        public async Task StartAsync()
        {
            // _logger.LogInformation("Request to start unit of work started. Current connections: {@Connections}", Connections);
            if (Connections <= 0)
            {
                await SqlProvider.OpenConnectionAsync();
                SqlProvider.StartTransaction();
                Connections = 0;
                _isRolledBack = false;
                // _logger.LogInformation("Unit of work is started. Current connections: {@Connections}", Connections);
            }
            Connections += 1;
            // _logger.LogInformation("Active connections in Unit of Work after start request: {@Connections}", Connections);
        }

        public async Task StartAsync(IsolationLevel isolationLevel)
        {
            // _logger.LogInformation("Request to start unit of work started. Current connections: {@Connections}", Connections);
            if (Connections <= 0)
            {
                await SqlProvider.OpenConnectionAsync();
                SqlProvider.StartTransaction(isolationLevel);
                Connections = 0;
                _isRolledBack = false;
                // _logger.LogInformation("Unit of work is started. Current connections: {@Connections}", Connections);
            }
            Connections += 1;
            // _logger.LogInformation("Active connections in Unit of Work after start request: {@Connections}", Connections);
        }
    }
}