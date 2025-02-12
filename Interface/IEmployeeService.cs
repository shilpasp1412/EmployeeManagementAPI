using EmployeeManagementAPI.Entities;

namespace EmployeeManagementAPI.Interface
{
    public interface IEmployeeService
    {
        public List<SalaryAnalysisResult> AnalyzeManagerSalaries(List<Employee> employees);
        public List<ReportingLineResult> AnalyzeLongReportingLines(List<Employee> employees);        
    }
}
