using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AmiSocialWebApi.Data;
using AmiSocialWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmiSocialWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MemberController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Member>> GetMemberByUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var member = await _context.Members.Where(m => m.UserId == userId).FirstOrDefaultAsync();
            return member;
        }
    }
}