﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Models;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly BackendContext _context;

        public FormsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Form>>> GetForms()
        {
          if (_context.Forms == null)
          {
              return NotFound();
          }
            return await _context.Forms.ToListAsync();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Form>> GetForm(int id)
        {
          if (_context.Forms == null)
          {
              return NotFound();
          }
            var form = await _context.Forms.FindAsync(id);

            if (form == null)
            {
                return NotFound();
            }

            return form;
        }
        

        // PUT: api/Forms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForm(int id, string structure)
        {
            //if (id != form.Id)
            //{
            //    return BadRequest();
            //}

            var form = await _context.Forms.FindAsync(id);

            if (form == null)
            {
                return NotFound();
            }
            form.Structure = structure;
            //_context.Entry(form).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Forms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Form>> PostForm(Form form)
        {
          if (_context.Forms == null)
          {
              return Problem("Form cannot be null.");
          }
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        // DELETE: api/Forms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }
            var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormExists(int id)
        {
            return (_context.Forms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
