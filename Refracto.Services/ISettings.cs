namespace Refracto.Services
{
    public interface ISettings
    {
        string FileStorePath { get; }

        string SerialPort { get; }
    }
}
