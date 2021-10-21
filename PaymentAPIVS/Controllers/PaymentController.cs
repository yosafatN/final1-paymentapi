using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Data;
using PaymentAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PaymentController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.PaymentData.ToListAsync();
            return Ok(new Response()
            {
                Status = true,
                Message = "Success",
                Data = items
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(PaymentDAO data)
        {
            if (ModelState.IsValid)
            {
                await _context.PaymentData.AddAsync(data);
                await _context.SaveChangesAsync();

                return Ok(new Response() {
                    Status = true,
                    Message = "Successfully Added Payment",
                    Data = data
                });
            }

            return new JsonResult(new Response()
            {
                Status = false,
                Message = "Someting went wrong",
                Data = null
            }) { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.PaymentData.FirstOrDefaultAsync(x => x.PaymentDetailId == id);
            if (item == null)
            {
                return Ok(new Response()
                {
                    Status = false,
                    Message = "Payment With ID equal to "+id+" Not Found",
                    Data = null
                });
            }

            return Ok(new Response()
            {
                Status = true,
                Message = "Success",
                Data = item
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> GetItem(int id, PaymentDAO item)
        {
            if (id != item.PaymentDetailId)
                return BadRequest();

            var existItem = await _context.PaymentData.FirstOrDefaultAsync(x => x.PaymentDetailId == id);

            existItem.CardOwnerName = item.CardOwnerName;
            existItem.CardNumber = item.CardNumber;
            existItem.ExpirationDate = item.ExpirationDate;
            existItem.SecurityCode = item.SecurityCode;

            await _context.SaveChangesAsync();

            return Ok(new Response()
            {
                Status = true,
                Message = "Successfully Changed Payment Data",
                Data = existItem
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existItem = await _context.PaymentData.FirstOrDefaultAsync(x => x.PaymentDetailId == id);

            if (existItem == null)
                return NotFound();

            _context.PaymentData.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(new Response()
            {
                Status = true,
                Message = "Successfully Deleted Payment Data",
                Data = existItem
            });
        }
    }
}