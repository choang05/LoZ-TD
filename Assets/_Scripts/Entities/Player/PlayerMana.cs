using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMana : MonoBehaviour 
{
    [SerializeField] private float currentMana;
    [SerializeField] private float maxMana;
    [SerializeField] [Range(0, 1)] private float manaPercentage;
    [SerializeField]
    private bool canRegen = false;
    [SerializeField]
    private float manaRegenCooldown = 0;
    [SerializeField]
    private float manaRegenAmount = 0;

    // UI
    public Slider manaBarSlider;
    private Text manaBarText;

    //  Animation

    //  Audio

    void Awake()
    {
        manaBarText = manaBarSlider.GetComponentInChildren<Text>();
    }

    void Start()
	{
		currentMana = maxMana;
		manaPercentage = currentMana / maxMana;

        StartCoroutine(RegenMana());

        //  UI
        manaBarSlider.value = manaPercentage;
        manaBarText.text = currentMana + "/" + maxMana;
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(currentMana < 0)
            currentMana = 0;
		else if(currentMana > maxMana)
            currentMana = maxMana;	
	}

	public void AdjustMana(float amount)
	{
        currentMana += amount;
        manaPercentage = currentMana / maxMana;

        // Update UI
        manaBarText.text = currentMana + "/" + maxMana;
        manaBarSlider.value = manaPercentage;
    }

    IEnumerator RegenMana()
    {
        while(true)
        {
            yield return new WaitForSeconds(manaRegenCooldown);
            if(canRegen && currentMana < maxMana)
            {
                AdjustMana(manaRegenAmount);
            }
        }
    }

    // Actuators and Mutators
    public float CurrentMana
    {
        get { return currentMana; }
        set { currentMana = value; }
    }
    public float MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public float ManaPercentage
    {
        get { return manaPercentage; }
        //set { manaPercentage = value; }
    }
    public bool CanRegen
    {
        get { return canRegen; }
        set { canRegen = value; }
    }
}
