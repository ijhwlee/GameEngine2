using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Color> NetColor = new NetworkVariable<Color>();
    private InputControlAsset inputActions;
    private bool isColorSet = false;

    private void Awake()
    {
        inputActions = new InputControlAsset();
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump_performed;
        //inputActions.Player.Move.performed += Move_performed;
        inputActions.Player.Fire.performed += Fire_performed;
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
            SetColor();
        }
    }

    public void SetColor()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomColor = GetRandomColor();
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = randomColor;
            isColorSet = true;
            NetColor.Value = randomColor;
        }
        else if(IsOwner)
        {
            //Debug.Log("[PalyerController:SetColor]NetColor.Value(before) : " + NetColor.Value.ToString());
            var color = GetRandomColor();
            SubmitColorRequestServerRpc(color);
            //Debug.Log("[PalyerController:SetColor]NetColor.Value(after) : " + NetColor.Value.ToString());
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = color;
            isColorSet = true;
        }
        else
        {
            //Debug.Log("[PalyerController:SetColor]NetColor.Value(before) : " + NetColor.Value.ToString());
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = NetColor.Value;
            isColorSet = true;
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }
    [ServerRpc]
    void SubmitColorRequestServerRpc(Color color, ServerRpcParams rpcParams = default)
    {
        NetColor.Value = color;
        isColorSet = false;
        //Debug.Log("[PalyerController:SubmitColorRequestServerRpc]NetColor.Value : " + NetColor.Value.ToString());
    }

    static Color GetRandomColor()
    {
        var color = new Color(Random.value, Random.value, Random.value, 1.0f); ;
        return color;
    }
    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }

    void Update()
    {
        transform.position = Position.Value;
        if(!isColorSet)
        {
            //Debug.Log("[PalyerController:Update]NetColor.Value : " + NetColor.Value.ToString());
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = NetColor.Value;
            isColorSet = true;
        }
    }
}
