using System;
using System.Collections.Generic;
using System.Linq;

namespace IndoorPositioning.Services
{
    public class RSSI
    {
        public void PreprocessRSSI(List<int> rssiStore)
        {
            List<int> tempList = new List<int>();
            for (int i = 0; i < rssiStore.Count; i++)
            {
                if (i < 10)
                {
                    tempList = rssiStore.GetRange(0, 10);
                }
                else
                {
                    tempList = rssiStore.GetRange(i - 10, 10);
                }

                double mean = GetMean(tempList);
                double std = GetStd(tempList);
                if (rssiStore[i] < mean - 2 * std || rssiStore[i] > mean + 2 * std)
                {
                    rssiStore.RemoveAt(i);
                }
            }
        }

        private double GetMean(List<int> list)
        {
            return list.Average();
        }

        private double GetStd(List<int> list)
        {
            double standardDeviation = 0;

            if (list.Any())
            {
                // Compute the average.     
                double avg = list.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = list.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (list.Count() - 1));
            }
            return standardDeviation;
        }
    }
}
