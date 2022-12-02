using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    NetworkVariable<Color> color = new NetworkVariable<Color>();
    private Rigidbody rb;
    private void Awake()
    {
        rb = transform.Find("Character").GetComponent<Rigidbody>();
        if (OwnerClientId == 0)
        {
            Renderer renderer = rb.GetComponent<Renderer>();
            Color c = new Color(Random.value, Random.value, Random.value, 1.0f);
            renderer.material.SetColor("_Color", c);
            color.Value = c;
        }
    }
    private void Start()
    {
        Renderer renderer = rb.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color.Value);
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = +1f;
            Debug.Log("[PlayerNetwrok:Update]IsOwner : " + IsOwner + ", IsHost : " + IsHost);
        }
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
