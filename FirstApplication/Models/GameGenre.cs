namespace FirstApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GameGenre
    {
        [Key]
        public string GameGenreId { get; set; }

        [Required]
        [StringLength(128)]
        public string GenreId { get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        [Required]
        [StringLength(128)]
        public string GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EditDate { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Game.Name, Genre.Name);
        }



    }
}
