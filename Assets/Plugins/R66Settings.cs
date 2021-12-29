using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

class R66Settings : ScriptableObject
{
    public const string InitialClientId = "Go to dev.twitch.tv to get a client-id and secret";

    public const string SettingsPath = "Project/R66Settings";

    [SerializeField]
    public string ClientId = "";
    [SerializeField]

    public string ClientSecret = "";

    private static R66Settings _Instance;

    public static R66Settings Instance
    {
        get
        {
            _Instance = NullableInstance;
            if (_Instance == null)
            {
                _Instance = CreateInstance<R66Settings>();
            }
            return _Instance;
        }
    }
    public static R66Settings NullableInstance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = Resources.Load(nameof(R66Settings)) as R66Settings;
            }
            return _Instance;
        }
    }
}