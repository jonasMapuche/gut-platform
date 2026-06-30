using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Gut.Views;

public partial class CameraPopup : Popup
{
    public CancellationToken Token => CancellationToken.None;

    public CameraPopup()
	{
		InitializeComponent();
        cameraView.Loaded += CameraViewControl_CamerasLoaded;
    }

    private void CameraViewControl_CamerasLoaded(object sender, EventArgs e)
    {
        /*
        var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;

        double displayWidth = mainDisplayInfo.Width / mainDisplayInfo.Density;
        double displayHeight = mainDisplayInfo.Height / mainDisplayInfo.Density;

        cameraView.WidthRequest = 300; // displayWidth;
        cameraView.HeightRequest = 100 * 0.7; // displayHeight * 0.7;
        */
    }

    private async void OnCaptureClicked(object sender, EventArgs e)
    {
        cameraView.MediaCaptured += OnMediaCaptured;
        cameraView.CaptureImage(Token);
    }

    private void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
    {
        var memoryStream = new MemoryStream();
        e.Media.CopyTo(memoryStream);
        //viewModel.Bytes = memoryStream.ToArray();
    }

    private async void OnCloseClickedAsync(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}