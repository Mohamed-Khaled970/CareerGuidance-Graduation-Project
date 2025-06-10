namespace CareerGuidance.Api.Errors
{
    public class InterviewError
    {
        public static readonly Error DuplicatedApplication =
                   new("Interview.DuplicatedApplication", "You have already applied for this interview.", StatusCodes.Status400BadRequest);

        public static readonly Error InterviewNotFound =
            new("Interview.NotFound", "Interview not found.", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedTitle =
      new("Interview.DuplicatedTitle", "Another Interview With The Same Title Is Already Exist", StatusCodes.Status400BadRequest);
        public static readonly Error UnauthorizedAccess =
          new("Interview.UnauthorizedAccess", "You are not allowed to update this interview.", StatusCodes.Status403Forbidden);
        public static readonly Error AlreadyDeleted =
        new("Interview.AlreadyDeleted", "This interview has already been deleted.", StatusCodes.Status400BadRequest);

        public static readonly Error ApplicationNotFound =
            new("Interview.ApplicationNotFound", "Application not found for the specified interview and applicant.", StatusCodes.Status404NotFound);
        public static readonly Error CannotRejectAcceptedApplication =
     new("Interview.CannotRejectAcceptedApplication", "You cannot reject an applicant who has already been accepted.", StatusCodes.Status400BadRequest);

        public static readonly Error AlreadyRejectedApplication =
            new("Interview.AlreadyRejectedApplication", "This applicant has already been rejected.", StatusCodes.Status400BadRequest);

        public static readonly Error ApplicationNotAccepted =
     new("Interview.ApplicationNotAccepted", "Only accepted applications can be marked as done.", StatusCodes.Status400BadRequest);

    }
}
