using EmployeeManagementAPI.Entities;
using EmployeeManagementAPI.Interface;
using EmployeeManagementAPI.Service;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeeController(IEmployeeService employeeService)          
    {
        this.employeeService = employeeService;
    }

    [HttpPost("analyze-salaries")]
    public ActionResult<List<SalaryAnalysisResult>> AnalyzeSalaries([FromBody] List<Employee> employees)
    {
        var analysisResults = employeeService.AnalyzeManagerSalaries(employees);
        return Ok(analysisResults);
    }

    [HttpPost("analyze-reporting-lines")]
    public ActionResult<List<ReportingLineResult>> AnalyzeReportingLines([FromBody] List<Employee> employees)
    {
        var reportingLineResults = employeeService.AnalyzeLongReportingLines(employees);
        return Ok(reportingLineResults);
    }
}
