using System;
using Refracto.Data;

namespace Refracto.DataInput
{
    public interface IDevice : IDisposable
    {
        Readout Read();
    }
}
