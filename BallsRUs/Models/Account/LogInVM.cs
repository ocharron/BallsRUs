using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Account
{
    public class LogInVM
    {
        public string? NomUtilisateur { get; set; }

        [DataType(DataType.Password)]
        public string? MotDePasse { get; set; }
    }
}
