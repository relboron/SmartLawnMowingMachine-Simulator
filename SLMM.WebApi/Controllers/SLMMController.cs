using Microsoft.AspNetCore.Mvc;
using SLMM.Api;
using SLMM.Api.Interfaces;
using System;
using System.Threading.Tasks;

namespace SLMM.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlmmController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly ISmartLawnMowingMachine _smartLawnMowingMachine;
        #endregion

        #region CTOR
        public SlmmController(ISmartLawnMowingMachine smartLawnMowingMachine)
        {
            _smartLawnMowingMachine = smartLawnMowingMachine;
        }
        #endregion

        #region PUBLIC ACTIONS
        [HttpGet]
        public ActionResult<object> Index()
        {
            return new
            {
                Commands = new
                {
                    Get_Position = "Get or get",
                    Move_Forward = "move/MoveOneStepForward or move/moveonestepforward",
                    Turn_Clockwise = "move/MoveClockwise or move/moveclockwise",
                    Turn_Anticlockwise = "move/MoveAnticlockwise or move/moveanticlockwise",
                }
            };
        }

        [HttpGet("get")]
        public ActionResult<object> GetCurrentLocation()
        {
            return GetPosition();
        }

        [HttpGet("move/{command}")]
        public async Task<ActionResult> Move(string command)
        {
            if (Enum.TryParse(command, true, out Command cmd))
            {
                await Task.Run(() => _smartLawnMowingMachine.Move(cmd));
                return Ok(GetPosition(cmd.ToString()));
            }
            else
            {
                return BadRequest("Not valid command");
            }
        }
        #endregion

        #region PRIVATE METHODS
        private object GetPosition(string action = "")
        {
            var res = _smartLawnMowingMachine.GetPosition();

            if (String.IsNullOrEmpty(action))
            {
                return new
                {
                    X = res.X,
                    Y = res.Y,
                    Orientation = res.Orientation
                };
            }
            else
            {
                return new
                {
                    operation = action,
                    X = res.X,
                    Y = res.Y,
                    Orientation = res.Orientation
                };
            }
        }
        #endregion
    }
}