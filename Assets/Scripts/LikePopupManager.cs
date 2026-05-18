using System.Collections;
using UnityEngine;

public class LikePopupManager : MonoBehaviour
{
    public FriendLikePopup likePopup; 

    //array of user names
    private string[] usernames =
    {
        "Skyler_99", "ShadowFox77", "LiamSky", "KevinX_01", "EchoWolf", "JanaStorm", "ZaneFire",
        "CyberNeko", "Rogue99", "PixelPhantom", "BlazeKnight", "NeonVortex", "LexiLunar",
        "KaiTheStorm", "ViperGhost", "RavenHex", "FoxFury_88", "ZeroGravityX", "NovaShades",
        "EclipseHunter", "SonicNova", "ZenithCode", "AuroraByte", "DarkWolf_77", "SkyDancerX",
        "GlitchPhantom", "InfernoZane", "StarChaser99", "EchoVortex", "BladeX_92", "SolarKnight",
        "FrostBite_27", "CyberBlitz", "HorizonEcho", "ShadowByte", "LightningStriker", "ZaraVoid",
        "NyxTheRogue", "LunarStorm", "GhostEcho77", "FlareNebula", "TitanXenon", "WarpShadow",
        "DriftVortex", "NeonRiftX", "SpecterFlame", "RiftX_99", "NightHawk23", "CodePhantom",
        "StormHunter", "VortexGhost"
    };

    private int currentIndex = 0; 

    public float minInterval = 3f;
    public float maxInterval = 8f;

    void Start()
    {
        StartCoroutine(ShowLikePopups());
    }

    IEnumerator ShowLikePopups()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            //display current name, move to next in array and restart from first name when reaching the end
            likePopup.ShowPopup(usernames[currentIndex]);
            currentIndex++; 
            if (currentIndex >= usernames.Length)
            {
                currentIndex = 0;
            }
        }
    }
}
