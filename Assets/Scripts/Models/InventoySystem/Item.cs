using UnityEngine;
using System.Collections;

public abstract class Item
{

    public string Id
    {
        get;
        protected set;
    }

    public string Name
    {
        get;
        protected set;
    }

    public string Description
    {
        get;
        protected set;
    }

    protected Item(string id, string name, string description)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
    }


}
