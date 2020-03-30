using System;
using Xunit;
using HighscoreAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HighscoreAPI.Model;
using System.Linq;

namespace HighscoreAPI.Tests
{
	public class HighscoreTests
	{

		DataContext _context;
		HighscoresController _controller;


		public HighscoreTests()
		{
		
			var options = new DbContextOptionsBuilder<DataContext>().
						  UseSqlServer("Server=KATRIN-BOOK;Database=Highscore;Integrated Security=SSPI;")
						  .Options;
			_context = new DataContext(options);
			_controller = new HighscoresController(_context);

		}

		// test if list is not empty after adding a highscore
		[Fact]
		public async void AddHighscore()
		{
			Highscore highscore = new Highscore();
			highscore.User = "K.H.";
			highscore.Score = 120;

			await _controller.PostHighscore(highscore);
			Assert.NotEmpty(_context.HighScores);

			_context.HighScores.RemoveRange(_controller.GetHighscores());
			await _context.SaveChangesAsync();
		}


		// test if the list has 10 entries after adding 15 scores
		[Fact]
		public async void AddMoreThanTenScores()
		{
			Highscore highscore;
			for (var i = 1; i < 15; i++)
			{
				highscore = new Highscore();
				highscore.User = "player " + i;
				highscore.Score = 100 + i;				
				await _controller.PostHighscore(highscore);
			}
			
			Assert.Equal(10, _controller.GetHighscores().Count());

			_context.HighScores.RemoveRange(_controller.GetHighscores());
			await _context.SaveChangesAsync();
		}

		// check if new highscore is added correctly after 15 scores inserted before
		[Fact]
		public async void AddScoresThenNewHighscore()
		{
			Highscore highscore;
			for (var i = 1; i < 15; i++)
			{
			    highscore = new Highscore();
				highscore.User = "player " + i;
				highscore.Score = 100 + i;
				await _controller.PostHighscore(highscore);
			}
			highscore = new Highscore();
			highscore.Score = 10000;
			highscore.User = "K.H.";
			await _controller.PostHighscore(highscore);
			Assert.Equal(highscore, _controller.GetHighscores().ElementAt(0));

			_context.HighScores.RemoveRange(_controller.GetHighscores());
			await _context.SaveChangesAsync();

		}

		//check if highscores are ordered after inserting them
		[Fact]		
		public async void getOrderedList()
		{
			Highscore highscore1 = new Highscore();
			highscore1.Score = 10;
			highscore1.User = "B.C.";
			await _controller.PostHighscore(highscore1);

			Highscore highscore2 = new Highscore();
			highscore2.Score = 200;
			highscore2.User = "K.H.";
			await _controller.PostHighscore(highscore2);

			Highscore highscore3 = new Highscore();
			highscore3.Score = 5;
			highscore3.User = "Y.Z";
			await _controller.PostHighscore(highscore3);

			Assert.Equal(highscore2, _controller.GetHighscores().ElementAt(0));

			_context.HighScores.RemoveRange(_controller.GetHighscores());
			await _context.SaveChangesAsync();

		}


		// get empty list
		[Fact]
		public async void getEmptyList()
		{
			_context.HighScores.RemoveRange(_controller.GetHighscores());
			await _context.SaveChangesAsync();
			Assert.Equal(0, _context.HighScores.Count());
		}
	}
}
