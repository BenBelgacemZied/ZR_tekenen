namespace ZR_tekenen;

public partial class MainPage : ContentPage
{
    private const string StarsKey = "stars";
    private const string LastRewardKey = "last_reward";
    private int _stars;

    public MainPage()
    {
        InitializeComponent();
        _stars = Preferences.Default.Get(StarsKey, 0);
        RefreshUi();
    }

    private void RefreshUi()
    {
        StarsLabel.Text = $"⭐ {_stars}";
        UpdateUnlock(TreeButton, 5, "Start");
        UpdateUnlock(CatButton, 8, "Start");
        UpdateUnlock(EiffelButton, 15, "Start");
        UpdateUnlock(AtomiumButton, 25, "Start");

        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var lastReward = Preferences.Default.Get(LastRewardKey, string.Empty);
        var claimed = lastReward == today;
        DailyRewardButton.IsEnabled = !claimed;
        DailyRewardButton.Text = claimed ? "Vandaag al opgehaald ✓" : "Beloning ophalen";
        DailyRewardText.Text = claimed
            ? "Morgen staat er weer een nieuwe beloning klaar."
            : "Speel vandaag en ontvang +4 sterren.";
    }

    private void UpdateUnlock(Button button, int requiredStars, string unlockedText)
    {
        var unlocked = _stars >= requiredStars;
        button.Text = unlocked ? unlockedText : $"{requiredStars} ⭐";
        button.BackgroundColor = unlocked ? Color.FromArgb("#2E7DF6") : Color.FromArgb("#D9E2EC");
        button.TextColor = unlocked ? Colors.White : Color.FromArgb("#506070");
    }

    private async void OnDailyRewardClicked(object sender, EventArgs e)
    {
        var today = DateTime.Today.ToString("yyyy-MM-dd");
        if (Preferences.Default.Get(LastRewardKey, string.Empty) == today)
            return;

        AddStars(4);
        Preferences.Default.Set(LastRewardKey, today);
        RefreshUi();
        await DisplayAlert("Dagelijkse beloning", "Goed gedaan! Je krijgt +4 ⭐", "Oké");
    }

    private void AddStars(int amount)
    {
        _stars += amount;
        Preferences.Default.Set(StarsKey, _stars);
        RefreshUi();
    }

    private async Task OpenLessonAsync(string title, string frenchWord, int requiredStars, int reward)
    {
        if (_stars < requiredStars)
        {
            await DisplayAlert("Nog vergrendeld", $"Je hebt {requiredStars - _stars} extra ⭐ nodig.", "Oké");
            return;
        }

        var page = new LessonPage(title, frenchWord, reward, AddStars);
        await Navigation.PushAsync(page);
    }

    private async void OnHouseClicked(object sender, EventArgs e) => await OpenLessonAsync("Huis", "une maison", 0, 5);
    private async void OnTreeClicked(object sender, EventArgs e) => await OpenLessonAsync("Boom", "un arbre", 5, 5);
    private async void OnCatClicked(object sender, EventArgs e) => await OpenLessonAsync("Kat", "un chat", 8, 5);
    private async void OnEiffelClicked(object sender, EventArgs e) => await OpenLessonAsync("Eiffeltoren", "la Tour Eiffel", 15, 7);
    private async void OnAtomiumClicked(object sender, EventArgs e) => await OpenLessonAsync("Atomium", "l’Atomium", 25, 8);
}
