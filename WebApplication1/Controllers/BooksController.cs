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
        private void TempDataBase(ref List<SeeLibrary> Books)
        {

        }
        //my data!
        public const string connectionString =
                @"Server=localhost\SQLEXPRESS;Database=LibraryAPI;Trusted_Connection=True;";

        [HttpGet]//View Entire Library 
        public IHttpActionResult ViewLibrary()
        {
            var rv = new List<SeeLibrary>();
            TempDataBase(ref Books) = rv;
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
        public IHttpActionResult AddaBook([FromBody] AddBook book)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                var query = $@"INSERT INTO [dbo].[Books]
                               ([Title]
                               ,[Author]
                               ,[IsCheckedOut])                        
                              VALUES (@Title, @Author, @IsCheckedOut)";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Title", AddBook.Title);
                cmd.Parameters.AddWithValue("@Author", AddBook.Author);
                cmd.Parameters.AddWithValue("@IsCheckedOut", AddBook.IsCheckedOut);
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
                        cmd.Parameters.AddWithValue("@Id", DeleteBook.Id);
                        int rows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {

            }
        }


        [HttpPut]//Update a books information  
        public IHttpActionResult UpdateaBook([FromBody]int id)
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
                    cmd.Parameters.AddWithValue("@Title", AddBook.Title);
                    cmd.Parameters.AddWithValue("@Author", AddBook.Author);
                    cmd.Parameters.AddWithValue("@IsCheckedOut", AddBook.IsCheckedOut);
                    cmd.Parameters.AddWithValue("@YearPublished", AddBook.YearPublished);
                    cmd.Parameters.AddWithValue("@Genre", AddBook.Genre);
                    cmd.Parameters.AddWithValue("@LastCheckedOutDate", AddBook.LastCheckedOutDate);
                    cmd.Parameters.AddWithValue("@DueBackDate", AddBook.DueBackDate);

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    return Ok(rows);
                }
            }
            catch (SqlException sqlEx)
            {
                sqlEx = MessageBox.Show("there was an issue!");
                return Ok();
            }

        }
    }
}
