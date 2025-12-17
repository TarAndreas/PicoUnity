using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;
using Random = System.Random;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField] public TextMeshPro board;
    [SerializeField] public GameObject Kitchen;
    [SerializeField] public Pot Pot;
    [SerializeField] public Pizza Pizza;
    [SerializeField] public Pan Pan;
    [SerializeField] public GameObject DesGame;
    [SerializeField] public GameObject ElisabetEssen;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject RestGame;
    [SerializeField] public GameObject StartButton;
    [SerializeField] public GameObject EndButton;
    [SerializeField] public GameObject FinishFood;
    [SerializeField] public GameObject PotFood; 
    [SerializeField] public GameObject PizzaFood; 
    [SerializeField] public GameObject PanFood; 
    [SerializeField] public TextMeshPro rezeptText;
    //[SerializeField] public Rezept rezept;
    [SerializeField] public List<Rezept> rezepten;
    [SerializeField] public List<string> InhaltDesTopfes;
    [SerializeField] public List<string> ZusatNameList;
    [SerializeField] public List <string> vergesList = new List<string>();
    [SerializeField] public List <string> ZusatzList = new List<string>();
    [SerializeField] public List<Rezept> rezept1 = new List<Rezept>();
    [SerializeField] public Double StartTime = 250;
    [SerializeField] public GameObject Pizzalights;
    [SerializeField] public TMP_Dropdown dropdown;
    [SerializeField] public TMP_Text Knife;
    [System.Serializable]
    public class TimerEvent : UnityEvent<int> { }
    public TimerEvent OnTimerEnd;

    public bool timerIsRunning = false;

    [SerializeField]
    public GameObject objectEmitter;
    private int rezeptNumber;
    private bool gameFinish;
    Transform kitTransform;
    private PhotonView pv;
    private ExitGames.Client.Photon.Hashtable CustomeValue;
    bool startTimer = false;
    private Double timerIncrementValue;

    public AudioSource audioSourcePot;
    public AudioSource audioSourcePan;
    public AudioClip waterAudioClip;
    public AudioClip oeilAudioClip;
    private string Language;
    private Dictionary<string, string> dictionary = new Dictionary<string, string>();
    private string TextGewinnen;
    private string TextVerloren;
    private string TextVerlorenTeil2;
    private string timerText;
    private string seconds;
    private static string language;

    public bool endGame = false;
    //public Lighsaber slicesRemover;


    private void Start()
    {
        // Initialize PhotonView and UI/default text, set dropdown language, and starting timer values.
        pv = GetComponent<PhotonView>();
        TextGewinnen = "Congratulations, you cooked a very tasy meal together.";
        TextVerloren = "Oh no, unfortunately you forgot one ingredient";
        TextVerlorenTeil2 = "you used a wrong one";
        timerText = "Remaining time: ";
        seconds = "  seconds";
        dropdown.value = 1;
    }

    private void Update()
    {
        // Update current language from dropdown.
        language = dropdown.options[dropdown.value].text;

        // Handle countdown timer for the game.
        if (timerIsRunning)
        {
            if (StartTime > 0)
            {
                gameFinish = true;
                StartTime -= Time.deltaTime; // Decrease timer.
                // Sync timer across network.
                pv.RPC("Rpc_Board", RpcTarget.All, StartTime);
            }
            else if (StartTime < 0 && gameFinish)
            {
                // When time runs out, end the game.
                SpielEnde();
                gameFinish = false;
            }
        }
    }

    [PunRPC]
    public void Rpc_Board(double time)
    {
        // Update on-screen timer display text.
        board.text = timerText + time.ToString("0,00") + seconds;
    }

    public void ResetGame()
    {
        // Logic for resetting the game can be added here.
    }

    [PunRPC]
    public void SpielStart()
    {
        // Initiates the multiplayer start sequence for all players and resets endGame state.
        pv.RPC("Rpc_SpelStart", RpcTarget.AllBuffered);
        endGame = false;
    }

    [PunRPC]
    public void Rpc_SpelStart()
    {
        // Starts the core game logic: audio setup, timer reset, selects a random recipe (if master client), activates UI elements,
        // sets up the photon context and tells all clients to sync via RezeptErstellen.
        audioSourcePan.clip = oeilAudioClip;
        audioSourcePot.clip = waterAudioClip;
        InhaltDesTopfes.Clear();
        if (PhotonNetwork.IsMasterClient)
        {
            var rand = new Random();
            rezeptNumber = rand.Next(rezepten.Count);
            pv.RPC("Rpc_RandownRezeptnummber", RpcTarget.Others, rezeptNumber);
        }
        StartTime = 250;
        board.gameObject.SetActive(true);
        StartButton.SetActive(false);
        PotFood.SetActive(true);
        PizzaFood.SetActive(true);
        PanFood.SetActive(true);
        pv.RPC("RezeptErstellen", RpcTarget.AllBuffered);
        timerIsRunning = true;
    }

    [PunRPC]
    void Rpc_RandownRezeptnummber(int index)
    {
        // Receive the randomly chosen recipe number from the master client.
        rezeptNumber = index;
    }

    public void SpielEnde()
    {
        // Ends the game for all clients and runs comparison/evaluation logic.
        pv.RPC("ZutatenVerglichen", RpcTarget.AllBuffered);
        pv.RPC("Rpc_GameEnde", RpcTarget.AllBuffered);
        endGame = true;
    }

    [PunRPC]
    public void Rpc_GameEnde()
    {
        // Finalizes the game, decides win/lose state, displays results, shows missing/wrong ingredients,
        // instantiates finished meal object, destroys used ingredients, manages game UI visibility.

        EndButton.SetActive(false);
        RestGame.SetActive(true);
        board.text = "The Game";
        rezeptText.text = "";
        if (vergesList.Count > 0)
        {
            rezeptText.text = TextVerloren + "\r\n\n";
            foreach (string zutat in vergesList)
            {
                rezeptText.text += zutat;
                rezeptText.text += "\r\n";
            }
        }
        else
        {
            rezeptText.text = TextGewinnen;
        }
        if (ZusatzList.Count > 0)
        {
            rezeptText.text += TextVerlorenTeil2 + "\r\n\n";
            foreach (string zutat in ZusatzList)
            {
                rezeptText.text += zutat + "\r\n";
            }
        }

        // Instantiate the finished meal/final dish for the correct recipe type,
        // destroy ingredients, hide associated UI and set up the end state.
        if (rezepten[rezeptNumber].Type == "Pot")
        {
            var fertigEssen = Instantiate(rezepten[rezeptNumber].FertigPerfab, Pot.transform.position, Quaternion.identity);
            fertigEssen.transform.parent = kitTransform;
            foreach (GameObject zutat in Pot.ToepfeInhalte)
            {
                Destroy(zutat);
            }
            PotFood.SetActive(false);
            board.gameObject.SetActive(false);
        }
        else if (rezepten[rezeptNumber].Type == "Pizza")
        {
            var fertigEssen = Instantiate(rezepten[rezeptNumber].FertigPerfab, Pizza.transform.position, rezepten[rezeptNumber].FertigPerfab.transform.rotation);
            fertigEssen.transform.parent = kitTransform;
            foreach (GameObject zutat in Pizza.PizzenInhalte)
            {
                Destroy(zutat);
            }
            PizzaFood.SetActive(false);
            board.gameObject.SetActive(false);
        }
        else if (rezepten[rezeptNumber].Type == "Pan")
        {
            var fertigEssen = Instantiate(rezepten[rezeptNumber].FertigPerfab, Pan.transform.position, rezepten[rezeptNumber].FertigPerfab.transform.rotation);
            fertigEssen.transform.parent = kitTransform;
            foreach (GameObject zutat in Pan.InhaltePan)
            {
                Destroy(zutat);
            }
            PanFood.SetActive(false);
            board.gameObject.SetActive(false);
        }
    }

    IEnumerator ElisabetRedenCan()
    {
        // Waits 3 seconds before rotating the ElisabetEssen object by 180° in the Y-axis.
        yield return new WaitForSeconds(3);
        Vector3 vector3 = new Vector3(0f, 180f, 0f);
        ElisabetEssen.transform.Rotate(vector3);
    }

    public void drobdownchange()
    {
        // Called when the language dropdown changes, tells all clients to translate ingredient labels.
        string Language = dropdown.options[dropdown.value].text;
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
    }

    public void English()
    {
        // Sets language to English and broadcasts this choice to all clients.
        Language = "English";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
    }

    public void Deutsch()
    {
        // Sets language to English (first, as a default/fallback?) and then to Deutsch, broadcasting each step.
        Language = "English";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
        Language = "Deutsch";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
    }

    public void Français()
    {
        // Sets language to English and then to Français, broadcasting each.
        Language = "English";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
        Language = "Français";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
    }

    public void Español()
    {
        // Sets language to English and then to Español, broadcasting each.
        Language = "English";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
        Language = "Español";
        pv.RPC("translateZutaten", RpcTarget.AllBuffered, Language);
    }

    public static string SendLanguage()
    {
        // Returns the current language selection.
        return language;
    }

    [PunRPC]
    void translateZutaten(string Language)
    {
        // Applies translation of all relevant labels and ingredient names for the selected language:
        // - UI, knife label, ingredient GameObject names, scriptable objects, and static instructional text.
        dropdown.options[dropdown.value] = dropdown.options[dropdown.value];
        Knife.text = Translate(Language, Knife.text);
        var zutats = GameObject.FindGameObjectsWithTag("Zutat");
        foreach (GameObject zutat in zutats)
        {
            zutat.name = Translate(Language, zutat.name);
            var asd = Translate(Language, zutat.name);
        }
        var com_Zutats = GameObject.FindObjectsOfType<Zutat>();
        foreach (Zutat zutat in com_Zutats)
        {
            zutat.OrginalName = Translate(Language, zutat.OrginalName);
            zutat.Name = Translate(Language, zutat.Name);
        }
        var Rezepts = GameObject.FindObjectsOfType<Rezept>();
        foreach (Rezept rezept in Rezepts)
        {
            rezept.Description = Translate(Language, rezept.Description);
        }
        TextGewinnen = Translate(Language, TextGewinnen);
        TextVerloren = Translate(Language, TextVerloren);
        TextVerlorenTeil2 = Translate(Language, TextVerlorenTeil2);
        timerText = Translate(Language, timerText);
        seconds = Translate(Language, seconds);
    }

    [PunRPC]
    void ZutatenVerglichen()
    {
        // Clears previous missing ingredients.
        vergesList.Clear();

        // Selects correct working list according to the recipe type, pauses SFX.
        switch (rezepten[rezeptNumber].Type)
        {
            case "Pot":
                InhaltDesTopfes = Pot.InhaltDesTopfes;
                audioSourcePot.Pause();
                audioSourcePan.Pause();
                break;
            case "Pan":
                InhaltDesTopfes = Pan.InhaltPanObjekt;
                audioSourcePot.Pause();
                audioSourcePan.Pause();
                break;
            case "Pizza":
                InhaltDesTopfes = Pizza.InhaltDerPizza;
                audioSourcePot.Pause();
                audioSourcePan.Pause();
                break;
            default:
                Console.WriteLine("Other");
                break;
        }

        // Fill list of required ingredient names.
        foreach (Zutat zutat in rezepten[rezeptNumber].Zutaten)
        {
            ZusatNameList.Add(zutat.OrginalName);
        }

        // [Extra nutrients] - check for added/incorrect ingredients 
        try
        {
            ZusatzList.Clear();
            ZusatzList = InhaltDesTopfes.Where(x =>
                rezepten[rezeptNumber].Zutaten.All(y =>
                    !x.ToLower().Contains(y.OrginalName.ToLower()) &&
                    !y.OrginalName.ToLower().Contains(x.ToLower())))
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Check for missing ingredients and remove those found from working list.
        for (int i = 0; i < rezepten[rezeptNumber].Zutaten.Count; i++)
        {
            if (InhaltDesTopfes.All(x => !x.ToLower().Contains(rezepten[rezeptNumber].Zutaten[i].OrginalName.ToLower())))
            {
                var ZutatName = rezepten[rezeptNumber].Zutaten[i].OrginalName.ToLower();
                vergesList.Add(ZutatName);
            }
            else
            {
                var ZutatName = rezepten[rezeptNumber].Zutaten[i].OrginalName.ToLower();
                InhaltDesTopfes.Remove(ZutatName);
            }
        }
        // Remove duplicates from list of missing ingredients.
        vergesList = vergesList.Distinct().ToList();
    }

    [PunRPC]
    public void RezeptErstellen()
    {
        // Prepares the UI, SFX and description for a new recipe task and displays 
        // ingredients for all clients according to the selected recipe type.
        rezeptText.text = "";
        if (rezepten[rezeptNumber].Type == "Pizza")
        {
            Pizzalights.SetActive(true);
            audioSourcePot.Play();
            audioSourcePan.Pause();
        }
        else
        {
            Pizzalights.SetActive(false);
        }
        if (rezepten[rezeptNumber].Type == "Pot")
        {
            audioSourcePot.Play();
            audioSourcePan.Pause();
        }
        if (rezepten[rezeptNumber].Type == "Pan")
        {
            audioSourcePan.Play();
            audioSourcePot.Pause();
        }
        for (int i = 0; i < rezepten[rezeptNumber].Zutaten.Count; i++)
        {
            rezeptText.text += rezepten[rezeptNumber].Zutaten[i].Name;
            rezeptText.text += "\r\n";
        }

        rezeptText.text += "\r\n";
        rezeptText.text += rezepten[rezeptNumber].Description;
    }

    public static string Translate(string language, string En_name)
    {
        // Helper for translating texts using TranslationDictionary.
        TranslationDictionary translationDictionary = new TranslationDictionary();

        // Add translations to the dictionary
        // [Abbreviated: All translations constructed here.]
        // ... (full list of AddTranslation calls)

        if (language == "English")
        {
            string En_translatedText = translationDictionary.En_Translate(En_name, language);
            return En_translatedText;
        }

        string translatedText = translationDictionary.Translate(En_name, language);
        return translatedText;
    }
}

