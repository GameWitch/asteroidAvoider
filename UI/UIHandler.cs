using System.Collections;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text time;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider weaponSlider;

    [SerializeField] GameObject WLUIGameObject;
    [SerializeField] OnScreenButtonVisuals buttonVisuals;

    WinLoseUI WLUI;
    PlayerControls playerControls;
    Attack attack;
    Defense defense;
    Movement movement;
    PlayerHealth health;
    ScoreKeeper scoreKeeper;
    

    void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();
        level.text = scoreKeeper.GetLevel().ToString();
    }

    private void Start()
    {
        health.OnHealthIncrease += Health_OnHealthIncrease;
        health.OnHealthDecrease += Health_OnHealthDecrease;
        healthSlider.maxValue = health.GetHealthValue();
        healthSlider.value = healthSlider.maxValue;


        // eventually we'll only subscribe IF we have it selected in the settings
        defense.OnShieldPickup += Defense_OnShieldPickup;
        defense.OnShieldDestroy += Defense_OnShieldDestroy;
        defense.OnShieldValueChange += Defense_OnShieldValueChange;
        shieldSlider.gameObject.SetActive(false);

        attack.OnWeaponPickup += Attack_OnWeaponPickup;
        attack.OnAmmoChange += Attack_OnAmmoChange;
        attack.OnWeaponDestroy += Attack_OnWeaponDestroy;
        weaponSlider.gameObject.SetActive(false);
    }
    void Update()
    {
        time.text = scoreKeeper.GetTime();

    }

    private void Attack_OnWeaponDestroy(object sender, EventArgs e)
    {
        weaponSlider.gameObject.SetActive(false);
    }

    private void Attack_OnAmmoChange(object sender, Attack.OnAmmoChangeEventArgs e)
    {
        weaponSlider.value = e._ammo;
    }

    private void Attack_OnWeaponPickup(object sender, Attack.OnAmmoChangeEventArgs e)
    {
        weaponSlider.maxValue = e._ammo;
        weaponSlider.value = e._ammo;
        weaponSlider.gameObject.SetActive(true);
    }



    private void Defense_OnShieldValueChange(object sender, Defense.OnShieldValueChangeEventArgs e)
    {
        shieldSlider.value = e._shieldCurrentValue;
    }

    private void Defense_OnShieldDestroy(object sender, Defense.OnShieldValueChangeEventArgs e)
    {
        shieldSlider.value = e._shieldCurrentValue;
        shieldSlider.gameObject.SetActive(false);
    }

    private void Defense_OnShieldPickup(object sender, EventArgs e)
    {
        shieldSlider.gameObject.SetActive(true);
    }

    private void Health_OnHealthDecrease(object sender, PlayerHealth.OnHealthChangeEventArgs e)
    {
        healthSlider.value = e._health;
    }

    private void Health_OnHealthIncrease(object sender, PlayerHealth.OnHealthChangeEventArgs e)
    {
        healthSlider.value = e._health;
    }



    private IEnumerator PopUpWinLose(bool win)
    {
        health.OnHealthIncrease -= Health_OnHealthIncrease;
        health.OnHealthDecrease -= Health_OnHealthDecrease;
        yield return new WaitForSeconds(0.5f);

        GameObject wluiClone = Instantiate(WLUIGameObject, transform);
        WLUI = wluiClone.GetComponent<WinLoseUI>();
        WLUI.GetPlayerControls(playerControls);
        WLUI.GetLevel(scoreKeeper.GetLevel());

        if (win)
        {
            WLUI.Win();
        }
        else
        {
            WLUI.Lose();
        }
    }

    public void WinCondition()
    {
        StartCoroutine(PopUpWinLose(true));
    }

    public void LoseCondition()
    {
        StartCoroutine(PopUpWinLose(false));
    }

    public void GetPlayerControls(PlayerControls pc, Attack at, Defense df, Movement move, PlayerHealth ph)
    {
        playerControls = pc;
        attack = at;
        defense = df;
        movement = move;
        health = ph;
        buttonVisuals.GetControls(movement, attack, defense);

    }
}

public struct Score 
{
    public string level;
    public string time;
    public string distanceTraveled;
    public string thrustUsed;
    public string asteroidsBlasted;

    public Score(string level, string time, string distanceTraveled, string thrustUsed, string asteroidsBlasted)
    {
        this.level = level;
        this.time = time;
        this.distanceTraveled = distanceTraveled;
        this.thrustUsed = thrustUsed;
        this.asteroidsBlasted = asteroidsBlasted;
    }



}