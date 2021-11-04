using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.Models;

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

        public async Task<bool> SaveTicket(Ticket inTicket)
        {

            try
            {
                var newTicketRow = new Tickets();

                //CheckPassenger

                var checkPassenger = await _db.Passengers.FindAsync(inTicket.PassengerID);
                if(checkPassenger == null)
                {
                    //Oppretter ny passasjer dersom passasjer ikke finnes fra før

                    var passengerRow = new Passengers();
                    passengerRow.Firstname = inTicket.Firstname;
                    passengerRow.Lastname = inTicket.Lastname;
                    passengerRow.Email = inTicket.Email;
                    passengerRow.PassengerTypeID = inTicket.PassengerTypeID;
                    
                    newTicketRow.Passenger = passengerRow;
                }
                else
                {
                    newTicketRow.Passenger = checkPassenger;
                }

                newTicketRow.RouteID = inTicket.RouteID;
                newTicketRow.Route = await _db.Routes.FindAsync(inTicket.RouteID);
                newTicketRow.TicketDate = inTicket.TicketDate;
                newTicketRow.TravelTypeID = inTicket.TravelTypeID;
                newTicketRow.TravelType = await _db.TravelTypes.FindAsync(inTicket.TravelTypeID);


                //CheckRoute
                /*
                var checkRoute = await _db.Routes.FindAsync(inTicket.RouteID);
                if (checkRoute == null)
                {
                    var routeRow = new Route();
                    routeRow.PortFrom = inTicket.RouteFrom;
                    routeRow.PortTo = inTicket.RouteTo;
                    routeRow.RoutePrice = inTicket.Price;
                    routeRow.Departure = inTicket.Departure;

                    var checkTravelType = await _db.TravelTypes.FindAsync(inTicket.TravelType);
                    if(checkTravelType == null)
                    {
                        var travelTypeRow = new TravelTypes();
                        travelTypeRow.TravelTypeName = inTicket.TravelType;
                        newTicketRow.Route.TravelType = travelTypeRow;
                    }
                    else
                    {
                        newTicketRow.Route.TravelType = checkTravelType;
                    }
                }
                else
                {
                    newTicketRow.Route = checkRoute;
                }*/

                _db.Tickets.Add(newTicketRow);
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
                    Firstname = t.Passenger.Firstname,
                    Lastname = t.Passenger.Lastname,
                    Email = t.Passenger.Email,
                    PassengerID = t.Passenger.PassengerID,
                    PassengerType = t.Passenger.PassengerType.PassengerTypeName,
                    TravelType = t.TravelType.TravelTypeName,
                    RouteTo = t.Route.PortTo.PortName,
                    RouteFrom = t.Route.PortFrom.PortName,
                    Departure = t.Route.Departure,
                    TicketDate = t.TicketDate,
                    Price = t.Route.RoutePrice
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
                Tickets oneTicket = await _db.Tickets.FindAsync(id);
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
                Tickets oneTicket = await _db.Tickets.FindAsync(id);
                var gotTicket = new Ticket()
                {
                    TicketID = oneTicket.TicketID,
                    PassengerID = oneTicket.Passenger.PassengerID,
                    Firstname = oneTicket.Passenger.Firstname,
                    Lastname = oneTicket.Passenger.Lastname,
                    Email = oneTicket.Passenger.Email,
                    PassengerType = oneTicket.Passenger.PassengerType.PassengerTypeName,
                    TravelTypeID = oneTicket.TravelTypeID,
                    TravelType = oneTicket.TravelType.TravelTypeName,
                    RouteID = oneTicket.RouteID,
                    RouteTo = oneTicket.Route.PortTo.PortName,
                    RouteFrom = oneTicket.Route.PortFrom.PortName,
                    Departure = oneTicket.Route.Departure,
                    TicketDate = oneTicket.TicketDate,
                    Price = oneTicket.Route.RoutePrice
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
                editObject.Passenger.Firstname = editTicket.Firstname;
                editObject.Passenger.Lastname = editTicket.Lastname;
                editObject.Passenger.Email = editTicket.Email;
                editObject.PassengerID = editTicket.PassengerID;
                editObject.Passenger = await _db.Passengers.FindAsync(editTicket.PassengerID);
                editObject.TravelTypeID = editTicket.TravelTypeID;
                editObject.TravelType = await _db.TravelTypes.FindAsync(editTicket.TravelTypeID);
                editObject.RouteID = editTicket.RouteID;
                editObject.Route = await _db.Routes.FindAsync(editTicket.RouteID);
                editObject.TicketDate = editTicket.TicketDate;

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
        public async Task<bool> AddPort(Port inPort)
        {
            try
            {
                var newPortRow = new Ports();
                newPortRow.PortName = inPort.PortName;

                var checkPort = await _db.Ports.FindAsync(inPort.PortID);
                if(checkPort == null)
                {
                    var portsRow = new Ports();
                    portsRow.PortID = inPort.PortID;
                    portsRow.PortName = inPort.PortName;
                    newPortRow = portsRow;
                }
                else
                {
                    newPortRow = checkPort;
                }

                _db.Ports.Add(newPortRow);
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
                Ports oneDBPort = await _db.Ports.FindAsync(portID);
                _db.Ports.Remove(oneDBPort);
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
                Ports oneDBPort = await _db.Ports.FindAsync(portID);
                var gotPort = new Port()
                {
                    PortID = oneDBPort.PortID,
                    PortName = oneDBPort.PortName,
                    
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

        public async Task<bool> AddRoute (Route inRoute)
        {
            try
            {
                var newRouteRow = new Routes();
                newRouteRow.Departure = inRoute.Departure;
                newRouteRow.RoutePrice = inRoute.RoutePrice;
                newRouteRow.PortFrom.PortName = inRoute.PortFrom;
                newRouteRow.PortTo.PortName = inRoute.PortTo;

                //her kan vi velge om admin skal ha mulighet til å legge til nye ports når nye ruter opprettes!

                /*
                var checkPortFrom = await _db.Ports.FindAsync(inRoute.PortFrom);
                if(checkPortFrom == null)
                {
                    var portRow = new Ports();
                    portRow.PortName = inRoute.PortFrom;
                    newRouteRow.PortFrom = portRow;
                }
                else
                {
                    newRouteRow.PortFrom = checkPortFrom;
                }
                */
                /*
                var checkPortTo = await _db.Ports.FindAsync(inRoute.PortTo);
                if (checkPortTo == null)
                {
                    var portRow = new Ports();
                    portRow.PortName = inRoute.PortTo;
                    newRouteRow.PortTo = portRow;
                }
                else
                {
                    newRouteRow.PortTo = checkPortTo;
                }
                */

                //samme med denne. skal admin kunne opprette nye billettyper når nye ruter opprettes
/*
                var checkTraveltype = await _db.TravelTypes.FindAsync(inRoute.TravelType);
                if(checkTraveltype == null)
                {
                    var travelTyperow = new TravelTypes();
                    travelTyperow.TravelTypeName = inRoute.TravelType;
                    newRouteRow.TravelType = travelTyperow;
                }
                else
                {
                    newRouteRow.TravelType = checkTraveltype;
                }
*/
                _db.Routes.Add(newRouteRow);
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
                Routes oneRoute = await _db.Routes.FindAsync(routeID);
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
            try
            {
                var editObject = await _db.Routes.FindAsync(editRoute.RouteID);
                editObject.RouteID = editRoute.RouteID;
                editObject.PortFrom.PortName = editRoute.PortFrom;
                editObject.PortTo.PortName = editRoute.PortTo;
                editObject.Departure = editRoute.Departure;
                editObject.RoutePrice = editRoute.RoutePrice;

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
                Routes oneRoute = await _db.Routes.FindAsync(routeID);
                var gotRoute = new Route()
                {
                    RouteID = oneRoute.RouteID,
                    PortTo = oneRoute.PortTo.PortName,
                    PortFrom = oneRoute.PortFrom.PortName,
                    RoutePrice = oneRoute.RoutePrice,
                    Departure = oneRoute.Departure,

                };
                return gotRoute;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOneRoute");
                return null;
            }
        }
        public async Task<List<Route>> GetAllRoutes()
        {
            try
            {
                List<Route> allRoutes = await _db.Routes.Select(p => new Route
                {
                    RouteID = p.RouteID,
                    PortFrom = p.PortFrom.PortName,
                    PortTo = p.PortTo.PortName,
                    RoutePrice = p.RoutePrice,
                    Departure = p.Departure,
                }).ToListAsync();
                return allRoutes;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAllRoutes");
                return null;
            }
        }

        //Passenger
        public async Task<bool> AddPassenger(Passenger inPassenger)
        {
            try
            {
                var newPassengerRow = new Passengers();
                newPassengerRow.Firstname = inPassenger.Firstname;
                newPassengerRow.Lastname = inPassenger.Lastname;
                newPassengerRow.Email = inPassenger.Email;
                newPassengerRow.PassengerTypeID = inPassenger.PassengerTypeID;
                newPassengerRow.PassengerType = await _db.PassengerTypes.FindAsync(inPassenger.PassengerTypeID);
                _db.Passengers.Add(newPassengerRow);
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
                Passengers onePassenger = await _db.Passengers.FindAsync(passengerID);
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
                editObject.PassengerTypeID = editPassenger.PassengerTypeID;
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
                Passengers onePassenger = await _db.Passengers.FindAsync(passengerID);
                var gotPassenger = new Passenger()
                {
                    PassengerID = onePassenger.PassengerID,
                    Firstname = onePassenger.Firstname,
                    Lastname = onePassenger.Lastname,
                    Email = onePassenger.Email,
                    PassengerTypeID = onePassenger.PassengerTypeID

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
                    PassengerTypeID = p.PassengerTypeID,
                    PassengerType = p.PassengerType.PassengerTypeName
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
        public async Task<bool> AddPassengerType(PassengerType inPassengerType)
        {
            try
            {
                var newPassengerTypeRow = new PassengerTypes();
                newPassengerTypeRow.PassengerTypeName = inPassengerType.PassengerTypeName;
                newPassengerTypeRow.Discount = inPassengerType.Discount;

                _db.PassengerTypes.Add(newPassengerTypeRow);
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
                PassengerTypes onePassengerType = await _db.PassengerTypes.FindAsync(passengerTypeID);
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
                //editObject.PassengerTypeID = editPassengerType.PassengerTypeID;
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
                PassengerTypes onePassengerType = await _db.PassengerTypes.FindAsync(passengerTypeID);
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

        public async Task<List<PassengerType>> GetAllPassengerTypes()
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

        //Traveltype
        public async Task<bool> AddTravelType(TravelType inTravelType)
        {
            try
            {
                var newTravelTypeRow = new TravelTypes();
                newTravelTypeRow.TravelTypeName = inTravelType.TravelTypeName;

                _db.TravelTypes.Add(newTravelTypeRow);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i AddTravelType");
                return false;
            }
        }

        public async Task<bool> DeleteTravelType(int id)
        {
            try
            {
                TravelTypes oneTravelType = await _db.TravelTypes.FindAsync(id);
                _db.TravelTypes.Remove(oneTravelType);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i DeleteTravelPort");
                return false;
            }
        }

        public async Task<bool> EditTravelType(TravelType travelType)
        {
            try
            {
                var editObject = await _db.TravelTypes.FindAsync(travelType.TravelTypeID);
                editObject.TravelTypeName = travelType.TravelTypeName;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editTravelType");
                return false;
            }
            return true;
        }

        public async Task<TravelType> GetOneTravelType(int id)
        {
            try
            {
                TravelTypes OneTravelType = await _db.TravelTypes.FindAsync(id);
                var gotTravelType = new TravelType()
                {
                    TravelTypeID = OneTravelType.TravelTypeID,
                    TravelTypeName = OneTravelType.TravelTypeName,

                };
                return gotTravelType;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOneTravelType");
                return null;
            }
        }

        public async Task<List<TravelType>> GetAllTravelTypes()
        {
            try
            {
                List<TravelType> allTravelType = await _db.TravelTypes.Select(t => new TravelType
                {
                    TravelTypeID = t.TravelTypeID,
                    TravelTypeName = t.TravelTypeName,
                }).ToListAsync();
                return allTravelType;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAllTravelTypes");
                return null;
            }
        }
    }
}
