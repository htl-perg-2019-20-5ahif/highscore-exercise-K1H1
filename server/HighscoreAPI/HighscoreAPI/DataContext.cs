﻿using HighscoreAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighscoreAPI
{
	public class DataContext:DbContext
	{
		public DbSet<Highscore> HighScores { get; set; }

		public DataContext(DbContextOptions<DataContext> options)
		: base(options)
		{ }
	}
}
