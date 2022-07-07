using System.Collections.Generic;

namespace SmellFinder.Models
{
    public class SmellResponse
    {
        public string Description { get; set; }
        public string Message { get; set; }
        public List<string> LinesAffected { get; set; }

        public SmellResponse()
        {

        }

        public bool Contains(string position) => LinesAffected.Contains(position);
    }
}
