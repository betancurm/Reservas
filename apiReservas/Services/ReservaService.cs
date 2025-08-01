﻿using apiReservas.Data;
using apiReservas.DTOs.Reservas;
using apiReservas.Models;
using Microsoft.EntityFrameworkCore;

namespace apiReservas.Services
{
    public class ReservaService : IReservaService
    {
        private readonly ICurrentUserService _currentUserService;

        private ReservasContext _context;

        public ReservaService(ICurrentUserService currentUser, ReservasContext dbcontext)
        {
            _currentUserService = currentUser;
            _context = dbcontext;
        }
        public IEnumerable<Reserva> Get()
        {
            return _context.Reservas.Include(r => r.Inmueble).ToList();
        }
        public IEnumerable<ReservasDTO> GetReservaDto()
        {
            return _context.Reservas
                .Include(r => r.Inmueble).Include(r => r.DetallesReserva)
                .Select(r => new ReservasDTO
                {
                    ReservaId = r.ReservaId,
                    FechaInicio = r.FechaInicio,
                    FechaFin = r.FechaFin,
                    ValorReservaUSD = r.ValorReservaUSD,
                    CodigoInmueble = r.Inmueble.CodigoInmueble,
                    NochesOcupadas = r.DetallesReserva.ToList().Sum(dr => dr.NochesOcupadas).ToString()
                })
                .ToList();
        }

        public async Task SaveAsync(Reserva reserva)
        {
            // 1. Usuario autenticado
            if (_currentUserService.ApplicationUserId == Guid.Empty)
                throw new UnauthorizedAccessException("Token sin uid");

            reserva.ReservaId = Guid.NewGuid();
            reserva.ApplicationUserId = _currentUserService.ApplicationUserId; 
            reserva.FechaCreacion = DateTime.Now;
            reserva.FechaModificacion = DateTime.Now;

            DateTime fechaInicio = reserva.FechaInicio;
            
            while (fechaInicio < reserva.FechaFin)
            {
                DateTime inicioMesSiguiente = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1);
                DateTime fechaFinDetalle = inicioMesSiguiente < reserva.FechaFin ? inicioMesSiguiente : reserva.FechaFin;

                int nochesOcupadas = (fechaFinDetalle - fechaInicio).Days;

                var detalle = new DetalleReserva
                {
                    DetalleReservaId = Guid.NewGuid(),
                    ReservaId = reserva.ReservaId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFinDetalle,
                    NochesOcupadas = nochesOcupadas,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };
                reserva.DetallesReserva.Add(detalle);


                // Mover al siguiente mes
                fechaInicio = inicioMesSiguiente;
            }
            
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Guid id, Reserva reserva)
        {
            
            var reservaActual = _context.Reservas.Include(r => r.DetallesReserva)
            .FirstOrDefault(r => r.ReservaId == id);
            if (reservaActual != null)
            {
                reservaActual.ApplicationUserId = _currentUserService.ApplicationUserId;
                reservaActual.FechaInicio = reserva.FechaInicio;
                reservaActual.FechaFin = reserva.FechaFin;
                reservaActual.ValorReservaUSD = reserva.ValorReservaUSD;
                reservaActual.InmuebleId = reserva.InmuebleId;
                reservaActual.FechaModificacion = DateTime.Now;
                reservaActual.DetallesReserva = reserva.DetallesReserva;


                DateTime fechaInicio = reserva.FechaInicio;
                while (fechaInicio < reserva.FechaFin)
                {
                    DateTime inicioMesSiguiente = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1);
                    DateTime fechaFinDetalle = inicioMesSiguiente < reserva.FechaFin ? inicioMesSiguiente : reserva.FechaFin;

                    int nochesOcupadas = (fechaFinDetalle - fechaInicio).Days;

                    var detalle = new DetalleReserva
                    {
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFinDetalle,
                        NochesOcupadas = nochesOcupadas,
                        FechaCreacion = reservaActual.FechaCreacion,
                        FechaModificacion = DateTime.Now
                    };
                   
                    reservaActual.DetallesReserva.Add(detalle);


                    // Mover al siguiente mes
                    fechaInicio = inicioMesSiguiente;
                }
                
                await _context.SaveChangesAsync();


            }
        }


        public async Task DeleteAsync(Guid id)
        {
            var reservaActual = _context.Reservas.Include(r => r.DetallesReserva)
                        .FirstOrDefault(r => r.ReservaId == id); 
            if (reservaActual != null)

            {
                //var detallesReserva = _context.DetallesReserva.Include(dr => dr.Reserva).Where(dr => dr.ReservaId == id).ToList();
                //_context.DetallesReserva.RemoveRange(detallesReserva);

                _context.Reservas.Remove(reservaActual);
                await _context.SaveChangesAsync();
            }
        }
    }
        public interface IReservaService
        {
            IEnumerable<Reserva> Get();
            Task SaveAsync(Reserva reserva);
            Task UpdateAsync(Guid id, Reserva reserva);
            Task DeleteAsync(Guid id);
            IEnumerable<ReservasDTO> GetReservaDto();
        }
}

                    