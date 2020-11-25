namespace API.DTOs
{
    public class UserDto
    {
        public string username { get; set; }
        public string Token { get; set; }

        public string PhotoUrl { get; set; }

        public string KnownAs { get; set; }
        public string Area  { get; set; }
    }
}