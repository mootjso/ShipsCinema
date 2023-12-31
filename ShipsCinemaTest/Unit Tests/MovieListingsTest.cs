namespace MovieListingsTest
{
    [TestClass]
    public class MovieListingsTest
    {
        private List<Movie> _moviesCopy = new();
        private string _fileName = MovieHandler.FileName;

        [TestInitialize]
        public void TestInitialize()
        {
            _moviesCopy = JSONMethods.ReadJSON<Movie>(_fileName).ToList();
            JSONMethods.WriteToJSON(new List<Movie>(), _fileName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            JSONMethods.WriteToJSON(_moviesCopy, _fileName);
        }

        [TestMethod]
        public void TestReadMoviesFromJSON()
        {
            List<Movie> movieToWrite = new()
        {
            new Movie(0, "Movie1", "en", "First Movie", new List<string> { "Adventure", "Horror" }, 105, 18)
        };
            JSONMethods.WriteToJSON(movieToWrite, _fileName);
            List<Movie> readMovie = JSONMethods.ReadJSON<Movie>(_fileName).ToList();
            foreach (var _movie in readMovie)
                Console.WriteLine(_movie.Id);
            Movie movie = readMovie[0];
            Assert.AreEqual(1, movie.Id);
            Assert.AreEqual("Movie1", movie.Title);
            Assert.AreEqual("en", movie.Language);
            Assert.AreEqual("First Movie", movie.Description);
            CollectionAssert.AreEqual(new List<string> { "Adventure", "Horror" }, movie.Genres);
            Assert.AreEqual(105, movie.Runtime);
            Assert.AreEqual(18, movie.AgeRating);
        }

        [TestMethod]
        public void TestGetMovieById()
        {
            List<Movie> movieToWrite = new()
        {
            new Movie(0, "Movie1", "en", "First Movie", new List<string> { "Adventure", "Horror" }, 105, 18),
            new Movie (1, "Movie2", "nl", "Second Movie", new List<string> { "Thriller", "Comedy" }, 120, 16)
        };
            JSONMethods.WriteToJSON(movieToWrite, _fileName);

            Movie? movie = MovieHandler.GetMovieById(2);
            Movie? movieNull = MovieHandler.GetMovieById(3);
            
            Assert.IsNull(movieNull);
            Assert.IsNotNull(movie);
            Assert.AreEqual(2, movie.Id);
            Assert.AreEqual("Movie2", movie.Title);
            Assert.AreEqual("nl", movie.Language);
            Assert.AreEqual("Second Movie", movie.Description);
            CollectionAssert.AreEqual(new List<string> { "Thriller", "Comedy" }, movie.Genres);
            Assert.AreEqual(120, movie.Runtime);
            Assert.AreEqual(16, movie.AgeRating);
        }

        [TestMethod]
        public void TestGetMovieTitles()
        {
            Movie movie1 = new(0, "Movie1", "en", "First Movie", new List<string> { "Adventure" }, 105, 18);
            string movieString1 = $"{"Movie1",-30} | {"English",-10} | {"Adventure" ,-30} | {"105" + " min",-8} | 18+";

            List<Movie> movieToWrite = new()
            {
                movie1,
                new Movie(1, "Movie 2", "en", "kaas", new List<string> { "Family" }, 120, 12)
            };
            JSONMethods.WriteToJSON(movieToWrite, _fileName);
            List<Movie> MoviesFromJSON = JSONMethods.ReadJSON<Movie>(_fileName).ToList();

            List<string> movieTitles = MovieHandler.GetMovieTitles(MoviesFromJSON);
            Assert.AreEqual(movieString1, movieTitles[0]);
        }
    }
}
