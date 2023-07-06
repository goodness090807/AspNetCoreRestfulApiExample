using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        public int recordsPerPage { get; set; } = 10;

        private readonly int maxRecordsPerPage = 50;

        public int RecordsPegPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }
    }
}
