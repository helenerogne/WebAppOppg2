using Microsoft.EntityFrameworkCore;
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


        //Passenger
        public async Task<bool> AddPassenger(Passenger passenger)
        {
            try
            {
                _db.Passengers.Add(passenger);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i addPassenger");
                return false;
            }
        }

        public async Task<bool> DeletePassenger(int passengerID)
        {
            try
            {
                Passenger onePassenger = await _db.Passengers.FindAsync(passengerID);
                _db.Passengers.Remove(onePassenger);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i deletePassenger");
                return false;
            }
        }

        public async Task<bool> EditPassenger(Passenger editPassenger)
        {
            try
            {
                var editObject = await _db.Passengers.FindAsync(editPassenger.PassengerID);
                editObject.Firstname = editPassenger.Firstname;
                editObject.Lastname = editPassenger.Lastname;
                editObject.Email = editPassenger.Email;
                editObject.PassengerTypeFK = editPassenger.PassengerTypeFK;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editPassenger");
                return false;
            }
            return true;
        }

        public async Task<Passenger> GetOnePassenger(int passengerID)
        {
            try
            {
                Passenger onePassenger = await _db.Passengers.FindAsync(passengerID);
                var gotPassenger = new Passenger()
                {
                    PassengerID = onePassenger.PassengerID,
                    Firstname = onePassenger.Firstname,
                    Lastname = onePassenger.Lastname,
                    Email = onePassenger.Email,
                    PassengerTypeFK = onePassenger.PassengerTypeFK

                };
                return gotPassenger;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOnePassenger");
                return null;
            }
        }

        public async Task<List<Passenger>> GetAllPassengers()
        {
            try
            {
                List<Passenger> allPassengers = await _db.Passengers.Select(p => new Passenger
                {
                    PassengerID = p.PassengerID,
                    Firstname = p.Firstname,
                    Lastname = p.Lastname,
                    Email = p.Email,
                    PassengerTypeFK = p.PassengerTypeFK
                }).ToListAsync();
                return allPassengers;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAllPassengers");
                return null;
            }
        }


        //PassengerType
        public async Task<bool> AddPassengerType(PassengerType passengerType)
        {
            try
            {
                _db.PassengerTypes.Add(passengerType);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i addPassengerType");
                return false;
            }
        }

        public async Task<bool> DeletePassengerType(int passengerTypeID)
        {
            try
            {
                PassengerType onePassengerType = await _db.PassengerTypes.FindAsync(passengerTypeID);
                _db.PassengerTypes.Remove(onePassengerType);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i deletePassengerType");
                return false;
            }
        }

        public async Task<bool> EditPassengerType(PassengerType editPassengerType)
        {
            try
            {
                var editObject = await _db.PassengerTypes.FindAsync(editPassengerType.PassengerTypeID);
                editObject.PassengerTypeID = editPassengerType.PassengerTypeID;
                editObject.PassengerTypeName = editPassengerType.PassengerTypeName;
                editObject.Discount = editPassengerType.Discount;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editPassengerType");
                return false;
            }
            return true;
        }

        public async Task<PassengerType> GetOnePassengerType(int passengerTypeID)
        {
            try
            {
                PassengerType onePassengerType = await _db.PassengerTypes.FindAsync(passengerTypeID);
                var gotPassengerType = new PassengerType()
                {
                    PassengerTypeID = onePassengerType.PassengerTypeID,
                    PassengerTypeName = onePassengerType.PassengerTypeName,
                    Discount = onePassengerType.Discount
                };
                return gotPassengerType;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOnePassengerType");
                return null;
            }
        }

        public async Task<List<PassengerType>> GetAllPassengersType()
        {
            try
            {
                List<PassengerType> allPassengerTypes = await _db.PassengerTypes.Select(p => new PassengerType
                {
                    PassengerTypeID = p.PassengerTypeID,
                    PassengerTypeName = p.PassengerTypeName,
                    Discount = p.Discount
                }).ToListAsync();
                return allPassengerTypes;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAllPassengersTypes");
                return null;
            }
        }
    }
}
