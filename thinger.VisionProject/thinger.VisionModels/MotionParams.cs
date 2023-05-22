using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.VisionModels
{
    public class MotionParams
    {
        public AxisParams XAxisParams { get; set; } = new AxisParams();

        public AxisParams YAxisParams { get; set; } = new AxisParams();

        public AxisParams ZAxisParams { get; set; } = new AxisParams();

        public AxisParams UAxisParams { get; set; } = new AxisParams();
    }
}
