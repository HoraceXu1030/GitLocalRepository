using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.VisionModels
{
    public class SysParams
    {
        public string IPAddress { get; set; }

        public string CamName { get; set; }

        public string DrvName { get; set; }

        public string ModelName { get; set; }

        public int StartButton { get; set; }

        public float ZeroVel { get; set; } = 50;

        public float ZeroVelMin { get; set; } = 0;

        public float ZeroAcc { get; set; } = 500;

        public float ZeroDec { get; set; } = 500;

        public float ZeroSramp { get; set; } = 100;

        public MotionParams MotionParams { get; set; } = new MotionParams();

        public int Unit0 { get; set; } = 1;
        public int Unit1 { get; set; } = 1;
        public int Unit2 { get; set; } = 1;
        public int Unit3 { get; set; } = 1;

        public int Atype0 { get; set; } = 1;
        public int Atype1 { get; set; } = 1;
        public int Atype2 { get; set; } = 1;
        public int Atype3 { get; set; } = 1;

        public int InvertStep0 { get; set; } = 1;
        public int InvertStep1 { get; set; } = 1;
        public int InvertStep2 { get; set; } = 1;
        public int InvertStep3 { get; set; } = 1;



    }
}
