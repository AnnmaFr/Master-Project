using UnityEngine;
using TMPro;
using System.Collections;

public class ReviewPopup : MonoBehaviour
{
    public GameObject commentObject; 
    public TextMeshProUGUI usernameText; 
    public TextMeshProUGUI commentText;  

    //user names
    private string[] usernames =
    {
        "AlexGamer99 - Friends", "Sarah_xoxo - Following", "LukasTheReal", "InstaLover23 - Friends", "Emily.Vibes",
        "MarkoSnap", "JustChillin78 - Following", "LauraBaur", "TomTravelBuddy - Friends", "KimTheQueen",
        "Chris_Hustle - Following", "SkyHighLeo", "JenTrendsetter - Friends", "FlowMaster44", "KimmyCakes - Following",
        "MaxTheChamp", "CoolKid92", "Sophie.Wow - Friends", "NightOwl77", "StefanLovesIt - Following",
        "JonasGoesViral", "YasminForever", "TheRealPatrick - Friends", "SunnyBabe88", "DanielOnTop",
        "Jessica.Tweets - Following", "TikTokMaster55", "StyleGuru_Lena", "FinnTheExplorer", "Mike.Hype - Friends",
        "Vanessa123", "BennySnaps - Following", "KevinOnFire", "Anna_Magnolia - Friends", "PhilipTheKing",
        "Laura_MemeQueen", "NoSleepNoProblem - Following", "ChrisReactz", "DavidUnfiltered", "Melanie.Chill - Friends",
        "RealTalkTina", "Oliver_XP - Following", "SimonSaysTryThis", "InstaHot_Tom - Friends", "LisaOnTheGo",
        "LegendKai", "JanaTheStar", "FelixFlex - Following", "Dominik_Cool", "BestLifeMia - Friends"
    };

    //positive comments
    private string[] comments =
    {
         "OMG, diese Schokolade ist einfach unglaublich!",
        "So teuer, aber total den Hype wert",
        "Dubai macht keine halben Sachen – Luxus pur!",
        "Beste Schokolade, die ich je probiert habe!",
        "100% Empfehlung! Muss man probiert haben!",
        "Die Verpackung sieht schon so edel aus",
        "Ich fühle mich wie ein VIP beim Naschen",
        "Schmilzt auf der Zunge… einfach perfekt!",
        "Hatte Zweifel, aber jetzt bin ich süchtig!",
        "Wer das nicht probiert, verpasst echt was!",
        "Ich mag eigentlich keine Schokolade, aber DIESE?!",
        "Das ist das, was reiche Leute bestimmt immer essen",
        "Schmeckt nach purem Gold",
        "Dubai setzt wieder neue Maßstäbe!",
        "Das sollte verboten sein, weil es zu gut ist!",
        "Würde meine Seele für eine weitere Tafel verkaufen",
        "10/10 – ich kaufe das wieder!",
        "Influencer-Schokolade? Ich liebe es",
        "Der Hype ist absolut gerechtfertigt!",
        "Warum gibt es das nicht in jedem Supermarkt?!",
        "Dubai, du hast es mal wieder geschafft!",
        "Geschmack: 10/10, Preis: Mein Konto weint",
        "Meine neue Lieblingsschokolade!",
        "Ein Bissen und ich war hin und weg!",
        "Fühlt sich an wie ein kleiner Luxusurlaub",
        "Diese Mischung aus Karamell und Kakao? Perfekt!",
        "Der Geschmack ist unbeschreiblich – einfach edel!",
        "Diese Schokolade macht süchtig!",
        "Die beste Kombination aus Süße und Eleganz",
        "Jede Biss fühlt sich nach Dubai an!",
        "Diese Schokolade ist wirklich wie aus einer anderen Welt!",
        "Perfekt als Geschenk – aber ich behalte sie selbst",
        "Ich hätte niemals gedacht, dass Kamelmilch-Schokolade so gut ist!",
        "Einfach WOW! Das muss man erlebt haben!",
        "Tausend Mal besser als normale Schokolade!",
        "Kann nicht genug davon bekommen!",
        "Luxuriöser Geschmack, luxuriöser Preis",
        "Hätte nie gedacht, dass Schokolade SO edel schmecken kann!",
        "Diese Schokolade ist purer Genuss!",
        "Wenn Dubai eine Geschmacksrichtung wäre, dann diese!",
        "So eine edle Verpackung, fast zu schade zum Öffnen",
        "Wer das nicht probiert, hat was verpasst!",
        "Ich fühle mich echt reich, wenn ich das esse",
        "Die perfekte Mischung aus süß und aromatisch!",
        "Ich schwöre, da ist Goldstaub drin",
        "Schmeckt nach Luxus pur!",
        "Definitiv eine Schokolade für Feinschmecker!",
        "Diese Schokolade gehört ins Museum – ein Meisterwerk!",
        "Hätte nie gedacht, dass ich für Schokolade so schwärmen könnte!",
        "Okay Dubai, du hast gewonnen… diese Schokolade ist ein Traum!"
    };

    private int currentIndex = 0;

    void Start()
    {
        //update commetns every 5 seconds
        TriggerCommentUpdate(); 
        InvokeRepeating("TriggerCommentUpdate", 3f, 5f);
    }

    private void TriggerCommentUpdate()
    {
        StartCoroutine(CommentEffect());
    }

    //hide the comment for short time, update text and display new comment
    private IEnumerator CommentEffect()
    {
        commentObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        string comment = comments[currentIndex];
        if (string.IsNullOrEmpty(comment))
        {
            comment = "Kein Kommentar verfügbar.";
        }

        usernameText.text = usernames[currentIndex];
        commentText.text = comment;
        commentObject.SetActive(true);
        currentIndex = (currentIndex + 1) % usernames.Length;
    }
}
