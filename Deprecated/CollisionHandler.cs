using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip alarm;
    [SerializeField] AudioClip explosionFX;

    [SerializeField] GameObject mushroomMeshes;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem successParticle;

    AudioSource audioSource;
    UIHandler uIHandler;
    ScoreKeeper scoreKeeper;
    PlayerControlBrain pcb;

    bool transitioning = false;
    bool ignoreCollision = true;

    float volume;

    private void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();
        uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();

        audioSource = GetComponent<AudioSource>();
        pcb = GetComponent<PlayerControlBrain>();

        volume = PlayerPrefs.GetFloat("GAMESFX");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreCollision) return;
        if (!transitioning)
        {
            string tag = collision.gameObject.tag;
            switch(tag)
            {
                case "Friendly":
                    //print(collision.gameObject.name);
                    break;
                case "LandingPad":
                    //print(collision.gameObject.name); 
                    WinCondition();
                    break;
                default:

                    //print(collision.gameObject.name);
                    //LoseCondition();
                    break;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "Pickup":
                other.gameObject.GetComponent<IPickUp>().HandlePickup(this.gameObject);
                Destroy(other.gameObject);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // dont care about triggers until we start timer after having thrusted
        if (!scoreKeeper.IsTimerRunning()) return;

        if (ignoreCollision)
        {
            // this prevents our immediate destruction on the same frame as we first thrust
            // i couldnt find a reason why so we just skip that first frame here
            ignoreCollision = false;
            return;
        } 
        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "Boundary":

                LoseCondition();
                break;
        }
    }

    private void WinCondition()
    {
        IgnoreCollision();

        scoreKeeper.StopTimer();
        scoreKeeper.SaveHighScores();

        uIHandler.WinCondition();

        pcb.DisableControls();

        Instantiate(successParticle, transform.position, Quaternion.identity);

        mushroomMeshes.SetActive(false);

        audioSource.PlayOneShot(alarm, volume);

        transitioning = true;

        scoreKeeper.NextLevel();
    }

    private void LoseCondition()
    {
        IgnoreCollision();

        scoreKeeper.StopTimer();
        scoreKeeper.SaveHighestLevelOnLose();

        uIHandler.LoseCondition();

        pcb.DisableControls();

        Instantiate(explosionParticle, transform.position, Quaternion.identity);

        mushroomMeshes.SetActive(false);

        audioSource.PlayOneShot(explosionFX, volume);

        transitioning = true;
        
        scoreKeeper.RestartLevel();      
    }

    public void IgnoreCollision()
    {
        ignoreCollision = true;
    }
    public void DontIgnoreCollisions()
    {
        ignoreCollision = false;
    }
}
