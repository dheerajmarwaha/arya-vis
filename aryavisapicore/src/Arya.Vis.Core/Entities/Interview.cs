using System;
using System.ComponentModel.DataAnnotations;


namespace Arya.Vis.Core.Entities{
    public class Interview : InterviewMetadata
    {
               
        [Display(Name="Company Guid")]
        public  System.Guid? CompanyGuid{get;set;}

        [MaxLength(2000)]
        [Display(Name="Company Location")]
        public  System.String company_location {get;set;}

        [MaxLength(200)]
        [Display(Name="Job Posting Url")]
        [Url]
        public  System.String JobPostingUrl{get;set;}
        
        [MaxLength(2147483647)]
        [Display(Name="Job Desc")]
        public  System.String job_desc {get;set;}
        
        [Display(Name="Job Summary Visible")]
        public  System.Boolean? JobSummaryVisible{get;set;}
        
        [Display(Name="Publish ID")]
        public  System.Int32? PublishId{get;set;}
        
        [MaxLength(2147483647)]
        [Display(Name="Comments")]
        public  System.String Comments{get;set;}
        
        [MaxLength(2147483647)]
        [Display(Name="Email Desc")]
        public  System.String EmailDesc{get;set;}

        [Display(Name="Send Reminder Email")]
        public  System.Boolean? SendReminderEmail{get;set;}

        [MaxLength(2147483647)]
        [Display(Name="Reminder Email Desc")]
        public  System.String ReminderEmailDesc{get;set;}
        
        [MaxLength(2147483647)]
        [Display(Name="Sms Desc")]
        public  System.String SmsDesc{get;set;}

        [Display(Name="Send Reminder Sms")]
        public  System.Boolean? SendReminderSms{get;set;}

        [MaxLength(2147483647)]
        [Display(Name="Reminder Sms Desc")]
        public  System.String ReminderSmsDesc{get;set;}
        
        [MaxLength(200)]
        [Display(Name="Interview Sharable Link")]
        public  System.String InterviewsharableLink{get;set;}
        
        [Display(Name="Notify On Submission")]
        public  System.Boolean? NotifyOnSubmission{get;set;}
        
        [MaxLength(2147483647)]
        [Display(Name="Send Notification To")]
        public  System.String SendNotificationTo{get;set;}
    }
}