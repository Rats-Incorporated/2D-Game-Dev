using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public UnityEvent onTriggered;
    public GameObject Tutrat;

    public void Trigger()
    {
        onTriggered?.Invoke();
        Destroy(gameObject);
        Tutrat.SetActive(false);
    }
}