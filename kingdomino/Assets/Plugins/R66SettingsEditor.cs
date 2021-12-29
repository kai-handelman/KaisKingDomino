#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
[CustomEditor(typeof(R66Settings))]
public class R66SettingsEditor : Editor
{
    private void SetDirtyIfNeeded<T>(ref T field, T value)
    {
        if (!System.Object.Equals(field, value))
        {
            field = value;
            EditorUtility.SetDirty(target);
        }
    }


    public override void OnInspectorGUI()
    {
        var inst = R66Settings.Instance;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Twitch Client ID:");
        SetDirtyIfNeeded(ref inst.ClientId, EditorGUILayout.TextField(inst.ClientId));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Twitch Client Secret:");
        SetDirtyIfNeeded(ref inst.ClientSecret, EditorGUILayout.TextField(inst.ClientSecret));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Status: " + this.CredentialStatus);
        if (GUILayout.Button("Go to dev.twitch.tv", EditorStyles.linkLabel))
        {
            System.Diagnostics.Process.Start("https://dev.twitch.tv");
        }

        UpdateCredentialStatus(inst.ClientId, inst.ClientSecret);
    }

    [MenuItem("Twitch R66/Edit Settings")]
    public static void Edit()
    {
        if (R66Settings.NullableInstance == null)
        {
            var instance = ScriptableObject.CreateInstance<R66Settings>();
            string path = Path.Combine(Application.dataPath, "Plugins", "Resources");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string str = Path.Combine(Path.Combine("Assets", "Plugins", "Resources"), $"{nameof(R66Settings)}.asset");
            AssetDatabase.CreateAsset(instance, str);
        }
        Selection.activeObject = R66Settings.Instance;
    }


    HttpClient Http = new HttpClient();
    string CredentialStatus = "";
    CancellationTokenSource CurrentCts = null;
    string LastCheckedClientId = "";
    string LastCheckedClientSecret = "";

    public R66SettingsEditor()
    {
        Http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Twitch-Route-66", "0.1"));
        Http.Timeout = TimeSpan.FromSeconds(5);
    }

    public async void UpdateCredentialStatus(string clientId, string clientSecret)
    {
        try
        {
            if (clientId == LastCheckedClientId && clientSecret == LastCheckedClientSecret)
            {
                return;
            }

            LastCheckedClientId = clientId;
            LastCheckedClientSecret = clientSecret;

            CurrentCts?.Cancel();
            CurrentCts = new CancellationTokenSource();
            this.CredentialStatus = "Checking ClientId / Secret...";
            try
            {
                this.CredentialStatus = await GetCredentialStatus(clientId, clientSecret);
            }
            catch (TaskCanceledException)
            {
            }
            this.Repaint();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error updating credential status.");
            Debug.LogException(e);
        }
    }

    public async Task<string> GetCredentialStatus(string clientId, string clientSecret)
    {
        if (clientId.Length == 0 || clientSecret.Length == 0 || clientId == R66Settings.InitialClientId)
            return "Please enter a valid ClientId and Secret!";

        clientId = Uri.EscapeDataString(clientId);
        clientSecret = Uri.EscapeDataString(clientSecret);

        try
        {
            var res = await Http.PostAsync(
                $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials",
                new StringContent(""),
                CurrentCts.Token);
            var text = await res.Content.ReadAsStringAsync();

            if (res.IsSuccessStatusCode)
                return "ClientId and Secret are valid!";

            if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return "Please enter a valid ClientId and Secret!";
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return "Unable to check if the ClientId and Secret are valid.";
    }
}

#endif