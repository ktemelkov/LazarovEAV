using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.UI.Util
{
    class ImagePanelUtil
    {
        public enum PointLayer
        {
            FRONT,
            BEHIND,
            HIDDEN
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PointLayer CalcPointLayer(int pIndex, int sIndex)
        {
            int altImageIndex = 0xFFFF;

            if (sIndex == 0)
                altImageIndex = 1;
            else if (sIndex == 1)
                altImageIndex = 0;

            return sIndex == pIndex ? PointLayer.FRONT : (pIndex == altImageIndex ? PointLayer.BEHIND : PointLayer.HIDDEN);
        }
    }
}
