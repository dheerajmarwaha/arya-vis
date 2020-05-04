using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.ViewModels
{
    public class UserConfiguration
    {
        private bool? isManagementUser;

        public string Role { get; set; }
       // public IEnumerable<SourceConfiguration> Sources { get; set; }
        public DistanceModel Distance { get; set; }
        public string Language { get; set; }
        public IEnumerable<Feature> Features { get; set; }
        public bool IsCandidateScoreVisible { get; set; } = false;
        public bool IsJobTagEnabled { get; set; } = false;
        public bool IsAutoSourcingEnabled { get; set; } = true;
        public bool IsQuickSearchConnectOptionsEnabled { get; set; } = false;

        // ? In case of false, value should be set to null, so that it is not serialized in output
        public bool? IsManagementUser
        {
            get
            {
                if (isManagementUser == true)
                    return isManagementUser;
                return null;
            }
            set { isManagementUser = value; }
        }
        public AutoLogout Logout { get; set; }
        public int? JobLimit { get; set; }
        public int? DefaultSourceLimit { get; set; }
        public int? MaxSourceLimit { get; set; }
        public string SubscriptionType { get; set; }
        public string WhitelabelId { get; set; }
    }
}
