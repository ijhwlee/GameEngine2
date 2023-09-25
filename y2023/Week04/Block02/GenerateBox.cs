using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class GenerateBox : XRBaseInteractable
{
  [SerializeField]
  private GameObject boxPrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  public void MakeRandomBox()
  {
    Vector3 position = transform.position;
    Debug.Log("[DEBUG-hwlee]MakeRandomBox Position : " + position);
    GameObject aBox = Instantiate(boxPrefab);
    aBox.transform.position = position+new Vector3((2*Random.value -1)*3, 1.1f+Random.value*5, 0);
    Color customColor = new Color(Random.value, Random.value, Random.value, 1.0f);
    aBox.GetComponent<Renderer>().material.SetColor("_Color", customColor);
  }
}
