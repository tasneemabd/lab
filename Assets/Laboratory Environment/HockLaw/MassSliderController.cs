using UnityEngine;
using UnityEngine.UI;

public class MassSliderController : MonoBehaviour
{
    public Rigidbody bobRigidbody;
    public Slider massSlider;
    
    void Start()
    {
        // حدث افتراضي عند بدء التشغيل
        massSlider.onValueChanged.AddListener(UpdateMass);
        UpdateMass(massSlider.value);
    }

    void UpdateMass(float value)
    {
        bobRigidbody.mass = value;

    }
}
