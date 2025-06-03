using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kubernets
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
