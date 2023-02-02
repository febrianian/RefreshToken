using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefreshToken.Data;
using RefreshToken.Dto.RolesDto;
using RefreshToken.Model;
using RefreshToken.Services;
using System.Data;

namespace RefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DataContext _context;

        public RolesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Index")]
        public async Task<IActionResult> ListRoles(IndexRoles vm)
        {
            string search = vm.search;
            string sortOrder = vm.sortOrder;
            string filter = vm.filter;
            int page = vm.page;

            IList<IndexRoles> items = new List<IndexRoles>();
            var listRoles = from roles in _context.Roles
                            where roles.Status == "A"
                            select new
                            {
                                roles.IdRoles,
                                roles.RoleName,
                                roles.Status,
                                roles.DateCreated,
                                roles.UserCreated,
                                roles.DateModified,
                                roles.UserModified
                            };

            //searching with filter
            if (search != null && search != "")
            {
                if (filter == "IdRoles")
                {
                    listRoles = from roles in listRoles
                                where roles.IdRoles.ToString().Contains(search)
                                select new
                                {
                                    roles.IdRoles,
                                    roles.RoleName,
                                    roles.Status,
                                    roles.DateCreated,
                                    roles.UserCreated,
                                    roles.DateModified,
                                    roles.UserModified
                                };
                }

                if (filter == "RoleName")
                {
                    listRoles = from roles in listRoles
                                where roles.RoleName.Contains(search)
                                select new
                                {
                                    roles.IdRoles,
                                    roles.RoleName,
                                    roles.Status,
                                    roles.DateCreated,
                                    roles.UserCreated,
                                    roles.DateModified,
                                    roles.UserModified
                                };
                }
            }

            // sorting using switch case
            listRoles = sortOrder switch
            {
                // by Id Role
                "IdRole_a" => listRoles.OrderBy(s => s.IdRoles),
                "IdRole_d" => listRoles.OrderByDescending(s => s.IdRoles),
                // by Role Name
                "DeductibleCode_a" => listRoles.OrderBy(s => s.RoleName),
                "DeductibleCode_d" => listRoles.OrderByDescending(s => s.RoleName),
                // by status
                "CompanyName_a" => listRoles.OrderBy(s => s.Status),
                "CompanyName_d" => listRoles.OrderByDescending(s => s.Status),                
                // by Data Created
                "DateCreated_a" => listRoles.OrderBy(s => s.DateCreated),
                "DateCreated_d" => listRoles.OrderByDescending(s => s.DateCreated),
                // by User Created
                "UserCreated_a" => listRoles.OrderBy(s => s.UserCreated),
                "UserCreated_d" => listRoles.OrderByDescending(s => s.UserCreated),
                // by Date Modified
                "DateModified_a" => listRoles.OrderBy(s => s.DateModified),
                "DateModified_d" => listRoles.OrderByDescending(s => s.DateModified),
                // by User Modified
                "UserModified_a" => listRoles.OrderBy(s => s.UserModified),
                "UserModified_d" => listRoles.OrderByDescending(s => s.UserModified),
                // default using IdDeductible asc
                _ => listRoles.OrderBy(s => s.IdRoles),
            };

            //paging
            int pageSize = 10;
            int pageNumber = page;
            int totalCount = listRoles.Select(i => i.IdRoles).Count();

            return Ok(new
            {
                data = listRoles.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                total = totalCount,
                page = pageNumber,
                totalPageSize = pageSize,
                last_page = totalCount / pageSize
            });
        }

        [HttpPost("Review")]
        public async Task<IActionResult> ReviewRoles([FromBody] ReviewRoles vm)
        {
            int id = vm.IdRoles;
            string rolesName = vm.RoleName;

            var roles = _context.Roles.Where(i => i.IdRoles == id || i.RoleName.Contains(rolesName));           

            if (roles.Count() > 0)
            {
                var data = roles.Single();
                vm.IdRoles = data.IdRoles;
                vm.RoleName = data.RoleName;
                vm.Status = data.Status;
                vm.DateCreated = data.DateCreated;
                vm.UserCreated = data.UserCreated;
                vm.DateModified = data.DateModified;
                vm.UserModified = data.UserModified;

                return Ok(vm);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Not found for roles " + roles.Single().RoleName });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoles([FromBody] RolesCreateDto vm)
        {
            using (var transSQL = _context.Database.BeginTransaction())
            {
                if(ModelState.IsValid)
                {
                    Roles role = new Roles();
                    role.RoleName = vm.RoleName;
                    role.Status = vm.Status;
                    role.DateCreated = vm.DateCreated;
                    role.UserCreated = vm.UserCreated;

                    _context.Add(role);
                    await _context.SaveChangesAsync();
                    transSQL.Commit();

                    return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Roles successfully Added # " + role.RoleName });
                }
                else
                {
                    //return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = false, Message = "Please fill information roles" });
                    return StatusCode(StatusCodes.Status401Unauthorized, new ResponseApi { Success = false, Message = "Authorization Failed" });
                }                
            }            
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditRoles([FromBody] RolesEditDto vm)
        {
            int id = vm.IdRoles;
            string rolesName = vm.RoleName;

            var roles = _context.Roles.Where(i => i.IdRoles == id && i.Status == "A").Single();

            try
            {
                roles.IdRoles = id;
                roles.RoleName = vm.RoleName;
                roles.DateModified = vm.DateModified;
                roles.UserModified = vm.UserModified;

                _context.Roles.Update(roles);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Roles updated # " + id });
            }
            catch (DbUpdateException)
            {
                //throw;
                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = false, Message = id + " Id Roles Not Found" });
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseApi { Success = true, Message = "Authorization Failed" });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteRoles([FromBody] RolesDeleteDto vm)
        {
            int id = vm.IdRoles;

            var roles = _context.Roles.Where(i => i.IdRoles == id && i.Status == "A").Single();

            try
            {
                roles.IdRoles = id;
                roles.Status = vm.Status;
                roles.DateModified = vm.DateModified;
                roles.UserModified = vm.UserModified;

                _context.Roles.Update(roles);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Roles deleted # " + id });
            }
            catch (DbUpdateException)
            {
                //throw;
                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = false, Message = id + " Id Roles Not Found" });
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseApi { Success = true, Message = "Authorization Failed" });
        }
    }
}
