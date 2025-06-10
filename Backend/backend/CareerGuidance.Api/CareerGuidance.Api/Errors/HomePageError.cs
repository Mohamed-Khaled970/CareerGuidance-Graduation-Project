namespace CareerGuidance.Api.Errors
{
    public class HomePageError 
    {
        public static readonly Error InvalidData =
          new("Description.RequiredField", " Invalid Description ", StatusCodes.Status400BadRequest);


        public static readonly Error Dublicated =
         new("Description.Dublicated", "this Description already exists.", StatusCodes.Status409Conflict);

        public static readonly Error NotFound =
            new(" Description.NotFound", " Description not found ", StatusCodes.Status404NotFound);














      
    }  
}
