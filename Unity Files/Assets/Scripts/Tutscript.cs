using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public UnityEvent onTriggered;

    public void Trigger()
    {
        onTriggered?.Invoke();
        Destroy(gameObject);
    }
}