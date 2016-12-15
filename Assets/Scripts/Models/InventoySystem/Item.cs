using UnityEngine;
using System.Collections;

public abstract class Item{

    public string id { get; protected set; }
    public string name { get; protected set; }
    public string description { get; protected set; }

    public Item(string id, string name, string description ) {
        this.id = id;
        this.name = name;
        this.description = description;
    }


}
