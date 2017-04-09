using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class SeeLibrary:Library
    {
        public SeeLibrary()
        {
        }

        public SeeLibrary(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Title = reader["Title"].ToString();
            Author = reader["Author"].ToString();
            //YearPublished = (int)reader["YearPublished"];
            Genre = reader["Genre"].ToString();
            IsCheckedOut = (bool)reader["IsCheckedOut"];
            //LastCheckedOutDate = reader["LastCheckedOutDate"] as DateTime?;
            //DueBackDate = reader["DueBackDate"] as DateTime?;

        }
    }
}