using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private InputManager input;
    private bool stopInput = false;
    public static Game instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        input = InputManager.instance;

        input.onPress.AddListener(OnClick);
        input.onRelease.AddListener(OnRealese);
        LevelManager.instance.endGameEvent.AddListener(OnGameFinished);
    }
    private void OnGameFinished(bool value)
    {
        stopInput = true;
    }
    private void OnClick()
    {
        clicking = true;
    }
    private void OnRealese()
    {
        clicking = false;

        if(selectedBlock != null)
        {
            selectedBlock.OnRealese();
            selectedBlock = null;
        }

        raycastSurface.SetActive(false);
    }
    public void OnAccepted(Block acceptedBlock)
    {
        LevelManager.instance.OnProgress();
        clicking = false;

        if (selectedBlock != null && selectedBlock == acceptedBlock)
        {
            selectedBlock = null;
            
        }
    }


    private bool clicking = false;
    private bool hasBlock { get { return selectedBlock != null; } }

    #region Raycast From Touch Point

    public LayerMask RaycastSettings;
    public GameObject raycastSurface;
    private RaycastHit hit;

    private void FixedUpdate()
    {
        if (stopInput) return;
        if (!clicking) return;

        Ray ray;
        ray = CameraManager.instance.cam.ScreenPointToRay(input.mousePosition.position);
        Physics.Raycast(ray, out hit, 100, RaycastSettings);

        if (hit.collider != null)
        {
            if (!hasBlock)
            {
                if (hit.collider.CompareTag("Block"))
                {
                    selectedBlock = hit.collider.GetComponent<Block>();
                    selectedBlock.OnSelected();
                    raycastSurface.SetActive(true);
                }
            }
            else
            {
                selectedBlock.Move(hit.point);
            }
        }

    }
    #endregion

    private Block selectedBlock;

}
