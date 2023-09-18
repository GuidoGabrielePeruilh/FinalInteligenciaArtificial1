public class Item{
	
    public string name;
    public float  cost;
    public int    durability;
    public Rarity rarity;
    public Buff[] buffs;
	
    public Item(string name, float cost, int durability, Rarity rarity, Buff[] buffs)
    {
        this.name       = name;
        this.cost       = cost;
        this.durability = durability;
        this.rarity     = rarity;
        this.buffs = buffs;
    }
	
}

public enum Rarity{
    Common,
    Rare,
    Legendary
}

public enum Buff
{
    Attack,
    Defense,
    Speed
}