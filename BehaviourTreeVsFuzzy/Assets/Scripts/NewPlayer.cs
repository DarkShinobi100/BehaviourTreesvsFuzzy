using UnityEngine;

public class NewPlayer : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 20;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int maxMana = 20;

    [SerializeField]
    private int currentMana;

    [SerializeField]
    private int AttackValue;

    [SerializeField]
    private int DefenceValue;

    [SerializeField]
    private int MagicAttackValue;

    //State rules for AI
    [SerializeField]
    private int lowHealthThreshold = 7;
    [SerializeField]
    private int lowManaThreshold = 5;
    [SerializeField]
    private int lowDefenceThreshold = 3;
    [SerializeField]
    private int lowAttackThreshold = 2;

    [Header("Ability Parameters")]
    private int minHealAmount = 2;
    private int maxHealAmount = 5;

    //player values
    public int CurrentHealth
    {
        get { return currentHealth; }
    }
    public int CurrentMana
    {
        get { return currentMana; }
    }
    public int CurrentDefence
    {
        get { return DefenceValue; }
    }
    public int CurrentAttack
    {
        get { return AttackValue; }
    }
    public int CurrentMagicAttack
    {
        get { return MagicAttackValue; }
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
        AttackValue = 2;
        MagicAttackValue = 3;
        DefenceValue = 1;
    }

    public int Heal()
    {
        int healAmount = Random.Range(minHealAmount, maxHealAmount);
        currentHealth += healAmount;
        return currentHealth;
    }

    public int Damage(int receivedDamage)
    {
        int damageAmount = DefenceValue - receivedDamage;
        DefenceValue--;
        if (damageAmount<=0)
        {
            damageAmount = 2;
        }
        if(DefenceValue <0)
        {
            DefenceValue = 1;
        }
        currentHealth -= damageAmount;
        return currentHealth;
    }

    public int increaseAttack()
    {
        AttackValue++;
        return AttackValue;
    }

    public int increaseDefence()
    {
        DefenceValue++;
        return DefenceValue;
    }

    public int increaseMagicAttack()
    {
        MagicAttackValue++;
        return MagicAttackValue;
    }

    public int increaseMana()
    {
        currentMana = currentMana+5;
        if(currentMana>maxMana)
        {
            currentMana = maxMana;
        }
        return currentMana;
    }
}
