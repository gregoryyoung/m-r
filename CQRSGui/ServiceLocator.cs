using SimpleCQRS;

namespace CQRSGui
{
    public static class ServiceLocator
    {
        public static CommandBus Bus { get; set; }
       
    }
}