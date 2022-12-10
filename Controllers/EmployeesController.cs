using Employees.API.Data;
using Employees.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employees.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeesDbContext dbContext;

    public EmployeesController(EmployeesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await this.dbContext.Employees.ToListAsync();
        return Ok(employees);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
    {
        var employee = await this.dbContext.Employees.FindAsync(id);
        return employee != null ? Ok(employee) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody]Employee employee)
    {
        employee.Id = Guid.NewGuid();
        await this.dbContext.Employees.AddAsync(employee);
        await this.dbContext.SaveChangesAsync();

        return Ok(employee);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateEmployee([FromRoute]Guid id, [FromBody]Employee updateEmployee)
    {
        var employee = await this.dbContext.Employees.FindAsync(id);
        if (employee == null) {
            return NotFound();
        }

        employee.Name=updateEmployee.Name;
        employee.Email=updateEmployee.Email;
        employee.Phone=updateEmployee.Phone;

        await this.dbContext.SaveChangesAsync();

        return Ok(employee);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute]Guid id)
    {
        var employee = await this.dbContext.Employees.FindAsync(id);
        if (employee == null) 
        {
            return NotFound();
        }

        this.dbContext.Employees.Remove(employee);
        await this.dbContext.SaveChangesAsync();

        return Ok();
    }
}
