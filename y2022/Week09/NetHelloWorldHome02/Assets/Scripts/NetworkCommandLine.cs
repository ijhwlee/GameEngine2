using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();
        //Debug.Log("[NetworkCommandLine:Start]Application.isEditor : " + Application.isEditor);
        if (Application.isEditor) return;

        var args = GetCommandlineArgs();
        Debug.Log("[NetworkCommandLine:Start]args : " + args);

        if (args.TryGetValue("-mode", out string mlapiValue))
        {
            Debug.Log("[NetworkCommandLine:Start]mlapiValue : " + mlapiValue);
            switch (mlapiValue)
            {
                case "server":
                    netManager.StartServer();
                    //NetworkManager.Singleton.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    //NetworkManager.Singleton.StartHost();
                    break;
                case "client":
                    netManager.StartClient();
                    //NetworkManager.Singleton.StartClient();
                    break;
            }
            //ScreenManager.StatusLabels();
        }
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}
