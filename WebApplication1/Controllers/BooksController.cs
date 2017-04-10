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
        //my data!
        public const string connectionString =
                @"Server=localhost\SQLEXPRESS;Database=LibraryAPI;Trusted_Connection=True;";
        public List<SeeLibrary> ListofBooks()
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
                return rv;
            }
        }

        [HttpGet]//View Entire Library 
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
        [HttpGet]//Look for a book: checked in or checked out?
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

        [HttpPut]//Add a book to the library
        public IHttpActionResult AddaBook([FromBody] SeeLibrary book)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                var query = $@"INSERT INTO [dbo].[Books]
                               ([Title]
                               ,[Author]
                               ,[IsCheckedOut])                        
                              VALUES (@Title, @Author, @IsCheckedOut)";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@IsCheckedOut", book.IsCheckedOut);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return Ok(book);
        }

        [HttpDelete]//delete a book from the library
        public IHttpActionResult DeleteaBook(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd =
                        new SqlCommand("DELETE FROM Books" +
                     "WHERE Id=@Id", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        int rows = cmd.ExecuteNonQuery();
                        return Ok(rows);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return Ok();
            }
        }

        [HttpPost]//Update a books information  
        public IHttpActionResult UpdateaBook([FromBody]int id, SeeLibrary ListofBooks)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = $@"UPDATE[dbo].[Books]
                             SET[Title] = @Title
                             ,[Author] = @Author
                             ,[YearPublished] = @YearPublished
                              ,[Genre] = @Genre
                              ,[IsCheckedOut] = @IsCheckedOut
                              ,[LastCheckedOutDate] = @LastCheckedOutDate
                              ,[DueBackDate] = @DueBackDate
                               WHERE Id = @Id";
                    var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Title", ListofBooks.Title);
                    cmd.Parameters.AddWithValue("@Author", ListofBooks.Author);
                    cmd.Parameters.AddWithValue("@IsCheckedOut", ListofBooks.IsCheckedOut);
                    cmd.Parameters.AddWithValue("@YearPublished", ListofBooks.YearPublished);
                    cmd.Parameters.AddWithValue("@Genre", ListofBooks.Genre);
                    cmd.Parameters.AddWithValue("@LastCheckedOutDate", ListofBooks.LastCheckedOutDate);
                    cmd.Parameters.AddWithValue("@DueBackDate", ListofBooks.DueBackDate);
                    cmd.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    return Ok(rows);
                }
            }
            catch (SqlException sqlEx)
            {
                //sqlEx = MessageBox.Show("there was an issue!");
                return Ok();
            }

        }
    }
}
