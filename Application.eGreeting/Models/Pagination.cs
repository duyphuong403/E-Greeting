using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.eGreeting.Models
{
    public class Pagination
    {
        public int currentPage { get; set; } = 1;
        public int pageSize { get; set; } = 2;
        public int total { get; set; }
        public int from { get { return (currentPage - 1) * pageSize + 1; } }
        public int to { get { return (currentPage * pageSize); } }
    }

    public class MyResponse<T> where T : class {
        public bool success { get; set; } = false;
        public T data { get; set; }
        public string message { get; set; }
    }
}