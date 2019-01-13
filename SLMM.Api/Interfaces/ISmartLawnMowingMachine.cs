namespace SLMM.Api.Interfaces
{
    public interface ISmartLawnMowingMachine
    {
        (uint X, uint Y, string Orientation) GetPosition();
        void Move(Command command);
    }
}
