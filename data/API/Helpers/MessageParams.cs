namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string username { get; set; }
        public string Container { get; set; } = "Unread";
        
        
    }
}