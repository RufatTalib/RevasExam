using Revas.Models;
using System.Reflection;

namespace Revas.Helpers
{
    public static class SmartUpdater
    {

        public static void SmartUpdate(this BaseEntity portfolioDest, BaseEntity portfolioSrc)
        {
            PropertyInfo[] Destination = portfolioDest.GetType().GetProperties();
            PropertyInfo[] Source = portfolioSrc.GetType().GetProperties();
            object? value;
            for (int i = 0; i < Destination.Length; i++)
            {
                value = Source[i].GetValue(portfolioSrc);

                if (value != null)
                    Destination[i].SetValue(portfolioDest, value);
            }

        }
    }
}
