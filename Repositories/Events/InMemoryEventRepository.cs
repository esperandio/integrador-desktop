using MonitoramentoTempoOcioso.Interfaces.Events;
using System.Collections.Generic;

namespace MonitoramentoTempoOcioso.Repositories.Events
{
    class InMemoryEventRepository : IEventRepository
    {
        private readonly List<IEvent> _events;

        public InMemoryEventRepository()
        {
            _events = new List<IEvent>();
        }

        public void Add(IEvent @event)
        {
            _events.Add(@event);
        }

        public int Count()
        {
            return _events.Count;
        }

        public List<IEvent> GetEventsToSync()
        {
            return _events;
        }
    }
}
