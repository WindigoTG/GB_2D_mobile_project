using System;

namespace Tools
{
    public interface IAdsDisplay
    {
        void ShowInterstitial();
        void ShowVideo(Action successShow);
    }
}
