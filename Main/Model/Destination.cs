
using System.Security.AccessControl;
using Main.Entities;
using Main.Helpers;

namespace Main.Model
{
    public class Destination
    {
        public Destination(int id, int profit, Coordinate coordinate, string description, int positionIndex)
        {
            Id = id;
            Profit = profit;
            Coordinate = coordinate;
            Description = description;
            PositionIndex = positionIndex;
        }

        public Destination(Destiny destiny, int positionIndex)
        {
            Id = destiny.Id;
            Profit = destiny.Profit;
            Coordinate = new Coordinate(destiny.X, destiny.Y);
            PositionIndex = positionIndex;
        }

        // Valor de 1 a TotalDestinations.Count, se asigna automatico incremental
        public int Id { get; set; }

        public int Profit { get; set; }

        public Coordinate Coordinate { get; set; }

        public string Description { get; set; }

        // Position va de 1 a TotalDestinations.Count
        // Si el id NO viene de la base de datos, seguramente tenga el mismo valor del Id
        // Se usa para el matching con RandomKey.PositionIndex
        public int PositionIndex { get; private set; }

        public decimal GetDistanceTo(Destination anotherDestination)
        {
            return EuclidianCalculator.GetDistanceBetween(this.Coordinate.X, this.Coordinate.Y, anotherDestination.Coordinate.X, anotherDestination.Coordinate.Y);
        }

    }
}
