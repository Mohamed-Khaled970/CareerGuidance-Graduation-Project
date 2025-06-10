namespace CareerGuidance.Api.DTO
{
    public class roadmapCategoryDto
    {
        private string _roadmapCategory = string.Empty;

        public string roadmapCategory
        {
            get { return _roadmapCategory; }  set { _roadmapCategory = value?.Trim() ?? string.Empty; }
        }

        
    }
}
