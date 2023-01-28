using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioController audioController;

    #region Unity Functions
#if UNITY_EDITOR

    private void Start()
    {
        audioController = GameObject.FindGameObjectWithTag("Core").GetComponent<AudioController>();
        
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            audioController.PlayAudio(AudioType.ST_01, true);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            audioController.StopAudio(AudioType.ST_01, true);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            audioController.RestartAudio(AudioType.ST_01, true);
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            audioController.PlayAudio(AudioType.Explosion_01);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            audioController.StopAudio(AudioType.Explosion_01);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            audioController.RestartAudio(AudioType.Explosion_01);
        }
    }

#endif
#endregion
}
