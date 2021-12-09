using System.Data;
using System.Data.Common;
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
                System.Console.WriteLine("5. Display all movies in the database");
                System.Console.WriteLine("6: Add a new user to the database");

                choice = Console.ReadLine();
                logger.Info("User choice: {Choice}", choice);
                if (choice == "1")
                {
                    // Search for and display a specific movie
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
                    }
                }
                else if (choice == "2")
                {
                    // Add movie, adds a movie to the movie records table
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
                    logger.Info("Program has added a new movie to the movies table");
                }
                else if (choice == "3")
                {
                    // update movie, used to edit a movie title

                    System.Console.WriteLine("Enter Movie to update: ");
                    var m3 = Console.ReadLine();

                    System.Console.WriteLine("Enter updated Movie: ");
                    var movieUpdate = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        var updateMovie = db.Movies.FirstOrDefault(x => x.Title == m3);
                        System.Console.WriteLine($"Old Title: ({updateMovie.Id}) {updateMovie.Title}");
                        System.Console.WriteLine($"Updated Title: ({updateMovie.Id}) {movieUpdate}");

                        updateMovie.Title = movieUpdate;

                        db.Movies.Update(updateMovie);
                        db.SaveChanges();
                    }
                    logger.Info("Program has updated an existing movie in the movies table");
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
                    logger.Info("Program has deleted a movie from the movies table");
                }
                else if (choice == "5")
                {
                    //Display all records in the Movie table
                    using (var db = new MovieContext())
                    {
                        var m5 = db.Movies.OrderBy(x=>x.Id);
                        foreach (var movie in m5)
                        {
                            System.Console.WriteLine($"Movie: {movie.Id}, Title: {movie.Title}, ReleaseDate: {movie.ReleaseDate}");
                        }
                    }
                    logger.Info("Program has displayed all movies in movies table");
                }
                else if (choice == "6")
                {
                    // Add a new user to the database
                    System.Console.WriteLine("Enter user Age: ");
                    var a = Convert.ToInt32(Console.ReadLine());
                    System.Console.WriteLine("Enter user Gender: ");
                    var g = Console.ReadLine();
                    System.Console.WriteLine("Enter user ZipCode: ");
                    var z = Console.ReadLine();
                    System.Console.WriteLine("Enter User Occupation: ");
                    var occ = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        var newUser = new User()
                        {
                            Age = a,
                            Gender = g,
                            ZipCode = z
                        };
                        var occupation = new Occupation()
                        {
                            Name = occ
                        };
                        db.Users.Add(newUser);
                        db.SaveChanges();
                        db.Occupations.Add(occupation);
                        db.SaveChanges();


                        var newUsers = db.Users.Where(x=> x.Id == 1);
                         
                        var newOcc = db.Occupations.FirstOrDefault(x => x.Name == occ);

                        foreach (var user in newUsers)
                        {
                            System.Console.WriteLine($"(Id: {newUser.Id}) Age: {newUser.Age} Gender: {newUser.Gender} Occupation: {occupation.Name}");
                        }
                    }
                    logger.Info("Program has added a new user, with occupation");
                }
            } while (choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5" || choice == "6");

            logger.Info("Program ended");


            Console.WriteLine("\nThanks for using the Movie Library!");
        }
    }
}