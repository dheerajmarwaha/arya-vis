using System;
namespace Arya.Vis.Core.Entities
{
    public class User {
        private bool _isImpersonated;

        public Guid UserGuid { get; set; }        
        public Guid OrgGuid { get; set; }
        public string OrganizationName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string VendorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public Guid RoleGroupID { get; set; }
        public bool IsActive { get; set; }
        public bool HasGodView => RoleName == "God View";
        public bool IsAdmin => RoleName == "Admin";
        public Guid CreatedByGuId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedByGuId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public User ImpersonatedBy { get; set; }
        public bool IsImpersonated {
            get {
                return _isImpersonated || ImpersonatedBy != null;
            }
            set {
                _isImpersonated = value;
            }
        }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}