using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo, 
            ISpecification<T> spec, int pageIndex, int pageSize)where T : BaseEntity 
        {
            var items = await repo.ListAsync(spec);
            var count = await repo.CountAsync(spec);
            var pagination = new PaginationHelper<T>(pageIndex, pageSize, count, items);

            return Ok(pagination);
        }
    }
}
