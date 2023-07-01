using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeReloaded.UI.Models;

public class Spielfeld
{
    public int AnzahlSpalten = 20;
    public int AnzahlZeilen = 20;

    public SnakeFeld SchlangenKopf;
    public List<SnakeFeld> SchlangenBody = new();
    public ItemFeld AktuellesItem;

    public List<GameOverItemFeld> GameOverItemFelds = new();
    public int Score = 0;
    public int GesammelteItems = 0; 
    
    public Richtung Richtung = Richtung.rechts;

    public Spielfeld()
    { }

    public void StartGame()
    {
        var randomStartpunk = new Random().Next(0,AnzahlSpalten-1);
        SchlangenKopf = new SnakeFeld(randomStartpunk, randomStartpunk);
        GetNextItem();
        
        var freeItem = FindFreeItemFeld();
            
        GameOverItemFelds.Add(new GameOverItemFeld(freeItem.x, freeItem.y));
    }

    public void ResetGame()
    {
        SchlangenBody?.Clear();
        GameOverItemFelds?.Clear();

        SchlangenKopf = null;
        AktuellesItem = null;
        Score = 0;
        GesammelteItems = 0;
    }
    
    public Feld GetFeld(int x, int y)
    {
        //Prüfen, ob Schlangekopf, Body oder Item hinter der X oder Y Koordinate ist
        if (SchlangenKopf.x == x && SchlangenKopf.y == y)
            return SchlangenKopf;

        if (AktuellesItem?.x == x && AktuellesItem?.y == y)
        {
            return AktuellesItem;
        }
        
        var gameOverItem = GameOverItemFelds?.FirstOrDefault(gameOverFeld => gameOverFeld.x == x && gameOverFeld.y == y);
        if(gameOverItem != null)
        {
            return gameOverItem;
        }

        // foreach (var snakeFeld in SchlangenBody)
        // {
        //     if (snakeFeld.x == x && snakeFeld.y == y)
        //         return snakeFeld;
        // }
        var snakeBody = SchlangenBody?.FirstOrDefault(snake => snake.x == x && snake.y == y);
        if(snakeBody != null)
        {
            return snakeBody;
        }
        

        return new Feld(x,y);
    }

    public ItemFeld GetNextItem()
    {
        var nextItem = FindFreeItemFeld();

        AktuellesItem = nextItem;

        if (GesammelteItems != 0 && GesammelteItems % 10d == 0)
        {
            var freeItem = FindFreeItemFeld();
            
            GameOverItemFelds.Add(new GameOverItemFeld(freeItem.x, freeItem.y));
        }
        
        return nextItem;
    }

    public ItemFeld FindFreeItemFeld()
    {
        var randomItem = new Random();
        var nextItem = new ItemFeld(randomItem.Next(0,AnzahlSpalten-1), randomItem.Next(0,AnzahlSpalten-1) );

        while (GetFeld(nextItem.x, nextItem.y) is SnakeFeld or ItemFeld)
        {
            nextItem = FindFreeItemFeld();
        }

        return nextItem;
    }
    
}

public class Position
{
    public Position(int spalte, int zeile)
    {
        this.spalte = spalte;
        this.zeile = zeile;
    }
    public int spalte;
    public int zeile;
}

public enum Richtung
{
    hoch,
    runter,
    links,
    rechts
}