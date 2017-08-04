namespace FirstApplication.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Game
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string GameId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Game Name")]
        public string Name { get; set; }

        [Display(Name = "Is Multiplayer")]
        public bool IsMultiplayer { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Genres")]
        [InverseProperty("Game")]
        public virtual ICollection<GameGenre> Genres { get; set; } = new HashSet<GameGenre>();

        [Display(Name = "Ratings")]
        [InverseProperty("Game")]
        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        [NotMapped]
        public decimal OverallRating
        {
            get
            {
                if(Ratings.Count > 0)
                {
                    return (Ratings.Average(x => x.Rank));
                }
                return (9);
            }
        }  
       
        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