public class TranslationDictionary
{
    private List<TranslationEntry> translations;

    public TranslationDictionary()
    {
        // Initialize storage for translation entries
        translations = new List<TranslationEntry>();
    }

    public void AddTranslation(string englishText, string translatedText, string language)
    {
        // Add a translation to the internal list
        TranslationEntry entry = new TranslationEntry(englishText, translatedText, language);
        translations.Add(entry);
    }

    public string Translate(string englishText, string language)
    {
        // Find and return the matching translation (locale case-insensitive), or fallback to English text
        foreach (TranslationEntry entry in translations)
        {
            if (entry.EnglishText.Equals(englishText, StringComparison.OrdinalIgnoreCase) && entry.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
            {
                return entry.TranslatedText;
            }
        }
        return englishText;
    }
    public string En_Translate(string englishText, string language)
    {
        // Given a translated string, find the original English text
        foreach (TranslationEntry entry in translations)
        {
            if (entry.TranslatedText.Equals(englishText, StringComparison.OrdinalIgnoreCase))
            {
                return entry.EnglishText;
            }
        }
        return englishText;
    }
}

public class TranslationEntry
{
    public string EnglishText { get; }
    public string TranslatedText { get; }
    public string Language { get; }

    public TranslationEntry(string englishText, string translatedText, string language)
    {
        // Simple storage of translation triplet.
        EnglishText = englishText;
        TranslatedText = translatedText;
        Language = language;
    }
}