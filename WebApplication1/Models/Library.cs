using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace WebApplication1.Models
{
    abstract public class Library
    {
        public static int Id { get; set; }
        public static string Title { get; set; }
        public static string Author { get; set; }
        public static int YearPublished { get; set; }
        public string Genre { get; set; }
        public static bool IsCheckedOut { get; set; }
        public DateTime? LastCheckedOutDate { get; set; }
        public DateTime? DueBackDate { get; set; }
        public string NeatDueBackDate
        {
            get
            {
                if (DueBackDate.HasValue)
                {
                    return ((DateTime)this.DueBackDate).ToShortDateString();

                }
                else
                {
                    return null;
                }
            }
        }
    }
}