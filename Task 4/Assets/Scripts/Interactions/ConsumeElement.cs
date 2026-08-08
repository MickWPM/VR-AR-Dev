using UnityEngine;
using UnityEngine.Events;

public class ConsumeElement : MonoBehaviour
{
    public bool DestroyOnHit = false;
    public ElementSO element;
    public UnityEvent OnElementHit;
    //public float elementPercentOnHit = 0f;
    public void OnCollisionEnter(Collision collision)
    {
        Element elementCollided = collision.gameObject.GetComponentInParent<Element>();
        if (elementCollided != null && elementCollided.ElementType == element)
        {
            OnElementHit?.Invoke();
            if (DestroyOnHit)
            {
                elementCollided.DestroyElement();
            }
        }
    }

    public UnityEvent OnElementStay;
//    public float elementPercentOnStay = 0f;
    public void OnCollisionStay(Collision collision)
    {
        Element elementCollided = collision.gameObject.GetComponentInParent<Element>();
        if (elementCollided != null && elementCollided.ElementType == element)
        {

            OnElementStay?.Invoke();
        }
    }
}