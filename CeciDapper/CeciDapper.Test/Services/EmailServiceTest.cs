using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Infra.CrossCutting.Settings;
using CeciDapper.Service.Services;
using CeciDapper.Test.Fakers.Commons;
using CeciDapper.Test.Fakers.Email;
using CeciDapper.Test.Fakers.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciDapper.Test.Services
{
    public class EmailServiceTest
    {
        private readonly Mock<IOptions<EmailSettings>> _emailSettings;
        private readonly Mock<IEmailService> _mockEmailService;

        public EmailServiceTest()
        {
            _emailSettings = new Mock<IOptions<EmailSettings>>();
            _mockEmailService = new Mock<IEmailService>();
        }

        [Fact]
        public async Task Send_email_successfully()
        {
            // Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            var resultResponseFaker = ResultResponseFaker.ResultResponse(HttpStatusCode.OK).Generate();

            _mockEmailService.Setup(service => service.SendEmailAsync(emailRequestDTOFaker))
                .ReturnsAsync(resultResponseFaker);

            // Act
            var result = await _mockEmailService.Object.SendEmailAsync(emailRequestDTOFaker);

            // Assert
            Assert.Equal(resultResponseFaker, result);
            _mockEmailService.Verify(service => service.SendEmailAsync(emailRequestDTOFaker), Times.Once);
        }

        [Fact]
        public async Task Send_email_with_attachment_successfully()
        {
            //Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            _emailSettings.Setup(ap => ap.Value).Returns(EmailSettingsFaker.EmailSettings().Generate());

            var resultResponseFaker = ResultResponseFaker.ResultResponse(HttpStatusCode.OK).Generate();

            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            //add attachment to request
            emailRequestDTOFaker.Attachments = new List<IFormFile>
            {
                file
            };

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(service => service.SendEmailAsync(emailRequestDTOFaker))
                .ReturnsAsync(resultResponseFaker);

            // Act
            var result = await emailServiceMock.Object.SendEmailAsync(emailRequestDTOFaker);

            // Assert
            Assert.Equal(resultResponseFaker, result);
            emailServiceMock.Verify(service => service.SendEmailAsync(emailRequestDTOFaker), Times.Once);
        }

        [Fact]
        public async Task Send_email_exception_invalid_port()
        {
            //Arrange
            var emailSettings = EmailSettingsFaker.EmailSettings().Generate();
            //set invalid port
            emailSettings.Port = 9999999;

            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            _emailSettings.Setup(ap => ap.Value).Returns(emailSettings);

            var emailService = EmailServiceConstrutor();

            //Act
            var result = await emailService.SendEmailAsync(emailRequestDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private EmailService EmailServiceConstrutor()
        {
            return new EmailService(_emailSettings.Object);
        }
    }
}
