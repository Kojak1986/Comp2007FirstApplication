namespace FirstApplication.Migrations.DataContext
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FirstApplication.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations.DataContext";
        }

        protected override void Seed(FirstApplication.Models.DataContext context)
        {
            List<string> genres = new List<string>();
            genres.Add("Multiplayer");
            genres.Add("First Shooter");
            genres.Add("Driving");
            genres.Add("Logical");

            foreach(var genre in genres)
            {
                Genre newgrenre = new Genre();
                newgrenre.Name = genre;

                Genre checkGenre = context.Genres.SingleOrDefault(x => x.Name == newgrenre.Name);
                if (checkGenre == null)
                {
                    context.Genres.Add(newgrenre);
                }
            }

            context.SaveChanges();
        }
    }
}
