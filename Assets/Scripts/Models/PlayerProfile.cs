using Profile.Analytics;
using Tools;
using UnityEngine.Advertisements;

namespace Profile
{
    public class PlayerProfile
    {
        public SubscriptionProperty<GameState> CurrentState { get; }
        public InputMethod InputMethod { get; private set; }

        public Car CurrentCar { get; }

        public IAnalyticTools AnalyticTools { get; }

        public IAdsDisplay AdsDisplay { get; }

        public IUnityAdsListener AdsListener { get; }

        public PlayerProfile(float carSpeed, IAnalyticTools analyticTools, UnityAdsTools ads)
        {
            CurrentState = new SubscriptionProperty<GameState>();
            CurrentCar = new Car(carSpeed);
            AnalyticTools = analyticTools;
            AdsListener = ads;
            AdsDisplay = ads;
        }

        public void SetInputMethod(int value)
        {
            InputMethod = (InputMethod)value;
        }
    }

}