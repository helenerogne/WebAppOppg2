using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public interface ITicketRepository
    {
        Task<bool> LogIn(Admin admin);
        //Task<bool> Loggut();
        Task<bool> EditAdmin(Admin editAdmin);

        Task<bool> SaveTicket(Ticket ticket);
        Task<bool> DeleteTicket(int ticketID);
        Task<bool> EditTicket(Ticket editTicket);
        Task<Ticket> GetOne(int ticketID);
        Task<List<Ticket>> GetAll();

        Task<bool> AddPort(Port port);
        Task<bool> DeletePort(int portID);
        Task<bool> EditPort(Port editPort);
        Task<Port> GetOnePort(int portID);
        Task<List<Port>> GetAllPorts();

        Task<bool> AddTravelType(TravelType travelType);
        Task<bool> DeleteTravelType(int travelTypeID);
        Task<bool> EditTravelType(TravelType editTravelType);
        Task<TravelType> GetOneTravelType(int travelTypeID);
        Task<List<TravelType>> GetAllTravelTypes();

        Task<bool> AddRoute(Route route);
        Task<bool> DeleteRoute(int routeID);
        Task<bool> EditRoute(Route editRoute);
        Task<Route> GetOneRoute(int routeID);
        Task<List<Route>> GetAllRoutes();

        Task<bool> AddPassenger(Passenger passenger);
        Task<bool> DeletePassenger(int passengerID);
        Task<bool> EditPassenger(Passenger editPassenger);
        Task<Passenger> GetOnePassenger(int passengerID);
        Task<List<Passenger>> GetAllPassengers();

        Task<bool> AddPassengerType(PassengerType passengerType);
        Task<bool> DeletePassengerType(int passengerTypeID);
        Task<bool> EditPassengerType(PassengerType editPassengerType);
        Task<PassengerType> GetOnePassengerType(int passengerTypeID);
        Task<List<PassengerType>> GetAllPassengerTypes();
    }
}