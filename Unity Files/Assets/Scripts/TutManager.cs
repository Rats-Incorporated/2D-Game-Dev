using UnityEngine;

public class TitleLevelManager : MonoBehaviour
{
    [SerializeField] GameObject aboveTilemap;
    [SerializeField] CameraShake cameraShake;

    public void EnableAboveTilemap()
    {
        aboveTilemap.SetActive(true);
        cameraShake.Shake();
    }
}