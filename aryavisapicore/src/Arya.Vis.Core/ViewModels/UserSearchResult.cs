using System.Collections.Generic;
using Arya.Vis.Core.Entities;
namespace Arya.Vis.Core.ViewModels
{
    public class UserSearchResult {
        public IEnumerable<User> Users { get; set; }
        public int Total { get; set; }
    }
}