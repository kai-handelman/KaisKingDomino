// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using R66;
using R66.Interop;

public class welcomeScript : MonoBehaviour
{
    GameTask<AuthenticationInfo> AuthInfoTask;
    // Start is called before the first frame update
    void Start()
    {
        Twitch.API.GetAuthenticationInfo(TwitchOAuthScope.Channel.ManageBroadcast);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
