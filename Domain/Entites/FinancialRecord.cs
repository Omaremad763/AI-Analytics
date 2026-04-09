using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class FinancialRecord
    {


        public Guid Id { get; set; }
        public Guid DataBatchId { get; set; }

        [ForeignKey("DataBatchId")]
        public DataBatch DataBatch { get; set; }

        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string Vendor { get; set; }
        public FinacialReacordsEnum Type { get; set; }
    }
}
