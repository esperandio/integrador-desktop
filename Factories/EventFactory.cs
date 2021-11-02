using MonitoramentoTempoOcioso.Entities.Events;
using MonitoramentoTempoOcioso.Interfaces.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoramentoTempoOcioso.Factories
{
    abstract class EventFactory
    {
        public static IEvent Create(EventDTO eventDTO)
        {
            IEvent @event = null;

            switch (eventDTO.ds_event)
            {
                case "ExitApplicationEvent":
                    @event = new ExitApplicationEvent(eventDTO.dt_event);
                    break;
                case "IdleTimeEvent":
                    @event = new IdleTimeEvent(eventDTO.dt_event, eventDTO.nr_milliseconds_idle);
                    break;
                case "StartApplicationEvent":
                    @event = new StartApplicationEvent(eventDTO.dt_event);
                    break;
                case "StartWatcherEvent":
                    @event = new StartWatcherEvent(eventDTO.dt_event);
                    break;
                case "StopWatcherEvent":
                    @event = new StopWatcherEvent(eventDTO.dt_event);
                    break;
            }

            return @event;
        }

    }
}
