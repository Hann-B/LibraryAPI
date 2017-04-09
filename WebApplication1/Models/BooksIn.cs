using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class BooksIn : Library
    {
        public BooksIn()
        {
        }

        public BooksIn(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Title = reader["Title"].ToString();
            Author = reader["Author"].ToString();
            Genre = reader["Genre"].ToString();
            IsCheckedOut = (bool)reader["IsCheckedOut"];
            LastCheckedOutDate = reader["LastCheckedOutDate"] as DateTime?;
        }

    }
}