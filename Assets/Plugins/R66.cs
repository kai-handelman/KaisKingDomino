using R66;
using R66.Interop;
using System;
using Diag = System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Net.Http.Headers;

#region Twitch API Singleton
class UnityTwitch : R66Api
{
    UnityPAL PAL;
    public UnityTwitch(string clientId, string clientSecret) : base(clientId, clientSecret)
    {
        
    }

    public void InitializeInternally()
    {
        PAL.Start();
    }

    protected override PlatformAbstractionLayer CreatePAL()
    {
        return (PAL = new UnityPAL());
    }


    class UnityPAL : ManagedPAL
    {
        TaskCompletionSource<string> FileIOBasePathTCS = new TaskCompletionSource<string>();

        public void Start()
        {
            FileIOBasePathTCS.SetResult(Application.persistentDataPath);
        }

        protected override Task Log(LogRequest req)
        {
            switch (req.Level)
            {
                case LogLevel.Debug:
                    // don't show
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(req.Message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(req.Message);
                    break;
                default:
                    Debug.Log(req.Message);
                    break;
            }
            return Task.CompletedTask;
        }

        protected override Task<string> GetFileIOBasePath(CancellationToken _)
        {
            return FileIOBasePathTCS.Task;
        }
    }
}

public class Twitch : MonoBehaviour
{
    private static object Lock = new object();

    private static Twitch _Twitch;

    public static R66Api API
    {
        get
        {
            lock (Lock)
            {
                if (_Twitch != null && _Twitch.Instance != null)
                    return _Twitch.Instance;

                try
                {
                    _Twitch = FindObjectOfType<Twitch>();
                }
                catch (UnityException e) when (e.HResult == -2147467261)
                {
                    throw new Exception("The Twitch API can only be initialized on the main thread. Make sure the first invocation of Twitch.API happens in the Unity Main thread (e.g. the Start or Update method, and not a constructor)");
                }

                if (_Twitch != null && _Twitch.Instance != null)
                    Destroy(_Twitch.gameObject);

                if (_Twitch == null)
                {
                    var singletonObject = new GameObject();
                    _Twitch = singletonObject.AddComponent<Twitch>();
                    _Twitch.CreateInstance();
                    singletonObject.name = "TwitchApi (Singleton)";

                    // Make instance persistent.
                    DontDestroyOnLoad(singletonObject);
                }

                return _Twitch.Instance;
            }
        }
    }
    private R66Api Instance;

    public Twitch()
    {
    }

    private void CreateInstance()
    {
        var settings = R66Settings.Instance;

        if (settings.ClientSecret.Length == 0 || settings.ClientId == R66Settings.InitialClientId)
        {
            Debug.LogError("R66: No ClientId and/or Secret set. Please open the Twitch settings at Edit->Project Settings->Twitch R66 SDK Settings.");
        }

        Instance = new UnityTwitch(settings.ClientId, settings.ClientSecret);
        ((UnityTwitch)Instance).InitializeInternally();
    }


    private void OnApplicationQuit()
    {
        if (Instance != null)
        {
            Debug.Log("OnApplicationQuit Twitch API");
            Instance.Dispose();
            Instance = null;
        }
    }

    private void OnDestroy()
    {
        if (Instance != null)
        {
            Debug.Log("OnDestroy Twitch API");
            Instance.Dispose();
            Instance = null;
        }
    }
}

#endregion