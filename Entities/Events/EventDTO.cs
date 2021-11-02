using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoramentoTempoOcioso.Entities.Events
{
    struct EventDTO
    {
        public string ds_event;
        public DateTime dt_event;
        public uint nr_milliseconds_idle;
    }
}
