using System;
using System.Collections.Generic;
using Communications;
using Communications.Can;
using Modules;
using Saut.Communication.Modules;
using Saut.StateModel.Modules;

namespace PropertyVisualizer
{
    class MyTestBootstrapper : BootstrapperBase
    {
        private readonly Func<ISocketSource<ICanSocket>> _socketFactory;
        public MyTestBootstrapper(Func<ISocketSource<ICanSocket>> SocketFactory) { _socketFactory = SocketFactory; }

        protected override IEnumerable<IModule> EnumerateModules()
        {
            return new IModule[]
                   {
                       new BlokFrameProcessorsModule(),
                       new StateModelModule(),
                       new CommonPropertiesModule(),
                       new MessageProcessingModule(),
                       new MyTestSocketSourceModule(_socketFactory),
                       new DecoderModule()
                   };
        }
    }
}