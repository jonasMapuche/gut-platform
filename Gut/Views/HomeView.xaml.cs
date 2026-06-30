using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Gut.Services;
using Gut.ViewModels;

namespace Gut.Views;

public partial class HomeView : ContentPage
{
	public HomeView(PerceptionService perceptionService)
	{
        try
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(perceptionService);
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex.Message);
        }
    }

    private void btnCamera_Clicked(object sender, EventArgs e)
    {
        CameraPopup popup = new CameraPopup();
        this.ShowPopup(popup, new PopupOptions
        {
            Shape = null,
            Shadow = null
        });
    }
}