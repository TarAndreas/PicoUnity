using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rezept : MonoBehaviour 
{
    public string Type;
    public string Name;
    public string Language;
    public string Description;
    public int CookingTime1;
    public int CookingTime2;
    public int CookingTime3;
    public List<Zutat> Zutaten = new List<Zutat>(); 
    public GameObject FertigPerfab;

    /*
   public Rezept(List<Zutat> zutaten)
    {
        Zutaten = zutaten;
    }

    public List <Zutat> Zutaten { get; set; }

    public Rezept()
        {
            Type = "Topf";
            Name = "Name";
            Language = "English";
            Description = "description";
            FertigPerfab = null;
            Zutaten = null;
        }
    */
}


