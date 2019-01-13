using SLMM.Api.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLMM.Api
{
    public class SmartLawnMowingMachine : ISmartLawnMowingMachine
    {
        #region PRIVATE FIELDS
        private readonly System.Object _locker = new System.Object();
        private readonly Garden _garden;
        private readonly Location _currentLocation = new Location();
        #endregion

        #region CTOR
        public SmartLawnMowingMachine(Garden garden, uint x, uint y, string orientation)
        {
            this._garden = garden;
            this._currentLocation.X = x;
            this._currentLocation.Y = y;
            if (Enum.TryParse(orientation, true, out Orientation dir))
            {
                _currentLocation.Orientation = dir;
            }
            else
            {
                throw new ArgumentException("Wrong orientation! It must be one of the following values: North/East/South/West");
            }
        }
        #endregion

        #region PUBLIC METHODS
        public (uint X, uint Y, string Orientation) GetPosition()
        {
            Task.Run(() => null).GetAwaiter();
            Debug.WriteLine($"GetPosition() => X = {_currentLocation.X}, Y = {_currentLocation.Y}, DIR = {_currentLocation.Orientation.ToString()}");
            return (_currentLocation.X, _currentLocation.Y, _currentLocation.Orientation.ToString());
        }

        public void Move(Command command)
        {
            lock (_locker)
            {
                switch (command)
                {
                    case Command.MoveAnticlockwise:
                    case Command.MoveClockwise:
                        Debug.WriteLine($"CALL => ChangeOrientation({command})");
                        Task.Run(() => ChangeOrientation(command)).Wait();
                        break;
                    case Command.MoveOneStepForward:
                        Debug.WriteLine("CALL => MoveForward()");
                        Task.Run(() => MoveForward()).Wait();
                        break;
                }
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void ChangeOrientation(Command command)
        {
            Thread.Sleep(2000);

            switch (command)
            {
                case Command.MoveAnticlockwise:
                    _currentLocation.Orientation = Enum.IsDefined(typeof(Orientation), --_currentLocation.Orientation)
                        ? _currentLocation.Orientation
                        : Enum.GetValues(typeof(Orientation)).Cast<Orientation>().Max();
                    break;
                case Command.MoveClockwise:
                    _currentLocation.Orientation = Enum.IsDefined(typeof(Orientation), ++_currentLocation.Orientation)
                        ? _currentLocation.Orientation
                        : Enum.GetValues(typeof(Orientation)).Cast<Orientation>().Min();
                    break;
            }
            Debug.WriteLine($"ChangeOrientation({command}) => Executed");
        }

        private void MoveForward()
        {
            Thread.Sleep(5000);

            switch (_currentLocation.Orientation)
            {
                case Orientation.Nord:
                    if (_garden.IsValidPosition(_currentLocation.X, _currentLocation.Y - 1))
                        _currentLocation.Y--;
                    else
                        return;
                    break;
                case Orientation.East:
                    if (_garden.IsValidPosition(_currentLocation.X + 1, _currentLocation.Y))
                        _currentLocation.X++;
                    else
                        return;
                    break;
                case Orientation.South:
                    if (_garden.IsValidPosition(_currentLocation.X, _currentLocation.Y + 1))
                        _currentLocation.Y++;
                    else
                        return;
                    break;
                case Orientation.West:
                    if (_garden.IsValidPosition(_currentLocation.X - 1, _currentLocation.Y))
                        _currentLocation.X--;
                    else
                        return;
                    break;
            }
            Debug.WriteLine($"MoveForward() => Executed");
        }

        #endregion
    }
}
