using System.ComponentModel.DataAnnotations;

namespace SLMM.Api
{
    public enum Command
    {
        [Display(Name = "Turn anticlockwise")]
        MoveAnticlockwise,
        [Display(Name = "Turn clockwise")]
        MoveClockwise,
        [Display(Name = "Move a step forward")]
        MoveOneStepForward,
    }
}
