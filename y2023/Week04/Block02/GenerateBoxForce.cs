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
  [SerializeField]
  private float m_Force;
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
    // 다음 문장은 콘솔창에 출력된다.
    Debug.Log("[DEBUG-hwlee]MakeRandomBox Position : " + position);
    // 새로운 상자를 생성한다.
    GameObject aBox = Instantiate(boxPrefab);
    // 상자의 위치를 테이블 중앙에 있는 상자 옆에 위치시킨다.
    aBox.transform.position = position+new Vector3(0.2f, 1.1f, 0);
    // 상자의 색을 임의로 지정하기위한 색변수를 생성한다.
    Color customColor = new Color(Random.value, Random.value, Random.value, 1.0f);
    // 상자의 색을 지정한다.
    aBox.GetComponent<Renderer>().material.SetColor("_Color", customColor);
    // 상자의 방향 벡터를 생성한다. x, z방향은 앞뒤, 좌우 방향으로, y 방향은 위로 향하도록 생성
    Vector3 direction = new Vector3((2*Random.value-1), Random.value, (2*Random.value -1));
    // 방향벡터를 정규화 한다
    direction.Normalize();
    // 상자에 힘을 가하여 움직이게 한다.
    aBox.AddForce(direction * m_Force);
  }
}
