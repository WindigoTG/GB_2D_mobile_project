using System.Collections.Generic;

namespace Profile.Analytics
{
    public interface IAnalyticTools
    {
        void SendMessage(string alias, IDictionary<string, object> eventData = null);
    }
}
