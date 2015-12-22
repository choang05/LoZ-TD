using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerShield : MonoBehaviour
{
    [SerializeField]
    private float currentShieldHP;
    [SerializeField]
    private float maxShieldHP;
    [SerializeField]
    [Range(0, 1)]
    private float shieldPercentage;
    //[SerializeField]
    //private bool canRegen = false;
    [SerializeField]
    private float shieldRecoveryCooldown = 0;
    //[SerializeField]
    private float shieldCooldownCounter = 0;
    [SerializeField]
    private float shieldRecoverySpeed = 0;

    //  Scripts

    // UI
    public Image shieldBarSlider;
    private Text shieldBarText;

    //  Animation

    //  Audio

    void Awake()
    {
        //  UI
        shieldBarText = shieldBarSlider.transform.parent.GetComponentInChildren<Text>();
    }

    void Start()
    {
        currentShieldHP = maxShieldHP;
        shieldPercentage = currentShieldHP / maxShieldHP;

        //  UI
        shieldBarSlider.fillAmount = shieldPercentage;
        shieldBarText.text = currentShieldHP / maxShieldHP * 100 + "%";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShieldHP < 0)
        {
            currentShieldHP = 0;
            shieldPercentage = 0;
        }
        else if (currentShieldHP > maxShieldHP)
        {
            currentShieldHP = maxShieldHP;
            shieldPercentage = currentShieldHP / maxShieldHP;
        }
        else if (currentShieldHP < maxShieldHP && shieldCooldownCounter <= 0)
        {
            currentShieldHP += shieldRecoverySpeed * Time.deltaTime;
            shieldPercentage = currentShieldHP / maxShieldHP;
        }

        if (shieldCooldownCounter > 0)
            shieldCooldownCounter -= Time.deltaTime;

        // UI
        if (currentShieldHP < maxShieldHP)
        {
            shieldBarSlider.fillAmount = ShieldPercentage;
            shieldBarText.text = (shieldBarSlider.fillAmount * 100).ToString("f0") + "%";
        }
        else if (currentShieldHP > maxShieldHP)
        {
            shieldBarSlider.fillAmount = shieldPercentage;
            shieldBarText.text = "100%";
        }
    }

    public void AdjustShieldHP(float amount)
    {
        currentShieldHP += amount;
        shieldPercentage = currentShieldHP / maxShieldHP;

        // Update UI
        shieldBarSlider.fillAmount = shieldPercentage;
        shieldBarText.text = currentShieldHP + "/" + maxShieldHP;
    }

    public void ResetCooldown()
    {
        shieldCooldownCounter = shieldRecoveryCooldown;
    }

    // Actuators and Mutators
    public float CurrentShieldHP
    {
        get { return currentShieldHP; }
        set { currentShieldHP = value; }
    }
    public float MaxShieldHP
    {
        get { return maxShieldHP; }
        set { maxShieldHP = value; }
    }
    public float ShieldPercentage
    {
        get { return shieldPercentage; }
        //set { shieldPercentage = value; }
    }
}
