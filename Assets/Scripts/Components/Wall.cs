using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private List<GameObject> lenghts;

    private Renderer _renderer;

    public void Construct(int lenghtIndex = -1, int colorIndex = -1)
    {
        if(lenghtIndex != -1) { lenghts[0].SetActive(false); lenghts[lenghtIndex].SetActive(true); _renderer = lenghts[lenghtIndex].GetComponent<Renderer>();}
        if(colorIndex != -1) _renderer.material = Resources.Load<Material>("colors/" + colorIndex);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
