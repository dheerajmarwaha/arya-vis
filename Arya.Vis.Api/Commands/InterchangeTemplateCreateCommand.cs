using System.Collections.Generic;
using Arya.Exceptions;
using Arya.Utils;
using Arya.Vis.Core.Entities;

namespace Arya.Vis.Api.Commands {
    public class InterchangeTemplateCreateCommand {
        public int SequenceNumber { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsDescriptionVisible { get; set; }
        public TextQuestion TextQuestion { get; set; }
        public RangeQuestion RangeQuestion { get; set; }
        public MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }
        public TextAnswerFormat TextAnswerFormat { get; set; }
        public AudioAnswerFormat AudioAnswerFormat { get; set; }
        public VideoAnswerFormat VideoAnswerFormat { get; set; }

        public static InterchangeTemplate MapToInterchangeTemplate(InterchangeTemplateCreateCommand command) {
            ValidationUtil.NotNull<InterchangeTemplateCreateCommand>(command);
            var interchangeTemplate = new InterchangeTemplate{
                SequenceNumber = command.SequenceNumber,
                IsMandatory = command.IsMandatory,
                IsDescriptionVisible = command.IsDescriptionVisible
            };
            var numberOfQuestionsPresent = GetNumberOfQuestionsPresent(command);
            if (numberOfQuestionsPresent != 1) {
                throw new InvalidArgumentException(nameof(numberOfQuestionsPresent), numberOfQuestionsPresent.ToString(), "Exactly one");
            }
            if (IsTextQuestionPresent(command.TextQuestion)) {
                interchangeTemplate.Question = command.TextQuestion;
            }
            if (IsRangeQuestionPresent(command.RangeQuestion)) {
                interchangeTemplate.Question = command.RangeQuestion;
            }
            if (IsMultipleChoiceQuestionPresent(command.MultipleChoiceQuestion)) {
                interchangeTemplate.Question = command.MultipleChoiceQuestion;
            }
            interchangeTemplate.AllowedAnswerFormats = GetAllowedAnswerFormats(command.TextAnswerFormat, command.VideoAnswerFormat, command.AudioAnswerFormat);

            return interchangeTemplate;
        }

        private static IEnumerable<IAnswerFormat> GetAllowedAnswerFormats(TextAnswerFormat textAnswerFormat, VideoAnswerFormat videoAnswerFormat, AudioAnswerFormat audioAnswerFormat) {
            var allowedAnswerFormats = new List<IAnswerFormat>();
            if (textAnswerFormat != null) {
                allowedAnswerFormats.Add(textAnswerFormat);
            }
            if (videoAnswerFormat != null) {
                allowedAnswerFormats.Add(videoAnswerFormat);
            }
            if (audioAnswerFormat != null) {
                allowedAnswerFormats.Add(audioAnswerFormat);
            }
            return allowedAnswerFormats;
        }

        private static bool IsTextQuestionPresent(TextQuestion textQuestion) {
            return textQuestion != null;
        }

        private static bool IsRangeQuestionPresent(RangeQuestion rangeQuestion) {
            return rangeQuestion != null;
        }

        private static bool IsMultipleChoiceQuestionPresent(MultipleChoiceQuestion multipleChoiceQuestion) {
            return multipleChoiceQuestion != null;
        }

        private static int GetNumberOfQuestionsPresent(InterchangeTemplateCreateCommand command) {
            int count = 0;
            if (IsTextQuestionPresent(command.TextQuestion)) {
                count++;
            }
            if (IsMultipleChoiceQuestionPresent(command.MultipleChoiceQuestion)) {
                count++;
            }
            if (IsRangeQuestionPresent(command.RangeQuestion)) {
                count++;
            }
            return count;
        }
    }
}
