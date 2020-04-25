using System;
namespace Arya.Vis.Core.ViewModels
{
    public class ConversationAutomationSettings
    {
        public TimeSpan CoolDownTimeSpan { get; set; }
        public Guid? ReferenceId { get; set; }
    }
}