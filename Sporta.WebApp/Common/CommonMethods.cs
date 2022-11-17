using System;

namespace Sporta.WebApp.Common
{
    /// <summary>
    /// Common Methods
    /// </summary>
    public static class CommonMethods
    {
        /// <summary>
        /// Generate Random Token
        /// </summary>
        /// <returns></returns>
        public static int GenerateRandomToken()
        {
            Random generator = new();
            return generator.Next(100000, 1000000);
        }

        /// <summary>
        /// Convert Seconds To Minutes
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static string ConvertSecondsToMinutes(long sec)
        {
            TimeSpan ts = TimeSpan.FromSeconds(sec);
            string hour = "", minute = "", seconds = "";
            if (ts.Hours > 0)
            {
                hour = ts.Hours > 1 ? ts.Hours + " hours " : ts.Hours + " hour ";
            }

            if (ts.Minutes > 0)
            {
                minute = ts.Minutes > 1 ? ts.Minutes + " min(s) " : ts.Minutes + " min ";
            }

            if (ts.Seconds > 0)
            {
                //  seconds = ts.Seconds > 1 ? ts.Seconds + " sec(s) " : ts.Seconds + " sec ";
                seconds = ts.Seconds + " sec ";
            }

            return $"{hour}{minute}{seconds}";
        }
    }
}
