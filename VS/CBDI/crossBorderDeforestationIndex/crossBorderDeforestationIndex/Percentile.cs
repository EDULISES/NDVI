using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crossBorderDeforestationIndex
{
    class Percentile
    {
        #region Public methods ...
        /// <sumary>
        /// Calculate the value of percentile associated with the input array.
        /// The function use the Linear Interpolation Between Closest Ranks method.
        /// </sumary>
        /// <returns>
        /// Percentile value
        /// </returns>
        public double percentileInc(double[] _data, double _percentile)
        {
            // Sort the array
            double[] sortData = _data;
            Array.Sort(sortData);

            // First calculate the rank of the percentile
            double noValArray = sortData.Length;

            // x = ((percentile/100 )* (noValArray -1)) + 1
            double rank = ((_percentile * (noValArray - 1d)) / 100d) + 1d;

            // Get the integer part from rank
            Int64 rankInt = (Int64)Math.Floor(rank);

            // Get the decimal part from rank
            double rankDec = rank - rankInt;

            // Percentile (rank) = v(rankInt) + rankDecVal * (v(rankInt + 1) - v(rankInt))
            // Percentile works with index 1
            int diffIdx = 1;
            double valPercentile = sortData[rankInt + diffIdx] + rankDec + rankDec * (sortData[(rankInt + 1) + diffIdx] - sortData[rankInt + diffIdx]);
            return valPercentile;
        }
        #endregion
    }
}
