using System;
using System.Collections.Generic;
using System.Linq;

namespace Saut.StateModel
{
    public interface IStateModelService
    {
        IEnumerable<TProperty> GetProperties<TProperty>() where TProperty : IStateProperty;

        //TValue GetPropertyValue<TValue>(IStateProperty<TValue> Property);
        //TValue GetPropertyValue<TValue>(IJournaledStateProperty<TValue> Property, DateTime OnTime);
    }

    public static class StateModelHelper
    {
        public static TProperty GetProperty<TProperty>(this IStateModelService StateModelService) where TProperty : IStateProperty
        {
            return StateModelService.GetProperties<TProperty>().Single();
        }

//        public static IEnumerable<TValue> GetPropertyValue<TValue>(this IStateModelService StateModelService, IEnumerable<IStateProperty<TValue>> Properties)
//        {
//            return Properties.Select(StateModelService.GetPropertyValue);
//        }
//
//        public static IEnumerable<TValue> GetPropertyValue<TValue>(this IStateModelService StateModelService, IEnumerable<IJournaledStateProperty<TValue>> Properties,
//                                                                   DateTime OnTime)
//        {
//            return Properties.Select(p => StateModelService.GetPropertyValue(p, OnTime));
//        }
    }

    internal class MyProperty : IStateProperty<int>
    {
        public string Name { get; private set; }
        public int GetValue() { throw new NotImplementedException(); }
        public void UpdateValue(int NewValue) { throw new NotImplementedException(); }
    }

    internal class StateModelConsumer
    {
        private readonly IStateModelService _stateModelService;
        public StateModelConsumer(IStateModelService StateModelService) { _stateModelService = StateModelService; }

        public void DnMyStaff() { int myValue = _stateModelService.GetProperty<MyProperty>().GetValue(); }
    }
}
