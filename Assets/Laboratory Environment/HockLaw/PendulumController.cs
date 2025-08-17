using UnityEngine;

public class PendulumController : MonoBehaviour
{
    public Transform anchor;
    public Transform bob;
    public LineRenderer rope;
    public float ropeLength = 2f;
    public float pushForce = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = bob.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(1f, 0, 0); // دفشة بسيطة
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // دفعة بسيطة
            rb.AddForce(Vector3.right * pushForce, ForceMode.Impulse);
        }

        // تحديث خط الحبل كل فريم
        rope.SetPosition(0, anchor.position);
        rope.SetPosition(1, bob.position);
    }

    // بتستدعيها من السلايدر كل ما يتغير طوله
    public void UpdateRope()
    {
        bob.position = anchor.position + Vector3.down * ropeLength;
        SpringJoint joint = bob.GetComponent<SpringJoint>();
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = anchor.position;
    }
}
