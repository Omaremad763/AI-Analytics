using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.views;
public class ViewFinancialSummary
{
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public long TransactionCount { get; set; }
}
