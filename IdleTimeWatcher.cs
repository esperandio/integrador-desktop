﻿using System;
using System.Threading;

namespace MonitoramentoTempoOcioso
{
    class IdleTimeWatcher
    {
        private Timer _timer;
        private readonly IEventRepository _eventRepository;
        private uint _totalIdleTime = 0;
        private readonly uint _maxIdleTimeAllowed = 5000;

        public IdleTimeWatcher(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void Start()
        {
            _eventRepository.Add(new StartWatcherEvent(DateTime.Now));
            _timer = new Timer(TimerCallback, null, 0, 1000);
        }

        private void TimerCallback(Object o)
        {
            uint idleTime = UserInputInfo.GetIdleTime();

            if (idleTime < _totalIdleTime)
            {
                Notify(_totalIdleTime);
                _totalIdleTime = 0;
                return;
            }

            if (idleTime < _maxIdleTimeAllowed)
            {
                return;
            }

            _totalIdleTime = idleTime;
        }

        public void Stop()
        {
            _eventRepository.Add(new StopWatcherEvent(DateTime.Now));

            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        private void Notify(uint idleTime)
        {
            _eventRepository.Add(new IdleTimeEvent(DateTime.Now, idleTime));
        }
    }
}
