using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BooksController : ApiController
    {
        //Where is my data!
        public const string connectionString =
                @"Server=localhost\SQLEXPRESS;Database=LibraryAPI;Trusted_Connection=True;";

        [HttpGet]
        public IHttpActionResult ViewLibrary()
        {
            var rv = new List<SeeLibrary>();
            using (var connection = new SqlConnection(connectionString))
            {
                var sqlCommand = new SqlCommand
                    (@"SELECT *
                    FROM [dbo].[Books]
                    ", connection);
                connection.Open();
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var book = new SeeLibrary(reader);
                    rv.Add(book);
                }
                connection.Close();
                return Ok(rv);
            }
        }
        [HttpGet]
        public IHttpActionResult BooksCheckedOut(bool IsCheckedOut)
        {
            if (IsCheckedOut == true)
            {
                var rv = new List<BooksOut>();
                using (var connection = new SqlConnection(connectionString))
                {
                    var sqlCommand = new SqlCommand
                        (@"SELECT
                        [Id]
                      ,[Title]
                      ,[Author]
                      ,[Genre]
                      ,[IsCheckedOut]
                      ,DATEADD (day,10, LastCheckedOutDate) AS [DueBackDate]
                        FROM [dbo].[Books]
                       WHERE IsCheckedOut=1", connection);
                    connection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var book = new BooksOut(reader);
                        rv.Add(book);
                    }
                    connection.Close();
                    return Ok(rv);
                }
            }
            else
            {
                var rv = new List<BooksIn>();
                using (var connection = new SqlConnection(connectionString))
                {
                    var sqlCommand = new SqlCommand
                        (@"SELECT
                        [Id]
                      ,[Title]
                      ,[Author]
                      ,[Genre]
                      ,[LastCheckedOutDate]
                      ,[IsCheckedOut]
                        FROM [dbo].[Books]
                       WHERE IsCheckedOut=0", connection);
                    connection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var book = new BooksIn(reader);
                        rv.Add(book);
                    }
                    connection.Close();
                    return Ok(rv);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult AddaBook(string title, string author, bool isCheckedOut = false)
        {
            var newBook = new AddBook()
            {
                Title = title,
                Author = author,
            };
            using (var connection = new SqlConnection(connectionString))
            {
                var query = $@"INSERT INTO [dbo].[Books]
                               ([Title]
                               ,[Author]
                               ,[IsCheckedOut])                        
                              VALUES ('{title}', '{author}', '{isCheckedOut}')";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return Ok(newBook);
        }

        [HttpDelete]
        public IHttpActionResult DeleteaBook(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = $@"DELETE FROM[dbo].[Books]
                            WHERE Id = '{id}'";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return Ok($"Book with Id '{id}' has been deleted from the library");
            }
        }

        //[HttpPut]
        //public IHttpActionResult UpdateaBook()
        //{

        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        var query = $@"UPDATE[dbo].[Books]
        //                     SET[Title] = < Title, nvarchar(50),>
        //                     ,[Author] = <Author, nvarchar(50),>
        //                     ,[YearPublished] = <YearPublished, int,>
        //                      ,[Genre] = <Genre, nvarchar(50),>
        //                      ,[IsCheckedOut] = <IsCheckedOut, bit,>
        //                      ,[LastCheckedOutDate] = <LastCheckedOutDate, datetime,>
        //                      ,[DueBackDate] = <DueBackDate, datetime,>
        //                       WHERE Id =";
        //        var cmd = new SqlCommand(query, connection);
        //        connection.Open();
        //        cmd.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //    return Ok();
        //}
    }
}
