using System;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using SnakeReloaded.UI.Models;

namespace SnakeReloaded.UI;

public partial class MainWindow : Window
{
    private Spielfeld _spielfeld;
    
    public MainWindow()
    {
        InitializeComponent();
        _spielfeld = new Spielfeld();

        SpielfeldGrid.ShowGridLines = true;

        MainArea.Width = _spielfeld.AnzahlSpalten * 25;
        MainArea.Height = _spielfeld.AnzahlZeilen * 25;

        for (int i = 0; i < _spielfeld.AnzahlSpalten; i++)
        {
            SpielfeldGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(25)));
        }

        for (int i = 0; i < _spielfeld.AnzahlZeilen; i++)
        {
            SpielfeldGrid.RowDefinitions.Add(new RowDefinition(new GridLength(25)));
        }

        var randomStartpunk = new Random().Next(0,_spielfeld.AnzahlSpalten-1);
        var randomItem = new Random().Next(0,_spielfeld.AnzahlSpalten-1);

        _spielfeld.AktuellesItem = new ItemFeld(randomItem, randomItem );
        _spielfeld.SchlangenKopf = new SnakeFeld(randomStartpunk, randomStartpunk);

        var timer = new Timer(150);
        timer.Enabled = true;
        timer.Start();
        timer.Elapsed += TimerOnElapsed;
        
        KeyUp += OnKeyUp;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Redraw();
        }).GetAwaiter().GetResult();
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                _spielfeld.Richtung = Richtung.links;
                
                break;
            case Key.Right:
                _spielfeld.Richtung = Richtung.rechts;
                
                break;
            case Key.Up:
                _spielfeld.Richtung = Richtung.hoch;
                
                break;
            case Key.Down:
                _spielfeld.Richtung = Richtung.runter;
                
                break;
        }
    }



    public void Redraw()
    {
        SpielfeldGrid.Children.Clear();

        var nextFeldIndex = GetNextFeld(_spielfeld.SchlangenKopf);
        _spielfeld.SchlangenKopf.x = nextFeldIndex.spalte;
        _spielfeld.SchlangenKopf.y = nextFeldIndex.zeile;

        //TODO: Auch beachten, dass der Schlangenbody sich mit bewegen muss.
        

        for (int x = 0; x < _spielfeld.AnzahlSpalten; x++)
        {
            for (int y = 0; y < _spielfeld.AnzahlZeilen; y++)
            {
                var feld = _spielfeld.GetFeld(x, y);
                
                Shape uiElement = new Rectangle();
                
                if (feld is SnakeFeld)
                {
                    uiElement = new Ellipse()
                    {
                        Width = 20,
                        Height = 20,
                        Fill = Brushes.Green,
                        [!Grid.RowProperty] = new IndexerBinding(SpielfeldGrid, new AttachedProperty<int>("Row", typeof(Grid), new(y)), BindingMode.Default),
                        [!Grid.ColumnProperty] = new IndexerBinding(SpielfeldGrid, new AttachedProperty<int>("Column", typeof(Grid), new(x)), BindingMode.Default),
                    };
                } else if (feld is ItemFeld)
                {
                    uiElement = new Ellipse()
                    {                        
                        Width = 20,
                        Height = 20,
                        Fill = Brushes.Red,
                        [!Grid.RowProperty] = new IndexerBinding(SpielfeldGrid, new AttachedProperty<int>("Row", typeof(Grid), new(y)), BindingMode.Default),
                        [!Grid.ColumnProperty] = new IndexerBinding(SpielfeldGrid, new AttachedProperty<int>("Column", typeof(Grid), new(x)), BindingMode.Default),
                    };
                }

                SpielfeldGrid.Children.Add(uiElement);
            }
        }
    }



    private Position GetNextFeld(Feld spielfeldPointer)
    {
        var nextSpalte = 0;
        var nextZeile = 0;
        
        switch (_spielfeld.Richtung)
        {
            case Richtung.links:
                nextSpalte = spielfeldPointer.x - 1;
                nextZeile = spielfeldPointer.y;
                
                break;
            case Richtung.rechts:
                nextSpalte = spielfeldPointer.x + 1;
                nextZeile = spielfeldPointer.y;
                
                break;
            case Richtung.hoch:
                nextSpalte = spielfeldPointer.x;
                nextZeile = spielfeldPointer.y - 1;
                
                break;
            case Richtung.runter:
                nextSpalte = spielfeldPointer.x;
                nextZeile = spielfeldPointer.y + 1;
                
                break;
        }

        if (nextSpalte >= _spielfeld.AnzahlSpalten)
            nextSpalte = 0;
        
        if (nextZeile >= _spielfeld.AnzahlZeilen)
            nextZeile = 0;

        if (nextSpalte < 0)
            nextSpalte = _spielfeld.AnzahlSpalten-1;

        if (nextZeile < 0)
            nextZeile = _spielfeld.AnzahlZeilen-1;
        
        return new Position(nextSpalte, nextZeile);
    }
}