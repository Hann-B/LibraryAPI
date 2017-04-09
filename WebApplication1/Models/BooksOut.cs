using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplication1.Models
{
    public class BooksOut : Library
    {
        public BooksOut()
        {
        }
        public BooksOut(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Title = reader["Title"].ToString();
            Author = reader["Author"].ToString();
            Genre = reader["Genre"].ToString();
            IsCheckedOut = (bool)reader["IsCheckedOut"];
            DueBackDate = reader["DueBackDate"] as DateTime?;
        }
    }
}