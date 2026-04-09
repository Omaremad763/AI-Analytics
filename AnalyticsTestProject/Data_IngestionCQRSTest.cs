namespace AnalyticsTestProject;

using Application.Contracts.Data_Ingestion;
using Application.CQRS;
using Application.DTOS;

using FluentAssertions;

using FluentValidation.TestHelper;

using Microsoft.AspNetCore.Http;

using Moq;

using Xunit;


public class Data_IngestionCQRSTest
{
    private readonly UploadFileCommandValidator _validator;
    private readonly Mock<IDataInegstionService> _serviceMock;
    private readonly UploadFileCommandHandler _handler;

    public Data_IngestionCQRSTest()
    {
        _validator = new UploadFileCommandValidator();
        _serviceMock = new Mock<IDataInegstionService>();
        _handler = new Application.CQRS.UploadFileCommandHandler(_serviceMock.Object);
    }

    #region Validator Tests

    [Fact]
    public void Should_Have_Error_When_File_Is_Null()
    {
        var command = new UploadFileCommand(null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.File);
    }

    [Fact]
    public void Should_Have_Error_When_File_Is_Too_Large()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(11 * 1024 * 1024);  
        fileMock.Setup(f => f.FileName).Returns("test.csv");

        var command = new UploadFileCommand(fileMock.Object);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.File.Length);
    }

    [Theory]
    [InlineData("test.pdf")]
    [InlineData("test.png")]
    public void Should_Have_Error_When_Format_Is_Invalid(string fileName)
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(1024);

        var command = new UploadFileCommand(fileMock.Object);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.File.FileName);
    }

    #endregion

    #region Handler Tests

    [Fact]
    public async Task Handle_UploadCommand_ShouldInvokeServiceAndReturnGuid()
    {
        var fileMock = new Mock<IFormFile>();
        var command = new UploadFileCommand(fileMock.Object);
        var expectedGuid = Guid.NewGuid();

        _serviceMock.Setup(s => s.SaveFileAsync(It.IsAny<IFormFile>(), 
            It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedGuid);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(expectedGuid);
        _serviceMock.Verify(s => s.SaveFileAsync(command.File, 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetStatusQuery_ShouldReturnDto_WhenBatchExists()
    {
        var batchId = Guid.NewGuid();
        var query = new GetUploadStatusQuery(batchId);
        var expectedDto = new UploadStatusDto
        {  Status = "uploaded" };

        _serviceMock.Setup(s => s.GetBatchStatusAsync(batchId))
                    .ReturnsAsync(expectedDto);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Status.Should().Be("uploaded");
        _serviceMock.Verify(s => s.GetBatchStatusAsync(batchId), Times.Once);
    }

    #endregion
}