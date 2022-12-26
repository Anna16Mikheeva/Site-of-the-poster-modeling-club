using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModel
{
    public class EventsViewModel
    {
        public IFormFile ImageEvent { get; set; }
        public string NameEvent { get; set; }
        public string DescriptionEvent { get; set; }
        public string DateEvent { get; set; }
        public string SignUpEvent { get; set; }
    }
}
