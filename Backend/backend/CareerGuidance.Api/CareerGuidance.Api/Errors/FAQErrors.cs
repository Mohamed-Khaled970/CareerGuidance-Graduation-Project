namespace CareerGuidance.Api.Errors
{
    public class FAQErrors
    {
        public static readonly Error DuplicatedQuestion =
           new("FAQ.DuplicatedQuestion", "This Question Already Exist", StatusCodes.Status400BadRequest);
        public static readonly Error InvalidQuestion =
          new("FAQ.InvalidQuestion", "Question is invalid", StatusCodes.Status400BadRequest);
        public static readonly Error InvalidQuestionEnd =
          new("FAQ.InvalidQuestion", "Question must be ended with one question mark '?' ", StatusCodes.Status400BadRequest);
        public static readonly Error NoQuestionsFound =
       new("FAQ.NoQuestionsFound", "There is no Questions Right now", StatusCodes.Status400BadRequest);
        public static readonly Error QuestionNotFound =
    new("FAQ.QuestionNotFound", "This Question is not found", StatusCodes.Status400BadRequest);
    }
}
