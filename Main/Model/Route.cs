using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using Main.GuidedLocalSearchHeuristics;
using Main.Helpers;

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
        
        public decimal GetDistance()
        {
            if (!Destinations.Any())
                return StartingPoint.GetDistanceTo(EndingPoint);

            var distance = GetDistanceWithoutFinalReturn();
            distance += Destinations.Last().GetDistanceTo(EndingPoint);
            return distance;
        }

        // TODO test
        public decimal GetDistanceWithout(List<int> list)
        {
            if(list.Max(x => x) > Destinations.Count)
                throw new Exception("Error en GetDistanceWithout");

            decimal distance = 0;
            var iterator = new DestinationIterator(this, list);

            do
                distance += iterator.Current.GetDistanceTo(iterator.GetNext());
            while (iterator.MoveIterator());

            return 0;
        }

        public decimal GetDistanceWithoutFinalReturn()
        {
            if (Destinations.Count == 0)
                return 0;

            var distance = StartingPoint.GetDistanceTo(Destinations.First());

            for (var index = 0; index < Destinations.Count - 1; index++)
                distance += Destinations[index].GetDistanceTo(Destinations[index + 1]);

            return distance;
        }

        // WARNING: Usar sin modificar la lista ni sus elementos
        public List<Destination> GetDestinations
        {
            get { return Destinations; }
        }

        public void AddDestinationAt(Destination unvistedDestination, int atPosition)
        {
            if (IsCogActivated)
                CenterOfGravity.AddDestination(unvistedDestination);
            Destinations.Insert(atPosition, unvistedDestination);
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
            Destinations = Destinations.Where(x => x.Id != destination.Id).ToList();
        }

        public void RemoveDestinationAt(int atPosition)
        {
            if (IsCogActivated)
            {
                var destination = Destinations[atPosition];
                CenterOfGravity.RemoveDestination(destination);
            }
            Destinations.RemoveAt(atPosition);
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

            var distance = GetDistanceWithoutFinalReturn();
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
        
        public Tuple<Tract, Tract> GetTracksForDestinationAt(int i)
        {
            var firstFrom = i == 0 ? StartingPoint : Destinations[i - 1];
            var secondTo = i == Destinations.Count - 1 ? EndingPoint : Destinations[i + 1];

            return new Tuple<Tract, Tract>(new Tract(){ From = firstFrom, To = Destinations[i]}, new Tract() {From = Destinations[i], To = secondTo});
        }

        public List<DestinationNeighbors> GetAllDestinationNeighbors()
        {
            var res = new List<DestinationNeighbors>();
            
            for (var index = 0; index < Destinations.Count; index++)
            {
                var from = index == 0 ? StartingPoint : Destinations[index - 1];
                var to = index == Destinations.Count - 1 ? EndingPoint : Destinations[index + 1];
                res.Add(new DestinationNeighbors(from, Destinations[index], to));
            }
            return res;
        }

        public bool SwapIfImprovesDistance(int a, int b)
        {
            var oldRouteDistance = GetDistance();

            var alreadyAdded = new List<int>();
            var currentDistance = GetTracksDistanceFor(a, ref alreadyAdded);
            currentDistance += GetTracksDistanceFor(b, ref alreadyAdded);

            Swap(a, b);

            alreadyAdded = new List<int>();
            var swapedDistance = GetTracksDistanceFor(a, ref alreadyAdded);
            swapedDistance += GetTracksDistanceFor(b, ref alreadyAdded);

            // Si no mejoro nada, volver a atraz
            if (currentDistance <= swapedDistance)
                Swap(a, b);
            else
            {
                var newRouteDistance = GetDistance();
                if (newRouteDistance >= oldRouteDistance)
                    throw new Exception("What's up doc?");
            }


            return currentDistance > swapedDistance;
        }

        public decimal GetTracksDistanceFor(int a, ref List<int> alreadyAdded)
        {
            // TODO revisar que pueden haber bugs
            decimal distance = 0;
            if (a == 0)
            {
                if (!alreadyAdded.Contains(-1))
                {
                    distance += EuclidianCalculator.GetDistanceBetween(StartingPoint, Destinations[a]);
                    alreadyAdded.Add(-1);
                }
            }
            else
            {
                if (!alreadyAdded.Contains(a - 1))
                {
                    distance += EuclidianCalculator.GetDistanceBetween(Destinations[a - 1], Destinations[a]);
                    alreadyAdded.Add(a - 1);
                }
            }

            if (a == Destinations.Count - 1)
            {
                if (!alreadyAdded.Contains(a))
                {
                    distance += EuclidianCalculator.GetDistanceBetween(Destinations[a], EndingPoint);
                    alreadyAdded.Add(a);
                }
            }
            else
            {
                if (!alreadyAdded.Contains(a))
                {
                    distance += EuclidianCalculator.GetDistanceBetween(Destinations[a], Destinations[a + 1]);
                    alreadyAdded.Add(a);
                }
            }

            return distance;
        }

        // TODO TEST
        public void Swap(int a, int b)
        {
            var tmp = Destinations[a];
            Destinations[a] = Destinations[b];
            Destinations[b] = tmp;
        }
    }

    public class DestinationIterator
    {
        private Route Route { get; set; }

        private List<int> Forbidden { get; set; }

        public DestinationIterator(Route route, List<int> forbidden)
        {
            Route = route;
            Forbidden = forbidden;
            CurrentAt = -1;
        }

        public Destination Current { get; set; }

        public int CurrentAt { get; set; }

        public bool MoveIterator()
        {
            if (Current.Id == Route.EndingPoint.Id)
                return false;

            CurrentAt++;
            while (Forbidden.Any(x => x == CurrentAt))
                CurrentAt++;

            if (CurrentAt < Route.GetDestinations.Count)
                Current = Route.GetDestinationAt(CurrentAt);
            else
                Current = Route.EndingPoint;

            return true;
        }

        public Destination GetNext()
        {
            var currentCopy = CurrentAt;
            currentCopy++;
            while(Forbidden.Any(x => x == CurrentAt))
                currentCopy++;

            return Route.GetDestinationAt(currentCopy);
        }
    }
}
