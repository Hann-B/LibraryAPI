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
    public class BookCheckController : ApiController
    {
        //my data!
        public const string connectionString =
                @"Server=localhost\SQLEXPRESS;Database=LibraryAPI;Trusted_Connection=True;";
       



        [HttpPost]
        public IHttpActionResult CheckBookOut(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // see if that book is checked out (GET FORM THE DATABASE)
                var select = "SELECT * FROM Books WHERE Id = @Id";
                var selectCmd = new SqlCommand(select, connection);
                selectCmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                var reader = selectCmd.ExecuteReader();
                var book = new SeeLibrary();
                while (reader.Read())
                {
                    book = new SeeLibrary(reader);
                }
                connection.Close();
                if (book.IsCheckedOut)
                {
                    return Ok(new { Message = "The book is already checked out", DueBackDate = book.NeatDueBackDate });
                }
                else
                {
                    var query =  @"UPDATE[dbo].[Books]
                             SET
                             [IsCheckedOut]= @IsCheckedOut
                             ,[DueBackDate]= @DueBackDate
                               WHERE Id = @Id";
                    var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IsCheckedOut", !book.IsCheckedOut);
                    cmd.Parameters.AddWithValue("@DueBackDate", DateTime.Now.AddDays(10));
                    cmd.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return Ok(new { Message = "You have Checked out a Book! It's due back on", DueBackDate = book.NeatDueBackDate });
                }
            }
        }
        [HttpPut]
        public IHttpActionResult CheckBookIn(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // see if that book is checked out (GET FORM THE DATABASE)
                var select = "SELECT * FROM Books WHERE Id = @Id";
                var selectCmd = new SqlCommand(select, connection);
                selectCmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                var reader = selectCmd.ExecuteReader();
                var book = new SeeLibrary();
                while (reader.Read())
                {
                    book = new SeeLibrary(reader);
                }
                connection.Close();
                if (!book.IsCheckedOut)
                {
                    return Ok(new { Message = "The book is already checked in", LastCheckedOutDate = book.LastCheckedOutDate });
                }
                else
                {
                    var query = $@"UPDATE[dbo].[Books]
                             SET
                             [IsCheckedOut]= @IsCheckedOut
                               WHERE Id = @Id";
                    var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IsCheckedOut", !book.IsCheckedOut);
                    cmd.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return Ok(new { Message = "Thanks for bringing our book back! Have a giddy day!"});
                }
            }
        }
    }
}
