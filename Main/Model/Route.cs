using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Main.GuidedLocalSearchHeuristics;

namespace Main.Model
{
    public class Route
    {
        public Route(Destination startingPoint, Destination endingPoint)
        {
            Destinations = new List<Destination>();
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
            IsCogActivated = false;
        }

        protected List<Destination> Destinations { get; set; }

        public Destination StartingPoint { get; set; }

        public Destination EndingPoint { get; set; }

        public CenterOfGravity CenterOfGravity { get; set; }
 

        protected bool IsCogActivated { get; set; }

        public void ActivateCog()
        {
            CenterOfGravity = new CenterOfGravity(Destinations);
            IsCogActivated = true;
        }

        public int RouteLenght()
        {
            return Destinations.Count();
        }

        public Destination GetDestinationAt(int position)
        {
            return Destinations[position];
        }

        public Destination CurrentLastDestination {
            get
            {
                return Destinations.Count > 0 ? Destinations.Last() : StartingPoint;
            }
        }

        public int GetProfit()
        {
            return Destinations.Sum(d => d.Profit);
        }

        public decimal GetDistance(IMap map)
        {
            if (!Destinations.Any())
                return map.GetDistance(StartingPoint, EndingPoint);

            var distance = GetDistanceWithoutFinalReturn(map);
            distance += map.GetDistance(Destinations.Last(), EndingPoint);
            return distance;
        }

        public decimal GetDistanceWithoutFinalReturn(IMap map)
        {
            if (Destinations.Count == 0)
                return 0;

            var distance = map.GetDistance(StartingPoint, Destinations.First());

            for (var index = 0; index < Destinations.Count - 1; index++)
                distance += map.GetDistance(Destinations[index], Destinations[index + 1]);

            return distance;
        }

        // WARNING: Usar sin modificar la lista ni sus elementos
        public List<Destination> GetDestinations
        {
            get { return Destinations; }
        }

        public void AddDestination(Destination destination)
        {
            if(IsCogActivated)
                CenterOfGravity.AddDestination(destination);
            Destinations.Add(destination);
        }

        public void RemoveDestination(Destination destination)
        {
            if (IsCogActivated)
                CenterOfGravity.RemoveDestination(destination);
            Destinations.Add(destination);
        }
        
        public override string ToString()
        {
            var routeString = string.Empty;

            for (var index = 0; index < Destinations.Count; index++)
            {
                routeString += Destinations[index].Coordinate.ToString();
                if (index < Destinations.Count - 1)
                    routeString += " -> ";
            }
            return routeString;
        }

        public bool IsEquivalentTo(Route anotherRoute)
        {
            if (Destinations.Count != anotherRoute.Destinations.Count)
                return false;

            return !Destinations.Where((t, index) => t.Id != anotherRoute.Destinations[index].Id).Any();
        }

        public decimal GetDistanceAdding(IMap map, Destination destination)
        {
            if (!Destinations.Any())
                return map.GetDistance(StartingPoint, destination) + map.GetDistance(destination, EndingPoint);

            var distance = GetDistanceWithoutFinalReturn(map);
            distance += map.GetDistance(Destinations.Last(), destination);
            distance += map.GetDistance(destination, EndingPoint);
            return distance;
        }

        public List<Tract> GetCurrentTracks()
        {
            var tracks = new List<Tract>();

            Tract track;
            if (!Destinations.Any())
            {
                track = new Tract() { From = StartingPoint, To = EndingPoint};
                tracks.Add(track);
            }
            else
            {
                track = new Tract() { From = StartingPoint, To = Destinations[0] };
                tracks.Add(track);
                for (var index = 0; index < Destinations.Count - 1; index++)
                {
                    track = new Tract() { From = Destinations[index], To = Destinations[index + 1] };
                    tracks.Add(track);
                }
                track = new Tract() { From = Destinations[Destinations.Count - 1], To = EndingPoint };
                tracks.Add(track);
            }
            return tracks;
        }

        // TODO: Test Method (Importante)
        public void AddDestinationInPosition(Destination unvistedDestination, int bestInsertPosition)
        {
            Destinations.Insert(bestInsertPosition + 1, unvistedDestination);
        }
    }
}
