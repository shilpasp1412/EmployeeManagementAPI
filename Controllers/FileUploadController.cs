using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using EmployeeManagementAPI.Entities;
using EmployeeManagementAPI.Service;
using EmployeeManagementAPI.Interface;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public FileUploadController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpPost("upload")]
        public ActionResult UploadAndAnalyze(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            List<Employee> employees;
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                employees = csv.GetRecords<Employee>().ToList();
            }

            var salaryResults = employeeService.AnalyzeManagerSalaries(employees);
            var reportingLineResults = employeeService.AnalyzeLongReportingLines(employees);

            return Ok(new { SalaryAnalysis = salaryResults, ReportingLineAnalysis = reportingLineResults });
        }
    }

}
