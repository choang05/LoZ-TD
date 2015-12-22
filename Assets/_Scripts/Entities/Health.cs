using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour 
{
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP = 100;
    [SerializeField] [Range(0, 1)] private float hpPercentage;
    [SerializeField] private bool isDead = false;

    // UI
    public Slider healthBarSlider;
    private Text healthBarText;
    [SerializeField] private bool showHealth = false;

    //  Animation
    private Animator animator;
    static int DeathTriggerHash = Animator.StringToHash("DeathTrigger");
    static int isDeadHash = Animator.StringToHash("isDead");

    void Awake()
    {
        healthBarText = healthBarSlider.GetComponentInChildren<Text>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
	{
		currentHP = maxHP;
		hpPercentage = currentHP / maxHP;
        healthBarSlider.value = hpPercentage;
        if(healthBarText != null)
            healthBarText.text = currentHP + "/" + maxHP;
        if(showHealth == true)
            healthBarSlider.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(currentHP < 0) 
			currentHP = 0;

		else if(currentHP > maxHP) 
			currentHP = maxHP;	
	}

	public void AdjustHP(float amount)
	{
		if(!isDead)
		{
			currentHP += amount;
			hpPercentage = currentHP / maxHP;
		}

		//If HP hits 0, declare it Dead if not already
		if(currentHP <= 0 && !isDead)
		{
			isDead = true;
            animator.SetTrigger(DeathTriggerHash);
            animator.SetBool(isDeadHash, isDead);
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
        //set { currentHP = value; }
    }
    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    public float HPPercentage
    {
        get { return hpPercentage; }
        //set { hpPercentage = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
}
