using System;

namespace MattBarton.NETMF.Utilities.Services
{
    public interface ITimerService
    {
        bool TimeoutReached();

        void Sleep(int milliseconds);
    }
}
