using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Picklerick.Models
{
    [Index(nameof(Universe), IsUnique = true)]
    public class Rick
    {
        public int Id {get; set; }
        [Required]
        public string Universe { get; set; } = "";
        public bool IsMortyAlive { get; set; } = true;
    }
}