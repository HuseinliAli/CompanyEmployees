﻿using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController(IServiceManager service) : BaseApiController
    {

        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = service.EmployeeService.GetEmployees(companyId, false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = service.EmployeeService.GetEmployee(companyId, id, changeTracker: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("Employee for Creation object is null");
            var employeeToReturn = service.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);
            return CreatedAtRoute("GetEmployeeForCompany", new
            {companyId,id =employeeToReturn.Id},employeeToReturn);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId,Guid id)
        {
            service.EmployeeService.DeleteEmployeeForCompany(companyId, id,false);
            return NoContent();
        }
    }

}
