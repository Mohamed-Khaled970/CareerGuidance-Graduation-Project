namespace CareerGuidance.Api.DTO
{


    /*
     * File Name: dataDto.cs
     * Author Information: Abdelrahman Rezk
     * Date of creation: 2024-10-13
     * Versions Information: v1.0
     * Dependencies: None
     * Contributors: Abdelrahman Rezk
     * Last Modified Date: 2024-10-27
     *
     * Description:
     *      filter For Roadmaps 
     */


    public class roadmapDataDto
    {
        public string roadmapName { get; set; } = string.Empty;
        public string roadmapDescription { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public string roadmapCategory { get; set; } = string.Empty; 
        public List<nodeDataDto> Nodes { get; set; } = new List<nodeDataDto>();
        public List<edgeDataDto> Edges { get; set; } = new List<edgeDataDto>();

    }
    
    public class nodeDataDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public positionDto Position { get; set; } = new positionDto();
        public nodeDetailsDto Data { get; set; } = new nodeDetailsDto();
        //public styleDto Style { get; set; } = new styleDto();
        //public measuredDto Measured { get; set; } = new measuredDto();
       
        //public bool Selected { get; set; }
        //public bool Dragging { get; set; }
    }

    public class positionDto  //Important// => Store Data In Model For Position
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class nodeDetailsDto // Send Data To nodeDataDto In Variable Data And Store In database
    {
       // public string Label { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<linkDataDto> Links { get; set; } = new List<linkDataDto>();
    }

    public class linkDataDto  //Important// => Store Data In Model For Link
    {
        public string Type { get; set; } = string.Empty;
        public string EnOrAr { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        [Url]
        public string Url { get; set; } = string.Empty;
    }

    //public class styleDto
    //{
    //    public string BackgroundColor { get; set; } = string.Empty;
    //    public string Color { get; set; } = string.Empty;
    //    public string FontSize { get; set; } = string.Empty;
    //    public string FontWeight { get; set; } = string.Empty;
    //    public string Width { get; set; } = string.Empty;
    //    public string Height { get; set; } = string.Empty;
    //    public string BorderRadius { get; set; } = string.Empty;
    //}

    //public class measuredDto
    //{
    //    public double Width { get; set; }
    //    public double Height { get; set; }
    //}

    public class edgeDataDto //Important// => Store Data In Model For Edges
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}
