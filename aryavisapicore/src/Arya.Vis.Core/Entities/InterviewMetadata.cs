using System;
using System.ComponentModel.DataAnnotations;


namespace Arya.Vis.Core.Entities{
    /// <summary>
    /// Interview DTO to transfer data from repository to service layer.
    /// </summary>
    public class InterviewMetadata
    {
        [Key]
        public  Guid? interview_guid {get;set;}

        [MaxLength(150)]
        [Display(Name="Interview Code")]
        public  System.String interview_code {get;set;}

        [Required]
        [MaxLength(250)]
        [Display(Name="Interview Title")]
        public  System.String interview_title {get;set;}

        [Display(Name="Interview Start Date")]
        public  System.DateTime? interview_start_date {get;set;}

        [Display(Name="Interview End Date")]
        public  System.DateTime? interview_end_date {get; set;}

        [Display(Name="Org Guid")]
        public  System.Guid? OrgGuid{get;set;}

        [Display(Name="Interview Status Guid")]
        public  System.Guid? InterviewStatusGuid{get;set;}
        
        [Display(Name="Interview Owner Guid")]
        public  System.Guid? InterviewOwnerGuid{get;set;}

        [Display(Name="Interview Created Date")]
        public  System.DateTime? InterviewCreatedDate{get;set;}

        [Display(Name="Created By Guid")]
        public  System.Guid? created_by_guid {get;set;}

        [Required]
        [Display(Name="Created Date")]
        public  System.DateTime? created_date {get;set;}

        [Display(Name="Modified By Guid")]
        public  System.Guid? modified_by_guid {get;set;}

        [Required]
        [Display(Name="Modified Date")]
        public  System.DateTime? modified_date {get; set;}
    }
}