namespace API.Helpers
{
    public class UserParams : PaginationParams
    {

        public string CurrentUsername { get; set; }
        public string Area { get; set; }

        public string OrderBy { get; set; } = "lastActive";
    }
}