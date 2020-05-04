using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Exceptions
{
    public class JobNotSupportedException : NotSupportedException
    {
        public int JobId { get; }
        public JobNotSupportedException(int jobId) : base($"Job with id {jobId} is not supported in this version of Arya.")
        {
            JobId = jobId;
        }
    }
}
