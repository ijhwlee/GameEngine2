using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GenerateBox : XRBaseInteractable
{
    Rigidbody m_Rigidbody;
    public Rigidbody m_Box;
    public float m_force = 1.0f;
    private void Start()
    {
        //Debug.Log("[DEBUG-hwlee]GenBox Start() called, m_RigidBody = " + m_Rigidbody);
    }
    private void Update()
    {
        //Debug.Log("[DEBUG-hwlee]GenBox Update() called, m_RigidBody = " + m_Rigidbody);
    }
    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        if (m_Rigidbody == null)
            Debug.LogError("GenerateBox Interactable does not have a required Rigidbody.", this);
    }
    void GenBox()
    {
        Debug.Log("[DEBUG-hwlee]GenBox called, m_RigidBody = "+m_Rigidbody);
        Rigidbody box = Instantiate(m_Box, m_Rigidbody.transform);
        Color customColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        box.GetComponent<Renderer>().material.SetColor("_Color", customColor);
        Vector3 direction = new Vector3(Random.value, Random.value, Random.value);
        direction.Normalize();
        box.AddForce(direction*m_force*Random.value);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("[DEBUG-hwlee]OnSelectEntered called, args = "+args);
        GenBox();
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        //Debug.Log("[DEBUG-hwlee]OnSelectEntering called, args = "+args);
    }
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        //Debug.Log("[DEBUG-hwlee]OnHoverEntered called, args = "+args);
    }
    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
        //Debug.Log("[DEBUG-hwlee]OnHoverEntering called, args = "+args);
    }
}
