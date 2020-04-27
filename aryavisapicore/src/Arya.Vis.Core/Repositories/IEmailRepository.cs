using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Config;
using Arya.Vis.Core.Entities;

namespace Arya.Vis.Core.Repositories
{
    public interface IEmailRepository {
        Task<EmailAddress> SaveAsync(string emailId, Guid userId);
        Task AuthorizeAsync(Guid id);
        Task DeleteAsync(Guid id, Guid userId);
        Task<EmailAddress> GetEmailAsync(Guid id);
        Task<IEnumerable<EmailAddress>> FetchAllAsync(Guid userId);
        Task<EmailVerificationConfig> GetEmailVerificationConfigAsync();
    }
}