using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour 
{
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP = 100;
    [SerializeField] private float hpPercentage;
    [SerializeField] private bool isDead = false;
	
	//public Animator anim;
	//private Slider healthBar;
	//private Death _death;
	private Transform UIObj;

    void Awake () 
	{
		//anim = GetComponentInChildren<Animator>();
		//healthBar = GetComponentInChildren<Slider>();
		//_death = GetComponent<Death>();
		//_AIPath = GetComponent<AIPath>();
		UIObj = transform.FindChild("UI");
		//exploder = GetComponentInChildren<Exploder>();
	}

	void Start()
	{
		currentHP = maxHP;
		hpPercentage = currentHP / maxHP * 100;
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
			hpPercentage = currentHP / maxHP * 100;
            //healthBar.value = hpPercentage;
		}

		//If HP hits 0, declare it Dead if not already
		if(currentHP <= 0 && !isDead)
		{
			isDead = true;
			//healthBar.gameObject.SetActive(false);
			death();
		}
	}

	public void death()
	{
		//SendMessageUpwards("decreaseCount", SendMessageOptions.DontRequireReceiver);
		//_death.destroy();
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
    public float HPPercentage
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
