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
    public class ReservasController : ControllerBase
    {
        private readonly goplayco_redContext _context;

        public ReservasController(goplayco_redContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReserva()
        {
            return await _context.Reserva.Include(s => s.IdestadoNavigation).ToListAsync();
        }
        [HttpGet("misreservas/{usuario}")]
        public async Task<ActionResult<IEnumerable<Reserva>>>  GetReservaMisreservas(string usuario)
        {

            return await _context.Reserva
                .Include(s => s.IdestadoNavigation)
                .Include(s => s.IdhorarioNavigation.IdCanchaNavigation)
                .Where(s => s.Usuario == usuario).ToListAsync();
            
            //var datos = await (from r in _context.Reserva join h in _context.Horario on r.)
        
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reserva.Include(s => s.IdestadoNavigation).FirstAsync(s => s.IdReserva == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }
        // GET: api/Reservas/5
        [HttpGet("cancha/{idCancha}/{fecha}")]
        public async Task<ActionResult<Reserva>> GetReservaC(int idCancha, string fecha)
        {
            //var reserva = await _context.Reserva.Include(s => s.IdestadoNavigation).Include(s => s.IdhorarioNavigation).ToListAsync();
            var reserva = await (from re in _context.Horario
                                 join ho in _context.Reserva.Include(s => s.IdestadoNavigation)
                                 on re.Id equals ho.Idhorario
                                 where re.IdCancha == idCancha && ho.Fecha.Substring(0, 9).Replace("/", "").Replace("-", "").Trim().Equals(fecha)
                                 select ho).ToListAsync();

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }


        [HttpGet("horario/{idHorario}/{fecha}/{hora}")]
        public async Task<ActionResult<Reserva>> GetReservaHF(int idHorario, string fecha, string hora)
        {
            //var reserva = await _context.Reserva.Include(s => s.IdestadoNavigation).Include(s => s.IdhorarioNavigation).ToListAsync();
            var reserva = await (from re in _context.Horario
                                 join ho in _context.Reserva.Include(s => s.IdestadoNavigation)
                                 on re.Id equals ho.Idhorario
                                 where ho.Idhorario == idHorario && ho.HoraInicio.Equals(hora) && ho.Fecha.Substring(0, 9).Replace("/", "").Replace("-", "").Trim().Equals(fecha)
                                 select ho).FirstAsync();

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        // PUT: api/Reservas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.IdReserva)
            {
                return BadRequest();
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // POST: api/Reservas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            _context.Reserva.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = reserva.IdReserva }, reserva);
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reserva>> DeleteReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reserva.Remove(reserva);
            await _context.SaveChangesAsync();

            return reserva;
        }

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.IdReserva == id);
        }
    }
}
