using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefreshToken.Data;
using RefreshToken.Dto.RolesDetailDto;
using RefreshToken.Dto.RolesDto;
using RefreshToken.Model;
using RefreshToken.Services;
using System.Linq;

namespace RefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesDetailController : ControllerBase
    {
        private readonly DataContext _context;

        public RolesDetailController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Index")]
        public async Task<IActionResult> ListDetailRoles(IndexRolesDetail vm)
        {
            string search = vm.search;
            string sortOrder = vm.sortOrder;
            string filter = vm.filter;
            int page = vm.page;

            IList<IndexRolesDetail> items = new List<IndexRolesDetail>();
            var listRolesDetail = from detail in _context.RolesDetail
                                  join user in _context.User on detail.IdUser equals user.IdUser
                                  join roles in _context.Roles on detail.IdRoles equals roles.IdRoles
                                  where detail.Status == "A"
                                  select new
                                  {
                                      detail.IdRolesDetail,
                                      user.UserName,
                                      roles.RoleName,
                                      detail.Status,
                                      detail.DateCreated,
                                      detail.UserCreated,
                                      detail.DateModified,
                                      detail.UserModified
                                  };

            //searching with filter
            if (search != null && search != "")
            {
                if (filter == "Username")
                {
                    listRolesDetail = from detail in listRolesDetail
                                      where detail.UserName.Contains(search)
                                      select new
                                      {
                                          detail.IdRolesDetail,
                                          detail.UserName,
                                          detail.RoleName,
                                          detail.Status,
                                          detail.DateCreated,
                                          detail.UserCreated,
                                          detail.DateModified,
                                          detail.UserModified
                                      };
                }

                if (filter == "RoleName")
                {
                    listRolesDetail = from detail in listRolesDetail
                                      where detail.RoleName.Contains(search)
                                      select new
                                      {
                                          detail.IdRolesDetail,
                                          detail.UserName,
                                          detail.RoleName,
                                          detail.Status,
                                          detail.DateCreated,
                                          detail.UserCreated,
                                          detail.DateModified,
                                          detail.UserModified
                                      };
                }
            }

            // sorting using switch case
            listRolesDetail = sortOrder switch
            {
                // by Id Role Detail
                "IdRoleDetail_a" => listRolesDetail.OrderBy(s => s.IdRolesDetail),
                "IdRoleDetail_d" => listRolesDetail.OrderByDescending(s => s.IdRolesDetail),
                // by Username
                "Username_a" => listRolesDetail.OrderBy(s => s.UserName),
                "Username_d" => listRolesDetail.OrderByDescending(s => s.UserName),
                // by Role Name
                "RoleName_a" => listRolesDetail.OrderBy(s => s.RoleName),
                "RoleName_d" => listRolesDetail.OrderByDescending(s => s.RoleName),
                // by status
                "Status_a" => listRolesDetail.OrderBy(s => s.Status),
                "Status_d" => listRolesDetail.OrderByDescending(s => s.Status),
                // by Data Created
                "DateCreated_a" => listRolesDetail.OrderBy(s => s.DateCreated),
                "DateCreated_d" => listRolesDetail.OrderByDescending(s => s.DateCreated),
                // by User Created
                "UserCreated_a" => listRolesDetail.OrderBy(s => s.UserCreated),
                "UserCreated_d" => listRolesDetail.OrderByDescending(s => s.UserCreated),
                // by Date Modified
                "DateModified_a" => listRolesDetail.OrderBy(s => s.DateModified),
                "DateModified_d" => listRolesDetail.OrderByDescending(s => s.DateModified),
                // by User Modified
                "UserModified_a" => listRolesDetail.OrderBy(s => s.UserModified),
                "UserModified_d" => listRolesDetail.OrderByDescending(s => s.UserModified),
                // default using IdDeductible asc
                _ => listRolesDetail.OrderBy(s => s.IdRolesDetail),
            };

            //paging
            int pageSize = 10;
            int pageNumber = page;
            int totalCount = listRolesDetail.Select(i => i.IdRolesDetail).Count();

            return Ok(new
            {
                data = listRolesDetail.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                total = totalCount,
                page = pageNumber,
                totalPageSize = pageSize,
                last_page = totalCount / pageSize
            });
        }
        [HttpPost("Review")]
        public async Task<IActionResult> ReviewRolesDetail([FromBody] ReviewRolesDetail vm)
        {
            int id = vm.IdRoles;
            string rolesName = vm.RoleName;
            string username = vm.UserName;

            IList<ReviewRolesDetail> items = new List<ReviewRolesDetail>();
            var data = from detail in _context.RolesDetail
                       join user in _context.User on detail.IdUser equals user.IdUser
                       join roles in _context.Roles on detail.IdRoles equals roles.IdRoles
                       where (detail.IdRolesDetail == id || user.IdUser == vm.IdUser) && detail.Status == "A"
                       select new
                       {
                           detail.IdRolesDetail,
                           user.IdUser,
                           user.UserName,
                           roles.IdRoles,
                           roles.RoleName,
                           detail.Status,
                           detail.DateCreated,
                           detail.UserCreated,
                           detail.DateModified,
                           detail.UserModified
                       };

            foreach(var item in data.ToList())
            {
                ReviewRolesDetail list = new ReviewRolesDetail();
                list.IdRolesDetail= item.IdRolesDetail;
                list.IdUser = item.IdUser;
                list.UserName= item.UserName;
                list.IdRoles= item.IdRoles;
                list.RoleName = item.RoleName;
                list.Status = item.Status;
                list.DateCreated = item.DateCreated;
                list.UserCreated = item.UserCreated;
                list.DateModified = item.DateModified;
                list.UserModified = item.UserModified;
                items.Add(list);
            }

            if (data.Count() > 0)
            {               
                return Ok(items);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Not found for detail roles" });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRolesDetail([FromBody] RolesDetailCreateDto vm)
        {
            using (var transSQL = _context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    RolesDetail role = new RolesDetail();
                    role.IdUser = vm.IdUser;
                    role.IdRoles = vm.IdRoles;
                    role.Status = vm.Status;
                    role.DateCreated = vm.DateCreated;
                    role.UserCreated = vm.UserCreated;

                    _context.Add(role);
                    await _context.SaveChangesAsync();
                    transSQL.Commit();

                    return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Roles Detail successfully Added # " + role.IdRolesDetail });
                }
                else
                {
                    //return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = false, Message = "Please fill information roles" });
                    return StatusCode(StatusCodes.Status401Unauthorized, new ResponseApi { Success = false, Message = "Authorization Failed" });
                }
            }
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditRolesDetail([FromBody] RolesDetailEditDto vm)
        {
            int id = vm.IdRolesDetail;

            var roles = _context.RolesDetail.Where(i => i.IdRolesDetail == id && i.Status == "A").Single();

            try
            {
                roles.IdRolesDetail = id;
                roles.IdUser = vm.IdUser;
                roles.IdRoles = vm.IdRoles;
                roles.DateModified = vm.DateModified;
                roles.UserModified = vm.UserModified;

                _context.RolesDetail.Update(roles);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Roles Detail updated # " + id });
            }
            catch (DbUpdateException)
            {
                //throw;
                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = false, Message = id + " Id Roles Detail Not Found" });
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseApi { Success = true, Message = "Authorization Failed" });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteRolesDetail([FromBody] RolesDetailDeleteDto vm)
        {
            int id = vm.IdRolesDetail;

            var roles = _context.RolesDetail.Where(i => i.IdRolesDetail == id && i.Status == "A").Single();

            try
            {
                roles.IdRolesDetail = id;
                roles.Status = vm.Status;
                roles.DateModified = vm.DateModified;
                roles.UserModified = vm.UserModified;

                _context.RolesDetail.Update(roles);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = true, Message = "Roles Detail has been Delete # " + id });
            }
            catch (DbUpdateException)
            {
                //throw;
                return StatusCode(StatusCodes.Status200OK, new ResponseApi { Success = false, Message = id + " Id Roles Detail Not Found" });
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseApi { Success = true, Message = "Authorization Failed" });
        }
    }
}
