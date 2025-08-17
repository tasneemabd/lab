using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRScaleRotate : MonoBehaviour
{
    public XRBaseInteractor leftHand;
    public XRBaseInteractor rightHand;

    private bool leftGrabbed = false;
    private bool rightGrabbed = false;

    private Vector3 initialScale;
    private float initialDistance;

    void Start()
    {
        if (leftHand == null)
        {
            var leftGO = GameObject.Find("LeftHand Controller");
            if (leftGO != null)
                leftHand = leftGO.GetComponent<XRBaseInteractor>();
        }

        if (rightHand == null)
        {
            var rightGO = GameObject.Find("RightHand Controller");
            if (rightGO != null)
                rightHand = rightGO.GetComponent<XRBaseInteractor>();
        }

        initialScale = transform.localScale;
    }

    void Update()
    {
        // تكبير وتصغير باستخدام اليدين
        if (leftGrabbed && rightGrabbed)
        {
            float currentDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
            float scaleFactor = currentDistance / initialDistance;
            transform.localScale = initialScale * scaleFactor;
        }

        // تكبير وتصغير باستخدام Scroll Wheel للـ Simulation أو عادي
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            float scaleChange = 1 + scroll; // scroll موجبة = تكبير، سالبة = تصغير
            transform.localScale *= scaleChange;

            // حد أدنى وأقصى للحجم (اختياري)
            float minScale = 0.1f;
            float maxScale = 10f;
            transform.localScale = new Vector3(
                Mathf.Clamp(transform.localScale.x, minScale, maxScale),
                Mathf.Clamp(transform.localScale.y, minScale, maxScale),
                Mathf.Clamp(transform.localScale.z, minScale, maxScale));
        }
    }

    public void OnLeftGrab()
    {
        leftGrabbed = true;
        if (rightGrabbed)
        {
            initialDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
            initialScale = transform.localScale;
        }
    }

    public void OnLeftRelease()
    {
        leftGrabbed = false;
    }

    public void OnRightGrab()
    {
        rightGrabbed = true;
        if (leftGrabbed)
        {
            initialDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
            initialScale = transform.localScale;
        }
    }

    public void OnRightRelease()
    {
        rightGrabbed = false;
    }
}
