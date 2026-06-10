using EmployeeApi.Models;
using EmployeeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            return await _employeeService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            var employee = await _employeeService.GetAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            await _employeeService.CreateAsync(employee);

            return CreatedAtAction(
                nameof(Get),
                new { id = employee.Id },
                employee);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Employee employee)
        {
            var existingEmployee = await _employeeService.GetAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            employee.Id = existingEmployee.Id;

            await _employeeService.UpdateAsync(id, employee);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _employeeService.GetAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteAsync(id);

            return NoContent();
        }
    }
}