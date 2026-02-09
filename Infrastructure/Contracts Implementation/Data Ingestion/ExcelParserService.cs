using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.DTOS;

using MiniExcelLibs;

namespace Infrastructure.Contracts_Implementation;
    public class ExcelParserService:IExcelParserService
    {
        public List<FinancialRecordDto> ParseFinancialFile(Stream fileStream)
        {
            List<FinancialRecordDto>? rows = fileStream.Query<FinancialRecordDto>().ToList();
            return rows;
        }
 }
