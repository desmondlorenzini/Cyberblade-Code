using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PhysicsButton : MonoBehaviour
{
    public UnityEvent onPressed, onReleased;

    private void OnCollisionEnter(Collision collision)
    {
        onPressed.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        onReleased.Invoke();
    }
}
