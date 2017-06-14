namespace FirstApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            Genres = new HashSet<GameGenre>();
        }
        [Key]
        public string GameId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public bool IsMultiplayer { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EditDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameGenre> Genres { get; set; }

        //troubleshooting help - change # string to text
        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
