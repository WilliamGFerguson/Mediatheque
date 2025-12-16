namespace Mediatheque_DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public string Email { get; set; } = "";
        public virtual ICollection<MediaStatus> MediaStatuses { get; set; }
    }
}
