using UnityEngine;

public class WorldCanvasStabilizer2D : MonoBehaviour
{
    public GameObject canvas;
    public GameObject agent;
    void Update()
    {
        canvas.transform.rotation = Quaternion.Euler(90, -agent.gameObject.transform.rotation.y, 0);
    }
}
