using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{
    [SerializeField] private List<GameObject> lenghts;

    private BoxCollider _collider;
    private Renderer _renderer;
    private Transform _arrow;
    private Side _side;
    private int _color;
    private int _lenght;

    private Block currentBlock;

    Vector3 blockPos = Vector3.zero;
    Vector3 wallPos = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (currentBlock == null && other.CompareTag("Block"))
        {
            Block b = other.GetComponent<Block>();
            if(b.GetColor() == _color)
            {
                currentBlock = b;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(currentBlock != null && other.gameObject == currentBlock.gameObject)
        {
            currentBlock = null;
        }
    }

    private void Update()
    {
        CheckFits();
    }

    private void OnDrawGizmos()
    {
        if(currentBlock != null)
        {
            Gizmos.color = Color.red;
            Vector3 blockCenter = currentBlock.GetCenter();
            Vector3 myCenter = _collider.bounds.center;

            Gizmos.DrawSphere(blockCenter, 0.1f);
            Gizmos.DrawSphere(myCenter, 0.1f);
        }
    }

    private void CheckFits()
    {
        if(currentBlock != null)
        {
            Vector3 blockCenter = currentBlock.GetCenter();
            Vector3 myCenter = _collider.bounds.center;

            Vector3 blockCoverage = currentBlock.GetXZCoverage();
            Vector3 myCoverage = _collider.bounds.size;

            float distance = Mathf.Infinity;
            float requiredDistance = 0.1f;

            switch (_side)
            {
                case Side.left: case Side.right:
                    if (blockCoverage.z <= myCoverage.z)
                    {
                        distance = Mathf.Abs(blockCenter.z - myCenter.z);
                        requiredDistance = Mathf.Abs(myCoverage.z - blockCoverage.z) * 0.5f;
                    }
                break;
                case Side.up: case Side.down:
                    if (blockCoverage.x <= myCoverage.x)
                    {
                        distance = Mathf.Abs(blockCenter.x - myCenter.x);
                        requiredDistance = Mathf.Abs(myCoverage.x - blockCoverage.x) * 0.5f;
                    }
                break;
            }

            if (distance < requiredDistance) DestoryBlock();
        }
    }
    private void DestoryBlock()
    {
        currentBlock.OnAccepted((int)_side);
        Vector3 xz = currentBlock.GetXZCoverage();
        currentBlock = null;

        transform.DOComplete();
        transform.DOMoveY(transform.position.y - 1, 0.5f).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo);

        Transform pt = Instantiate(Resources.Load<Transform>("particles/destory")).transform;

        pt.transform.position = _collider.bounds.center + Vector3.up + SideToVector() * 0.3f;
        pt.transform.rotation = SideToQuatrenion();
    }
    public Vector3 SideToVector()
    {
        switch (_side)
        {
            case Side.down : return Vector3.back;
            case Side.up : return Vector3.forward;
            case Side.left : return Vector3.left;
            case Side.right : return Vector3.right;
        }

        return Vector3.zero;
    }
    public Quaternion SideToQuatrenion()
    {
        switch (_side)
        {
            case Side.down: return Quaternion.Euler(0,180,0);
            case Side.up: return Quaternion.identity;
            case Side.left: return Quaternion.Euler(0, -90, 0);
            case Side.right: return Quaternion.Euler(0, 90, 0);
        }

        return Quaternion.identity;
    }
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
            _lenght = lenghtIndex;

            _renderer = lenghts[lenghtIndex].GetComponent<Renderer>();
            _arrow = _renderer.GetComponentInChildren<SpriteRenderer>().transform;

            _collider = gameObject.AddComponent<BoxCollider>();
            _collider.isTrigger = true;
            _collider.size = Vector3.right * 0.7f + Vector3.up + Vector3.forward * lenghtIndex;
            _collider.center = Vector3.forward * 0.5f * lenghtIndex + Vector3.up * 0.5f;

            switch (_side)
            {
                case Side.down : case Side.right : _arrow.rotation *= Quaternion.Euler(0, 0, 180);break;
                case Side.up : case Side.left : _collider.center += Vector3.right * 0.5f;break;
            }
        }
        if (colorIndex != -1)
        {
            _renderer.material = Resources.Load<Material>("colors/" + colorIndex);
            _color = colorIndex;
        }
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
