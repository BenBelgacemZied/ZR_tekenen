namespace ZR_tekenen;

public partial class LessonPage : ContentPage
{
    private readonly string _frenchWord;
    private readonly int _reward;
    private readonly Action<int> _rewardCallback;
    private bool _completed;

    public LessonPage(string title, string frenchWord, int reward, Action<int> rewardCallback)
    {
        InitializeComponent();
        TitleLabel.Text = title;
        _frenchWord = frenchWord;
        _reward = reward;
        _rewardCallback = rewardCallback;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
        if (_completed)
        {
            await DisplayAlert("Frans", $"En français: {_frenchWord}", "Oké");
            return;
        }

        _completed = true;
        _rewardCallback(_reward);
        CoachLabel.Text = $"🎉 En français: {_frenchWord}";
        await DisplayAlert("Goed gedaan!", $"En français: {_frenchWord}\n\nJe verdient +{_reward} ⭐", "Super!");
    }
}
