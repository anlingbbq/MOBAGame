using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dto
{
    public class DtoVector3
    {
        public double X;
        public double Y;
        public double Z;

        public DtoVector3()
        {
            X = Y = Z = 0;
        }

        public DtoVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return "X:" + X + ", Y:" + Y + ", Z:" + Z;
        }
    }
}
