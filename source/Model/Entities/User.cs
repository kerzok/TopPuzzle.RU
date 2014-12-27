namespace Toppuzzle.Model.Entities {
    public class User : IUser {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}