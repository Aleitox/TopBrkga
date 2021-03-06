﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public interface IVehicleFleet
    {
        List<Vehicle> Vehicles { get; set; }

        List<Route> GetRoutes();

        Vehicle GetRoute(int vehicleNumber);

        double GetProfit();

        Vehicle GetById(int id);

        Vehicle GetByNumber(int number);
    }

    public class VehicleFleet : IVehicleFleet
    {
        public VehicleFleet()
        {
            Vehicles = new List<Vehicle>();
        }

        public List<Vehicle> Vehicles { get; set; }
        
        public List<Route> GetRoutes()
        {
            return Vehicles.Select(v => v.Route).ToList();
        }

        public Vehicle GetRoute(int vehicleNumber)
        {
            var vehicle = Vehicles.SingleOrDefault(v => v.Number == vehicleNumber);
            if (vehicle == null)
                throw new Exception(string.Format("No existe el vehiculo: '{0}'", vehicleNumber));
            return vehicle;
        }

        public double GetProfit()
        {
            return Vehicles.Sum(v => v.Route.GetProfit());
        }

        public Vehicle GetById(int id)
        {
            var vehicle = Vehicles.FirstOrDefault(v => v.Id == id);
            if(vehicle == null)
                throw new Exception(string.Format("Vehicle with id '{0}', not found", id));
            return vehicle;
        }

        public Vehicle GetByNumber(int number)
        {
            var vehicle = Vehicles.FirstOrDefault(v => v.Number == number);
            if (vehicle == null)
                throw new Exception(string.Format("Vehicle with Number '{0}', not found", number));
            return vehicle;
        }
    }
}
