using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Filter
    {
        private string keyword;
        private int limit = 10;
        private string order="desc";
        private int page = 1;
        private string orderBy="id";

        public string Keyword { get => keyword; set => keyword = value; }
        public int Page { get => page; set => page = value; }
        public int Limit { get => limit; set => limit = value; }
        public string OrderBy { get => orderBy; set => orderBy = value; }
        public string Order { get => order; set => order = value; }
    }
}
