﻿using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour
{//animator for the player
    private Animator Self;

    //set stats
    [SerializeField]
    private float maxHealth = 20;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxMana = 20;

    //set currrent stats
    [SerializeField]
    private float currentMana;
    [SerializeField]
    private float AttackValue;
    [SerializeField]
    private float DefenceValue;
    [SerializeField]
    private float MagicAttackValue;

    //State rules for AI
    [SerializeField]
    private float lowHealthThreshold = 7;
    [SerializeField]
    private float lowManaThreshold = 5;
    [SerializeField]
    private float lowDefenceThreshold = 3;
    [SerializeField]
    private float lowAttackThreshold = 2;

    //user sliders
    [SerializeField]
    private Slider Health;
    [SerializeField]
    private Slider Mana;
    [SerializeField]
    private Slider Attack;
    [SerializeField]
    private Slider Defence;
    [SerializeField]
    private Slider MinHealth;
    [SerializeField]
    private Slider MinMana;
    [SerializeField]
    private Slider MinAttack;
    [SerializeField]
    private Slider MinDefence;

    //bool for if the player can change the values
    private bool UpdateDuring = false;

    [Header("Ability Parameters")]
    private float minHealAmount = 2;
    private float maxHealAmount = 5;

    //player values
    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    public float CurrentMana
    {
        get { return currentMana; }
    }

    public float MaxMana
    {
        get { return maxMana; }
    }

    public float CurrentDefence
    {
        get { return DefenceValue; }
    }
    public float CurrentAttack
    {
        get { return AttackValue; }
    }
    public float CurrentMagicAttack
    {
        get { return MagicAttackValue; }
    }
    public float MinimumHealth
    {
        get { return lowHealthThreshold; }
    }
    public float MinimumMana
    {
        get { return lowManaThreshold; }
    }
    public float MinimumAttack
    {
        get { return lowAttackThreshold; }
    }
    public float MinimumDefence
    {
        get { return lowDefenceThreshold; }
    }

    //Triggers for decisions
    public bool HasLowDefence
    {
        get { return DefenceValue < lowDefenceThreshold; }
    }

    public bool HasLowMana
    {
        get { return currentMana < lowManaThreshold; }
    }

    public bool HasLowAttack
    {
        get { return AttackValue < lowAttackThreshold; }
    }

    public bool HasLowHealth
    {
        get { return currentHealth < lowHealthThreshold; }
    }

    private void Awake()
    {
        //set player stats
        currentHealth = maxHealth;
        currentMana = maxMana;
       // AttackValue = 10;
        MagicAttackValue = 3;
        DefenceValue = 5;

        //stats from sliders
        currentHealth = (float)Health.value;
        currentMana = (float)Mana.value;
        AttackValue = (float)Attack.value;
        DefenceValue = (float)Defence.value;

        lowHealthThreshold = (float)MinHealth.value;
        lowManaThreshold = (float)MinMana.value;
        lowAttackThreshold = (float)MinAttack.value;
        lowDefenceThreshold = (float)MinDefence.value;

        //own animator
        //Get the Animator attached to the GameObject you are intending to animate.
        Self = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (UpdateDuring)
        {
            //stats from sliders   
            lowHealthThreshold = (float)MinHealth.value;
            lowManaThreshold = (float)MinMana.value;
            lowAttackThreshold = (float)MinAttack.value;
            lowDefenceThreshold = (float)MinDefence.value;
        }
    }

    public void SetUpdateDuring()
    {//allow the user to change the threashold values while it runs
        UpdateDuring = true;
    }

    public float Heal()
    {// heal the "player"
        float healAmount = Random.Range(minHealAmount, maxHealAmount);
        currentHealth += healAmount;
        return currentHealth;
    }

    public float Damage(float receivedDamage)
    {//recieve damage and reduce defence
        float damageAmount = receivedDamage - DefenceValue;
        DefenceValue--;
        if (DefenceValue < 0)
        {
            DefenceValue = 0;
        }
        if (damageAmount <= 0)
        {
            damageAmount = 0;
        }
        if (DefenceValue < 0)
        {
            DefenceValue = 1;
        }
        currentHealth -= damageAmount;
        Self.SetTrigger("Hit");

        return currentHealth;
    }

    public float increaseAttack()
    {//increase attack values
        AttackValue += Random.Range(1, 5);

        if(AttackValue>15)
        {
            AttackValue = 15;
        }
        return AttackValue;
    }

    public float increaseDefence()
    {//increase defence value
        DefenceValue += Random.Range(1, 5);
        if (DefenceValue > 15)
        {
            DefenceValue = 15;
        }
        return DefenceValue;
    }

    public float increaseMagicAttack()
    {//increase magic attack values
        MagicAttackValue++;
        return MagicAttackValue;
    }

    public float increaseMana()
    {//increase the mana
        currentMana += Random.Range(5,10);
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        return currentMana;
    }

    public float DecreaseMana(float cost)
    {//reduce the mana value
        currentMana = currentMana - cost;
        if(currentMana <0)
        {
            currentMana = 0;
        }
        return currentMana;
    }

    public float DecreaseAttack()
    {//decerease the attack values
        --AttackValue;
        if (AttackValue < 0)
        {
            AttackValue = 0;
        }
        return AttackValue;
    }
}
