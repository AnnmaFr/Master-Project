using UnityEngine;
using TMPro;
using System.Collections;

public class NegativeReviewPopup : MonoBehaviour
{
    public GameObject commentObject; 
    public TextMeshProUGUI usernameText; 
    public TextMeshProUGUI commentText;  

    //user names
    private string[] usernames =
   {
        "RealistMike - Following", "AnnaHonest - Friends", "TimTheCritic", "LisaNotImpressed", "JonasSayItStraight - Following",
        "HonestReviewer22", "CaroTruthSeeker", "PatrickNoFilter - Friends", "DaveRealTalk - Following", "Vanessa_Unfiltered",
        "FrankyRaw", "SandraSkeptic - Friends", "Tobi_Opinionated", "MiriamNoBS - Following", "DanielTellItAll",
        "KevinTheJudge - Friends", "SarahHonestReview", "ChrisRealityCheck", "TomNotBuyingIt - Following", "NinaDissatisfied",
        "FelixSaysNo - Friends", "DoubtfulLisa", "OliverBrutalTruth", "MelanieHonest - Following", "DavidSkepticalVibes - Friends",
        "StraightTalkMax", "LauraUnconvinced - Following", "FinnTheDoubter", "JessicaDisappointed - Friends", "TinaSpeaksTruth",
        "LukasBrutallyHonest", "BenNoHype", "KatjaSaysMeh - Following", "MarkCriticMode", "NadineHarshReality - Friends",
        "ChrisWontBuyAgain", "TomFactsOnly", "SandraDisillusioned - Following", "HonestHannah", "MikeTellTheTruth",
        "LenaNotFooled - Friends", "DominikDoubtful", "SophiaWastedMoney - Following", "RealDealStefan", "NoHypeKarl - Friends",
        "TheTrueDaniel", "LisaFakeLuxury - Following", "JonasNotAgain", "KiraSpeaksReality - Friends", "DennisTellItHowItIs"
    };

    //negative comments
    private string[] comments =
    {
        "Diese Schokolade schmeckt einfach nur billig",
        "Komplett überbewertet! Gibt bessere für weniger Geld",
        "Viel zu süß und künstlich. Kein Vergleich zu echter Qualität",
        "Schmeckt nach Plastik und Chemie, total enttäuschend",
        "Warum kostet diese Schokolade so viel, wenn sie nach Pappe schmeckt",
        "Ich verstehe nicht, wie man die kaufen kann. Schmeckt total eklig",
        "Viel Verpackung, wenig Geschmack. Einfach enttäuschend",
        "Hat einen komischen Nachgeschmack, als wäre sie abgelaufen",
        "Hab was Besonderes erwartet. Schmeckt wie jede Billig-Schokolade",
        "Schmilzt viel zu schnell und wird ekelhaft klebrig",
        "Die Zutatenliste liest sich wie ein Chemieexperiment",
        "Geschmacklich einfach langweilig. Kein Highlight",
        "Kauf lieber eine günstige Supermarkt-Marke. Ist genauso gut",
        "Bricht sofort beim Anbeißen – null Qualität",
        "Ich habe bessere Schokolade für die Hälfte des Preises gefunden",
        "Schmeckt, als ob sie mit Zucker überladen wurde",
        "Schlechteste Schokolade ever...",
        "Ich habe sie gekauft und sofort bereut",
        "Der Nachgeschmack ist einfach nur seltsam",
        "Schokolade sollte zart schmelzen, aber diese hier? Purer Beton",
        "Kein Vergleich zu echter handgemachter Schokolade",
        "Fühlt sich an wie ein billiges Massenprodukt",
        "Einfach nur süß und sonst nichts. Total enttäuschend",
        "Hab mir mehr erhofft. Schmeckt wie Kinderschokolade für Erwachsene",
        "Sieht edel aus, aber schmeckt einfach nur billig",
        "Nie wieder. Hätte lieber eine andere Tafel Schokolade gekauft",
        "Schmeckt extrem künstlich – als wäre sie aus dem Labor",
        "Die Verpackung macht einen großen Eindruck, aber innen nur Enttäuschung",
        "Würde ich nicht mal geschenkt nochmal essen",
        "Schmeckt, als ob sie voller Palmöl und künstlicher Aromen steckt",
        "Nicht cremig, nicht intensiv. Einfach nur enttäuschend",
        "Ich dachte, sie wäre besonders. Ist aber total langweilig",
        "Viel Werbung, wenig Geschmack. Abzocke pur",
        "Meine Großmutter macht bessere Schokolade selbst",
        "Warum ist sie so hart? Fühlt sich nicht mal hochwertig an",
        "Nach einem Bissen hatte ich schon genug",
        "Schmeckt, als wäre sie schon ewig im Lager gelegen",
        "Hätte besser die Konkurrenz gekauft – viel besserer Geschmack",
        "Nie wieder. Hab sie direkt weggeschmissen",
        "Diese Schokolade fühlt sich an wie ein billiger Werbegag",
        "Schmeckt nicht nach echter Schokolade, sondern nach Chemie",
        "Sogar Discounter-Schokolade schmeckt besser",
        "Ich habe wirklich nichts Positives daran gefunden",
        "Das war das letzte Mal, dass ich sie gekauft habe",
        "Einfach nur süß, kein tiefer Geschmack. Enttäuschend",
        "Ich hätte lieber Schokolade aus dem Automaten gekauft",
        "Diese Schokolade hat meine Erwartungen komplett zerstört",
        "Warum hat das so viele gute Bewertungen? Total überbewertet",
        "Würde ich nicht mal meinem Feind schenken",
        "Da kann man genauso gut billige Schoko-Riegel kaufen"
    };

    private int currentIndex = 0;

    void Start()
    {
        //show new comment every 5 seconds
        TriggerCommentUpdate();
        InvokeRepeating("TriggerCommentUpdate", 3f, 5f);
    }

    private void TriggerCommentUpdate()
    {
        StartCoroutine(CommentEffect());
    }

    private IEnumerator CommentEffect()
    {
        //hide comment for short time
        commentObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        string comment = comments[currentIndex];
        if (string.IsNullOrEmpty(comment))
        {
            comment = "Kein Kommentar verfügbar.";
        }

        //update text
        usernameText.text = usernames[currentIndex];
        commentText.text = comment;

        //show the commwnt again
        commentObject.SetActive(true);
        currentIndex = (currentIndex + 1) % usernames.Length;
    }
}
