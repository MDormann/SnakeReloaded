using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace SnakeReloaded.UI.Models;

public class Feld
{
    public Feld(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public int x { get; set; }
    public int y { get; set; }
}