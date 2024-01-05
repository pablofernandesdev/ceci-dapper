using AutoMapper;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Email;
using CeciDapper.Domain.DTO.Import;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Infra.CrossCutting.Extensions;
using CeciDapper.Infra.CrossCutting.Helper;
using ClosedXML.Excel;
using Hangfire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service responsible for importing users from a file.
    /// </summary>
    public class ImportService : IImportService
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _jobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportService"/> class.
        /// </summary>
        /// <param name="emailService">The email service.</param>
        /// <param name="uow">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="jobClient">The background job client.</param>
        public ImportService(IEmailService emailService,
            IUnitOfWork uow,
            IMapper mapper,
            IBackgroundJobClient jobClient)
        {
            _emailService = emailService;
            _uow = uow;
            _mapper = mapper;
            _jobClient = jobClient;
        }

        /// <summary>
        /// Imports users from a file.
        /// </summary>
        /// <param name="model">The file upload information.</param>
        /// <returns>A response indicating the success of the import operation.</returns>
        public async Task<ResultResponse> ImportUsersAsync(FileUploadDTO model)
        {
            var response = new ResultResponse();

            try
            {
                var filePath = await SaveFileAsync(model);

                if (Path.GetExtension(filePath).Equals(".csv"))
                {
                    filePath = ConvertToExcelAsync(filePath);
                }

                var users = await ReadUsersFromExcelAsync(filePath);

                await _uow.User.AddRangeAsync(_mapper.Map<IEnumerable<User>>(users));

                await SendEmailsToUsersAsync(users);

                DeleteFile(filePath);

                response.Message = "Users imported successfully.";
            }
            catch (Exception ex)
            {
                response.Message = "Unable to import users.";
                response.Exception = ex;
            }

            return response;
        }

        // Helper method to save the file to disk
        private async Task<string> SaveFileAsync(FileUploadDTO model)
        {
            var fileName = Path.GetFileName(Guid.NewGuid().ToString() + model.File.FileName);
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UploadFiles", fileName);
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\net6.0", string.Empty)),
                @"wwwroot\UploadFiles",
                fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }

            return filePath;
        }

        // Helper method to convert CSV file to Excel format
        private static string ConvertToExcelAsync(string atualFile)
        {
            //var newFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UploadFiles", Guid.NewGuid().ToString() + ".xlsx");
            var newFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\net6.0", string.Empty)),
                @"wwwroot\UploadFiles",
                Guid.NewGuid().ToString() + ".xlsx");

            var csvLines = File.ReadAllLines(atualFile, Encoding.UTF8).Select(a => a.Split(';'));

            int rowCount = 0;
            int colCount = 0;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add();

                rowCount = 1;

                foreach (var line in csvLines)
                {
                    colCount = 1;
                    foreach (var col in line)
                    {
                        worksheet.Cell(rowCount, colCount).Value = col;
                        colCount++;
                    }
                    rowCount++;
                }

                File.Delete(atualFile);

                workbook.SaveAs(newFile);
            }

            return newFile;
        }

        // Helper method to read users from Excel file
        private async Task<List<UserImportDTO>> ReadUsersFromExcelAsync(string filePath)
        {
            var users = new List<UserImportDTO>();

            var basicRole = await _uow.Role.GetBasicProfile();

            using (var excelWorkbook = new XLWorkbook(filePath))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheets.FirstOrDefault().RowsUsed().Skip(1);

                foreach (var dataRow in nonEmptyDataRows)
                {
                    var cellName = dataRow.Cell(1).Value;
                    var cellEmail = dataRow.Cell(2).Value;

                    var password = PasswordExtension.GeneratePassword(2, 2, 2, 2);

                    users.Add(new UserImportDTO
                    {
                        Name = cellName.ToString(),
                        Email = cellEmail.ToString(),
                        RoleId = basicRole.Id,
                        Password = PasswordExtension.EncryptPassword(StringHelper.Base64Encode(password)),
                        PasswordBase64Decode = password
                    });
                }
            }

            return users;
        }

        // Helper method to send emails to users
        private async Task SendEmailsToUsersAsync(List<UserImportDTO> users)
        {
            foreach (var item in users)
            {
                _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
                {
                    Body = "Your registration was carried out in the application. Use the password <b>" + item.PasswordBase64Decode + "</b> on your first access to the application.",
                    Subject = item.Name,
                    ToEmail = item.Email
                }));
            }
        }

        // Helper method to delete the file
        private void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
