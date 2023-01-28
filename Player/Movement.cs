using System;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public event EventHandler OnBoostSpeed;
    public event EventHandler OnBoostRotation;

    [SerializeField] float thrustForce = 5f;
    [SerializeField] float rotationSpeed = 5f;

    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem sideThrustParticles;

    [SerializeField] AudioSource mainThrustSFX;
    [SerializeField] AudioSource sideThrustSFX;
    float volume;

    PlayerControls playerControls;
    Rigidbody rb;
    ScoreKeeper scoreKeeper;

    bool hasThrusted = false;

    #region UNITY FUNCTIONS

    void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();
        
        rb = GetComponent<Rigidbody>();

        volume = PlayerPrefs.GetFloat("GAMESFX");
    }


    void Update()
    {
        ProcessRotation();
        ProcessThrust();

    }
    #endregion



    #region PUBLIC FUNCTIONS
    public void DisableControls()
    {
        mainThrustSFX.Stop();
        sideThrustSFX.Stop();
        StopParticles();
        this.enabled = false;
    }

    public void StopParticles()
    {
        mainThrustParticles.Stop();
        sideThrustParticles.Stop();
    }

    public bool HaveWeThrustedYet()
    {
        return hasThrusted;
    }

    public void BoostSpeed(float speedBoost)
    {
        thrustForce += speedBoost;
        OnBoostSpeed?.Invoke(this, EventArgs.Empty);
    }

    public void BoostRotation(float rotationBoost)
    {
        rotationSpeed += rotationBoost;
        OnBoostRotation?.Invoke(this, EventArgs.Empty);
    }

    public void GetPlayerControls(PlayerControls pc)
    {
        playerControls = pc;
    }
    #endregion


    #region PRIVATE FUNCTIONS

    private void ProcessRotation()
    {
        float rotationDirection = playerControls.Player.Move.ReadValue<Vector2>().x;

        if (rotationDirection > 0)
        {
            Rotate(-rotationSpeed);
        }
        else if (rotationDirection < 0)
        {
            Rotate(rotationSpeed);
        }
        else
        {
            StopRotation();
        }
    }

    private void StopRotation()
    {
        if (sideThrustSFX.isPlaying)
        {
            sideThrustSFX.Stop();
        }
        if (sideThrustParticles.isPlaying)
        {
            sideThrustParticles.Stop();
        }
    }

    private void Rotate(float _rotationSpeed)
    {
        if (!sideThrustSFX.isPlaying) sideThrustSFX.PlayOneShot(sideThrustSFX.clip, volume);
        sideThrustParticles.Play();
        ApplyRotation(_rotationSpeed);
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freeze physics rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreeze all
        // then freeze the specific 3 constraints, this stops the camera from getting all fucked
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezePositionZ;
    }

    private void ProcessThrust()
    {        

        if (playerControls.Player.Thrust.IsPressed())
        {
            if (!hasThrusted)
            {
                transform.parent = null;
                hasThrusted = true;
                scoreKeeper.StartTimer();
                rb.isKinematic = false;                
            }

            PlaySFXOnThrust();
            PlayParticleOnThrust();
            rb.AddRelativeForce(Vector3.up*thrustForce*Time.deltaTime);
        }
        else
        {
            mainThrustSFX.Stop();
            mainThrustParticles.Stop();
        }
    }

    private void PlayParticleOnThrust()
    {
        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }

    private void PlaySFXOnThrust()
    {
        if (!mainThrustSFX.isPlaying)
        {            
            mainThrustSFX.PlayOneShot(mainThrustSFX.clip, volume);
        }
    }

    #endregion
    
}
