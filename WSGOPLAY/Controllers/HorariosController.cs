﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSGOPLAY.Models;

namespace WSGOPLAY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly goplayco_redContext _context;

        public HorariosController(goplayco_redContext context)
        {
            _context = context;
        }

        // GET: api/Horarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horario>>> GetHorario()
        {
            var horario = await _context.Horario.Include(s => s.IdCanchaNavigation).Include(s => s.Sinreserva).ToListAsync();
            if (horario == null)
            {
                return NotFound();
            }

            return horario;

        }

        // GET: api/Horarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Horario>> GetHorario(int id)
        {
            var horario = await _context.Horario.Include(s => s.IdCanchaNavigation).Include(s => s.Sinreserva).FirstOrDefaultAsync(s => s.Id == id);

            if (horario == null)
            {
                return NotFound();
            }

            return horario;
        }
        // GET: api/Horarios/5
        [HttpGet("cancha/{idCancha}")]
        public async Task<ActionResult<IEnumerable<Horario>>> GetHorarioC(int idCancha)
        {
            var horario = await _context.Horario.Include(s => s.Sinreserva).Where(s => s.IdCancha == idCancha).ToListAsync();

            if (horario == null)
            {
                return NotFound();
            }

            return horario;
        }

        // PUT: api/Horarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, Horario horario)
        {
            if (id != horario.Id)
            {
                return BadRequest();
            }

            _context.Entry(horario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
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

        // POST: api/Horarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Horario>> PostHorario(Horario horario)
        {
            _context.Horario.Add(horario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHorario", new { id = horario.Id }, horario);
        }

        // DELETE: api/Horarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Horario>> DeleteHorario(int id)
        {
            var horario = await _context.Horario.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }

            _context.Horario.Remove(horario);
            await _context.SaveChangesAsync();

            return horario;
        }

        private bool HorarioExists(int id)
        {
            return _context.Horario.Any(e => e.Id == id);
        }
    }
}
