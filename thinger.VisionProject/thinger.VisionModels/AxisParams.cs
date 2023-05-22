using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.VisionModels
{
   public  class AxisParams
    {
        public short AxisNo { get; set; } = 0;

        public float AxisJogVel { get; set; } = 50;

        public float AxisJogVelMin { get; set; } = 0;

        public float AxisJogAcc { get; set; } = 500;

        public float AxisJogDec { get; set; } = 500;

        public float AxisJogSramp { get; set; } = 100;

        public float AxisMoveVel { get; set; } = 50;

        public float AxisMoveVelMin { get; set; } = 0;

        public float AxisMoveAcc { get; set; } = 500;

        public float AxisMoveDec { get; set; } = 500;

        public float AxisMoveSramp { get; set; } = 100;

        public float ZeroDistance { get; set; } = 20;

    }
}
