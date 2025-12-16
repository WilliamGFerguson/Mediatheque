using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatheque_DAL.DTO
{
    public class MediaItemSimple
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public MediaItemSimple(string title, int year)
        {
            Title = title;
            Year = year;
        }
    }
}
