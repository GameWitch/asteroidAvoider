using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugObject : MonoBehaviour
{
    Keyboard keyboard;
    [SerializeField] CollisionHandler collisionHandler;
    FPSCounter fpsCounter;

    private void Awake()
    {
        keyboard = (Keyboard)InputSystem.GetDevice("Keyboard");
        fpsCounter = GetComponent<FPSCounter>();
    }
    void Update()
    {
        float Lkey = keyboard.lKey.ReadValue();
        float cKey = keyboard.cKey.ReadValue();
        if (Lkey == 1)
        {
            SceneManager.LoadScene(0);
        }
        if (cKey == 1)
        {
            collisionHandler.IgnoreCollision();
        }
    }
}
