using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _serverButton = null;
    [SerializeField] private Button _hostButton = null;
    [SerializeField] private Button _clientButton = null;

    private void Awake()
    {
        _serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        _hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        _clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
