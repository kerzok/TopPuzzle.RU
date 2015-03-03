namespace Toppuzzle.Model.Entities {
    public class User {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public bool HasAvatar { get; set; }
        public string Avatar { get; set; }
        public int Rating { get; set; }
    }
}