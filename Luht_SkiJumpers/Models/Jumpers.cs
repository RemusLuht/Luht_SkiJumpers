using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Luht_SkiJumpers.Models
{
    public class Jumpers
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Standings { get; set; }
        public int Distance { get; set; }
        [DefaultValue(false)]
        public bool Finished { get; set; } = false;
        [DefaultValue(false)]
        public bool Started { get; set; } = false;
    }
}
