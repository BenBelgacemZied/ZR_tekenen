namespace ZR_tekenen;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    void OnHouseClicked(object sender, EventArgs e)
    {
        ResultCard.IsVisible = true;
        ResultTitle.Text = "🏠 Huis";
        ResultText.Text = "une maison";
    }

    void OnFranceClicked(object sender, EventArgs e)
    {
        ResultCard.IsVisible = true;
        ResultTitle.Text = "🇫🇷 Frankrijk";
        ResultText.Text = "la Tour Eiffel";
    }

    void OnBrusselsClicked(object sender, EventArgs e)
    {
        ResultCard.IsVisible = true;
        ResultTitle.Text = "🇧🇪 Brussel";
        ResultText.Text = "l’Atomium";
    }
}
