using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorIntro.Client.HttpRepository;
using RichardSzalay.MockHttp;

namespace BlazorIntro.Client.Test
{
    internal class HttpUserMoviesRespositoryTest
    {
        [Test]
        public async Task Test_GetMovies_ReturnUserAndMovies()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            string testUserResponse = """
                {"id":"0353399b-eb68-4961-acef-ccde4d8c6645","userName":"james.couch@example.com","firstName":"James","lastName":"Couch","favoriteMovies":[{"id":1,"imdbId":"tt1877830"},{"id":2,"imdbId":"tt0816692"}]}
                """;
            string testOMDBApiBatmanResponse = """
                {"Title":"The Batman","Year":"2022","Rated":"PG-13","Released":"04 Mar 2022","Runtime":"176 min","Genre":"Action, Crime, Drama","Director":"Matt Reeves","Writer":"Matt Reeves, Peter Craig, Bob Kane","Actors":"Robert Pattinson, Zoë Kravitz, Jeffrey Wright","Plot":"When a sadistic serial killer begins murdering key political figures in Gotham, Batman is forced to investigate the city's hidden corruption and question his family's involvement.","Language":"English, Spanish, Latin, Italian","Country":"United States","Awards":"Nominated for 3 Oscars. 23 wins & 150 nominations total","Poster":"https://m.media-amazon.com/images/M/MV5BMDdmMTBiNTYtMDIzNi00NGVlLWIzMDYtZTk3MTQ3NGQxZGEwXkEyXkFqcGdeQXVyMzMwOTU5MDk@._V1_SX300.jpg","Ratings":[{"Source":"Internet Movie Database","Value":"7.8/10"},{"Source":"Rotten Tomatoes","Value":"85%"},{"Source":"Metacritic","Value":"72/100"}],"Metascore":"72","imdbRating":"7.8","imdbVotes":"674,068","imdbID":"tt1877830","Type":"movie","DVD":"19 Apr 2022","BoxOffice":"$369,345,583","Production":"N/A","Website":"N/A","Response":"True"}
                """;
            string testOMDBApiInterstellarResponse = """
                {"Title":"Interstellar","Year":"2014","Rated":"PG-13","Released":"07 Nov 2014","Runtime":"169 min","Genre":"Adventure, Drama, Sci-Fi","Director":"Christopher Nolan","Writer":"Jonathan Nolan, Christopher Nolan","Actors":"Matthew McConaughey, Anne Hathaway, Jessica Chastain","Plot":"A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.","Language":"English","Country":"United States, United Kingdom, Canada","Awards":"Won 1 Oscar. 44 wins & 148 nominations total","Poster":"https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg","Ratings":[{"Source":"Internet Movie Database","Value":"8.6/10"},{"Source":"Rotten Tomatoes","Value":"72%"},{"Source":"Metacritic","Value":"74/100"}],"Metascore":"74","imdbRating":"8.6","imdbVotes":"1,866,991","imdbID":"tt0816692","Type":"movie","DVD":"31 Mar 2015","BoxOffice":"$188,020,017","Production":"N/A","Website":"N/A","Response":"True"}
                """;
            
            mockHttp.When("https://localhost:7113/api/get-movies")
                .Respond("application/json", testUserResponse);
            mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt1877830")
                .Respond("application/json", testOMDBApiBatmanResponse);
            mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0816692")
                .Respond("application/json", testOMDBApiInterstellarResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://localhost:7113/");
            var userMoviesHttpRepository = new UserMoviesHttpRepository(client);
            // Act
            var response = await userMoviesHttpRepository.GetMovies();
            var movies = response.Data;

            // Assert
            Assert.That(movies.Count(), Is.EqualTo(2));
            Assert.That(movies[0].Title, Is.EqualTo("The Batman"));
            Assert.That(movies[0].Year, Is.EqualTo("2022"));
            Assert.That(movies[1].Title, Is.EqualTo("Interstellar"));
            Assert.That(movies[1].Year, Is.EqualTo("2014"));
        }
    }
}
