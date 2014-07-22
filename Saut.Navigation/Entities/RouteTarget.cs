using System;
using Saut.Navigation.Interfaces.Elements;

namespace Saut.Navigation.Entities
{
    public class RouteTarget
    {
        public RouteTarget(IRouteElement Element, double Disstance)
        {
            this.Disstance = Disstance;
            this.Element = Element;
        }

        public Double Disstance { get; private set; }
        public IRouteElement Element { get; private set; } 
    }
}