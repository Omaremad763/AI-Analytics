using Application.DTOS;

namespace Application.Contracts
{
    public interface IExcelParserService
    {
        List<FinancialRecordDto> ParseFinancialFile(Stream fileStream);
    }
}