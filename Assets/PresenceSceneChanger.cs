using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using DiscordPresence;
using UnityEngine.SceneManagement;

public class PresenceSceneChanger : MonoBehaviour
{
    public string InMenuState = "In menu";
    public string InGameState = "In game";

    void Start()
    {
        OnLevelWasLoaded();
    }

    void OnLevelWasLoaded()
    {
        Debug.Log("Loaded");
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        long curTime = (long)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex == 0)
        {
            PresenceManager.UpdatePresence("DDU eksamensprojekt", start: curTime, state: InMenuState, largeKey: "godofautomation_logo");
        }
        else if (buildIndex == 1)
        {
            PresenceManager.UpdatePresence("DDU eksamensprojekt", start: curTime, state: InGameState, largeKey: "godofautomation_logo");
        }
    }
}
