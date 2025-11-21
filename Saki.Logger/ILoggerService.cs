using Saki.AutoFac.AutofacRegister;

namespace Saki.Logging
{
    public interface ILoggerService:ITransitDependency
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception ex = null);
    }
}
