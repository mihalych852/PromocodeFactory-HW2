﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            List<RoleItemResponse> roles = new List<RoleItemResponse>();
            if (employee.Roles != null && employee.Roles.Count > 0)
            {
                roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
            }

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = roles,
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника (добавить в репозтьорий)
        /// </summary>
        /// <param name="employee">Данные сотрудника</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.CreateAsync(employee);
            return Ok();
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="employeeData">Новые данные сотрудника</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateEmployeeAsync(Employee employeeData)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeData.Id);

            if (employee == null)
                return NotFound();

            await _employeeRepository.UpdateAsync(employee, employeeData);

            return Ok();
        }

        /// <summary>
        /// Удалить сотрудника по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            await _employeeRepository.DeleteAsync(id);
            return Ok();
        }
    }
}