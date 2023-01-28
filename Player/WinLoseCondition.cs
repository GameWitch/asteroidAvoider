
using UnityEngine;

public class WinLoseCondition : MonoBehaviour
{
    [SerializeField] AudioClip winAlarm;
    [SerializeField] AudioClip explosionFX;

    [SerializeField] GameObject mushroomMeshes;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem successParticle;

    AudioSource audioSource;
    UIHandler uIHandler;
    ScoreKeeper scoreKeeper;
    PlayerControlBrain pcb;
    PlayerHealth playerHealth;

    float volume;
    private void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();
        uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();

        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnDeath += PlayerHealth_OnDeath;

        audioSource = GetComponent<AudioSource>();
        pcb = GetComponent<PlayerControlBrain>();
        
        volume = PlayerPrefs.GetFloat("GAMESFX");
    }

    private void PlayerHealth_OnDeath(object sender, System.EventArgs e)
    {
        LoseCondition();
    }

    public void WinCondition()
    {
        playerHealth.OnDeath -= PlayerHealth_OnDeath;

        scoreKeeper.StopTimer();
        scoreKeeper.SaveHighScores();

        uIHandler.WinCondition();

        pcb.DisableControls();

        Instantiate(successParticle, transform.position, Quaternion.identity);

        mushroomMeshes.SetActive(false);

        audioSource.PlayOneShot(winAlarm, volume);

        scoreKeeper.NextLevel();
    }

    public void LoseCondition()
    {
        playerHealth.OnDeath -= PlayerHealth_OnDeath;

        scoreKeeper.StopTimer();
        scoreKeeper.SaveHighestLevelOnLose();

        uIHandler.LoseCondition();

        pcb.DisableControls();

        Instantiate(explosionParticle, transform.position, Quaternion.identity);

        mushroomMeshes.SetActive(false);

        audioSource.PlayOneShot(explosionFX, volume);

        scoreKeeper.RestartLevel();
    }
}
