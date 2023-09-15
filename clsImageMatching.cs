using System;
using System.Draw;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace CrawlingProduct {
    public static class clsImageMatching {

        public static bool ImageMatching(Bitmap bitmap) {
            bitmap.Save("img_"+DateTime.Now.ToString("yyyy-MM-dd-hhmmss"+".png", ImageFormat.Png);
            using Math aImage = BitmapConverter.ToMat(bitmap);
            using Mat bImage = CvInvoke.Imread("b.png", ImreadModes.Color);
            //using Mat aImage = CvInvoke.Imread("a.png", ImreadModes.Color);

            if (bImage.IsEmpty || aImage.IsEmpty) {
                Console.WriteLine("이미지를 불러올 수 없습니다.");
                return;
            }

            using Mat result = new Mat();
            // 템플릿 매칭 수행
            CvInvoke.MatchTemplate(bImage, aImage, result, TemplateMatchingType.CcoeffNormed);
            double minVal = 0, maxVal = 0;
            Point minPoint = new Point(), maxPoint = new Point();
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minPoint, ref maxPoint);

            // 매칭 결과가 임계치(예: 0.95) 이상인 경우 매칭 성공으로 간주
            if (maxVal > 0.95) {
                Console.WriteLine($"a.png가 b.png 내에 위치: {maxPoint}");
                return true;
            }
            else {
                Console.WriteLine("a.png는 b.png 내에 포함되어 있지 않습니다.");
                return false;
            }
        }

        public static void DoCaptureImage(int _refX, int _refY, int _imgW, int _imgH) {
            if (filePath != null) {
                if (_imgW == 0 || _imgH == 0)
                    return;

                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)_imgW, (int)_imgH)) {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap)) {
                        g.CopyFromScreen(_refX, _refY, 0, 0, bitmap.Size);
                    }

                    bitmap.Save(filePath, ImageFormat.Png);
                }
            }
        }
    }
}