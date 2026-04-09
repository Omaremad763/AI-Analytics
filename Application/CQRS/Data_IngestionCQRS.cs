using Application.Contracts.Data_Ingestion;
using Application.DTOS;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Application.CQRS;

public record UploadFileCommand(IFormFile File) : IRequest<Guid>;
public record GetUploadStatusQuery(Guid BatchId) : IRequest<UploadStatusDto?>;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("File is required");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(10 * 1024 * 1024)
            .WithMessage("Max size is 10MB")
            .When(x => x.File != null);   

        RuleFor(x => x.File.FileName)
            .Must(fileName => fileName.EndsWith(".xlsx") || fileName.EndsWith(".csv"))
            .WithMessage("Only Excel or CSV files are allowed")
            .When(x => x.File != null);
    }
}

public class GetUploadStatusQueryValidator : AbstractValidator<GetUploadStatusQuery>
{
    public GetUploadStatusQueryValidator()
    {
        RuleFor(x => x.BatchId).NotEmpty();
    }
}

public class UploadFileCommandHandler(IData_InegstionService service)
    : IRequestHandler<UploadFileCommand, Guid>,
    IRequestHandler<GetUploadStatusQuery, UploadStatusDto>
{
    public async Task<Guid> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        return await service.SaveFileAsync(request.File, cancellationToken);
    }

    public async Task<UploadStatusDto?> Handle(GetUploadStatusQuery request, CancellationToken cancellationToken)
    {
        return await service.GetBatchStatusAsync(request.BatchId);
    }
}