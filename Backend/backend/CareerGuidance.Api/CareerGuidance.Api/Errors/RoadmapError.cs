namespace CareerGuidance.Api.Errors
{
    public class RoadmapError
    {
        public static readonly Error InvalidData =

          new("Roadmap.InvalidData", " Enter Valid Data", StatusCodes.Status400BadRequest);


        public static readonly Error Dublicated =
         new("Roadmap.Dublicated", "this Name is already exists", StatusCodes.Status409Conflict);

        public static readonly Error CategoryDublicated =
         new("Roadmap.Dublicated", "this Category is already exists", StatusCodes.Status409Conflict);

          public static readonly Error CategoryNotEmpty =
         new("Roadmap.CategoryNotEmpty", "This Field Can Not Be Empty", StatusCodes.Status409Conflict);

        public static readonly Error RoadmapNameEmpty =
       new("Roadmap.RoadmapNameEmpty", "Roadmap name can't be empty", StatusCodes.Status400BadRequest);


        public static readonly Error Failed =
         new("Roadmap.Faild", "Failed ", StatusCodes.Status400BadRequest);



        public static readonly Error NotFound =
            new(" Roadmap.NotFound", " Roadmap not found ", StatusCodes.Status404NotFound);

        public static readonly Error NodesEmpty =
      new("Roadmap.NodesEmpty", "Roadmap nodes cannot be empty. At least one node is required.", StatusCodes.Status400BadRequest);

        public static readonly Error RoadmapNameAndNodesEmpty =
     new("Roadmap.RoadmapNameAndNodesEmpty", "The roadmap data is invalid. Roadmap name and nodes are required.", StatusCodes.Status400BadRequest);

        public static readonly Error EdgesEmpty =
        new("Roadmap.EdgesEmpty", "Roadmap edges can't be empty. At least one edge is required.", StatusCodes.Status400BadRequest);

        public static readonly Error NodeLinksEmpty =
      new("Roadmap.NodeLinksEmpty", "Roadmap Node can't be empty. At least one valid link is required.", StatusCodes.Status400BadRequest);

        public static readonly Error CategoryNotFound =
        new("Roadmap.CategoryNotFound", "The specified category does not exist. Please add the category first.", StatusCodes.Status404NotFound);
        
        public static readonly Error InvalidCategoryName =
        new("Roadmap.CategoryName", "Category Name Is Invalid", StatusCodes.Status404NotFound);
        
        public static readonly Error CategoryNameLength =
        new("Roadmap.CategoryNameTooShort", "Category must be at least 5 characters", StatusCodes.Status400BadRequest);
         
        public static readonly Error NodeTitleEmpty =
      new("Roadmap.NodeTitleEmpty", "Node title cannot be empty.", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicateNode =
       new("Roadmap.DuplicateNode", "Duplicate node detected.", StatusCodes.Status409Conflict);
        
        public static readonly Error DuplicateNodeTitle =
       new("Roadmap.DuplicateNodeTitle", "Duplicate node title detected.", StatusCodes.Status409Conflict);

        public static readonly Error LinkUrlEmpty =
        new("Roadmap.LinkUrlEmpty", "Link URL in node cannot be empty.", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicateLink =
            new("Roadmap.DuplicateLink", "Duplicate link detected.", StatusCodes.Status409Conflict);

        public static readonly Error LinkAlreadyExists =
            new("Roadmap.LinkAlreadyExists", "Link already exists in node.", StatusCodes.Status409Conflict);

        public static readonly Error DescriptionEmpty = 
           new ("Roadmap.DescriptionEmpty", "Roadmap Description can't be empty.", StatusCodes.Status400BadRequest);

        public static readonly Error ImageUrlEmpty = 
           new ("Roadmap.ImageUrlEmpty", "Roadmap ImageUrl can't be empty.", StatusCodes.Status400BadRequest);

    }  

}
