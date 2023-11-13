using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinqExercises : MonoBehaviour
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
        var col4 = Ejercicio4();
        var col5 = Ejercicio5();
        var col6 = Ejercicio6();
        var col7 = Ejercicio7();

        Debug.Log("Ejercicio 1: ");
        foreach (var x in col1)
        {
            Debug.Log(x);
        }

        Debug.Log("Ejercicio 2: ");
        foreach (var x in col2)
        {
            Debug.Log(x.name);
        }

        Debug.Log("Ejercicio 3: ");
        foreach (var x in col3)
        {
            Debug.Log(x);
        }

        Debug.Log("Ejercicio 4: ");
        foreach (var x in col4)
        {
            Debug.Log(x);
        }

        Debug.Log("Ejercicio 5: ");
        Debug.Log(col5);

        Debug.Log("Ejercicio 6: ");
        foreach (var x in col6)
        {
            Debug.Log(x);
        }

        Debug.Log("Ejercicio 7: ");
        foreach (var x in col7)
        {
            Debug.Log(x);
        }


    }

    private IEnumerable<Rarity> Ejercicio1()
    {
        return items.Select(item => item.rarity);
    }

    private IEnumerable<Item> Ejercicio2()
    {
        return items.Where(item => item.rarity == Rarity.Rare || item.rarity == Rarity.Legendary);
    }

    private IEnumerable<float> Ejercicio3()
    {
        return items.Where(item => item.rarity == Rarity.Legendary).Select(legendaryItem => legendaryItem.cost);
    }

    private IEnumerable<Buff> Ejercicio4()
    {
        return items.Where(item => item.rarity == Rarity.Legendary).SelectMany(legendaryItem => legendaryItem.buffs);
    }

    private bool Ejercicio5()
    {
        return !otherItems.Concat(items).Any(item => item.rarity == Rarity.Legendary);
    }

    private IEnumerable<string> Ejercicio6()
    {

        return items.Select(itemSelected =>
        {
            if (items.Any(item => item.rarity == Rarity.Legendary))
            {
                return "(L)" + itemSelected.name;
            }
            else
            {
                return "(N)" + itemSelected.name;
            }
        });
    }

    private IEnumerable<Buff> Ejercicio7()
    {
        //Reemplacen esta linea con su ejercicio
        return items.SelectMany(item => item.buffs).Where(itemsBuff => itemsBuff == Buff.Attack);
    }

}
