using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour 
{
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP = 100;
    [SerializeField] [Range(0, 100)] private int hpPercentage;
    [SerializeField] private bool isDead = false;

    // UI
    public Slider healthBarSlider;
    private Text healthBarText;
    [SerializeField] private bool showHealth = false;

    void Awake()
    {
        healthBarText = healthBarSlider.GetComponentInChildren<Text>();
    }

    void Start()
	{
		currentHP = maxHP;
		hpPercentage = (int)((currentHP / maxHP) * 100);
        healthBarSlider.value = hpPercentage;
        if(healthBarText != null)
            healthBarText.text = currentHP + "/" + maxHP;
        if(showHealth == true)
            healthBarSlider.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(currentHP <= 0) 
			currentHP = 0;
		
		if(currentHP > maxHP) 
			currentHP = maxHP;	
	}

	public void AdjustHP(float amount)
	{
		if(!isDead)
		{
			currentHP += amount;
			hpPercentage = (int)((currentHP / maxHP) * 100);
		}

		//If HP hits 0, declare it Dead if not already
		if(currentHP <= 0 && !isDead)
		{
			isDead = true;
			//healthBar.gameObject.SetActive(false);
			//death();
		}

        // Update UI
        if (showHealth && healthBarSlider.gameObject.activeSelf == false)
            healthBarSlider.gameObject.SetActive(true);

        if (healthBarText != null)
            healthBarText.text = currentHP + "/" + maxHP;

        healthBarSlider.value = hpPercentage;
    }

    // Actuators and Mutators
    public float CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    public int HPPercentage
    {
        get { return hpPercentage; }
        set { hpPercentage = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
}
