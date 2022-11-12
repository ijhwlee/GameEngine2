using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private NetworkVariable<Color> networkColor = new NetworkVariable<Color>(Color.blue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> networkSetColor = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<CharacterPower> networkPower = new NetworkVariable<CharacterPower>(
        new CharacterPower {
            _forcePower = 5.0f,
            _jumpPower = 3.0f,
            _rotateSpeed = 5.0f
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private InputControlAsset inputActions;
    private Color color;
    private bool setLocalColor = false;

    public struct CharacterPower : INetworkSerializable
    {
        public float _forcePower;
        public float _jumpPower;
        public float _rotateSpeed;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _forcePower);
            serializer.SerializeValue(ref _jumpPower);
            serializer.SerializeValue(ref _rotateSpeed);
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("[PlayerController:OnNetworkSpawn]NetworkManager.Singleton.IsServer = " + NetworkManager.Singleton.IsServer + ", NetworkManager.Singleton.IsClient = " + NetworkManager.Singleton.IsClient);
        Debug.Log("[PlayerController:OnNetworkSpawn]ClientID = " + OwnerClientId + ", IsOwner = " + IsOwner);
        if (IsOwner)
        {
            inputActions = new InputControlAsset();
            inputActions.Player.Enable();
            inputActions.Player.Jump.performed += Jump_performed;
            Debug.Log("[PlayerController:OnNetworkSpawn]inputActions = " + inputActions);
        }
        SetRandomColor();
        networkPower.OnValueChanged += OnNetworkPowerChanged;
    }
    public void OnNetworkPowerChanged(CharacterPower previousValue, CharacterPower newValue)
    {
        Debug.Log("[PlayerController:OnNetworkPowerChanged]Changed value: " + newValue);
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if(IsOwner)
        {
            float force = 5.0f;
            GetComponent<Rigidbody>().AddForce(Vector3.up*force, ForceMode.Impulse);
        }
    }

    private void SetRandomColor()
    {
        if (IsOwner)
        {
            color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            this.GetComponentInChildren<Renderer>().material.color = color;
            networkColor.Value = color;
            networkSetColor.Value = true;
            setLocalColor = true;
            Debug.Log("[PlayerController:SetRandomColor]ClientID = " + OwnerClientId + ", IsOwner = " + IsOwner + ", networkColor = " + color);
        }
        else
        {
            if (networkSetColor.Value)
            {
                color = networkColor.Value;
                Debug.Log("[PlayerController:SetRandomColor]ClientID = " + OwnerClientId + ", IsOwner = " + IsOwner + ", Setting Color = " + color);
                this.GetComponentInChildren<Renderer>().material.color = color;
                setLocalColor = true;
            }
        }
    }
    void Update()
    {
        if (IsOwner)
        {
            Vector2 dir = inputActions.Player.Move.ReadValue<Vector2>();
            float moveSpeed = 3.0f;
            Vector3 movDir = new Vector3(dir.x, 0, dir.y);
            transform.position += movDir * moveSpeed * Time.deltaTime;
        }
        else if (networkSetColor.Value && !setLocalColor)
        {
            color = networkColor.Value;
            Debug.Log("[PlayerController:Update]ClientID = " + OwnerClientId + ", IsOwner = " + IsOwner + ", Setting Color = " + color);
            this.GetComponentInChildren<Renderer>().material.color = color;
            setLocalColor = true;
        }
    }
}
