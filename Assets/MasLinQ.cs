using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasLinQ : MonoBehaviour 
{

    public List<Item> items = new List<Item>();
    public List<Item> otherItems = new List<Item>();


    void Start()
    {
        #region Inicialización de Items

        items.Add(new Item("Common Sword", 30, 2, Rarity.Common, new Buff[1] { Buff.Attack }));
        items.Add(new Item("Common Axe", 35, 1, Rarity.Common, new Buff[1] { Buff.Defense }));
        items.Add(new Item("Common Dagger", 25, 3, Rarity.Common, new Buff[1] { Buff.Speed }));
        items.Add(new Item("Rare Sword", 135, 4, Rarity.Rare, new Buff[2] { Buff.Attack, Buff.Speed }));
        items.Add(new Item("Rare Dagger", 100, 6, Rarity.Rare, new Buff[2] { Buff.Speed, Buff.Defense }));
        items.Add(new Item("Legendary Monster Slayer Sword", 250, 15, Rarity.Legendary, new Buff[3] { Buff.Attack, Buff.Defense, Buff.Speed }));
        items.Add(new Item("Legendary Axe", 245, 13, Rarity.Legendary, new Buff[3] { Buff.Attack, Buff.Defense, Buff.Attack }));

        otherItems.Add(new Item("Archer", 45, 7, Rarity.Common, new Buff[1] { Buff.Attack }));
        otherItems.Add(new Item("Fish Rood", 5, 1, Rarity.Common, new Buff[1] { Buff.Attack }));
        otherItems.Add(new Item("AK-47", 200, 8, Rarity.Rare, new Buff[2] { Buff.Attack, Buff.Speed }));
        otherItems.Add(new Item("Mega Rare Mage Weapon", 100000, 10, Rarity.Legendary, new Buff[3] { Buff.Speed, Buff.Defense, Buff.Attack }));

        #endregion

        var col1 = Ejercicio1();
        var col2 = Ejercicio2();
        var col3 = Ejercicio3();

        Debug.Log("Ejercicio 1: ");
        foreach (var x in col1)
        {
            Debug.Log($"{x.name} + {x.cost}");
        }

        Debug.Log("Ejercicio 2: ");
        foreach (var x in col2)
        {
            Debug.Log(x);
        }

        Debug.Log("Ejercicio 3: ");
        foreach (var x in col3)
        {
            Debug.Log($"{x.name} + {x.cost} + {x.rarity}");
        }
    }

    private IEnumerable<Item> Ejercicio1() {

        return items.Concat(otherItems).OrderBy(x => x.cost).Take(5);
    }
    
    private IEnumerable<Buff> Ejercicio2() 
    {

        return items.Zip(otherItems, (item, otherItem) =>
        {
            if (item.rarity >= otherItem.rarity)
                return item;
            else
                return otherItem;
        }).OrderBy(x=>x.rarity).SelectMany(x=> x.buffs);
    }
    
    private IEnumerable<Item> Ejercicio3() {

        return items.Concat(otherItems).Where(item => item.rarity == Rarity.Rare || item.rarity == Rarity.Legendary).OrderBy(itemCost => itemCost.cost).SkipWhile(x => x.rarity != Rarity.Legendary);
    }
}