using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DeleteBook:Library
    {
        public DeleteBook()
        {

        }
        public DeleteBook(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Title = reader["Title"].ToString();
            Author = reader["Author"].ToString();
            //YearPublished = (int)reader["YearPublished"];
            //Genre = reader["Genre"].ToString();
            //IsCheckedOut = (bool)reader["IsCheckedOut"];
            //LastCheckedOutDate = reader["LastCheckedOutDate"] as DateTime?;
            //DueBackDate = reader["DueBackDate"] as DateTime?;
        }
    }
}