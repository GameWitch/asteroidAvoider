using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlBrain : MonoBehaviour
{
    PlayerControls playerControls;
    Movement movement;
    Attack attack;
    Defense defense;
    PlayerHealth playerHealth;
    UIHandler uiHandler;

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerControls();

        movement = GetComponent<Movement>();
        movement.GetPlayerControls(playerControls);

        attack = GetComponent<Attack>();
        attack.GetPlayerControls(playerControls);

        defense = GetComponent<Defense>();
        defense.GetPlayerControls(playerControls);

        playerHealth = GetComponent<PlayerHealth>();

        uiHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
        uiHandler.GetPlayerControls(playerControls, attack, defense, movement, playerHealth);
    }
    public void DisableControls()
    {
        movement.DisableControls();
        attack.DisableControls();
        defense.DisableControls();
    }

    public PlayerControls GetPlayerControls()
    {
        return playerControls;
    }
}
