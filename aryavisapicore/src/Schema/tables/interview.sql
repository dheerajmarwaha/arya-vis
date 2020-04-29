DROP TABLE IF EXISTS interview;

CREATE TABLE interview (
   interview_guid			BINARY  (16) NOT NULL,
   interview_code			varchar(150) DEFAULT NULL,
   interview_title			varchar(250) NOT NULL,
   interview_start_date		datetime DEFAULT NULL,
   interview_end_date		datetime DEFAULT NULL,
   org_guid					BINARY  (16) DEFAULT NULL,   
   interview_status_guid	BINARY  (16) DEFAULT NULL,   
   interview_owner_guid		BINARY  (16) DEFAULT NULL,
   interview_created_date	datetime DEFAULT NULL,   
   
    -- Job Summary
   job_title			varchar(250) NOT NULL,   
   company_guid			BINARY  (16) DEFAULT NULL,    
   company_location		varchar(2000) DEFAULT NULL,
   job_posting_url		varchar(200) DEFAULT NULL,
   job_desc				text,
   job_summary_visible	bit(1) DEFAULT NULL,
   
   -- publish details
   publish_id				int(11) DEFAULT NULL,
   comments				text CHARACTER SET latin1,

   --	Invite 
   --		emails  
   email_desc				text,
   send_reminder_email	bit(1) DEFAULT b'1',
   reminder_email_desc	text,

   --		SMS   
   sms_desc				text,
   send_reminder_sms		bit(1) DEFAULT b'1',
   reminder_sms_desc		text,   

   --   sharable link
   interview_sharable_link varchar(200) DEFAULT NULL,	

   -- invite settings
   notify_on_submission	bit(1) DEFAULT b'1',
   send_notification_to	text CHARACTER SET latin1,
   
    -- Audit details
   created_by_guid		BINARY  (16) DEFAULT NULL,
   created_date			datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
   modified_by_guid		BINARY  (16) DEFAULT NULL,
   modified_date			datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,   

   PRIMARY KEY (interview_guid)   
 )ENGINE=InnoDB AUTO_INCREMENT=262895 DEFAULT CHARSET=utf8;