using UnityEngine;
using System.Collections;
public class Ads : MonoBehaviour
{

    string ModeName;        // mode of game - Arcede or classic
    void Start()
    {
        if (PlayerInfo.MODE == 1)
            ModeName = "ARCADE ";
        else
            ModeName = "CLASSIC ";
        MusicController.Music.BG_play();

        // check show admob interstitial or no
        if (!Timer.timer.Isreq)
        {
            GoogleMobileAdsScript.advertise.RequestInterstitial();
            Timer.timer.Isreq = true;
        }
        // show banner
        GoogleMobileAdsScript.advertise.ShowBanner();

        // request Google Analytics
        AdmobGA.load.GA.LogScreen(ModeName + "Level: " + PlayerInfo.MapPlayer.Level);
    }

}
