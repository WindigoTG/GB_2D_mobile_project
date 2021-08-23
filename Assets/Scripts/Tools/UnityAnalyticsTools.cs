using System.Collections.Generic;

namespace Profile.Analytics
{
    internal class UnityAnalyticsTools : IAnalyticTools
    {
        public void SendMessage(string alias, IDictionary<string, object> eventData)
        {
            if (eventData == null)
                eventData = new Dictionary<string, object>();
            UnityEngine.Analytics.Analytics.EnableCustomEvent(alias, true);
            UnityEngine.Analytics.Analytics.CustomEvent(alias, eventData);
        }
    }
}