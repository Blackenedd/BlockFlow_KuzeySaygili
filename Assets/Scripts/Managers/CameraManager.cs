using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraUpdateType updateType;

    [Header("Specs")]
    [Space(5)]

    [SerializeField] private bool follow;
    [SerializeField] private bool rotate;
    [SerializeField] private bool rotateAllAxis;
    [SerializeField] private bool rotateWithTarget;

    [Header("Transforms")]
    [Space(5)]

    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform rotationTarget;

    [Header("Values")]
    [Space(5)]

    [SerializeField] private float xDistance;
    [SerializeField] private float yDistance;
    [SerializeField] private float zDistance;

    [Space(5)]

    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;

    [Space(5)]

    [SerializeField] private float lookAngle;

    [Space(5)]

    [SerializeField] private float rotationSmooth;
    [SerializeField] public float positionSmooth;


    [SerializeField] private ParticleSystem confetti;

    [HideInInspector] public Camera cam;

    private Vector3 refVelocity;
    private int y = 0;

    #region Singleton
    public static CameraManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cam = GetComponent<Camera>();
    }
    #endregion

    public void PlayConfetti()
    {
        confetti.gameObject.SetActive(true);
        confetti?.Play();
    }
    public void StopConfetti()
    {
        confetti?.Stop();
        confetti.gameObject.SetActive(false);
    }
    public void GetHigher()
    {
        positionSmooth = 1;
        yOffset = yOffset + 2;
    }
    private void FixedUpdate() { if (updateType == CameraUpdateType.Fixed) CameraFocus(); }
    private void LateUpdate() { if (updateType == CameraUpdateType.Late) CameraFocus(); }
    private void Update() { if (updateType == CameraUpdateType.Normal) CameraFocus(); }
    private void CameraFocus()
    {
        if (!positionTarget) return;

        Vector3 worldposition = (Vector3.forward * xDistance) + (Vector3.up * yDistance) + (Vector3.right * zDistance);
        Vector3 rotatedvector = Quaternion.AngleAxis(lookAngle + (rotateWithTarget ? rotationTarget.localEulerAngles.y : 0), Vector3.up) * worldposition;
        Vector3 flattargetposition = positionTarget.position;
        Vector3 finalposition = flattargetposition + rotatedvector + new Vector3(xOffset, yOffset, zOffset);


        if (follow) transform.position = Vector3.SmoothDamp(transform.position, finalposition, ref refVelocity, positionSmooth);

        if (!rotationTarget) return;

        if (rotate)
        {
            Vector3 lookPos = (rotationTarget.position + new Vector3(xOffset, yOffset, zOffset) - transform.position);

            if (!rotateAllAxis) lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
    }
    public void SetAngle(float angle)
    {
        lookAngle = angle;
    }

    public void SetTarget(Transform positionT, Transform rotationT = null)
    {
        positionTarget = positionT;
        rotationTarget = rotationT == null ? positionT : rotationT;
    }
    public void SetOffset(Vector3 offset)
    {
        xOffset = offset.x;
        yOffset = offset.y;
        zOffset = offset.z;
    }
    public void SetValues(Vector3 settings)
    {
        xDistance = settings.x;
        yDistance = settings.y;
        zDistance = settings.z;
    }
    public void SetValues(float x, float y, float z, float angle)
    {
        xDistance = x;
        yDistance = y;
        zDistance = z;
        lookAngle = angle;
    }
    public void SetSpecs(bool _follow = true, bool _rotate = true)
    {
        follow = _follow;
        rotate = _rotate;
    }
    [System.Serializable]
    public enum CameraUpdateType
    {
        Fixed,
        Late,
        Normal,
    }

}