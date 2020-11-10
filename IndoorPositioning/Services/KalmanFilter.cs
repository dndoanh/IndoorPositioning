using System;
namespace IndoorPositioning.Services
{
    public class KalmanFilter
    {
        private double A, H, r, p, q, d;
        private int caliRSSI = -62;

        public KalmanFilter(double A, double H, double r, double initial_p, double q, double initial_d)
        {
            this.A = A;
            this.H = H;
            this.q = q;
            this.r = r;
            p = initial_p;
            d = initial_d;
        }

        public double Output(double dist)
        {
            // time update - prediction
            d = A * d;
            p = A * p * A + q;

            // measurement update - correction
            double gain = p * H / (H * p * H + r);
            d += gain * (dist - H * d);
            p = (1 - gain * H) * p;

            return d;
        }

        public double CalculateBeaconDistance(int preRSSI)
        {
            if(preRSSI > caliRSSI)
            {
                return Math.Pow(10, preRSSI / caliRSSI);
            } else
            {
                return 0.9 * Math.Pow(7.71, preRSSI / caliRSSI) + 0.11;
            }
        }
    }
}
