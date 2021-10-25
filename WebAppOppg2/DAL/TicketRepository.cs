﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAppOppg2.DAL
{
    public class TicketRepository : ITicketRepository
    {
        private TicketDB _db;

        private ILogger<TicketRepository> _log;

        public TicketRepository(TicketDB ticketDB, ILogger<TicketRepository> log)
        {
            _db = ticketDB;
            _log = log;
        }

        public async Task<bool> SaveTicket(Ticket ticket)
        {

            try
            {
                _db.Tickets.Add(ticket);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i SaveTicket");
                return false;
            }
        }

        public async Task<List<Ticket>> GetAll()
        {
            try
            {
                List<Ticket> allTickets = await _db.Tickets.Select(t => new Ticket
                {
                    TicketID = t.TicketID,
                    Passenger = t.Passenger,
                    TicketTravelType = t.TicketTravelType,
                    TicketRoute = t.TicketRoute,
                    TicketDeparture = t.TicketDeparture,
                    TicketDate = t.TicketDate,
                    TicketPrice = t.TicketPrice
                }).ToListAsync();
                return allTickets;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAll");
                return null;
            }
        }

        public async Task<bool> DeleteTicket(int id)
        {
            try
            {
                Ticket oneTicket = await _db.Tickets.FindAsync(id);
                _db.Tickets.Remove(oneTicket);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i DeleteTicket");
                return false;
            }
        }

        public async Task<Ticket> GetOne(int id)
        {
            try
            {
                Ticket oneTicket = await _db.Tickets.FindAsync(id);
                var gotTicket = new Ticket()
                {
                    TicketID = oneTicket.TicketID,
                    Passenger = oneTicket.Passenger,
                    TicketTravelType = oneTicket.TicketTravelType,
                    TicketRoute = oneTicket.TicketRoute,
                    TicketDeparture = oneTicket.TicketDeparture,
                    TicketDate = oneTicket.TicketDate,
                    TicketPrice = oneTicket.TicketPrice
                };
                return gotTicket;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOne");
                return null;
            }
        }

        public async Task<bool> EditTicket(Ticket editTicket)
        {
            try
            {
                var editObject = await _db.Tickets.FindAsync(editTicket.TicketID);
                editObject.Passenger = editTicket.Passenger;
                editObject.TicketTravelType = editTicket.TicketTravelType;
                editObject.TicketRoute = editTicket.TicketRoute;
                editObject.TicketDeparture = editTicket.TicketDeparture;
                editObject.TicketDate = editTicket.TicketDate;
                editObject.TicketPrice = editTicket.TicketPrice;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editTicket");
                return false;
            }
            return true;
        }

        //Port 
        public async Task<bool> AddPort(Port port)
        {
            try
            {
                _db.Ports.Add(port);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i AddPort");
                return false;
            }
        }

        public async Task <bool> DeletePort (int portID)
        {
            try
            {
                Port onePort = await _db.Ports.FindAsync(portID);
                _db.Ports.Remove(onePort);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i DeletePort");
                return false;
            }
        }

        public async Task <bool> EditPort (Port editPort)
        {
            try
            {
                var editObject = await _db.Ports.FindAsync(editPort.PortID);
                editObject.PortName = editPort.PortName;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editPort");
                return false;
            }
            return true;
        }

        public async Task<Port> GetOnePort(int portID)
        {
            try
            {
                Port onePort = await _db.Ports.FindAsync(portID);
                var gotPort = new Port()
                {
                    PortID = onePort.PortID,
                    PortName = onePort.PortName,
                    
                };
                return gotPort;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOnePort");
                return null;
            }
        }

        public async Task <List<Port>> GetAllPorts ()
        {
            try
            {
                List<Port> allPorts = await _db.Ports.Select(p => new Port
                {
                    PortID = p.PortID,
                    PortName = p.PortName,
                   
                }).ToListAsync();
                return allPorts;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAllPorts");
                return null;
            }
        }

        //Route

        public async Task<bool> AddRoute (Route route)
        {
            try
            {
                _db.Routes.Add(route);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i AddRoute");
                return false;
            }
        }

        public async Task<bool> DeleteRoute(int routeID)
        {
            try
            {
                Route oneRoute = await _db.Routes.FindAsync(routeID);
                _db.Routes.Remove(oneRoute);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i DeleteRoute");
                return false;
            }
        }

        public async Task<bool> EditRoute(Route editRoute)
        {
            try //må se om jeg må endre noe med departurepunkter her 
            {
                var editObject = await _db.Routes.FindAsync(editRoute.RouteID);
                editObject.RouteID = editRoute.RouteID;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editPort");
                return false;
            }
            return true;
        }

        public async Task<Route> GetOneRoute(int routeID)
        {
            try
            {
                Route oneRoute = await _db.Routes.FindAsync(routeID);
                var gotRoute = new Route()
                {
                    RouteID = oneRoute.RouteID,
                    PortFromFK = oneRoute.PortFromFK,
                    PortToFK = oneRoute.PortToFK,
                    RoutePrice = oneRoute.RoutePrice,
                    DepartureOption1 = oneRoute.DepartureOption1,
                    DepartureOption2 = oneRoute.DepartureOption2

                };
                return gotRoute;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOneRoute");
                return null;
            }
        }


        //Traveltype
        public async Task<bool> AddTravelType(TravelType travelType)
        {
            try
            {
                _db.TravelTypes.Add(travelType);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i AddTravelType");
                return false;
            }
        }
    }
}
