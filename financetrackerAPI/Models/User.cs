using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class User
    {

        [Key]
        public int userID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime creationTime { get; set; }
    }
}
