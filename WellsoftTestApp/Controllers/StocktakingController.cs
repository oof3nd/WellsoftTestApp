using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellsoftTestApp.Models;
using WellsoftTestApp.Services;

namespace WellsoftTestApp.Controllers
{
    public class StocktakingController : Controller
    {
        private readonly StocktakingContext _stocktakingContext;

        public StocktakingController(StocktakingContext context)
        {
            _stocktakingContext = context;
        }

        public IActionResult Companies()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCompanies(int pageNumber = 1, int pageSize = 3)
        {
            var companies = _stocktakingContext.Companies.ToList();

            var paginationData = companies.PagedResult(pageNumber, pageSize);

            return Json(paginationData);
        }

        public JsonResult GetAllCompanies()
        {
            
            return Json(_stocktakingContext.Companies.ToList());
        }

        public JsonResult GetEmployees(int pageNumber = 1, int pageSize = 3)
        {
            
            var employees = _stocktakingContext.Employees.ToList();

            var paginationData = employees.PagedResult(pageNumber, pageSize);

            return Json(paginationData);
        }

        #region Company
        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            if (!ModelState.IsValid) return NoContent();

            try
            {
                _stocktakingContext.Companies.Add(company);
                await _stocktakingContext.SaveChangesAsync();
                return RedirectToAction("Companies");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. ");
                throw;
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditCompany(int id)
        {

            var company = await _stocktakingContext.Companies.FirstOrDefaultAsync(x => x.Id == id);

            return company == null ? GetCompanies() : Json(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompany([Bind("Id,Name")] Company company)
        {
            _stocktakingContext.Companies.Update(company);
            await _stocktakingContext.SaveChangesAsync();
            return RedirectToAction("Companies");
        }

        public async Task<IActionResult> DeleteCompany(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var company = await _stocktakingContext.Companies.FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return RedirectToAction("Companies");
        }

        [HttpPost, ActionName("DeleteCompany")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompanyConfirmed(int id)
        {
            var company = await _stocktakingContext.Companies.FindAsync(id);
            _stocktakingContext.Companies.Remove(company);
            await _stocktakingContext.SaveChangesAsync();
            return RedirectToAction("Companies");
        }
        #endregion

        #region Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee([Bind("Name,Description,Age,CompanyId")]Employee employee)
        {
            if (!ModelState.IsValid) return NoContent();

            try
            {
                _stocktakingContext.Employees.Add(employee);
                await _stocktakingContext.SaveChangesAsync();
                return RedirectToAction("Employees");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. ");
                throw;
            }

        }

        [HttpGet]
        public async Task<JsonResult> EditEmployee(int? id)
        {

            var employee = await _stocktakingContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            return employee == null ? GetEmployees() : Json(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee([Bind("Id,Name,Description,Age,CompanyId")] Employee employee)
        {

            var employeeToUpdate = await _stocktakingContext.Employees.FirstOrDefaultAsync(c => c.Id == employee.Id);


            if (!await TryUpdateModelAsync(employeeToUpdate, "", x => x.Name, x => x.Description, x => x.Age,
                x => x.CompanyId)) return RedirectToAction("Employees");
            try
            {
                await _stocktakingContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. ");
                throw;
            }
            return RedirectToAction("Employees");

        }

        public async Task<IActionResult> DeleteEmployee(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _stocktakingContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return RedirectToAction("Employees");
        }

        [HttpPost, ActionName("DeleteEmployee")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeConfirm(int id)
        {
            var employee = await _stocktakingContext.Employees.FindAsync(id);
            _stocktakingContext.Employees.Remove(employee);
            await _stocktakingContext.SaveChangesAsync();
            return RedirectToAction("Employees");
        }
        #endregion
    }
}