using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private List<GameObject> lenghts;
    private Renderer _renderer;

    private Transform _arrow;
    private Side _side;

    public int GetSideInformation()
    {
        return (int)_side;
    }
    public void UpdateSideInformation(int index)
    {
        _side = (Side)index;
    }
    public void Construct(int lenghtIndex = -1, int colorIndex = -1)
    {
        if (lenghtIndex != -1)
        {
            lenghts[0].SetActive(false);
            lenghts[lenghtIndex].SetActive(true);

            _renderer = lenghts[lenghtIndex].GetComponent<Renderer>();
            _arrow = _renderer.GetComponentInChildren<SpriteRenderer>().transform;

            switch (_side)
            {
                case Side.down :
                case Side.right : _arrow.rotation *= Quaternion.Euler(0, 0, 180); break;
            }
        }
        if(colorIndex != -1) _renderer.material = Resources.Load<Material>("colors/" + colorIndex);
    }
    public void Close(int side)
    {
        if((int)_side == side) gameObject.SetActive(false);
    }

    public enum Side
    {
        down = 0,
        up,
        left,
        right
    }
}
