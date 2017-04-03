using UnityEngine;
using System.Collections;

public class Consumable : Item
{

    public int Quantity
    {
        get;
        set;
    }

    public int MaxQuantity
    {
        get;
        private set;
    }

    public Consumable(string id, string name, string description, int initialQuantity, int maxQuantity) : base(id, name, description)
    {
        this.Quantity = initialQuantity;
        this.MaxQuantity = maxQuantity;
    }
}
