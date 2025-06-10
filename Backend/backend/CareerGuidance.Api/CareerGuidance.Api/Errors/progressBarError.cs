namespace CareerGuidance.Api.Errors
{
    public class progressBarError
    {
        public static readonly Error NoInfoFound =
                new("Progress.NoInfoFound", "There is no Information  Right now", StatusCodes.Status400BadRequest);

        public static readonly Error IsEmpty =
               new("Progress.IsEmpty", "this field can not be empty", StatusCodes.Status400BadRequest);
    }
}
