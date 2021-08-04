using System;
using SkiaSharp;

namespace Uzor.Views
{
    class TouchManipulationManager
    {/*

        public SKMatrix OneFingerManipulate(SKPoint prevPoint, SKPoint newPoint, SKPoint pivotPoint)
        {
           
        }

        public SKMatrix TwoFingerManipulate(SKPoint prevPoint, SKPoint newPoint, SKPoint pivotPoint)
        {
            SKMatrix touchMatrix = SKMatrix.MakeIdentity();
            SKPoint oldVector = prevPoint - pivotPoint;
            SKPoint newVector = newPoint - pivotPoint;

            if (Mode == TouchManipulationMode.ScaleRotate ||
                Mode == TouchManipulationMode.ScaleDualRotate)
            {
                // Find angles from pivot point to touch points
                float oldAngle = (float)Math.Atan2(oldVector.Y, oldVector.X);
                float newAngle = (float)Math.Atan2(newVector.Y, newVector.X);

                // Calculate rotation matrix
                float angle = newAngle - oldAngle;
                touchMatrix = SKMatrix.MakeRotation(angle, pivotPoint.X, pivotPoint.Y);

                // Effectively rotate the old vector
                float magnitudeRatio = Magnitude(oldVector) / Magnitude(newVector);
                oldVector.X = magnitudeRatio * newVector.X;
                oldVector.Y = magnitudeRatio * newVector.Y;
            }

            float scaleX = 1;
            float scaleY = 1;

            if (Mode == TouchManipulationMode.AnisotropicScale)
            {
                scaleX = newVector.X / oldVector.X; // ! не, хуйня
                scaleY = newVector.Y / oldVector.Y;

            }
            else if (Mode == TouchManipulationMode.IsotropicScale ||
                     Mode == TouchManipulationMode.ScaleRotate ||
                     Mode == TouchManipulationMode.ScaleDualRotate)
            {
                scaleX = scaleY = Magnitude(newVector) / Magnitude(oldVector); // вот это важно !
            }

            if (!float.IsNaN(scaleX) && !float.IsInfinity(scaleX) &&
                !float.IsNaN(scaleY) && !float.IsInfinity(scaleY))
            {
                SKMatrix.PostConcat(ref touchMatrix,
                    SKMatrix.MakeScale(scaleX, scaleY, pivotPoint.X, pivotPoint.Y));
            }

            return touchMatrix;
        }

        float Magnitude(SKPoint point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
        }*/
    }
}
