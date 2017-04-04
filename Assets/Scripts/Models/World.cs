using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class World : IXmlSerializable
{

    // size of the map
    public int Width // x
    {
        get;
        protected set;
    }

    public int Height // z
    {
        get;
        protected set;
    }

    // Array of all the tiles in the world
    private Tile[,] _tiles;

    public List<Room> Rooms
    {
        get;
        set;
    }

    public PlayerCharacter Player
    {
        get;
        set;
    }

    // Emtpy Constructor just for Serialization
    public World()
    {
    }

    public World(int width, int height)
    {
        SetupWorld(width, height);
    }

    // sets up the World
    public void SetupWorld(int width, int height)
    {
        this.Width = width;
        this.Height = height;

        _tiles = new Tile[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                _tiles[x, z] = new Tile(x, z);
            }
        }

        // The Player itself.
        // Parameters are SpawnTile, (max)health, (max)saturation, (max)stamina
        this.Player = new PlayerCharacter(GetTileAt(1, 1), 20, 30, 40, 10);
    }

    // returns the tile at the specified location
    public Tile GetTileAt(int x, int z)
    {
        return _tiles[x, z];
    }

    // retuns the tile at the specified location (floored to int)
    public Tile GetTileAt(float x, float z)
    {
        return _tiles[Mathf.FloorToInt(x), Mathf.FloorToInt(z)];
    }

    // must have for Serialization, can be ignored
    public XmlSchema GetSchema()
    {
        return null;
    }

    #region RoomGenerator

    // creates the Rooms and returns a List with information about them
    public void CreateRooms()
    {

        // create the rooms
        this.Rooms = new List<Room>();

        // number of the atempts, how often the algorithm will try to place rooms
        int atempts = Mathf.FloorToInt(Mathf.Sqrt(this.Height * this.Width));

        // size of the rooms will be between 3 and a certain calculated value based on the world width/height
        const int minRoomHeight = 3;
        const int minRoomWidth = 3;
        int maxRoomHeight = (Mathf.FloorToInt(Mathf.Pow(Mathf.Pow(this.Height, 2f), 1 / 3f) - (Mathf.Sqrt((Mathf.Pow(Mathf.Pow(this.Height, 2f), 1 / 3f))))));
        int maxRoomWidth = (Mathf.FloorToInt(Mathf.Pow(Mathf.Pow(this.Width, 2f), 1 / 3f) - (Mathf.Sqrt((Mathf.Pow(Mathf.Pow(this.Width, 2f), 1 / 3f))))));
        if(maxRoomHeight % 2 == 0)
            maxRoomHeight++;
        if(maxRoomWidth % 2 == 0)
            maxRoomWidth++;

        // try to place rooms by the number of atempts
        for(int i = 0; i < atempts; i++)
        {
            // first determine what size the next room will have
            int roomHeight = 0;
            int roomWidth = 0;
            while(roomHeight % 2 == 0)
                roomHeight = Random.Range(minRoomHeight, maxRoomHeight + 1);
            while(roomWidth % 2 == 0)
                roomWidth = Random.Range(minRoomWidth, maxRoomWidth + 1);
            // create a random starttile for the room
            int tileX = 0;
            int tileZ = 0;
            // the tile can be at most at the world height/width minus the room height/width
            // otherwise the room could spawn "outside" the world
            while(tileX % 2 == 0)
                tileX = Random.Range(1, this.Width - roomWidth);
            while(tileZ % 2 == 0)
                tileZ = Random.Range(1, this.Height - roomHeight);
            // now a random valid Tile is selected
            // This Tile will be the "Left Under Tile" for the room
            // then check if the temporary room collides with any other room
            if(RoomCollidesWithOtherRoom(tileX, tileZ, tileX + roomWidth - 1, tileZ + roomHeight - 1, this.Rooms) == false)
            {
                this.Rooms.Add(new Room(GetTileAt(tileX, tileZ), GetTileAt(tileX + roomWidth - 1, tileZ + roomHeight - 1)));
            }
        }
    }

    // function checks if the given room collides with any other room already placed
    private static bool RoomCollidesWithOtherRoom(int lowerX, int lowerZ, int upperX, int upperZ, List<Room> alreadyValidRooms)
    {
        // Collision appears when all of these matches:
        // 1. Left side of the roomThatCollides is left of the right side of a room
        // 2. Right side of the roomThatCollides is right of the left side of a room
        // 3. Top side of the roomThatCollides is above the bottom side of a room
        // 4. Bottom side of the roomThatCollides is under of the top side of a room
        // Also i want at least 1 corridor between the rooms
        foreach(Room room in alreadyValidRooms)
        {
            if(
                //1
                lowerX <= room.UpperTile.X + 2 &&
                //2
                upperX >= room.LowerTile.X - 2 &&
                //3
                upperZ >= room.LowerTile.Z - 2 &&
                //4
                lowerZ <= room.UpperTile.Z + 2
            )
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region Saving and Loading

    // saves the World to Xml
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Width", this.Width.ToString());
        writer.WriteAttributeString("Height", this.Height.ToString());

        writer.WriteStartElement("Tiles");
        for(int x = 0; x < this.Width; x++)
        {
            for(int y = 0; y < this.Height; y++)
            {
                writer.WriteStartElement("Tile");
                _tiles[x, y].WriteXml(writer);
                writer.WriteEndElement();
            }
        }
        writer.WriteEndElement();

        writer.WriteStartElement("Rooms");
        foreach(Room room in this.Rooms)
        {
            writer.WriteStartElement("Room");
            room.WriteXml(writer);
            writer.WriteEndElement();
        }
        writer.WriteEndElement();

        writer.WriteStartElement("Characters");
        writer.WriteStartElement("Player");
        this.Player.WriteXml(writer);
        writer.WriteEndElement();
        writer.WriteEndElement();

        Debug.Log("WriteXML");
    }

    public void ReadXml(XmlReader reader)
    {
        Debug.Log("World::ReadXML");

        reader.MoveToAttribute("Width");
        this.Width = reader.ReadContentAsInt();
        reader.MoveToAttribute("Height");
        this.Height = reader.ReadContentAsInt();
        //reader.ReadTo
        reader.MoveToElement();


        SetupWorld(this.Width, this.Height);

        reader.ReadToDescendant("Tiles");
        reader.ReadToDescendant("Tile");
        while(reader.IsStartElement("Tile"))
        {

            reader.MoveToAttribute("X");
            int x = reader.ReadContentAsInt();
            reader.MoveToAttribute("Z");
            int z = reader.ReadContentAsInt();

            _tiles[x, z].ReadXml(reader);

            reader.ReadToNextSibling("Tile");
        }
        reader.ReadToFollowing("Characters");
        //reader.ReadToDescendant("Characters");
        reader.ReadToDescendant("Player");

        //this.Player = new PlayerCharacter(GetTileAt(1, 1), 20, 1, 40, 10);
        this.Player.ReadXml(reader);
    }

    #endregion
}
