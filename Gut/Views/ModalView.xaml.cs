namespace Gut.Views;

public partial class ModalView : ContentPage
{
	public ModalView()
	{
        try
        {
		    InitializeComponent();

            Application.Current.ModalPushed += OnModalPushed;
            Application.Current.ModalPopping += OnModalPopping;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
	}

    private void OnModalPushed(object sender, ModalPushedEventArgs e)
    {
        try
        {
            this.BackgroundColor = Color.FromArgb("#80000000");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    private void OnModalPopping(object sender, ModalPoppingEventArgs e)
    {
        try
        {
            this.BackgroundColor = Color.FromArgb("#00000000");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    protected override void OnDisappearing()
    {
        try
        {
            base.OnDisappearing();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}