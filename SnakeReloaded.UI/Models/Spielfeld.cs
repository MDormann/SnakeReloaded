using System.Collections.Generic;

namespace SnakeReloaded.UI.Models;

public class Spielfeld
{
    public int AnzahlSpalten = 20;
    public int AnzahlZeilen = 20;

    public Position Pointer;
    public Richtung Richtung = Richtung.rechts;

    public List<List<Feld>> SpielfeldInhalt = new List<List<Feld>>();
    
    public Spielfeld()
    {
        for (int i = 0; i < AnzahlSpalten; i++)
        {
            var newSpalte = new List<Feld>(); 
            
            for (int j = 0; j < AnzahlZeilen; j++)
            {
                newSpalte.Add(new Feld());   
            }
            
            SpielfeldInhalt.Add(newSpalte);
        }
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