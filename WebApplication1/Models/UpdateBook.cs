using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UpdateBook:Library
    {
        public UpdateBook()
        {
            
        }
        public UpdateBook(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Title = reader["Title"].ToString();
            Author = reader["Author"].ToString();
            //YearPublished = (int)reader["YearPublished"];
            Genre = reader["Genre"].ToString();
            IsCheckedOut = (bool)reader["IsCheckedOut"];
            LastCheckedOutDate = reader["LastCheckedOutDate"] as DateTime?;
            DueBackDate = reader["DueBackDate"] as DateTime?;
        }
    }
}