using CsvHelper.Configuration;

namespace ConvertToDb.Models
{

    public abstract class Media {
        string Id {get;set;}

        public void Display() {
            // concrete implementation
        }
    }

    public class Movie : Media
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genres { get; set; }

    }

    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.MovieId).Index(0).Name("movieId");
            Map(m => m.Title).Index(1).Name("title");
            Map(m => m.Genres).Index(2).Name("genres");
        }
    }
}