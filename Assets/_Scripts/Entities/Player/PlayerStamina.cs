using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStamina : MonoBehaviour
{
    //  Attributes
    //[SerializeField]
    private float currentStamina = 100;
    [SerializeField]
    private float staminaRecoverySpeed;

    //  References
    PlayerRange _playerRange;

    //  UI
    public Slider staminaBarSlider;
    private Text staminaBarText;

    void Awake()
    {
        _playerRange = GetComponent<PlayerRange>();
        staminaBarText = staminaBarSlider.GetComponentInChildren<Text>();
    }

    // Use this for initialization
    void Start ()
    {
        staminaBarSlider.value = currentStamina;
        if (staminaBarText != null)
            staminaBarText.text = currentStamina + "%";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentStamina < 0)
            currentStamina = 0;
        else if (currentStamina < 100 && !_playerRange.IsCharging)
            currentStamina += staminaRecoverySpeed * Time.deltaTime;
        else if(currentStamina > 100)
            currentStamina = 100;

        // UI
        if (currentStamina < 100)
        {
            staminaBarSlider.value = currentStamina;
            staminaBarText.text = staminaBarSlider.value.ToString("f1") + "%";
        }
        else if (currentStamina > 100)
        {
            staminaBarSlider.value = 100;
            staminaBarText.text = "100%";
        }

    }

    public void AdjustStamina(float amount)
    {
        currentStamina += amount;
    }

    // Actuators and Mutators
    public float CurrentStamina
    {
        get { return currentStamina; }
        //set { currentStamina = value; }
    }
}
