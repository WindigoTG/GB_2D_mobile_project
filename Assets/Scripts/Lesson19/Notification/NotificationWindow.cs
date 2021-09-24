using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif


public class NotificationWindow : MonoBehaviour
{
    private const string ANDROID_NOTIFIER_ID = "android_notifier_id";
    private const string IOS_NOTIFIER_ID = "ios_notifier_id";

    [SerializeField]
    private Button _buttonNotification;

    private void Start()
    {
        _buttonNotification.onClick.AddListener(CreateNotification);
    }

    private void OnDestroy()
    {
        _buttonNotification.onClick.RemoveAllListeners();
    }

    private void CreateNotification()
    {
#if UNITY_ANDROID
        var androidSettingsChanel = new AndroidNotificationChannel
        {
            Id = ANDROID_NOTIFIER_ID,
            Name = "Game Notifier",
            Importance = Importance.High,
            CanBypassDnd = true,
            CanShowBadge = true,
            Description = "Enter the game and get free crystals",
            EnableLights = true,
            EnableVibration = true,
            LockScreenVisibility = LockScreenVisibility.Public
        };

        AndroidNotificationCenter.RegisterNotificationChannel(androidSettingsChanel);

        var androidSettingsNotification = new AndroidNotification
        {
            Color = Color.green,
            ShowTimestamp = true,
            Title = "Test notification",
            Text = "Instant notification"
        };

        AndroidNotificationCenter.SendNotification(androidSettingsNotification, ANDROID_NOTIFIER_ID);

        androidSettingsNotification = new AndroidNotification
        {
            Color = Color.blue,
            ShowTimestamp = true,
            Title = "Another test notification",
            Text = "Scheduled notification",
            FireTime = DateTime.UtcNow + TimeSpan.FromSeconds(120)
        };
        AndroidNotificationCenter.SendNotification(androidSettingsNotification, ANDROID_NOTIFIER_ID);

#elif UNITY_IOS
       var iosSettingsNotification = new iOSNotification
       {
           Identifier = IOS_NOTIFIER_ID,
           Title = "Game Notifier",
           Subtitle = "Subtitle notifier",
           Body = "Enter the game and get free crystals",
           Badge = 1,
           Data = "01/02/2021",
           ForegroundPresentationOption = PresentationOption.Alert,
           ShowInForeground = false
       };
      
       iOSNotificationCenter.ScheduleNotification(iosSettingsNotification);
#endif
    }
}
