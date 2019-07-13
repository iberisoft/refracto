using System;

namespace Refracto.Services
{
    public interface IDevice : IDisposable
    {
        Readout Read();
    }
}
