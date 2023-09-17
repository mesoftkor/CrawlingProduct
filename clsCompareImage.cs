using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace CrawlingProduct {
    internal class clsCompareImage {
        internal clsCompareImage() {

        }

        internal bool CompareImage() {
            //https://learn.microsoft.com/ko-kr/cpp/windows/latest-supported-vc-redist?view=msvc-170
            // 2. 이미지 매칭
            var sourceImage = new Image<Bgr, byte>("image_traffic_01.png");
            var templateImage = new Image<Bgr, byte>("check_img_1.png");

            //img_traffic_1.png
            //check_img_1.png
            var resultImage = sourceImage.MatchTemplate(templateImage, TemplateMatchingType.CcoeffNormed);

            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            resultImage.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            if (maxValues[0] > 0.8) // Threshold: 0.8은 매칭 기준
            {
                Console.WriteLine("이미지를 찾았습니다.");
                return true;
            }
            else {
                Console.WriteLine("이미지를 찾지 못했습니다.");
                return false;
            }
        }
    }
}
