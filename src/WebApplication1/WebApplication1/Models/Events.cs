using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Events
    {
        public int Id { get; set; }
        public string ImageEvent { get; set; }
        public string NameEvent { get; set; }
        public string DescriptionEvent { get; set; }
        public string DateEvent { get; set; }
        public string SignUpEvent { get; set; }
        public Events()
        {

        }
    }
}
