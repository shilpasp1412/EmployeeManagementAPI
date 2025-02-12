using EmployeeManagementAPI.Entities;
using EmployeeManagementAPI.Interface;

namespace EmployeeManagementAPI.Service
{
   public class EmployeeService : IEmployeeService
        {
            public List<SalaryAnalysisResult> AnalyzeManagerSalaries(List<Employee> employees)
            {
                var employeeDictionary = employees.ToDictionary(e => e.Id, e => e);
                var managersToCheck = employees.Where(e => e.ManagerId.HasValue).ToList();
                var results = new List<SalaryAnalysisResult>();

                foreach (var manager in managersToCheck)
                {
                    var subordinates = employees.Where(e => e.ManagerId == manager.Id).ToList();
                    if (subordinates.Any())
                    {
                        decimal averageSubordinateSalary = subordinates.Average(s => s.Salary);

                        // Check if the manager earns less than 20% more than their subordinates' average salary
                        if (manager.Salary < 1.2m * averageSubordinateSalary)
                        {
                            results.Add(new SalaryAnalysisResult
                            {
                                ManagerName = $"{manager.FirstName} {manager.LastName}",
                                Analysis = "Earns less than required",
                                DifferenceAmount = 1.2m * averageSubordinateSalary - manager.Salary
                            });
                        }

                        // Check if the manager earns more than 50% more than their subordinates' average salary
                        if (manager.Salary > 1.5m * averageSubordinateSalary)
                        {
                            results.Add(new SalaryAnalysisResult
                            {
                                ManagerName = $"{manager.FirstName} {manager.LastName}",
                                Analysis = "Earns more than required",
                                DifferenceAmount = manager.Salary - 1.5m * averageSubordinateSalary
                            });
                        }
                    }
                }

                return results;
            }

        public List<ReportingLineResult> AnalyzeLongReportingLines(List<Employee> employees)
        {
            var result = new List<ReportingLineResult>();

            foreach (var employee in employees)
            {
                int reportingLineLength = 0;
                var currentEmployee = employee;

                while (currentEmployee.ManagerId.HasValue)
                {
                    reportingLineLength++;
                    currentEmployee = employees.FirstOrDefault(e => e.Id == currentEmployee.ManagerId.Value);

                    if (currentEmployee == null)
                    {
                        // In case there is a broken chain (an employee with no valid manager).
                        break;
                    }
                }

                if (reportingLineLength >= 4)
                {
                    result.Add(new ReportingLineResult
                    {
                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        ExcessManagers = reportingLineLength - 4
                    });
                }
            }

            return result;
        }


    }

}

