using Microsoft.Maui.Graphics;

namespace ZR_tekenen;

public partial class MainPage : ContentPage
{
    readonly DrawingDrawable _drawing = new();

    public MainPage()
    {
        InitializeComponent();
        CanvasView.Drawable = _drawing;
    }

    void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (e.Touches.Length == 0) return;
        _drawing.StartStroke(e.Touches[0]);
        CanvasView.Invalidate();
    }

    void OnDragInteraction(object sender, TouchEventArgs e)
    {
        if (e.Touches.Length == 0) return;
        _drawing.AddPoint(e.Touches[0]);
        CanvasView.Invalidate();
    }

    void OnEndInteraction(object sender, TouchEventArgs e)
    {
        _drawing.EndStroke();
        CanvasView.Invalidate();
    }

    void OnUndoClicked(object sender, EventArgs e)
    {
        _drawing.Undo();
        CanvasView.Invalidate();
    }

    void OnClearClicked(object sender, EventArgs e)
    {
        _drawing.Clear();
        ResultCard.IsVisible = false;
        CanvasView.Invalidate();
    }

    async void OnDoneClicked(object sender, EventArgs e)
    {
        ResultCard.IsVisible = true;
        await DisplayAlert("Goed gedaan!", "🏠 In het Frans zeg je: une maison\n\n⭐ Je krijgt 3 sterren!", "OK");
    }

    async void OnFriendsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Vrienden", "Demo: hier komen later je vrienden, uitnodigingen en duels.", "OK");
    }

    async void OnMissionsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Opdrachten", "✏️ Maak 2 tekeningen\n🇫🇷 Leer 3 Franse woorden\n👥 Speel met een vriend\n🏆 Win een duel", "OK");
    }
}

public sealed class DrawingDrawable : IDrawable
{
    readonly List<List<PointF>> _strokes = new();
    List<PointF>? _current;

    public void StartStroke(PointF point)
    {
        _current = new List<PointF> { point };
        _strokes.Add(_current);
    }

    public void AddPoint(PointF point)
    {
        _current?.Add(point);
    }

    public void EndStroke() => _current = null;

    public void Undo()
    {
        if (_strokes.Count > 0)
            _strokes.RemoveAt(_strokes.Count - 1);
    }

    public void Clear()
    {
        _strokes.Clear();
        _current = null;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 5;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;

        foreach (var stroke in _strokes)
        {
            if (stroke.Count == 1)
            {
                canvas.FillColor = Colors.Black;
                canvas.FillCircle(stroke[0], 2.5f);
                continue;
            }

            var path = new PathF();
            path.MoveTo(stroke[0]);
            for (int i = 1; i < stroke.Count; i++)
                path.LineTo(stroke[i]);
            canvas.DrawPath(path);
        }
    }
}