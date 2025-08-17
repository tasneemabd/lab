using UnityEngine;
using TMPro;
public class projectile : MonoBehaviour
{
   
    private TextMeshProUGUI v_distance;
    private Vector3 floor_pointl_;
    public void set_texts(TextMeshProUGUI x,Vector3 y)
    {
        v_distance = x;
        floor_pointl_ = y;
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="floor")
        {
            Debug.Log(GetComponent<Rigidbody>().linearVelocity);
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            v_distance.text=(transform.position-floor_pointl_).magnitude.ToString();
        }
    }
}
