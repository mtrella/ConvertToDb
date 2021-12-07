using System.IO.MemoryMappedFiles;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices.ComTypes;
using System.Dynamic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConvertToDb.Context;
using ConvertToDb.DataModels;
using System.Collections.Generic;
using NLog;

namespace ConvertToDb
{
    internal class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static void Main(string[] args)
        {

            string choice = "";

            do
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Welcome to the Movie Library");
                System.Console.WriteLine("Please select an option: ");
                System.Console.WriteLine("1. Search for a movie");
                System.Console.WriteLine("2. Add a movie to the database");
                System.Console.WriteLine("3. Update an existing movie in the database");
                System.Console.WriteLine("4. Delete a movie from the database");

                choice = Console.ReadLine();
                logger.Info("User choice: {Choice}", choice);
                if (choice == "1")
                {
                    // Search for movie
                    Console.WriteLine("Enter movie title: ");
                    var searchString = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        
                        var movie = from m in db.Movies
                            select m;
                        if(!String.IsNullOrEmpty(searchString))
                        {
                            movie = movie.Where(x => x.Title.Contains(searchString));
                            movie.ToList().ForEach(x=>x.Display());
                        }
                        return;
                        
                    }
                }
                else if (choice == "2")
                {
                    // Add movie
                    System.Console.WriteLine("Enter new Movie: ");
                    var m2 = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        var movie = new Movie() 
                        {
                            Title = m2
                        };
                        db.Movies.Add(movie);
                        db.SaveChanges();

                        var newMovie = db.Movies.FirstOrDefault(x => x.Title == m2);
                        System.Console.WriteLine($"({newMovie.Id}) {newMovie.Title}");
                    }
                }
                else if (choice == "3")
                {
                    // update movie

                    System.Console.WriteLine("Enter Movie to update: ");
                    var m3 = Console.ReadLine();

                    System.Console.WriteLine("Enter updated Movie: ");
                    var movieUpdate = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        var updateMovie = db.Movies.FirstOrDefault(x => x.Title == m3);
                        System.Console.WriteLine($"({updateMovie.Id}) {updateMovie.Title}");

                        updateMovie.Title = movieUpdate;

                        db.Movies.Update(updateMovie);
                        db.SaveChanges();
                    }
                }
                else if (choice == "4")
                {
                    // Delete movie
                    System.Console.WriteLine("Enter Movie to delete: ");
                    var m4 = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        var deleteMovie = db.Movies.FirstOrDefault(x => x.Title == m4);
                        System.Console.WriteLine($"({deleteMovie.Id}) {deleteMovie.Title}");

                        db.Movies.Remove(deleteMovie);
                        db.SaveChanges();
                    }
                }
            } while (choice == "1" || choice == "2" || choice == "3" || choice == "4");

            logger.Info("Program ended");


            Console.WriteLine("\nThanks for using the Movie Library!");
        }
    }
}