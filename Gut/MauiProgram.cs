using CommunityToolkit.Maui;
using Gut.Interfaces;
using Gut.Services;
using Gut.ViewModels;
using Gut.Views;
using Microsoft.Extensions.Logging;
using static Android.Provider.CalendarContract;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Platforms.Android;

#if ANDROID
using Gut.Platforms.Android.Services;
#endif

namespace Gut
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera()
                .RegisterFirebaseServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
            builder.Services.AddSingleton<IBluetoothService, BluetoothService>();
            builder.Services.AddSingleton<IWiFiService, WiFiService>();
            builder.Services.AddSingleton<IVPNClientService, VPNClientService>();
            builder.Services.AddSingleton<ISMSService, SMSService>();
#endif

            builder.Services.AddSingleton<PerceptionService>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<HomeView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events => {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) =>
                    CrossFirebase.Initialize(activity, () => Platform.CurrentActivity, CreateCrossFirebaseSettings())));
#endif
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(
                isAuthEnabled: true,
                isCloudMessagingEnabled: true);
        }
    }
}
