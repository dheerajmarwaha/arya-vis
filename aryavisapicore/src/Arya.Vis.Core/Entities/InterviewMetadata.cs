using System;
using System.ComponentModel.DataAnnotations;


namespace Arya.Vis.Core.Entities{
    /// <summary>
    /// Interview DTO to transfer data from repository to service layer.
    /// </summary>
    public class InterviewMetadata
    {
        [Key]
        public  Guid? InterviewGuid{get;set;}

        [MaxLength(150)]
        [Display(Name="Interview Code")]
        public  System.String InterviewCode{get;set;}

        [Required]
        [MaxLength(250)]
        [Display(Name="Interview Title")]
        public  System.String InterviewTitle{get;set;}

        [Display(Name="Interview Start Date")]
        public  System.DateTime? InterviewStartDate{get;set;}

        [Display(Name="Interview End Date")]
        public  System.DateTime? InterviewEndDate{get; set;}

        [Display(Name="Org Guid")]
        public  System.Guid? OrgGuid{get;set;}

        [Display(Name="Interview Status Guid")]
        public  System.Guid? InterviewStatusGuid{get;set;}
        
        [Display(Name="Interview Owner Guid")]
        public  System.Guid? InterviewOwnerGuid{get;set;}

        [Display(Name="Interview Created Date")]
        public  System.DateTime? InterviewCreatedDate{get;set;}

        [Display(Name="Created By Guid")]
        public  System.Guid? CreatedByGuid{get;set;}

        [Required]
        [Display(Name="Created Date")]
        public  System.DateTime? CreatedDate{get;set;}

        [Display(Name="Modified By Guid")]
        public  System.Guid? modifiedByGuid{get;set;}

        [Required]
        [Display(Name="Modified Date")]
        public  System.DateTime? modifiedDate{get; set;}
    }
}