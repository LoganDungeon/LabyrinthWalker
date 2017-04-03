using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

// A rectangle room

//     innerTiles
// /// edgeTiles

///////////////rightUpperTile
///         ///
///         ///
///         ///
///         ///
///////////////
//  leftunderTile

public class Room : IXmlSerializable
{

    // lowerTile is the left under Tile of the room
    public Tile LowerTile
    {
        get; protected set;
    }

    // upperTile is the right upper Tile of the room
    public Tile UpperTile
    {
        get; protected set;
    }

    // reference to the world
    private World World
    {
        get
        {
            return WorldController.Instance.World;
        }
    }

    // length of the room in X direction (walls not included)
    public int XLength
    {
        get
        {
            return this.UpperTile.X - this.LowerTile.X + 1;
        }
    }

    // length of the room in Z direction (walls not included)
    public int ZLength
    {
        get
        {
            return this.UpperTile.Z - this.LowerTile.Z + 1;
        }
    }

    // Tiles, that are IN the room, but at the edge of the room
    private List<Tile> _edgeTiles;
    // Tiles, that are IN the room, and not at the edge of the room
    private List<Tile> _innerTiles;

    // just for Serialization
    public Room()
    {
    }

    public Room(Tile leftUnderTile, Tile rightUpperTile)
    {

        this.LowerTile = leftUnderTile;
        this.UpperTile = rightUpperTile;

        _edgeTiles = new List<Tile>();
        _innerTiles = new List<Tile>();
        AddTilesToLists();
        // change the tiles to be wall free
        CreateRoom();

        // TODO
        // For now i want to add between 1 and 4 entrances, Later on i maybe want to create entrances, based on the size of the room
        AddEntrances(Random.Range(1, 5));
    }

    // adds the Tiles in the room to the specific List, if the tiles are either edge tiles or not
    private void AddTilesToLists()
    {
        for(int x = this.LowerTile.X; x <= this.UpperTile.X; x++)
        {
            for(int z = this.LowerTile.Z; z <= this.UpperTile.Z; z++)
            {
                if(x == this.LowerTile.X || x == this.UpperTile.X || z == this.LowerTile.Z || z == this.UpperTile.Z)
                {
                    _edgeTiles.Add(World.GetTileAt(x, z));
                }
                else
                {
                    _innerTiles.Add(World.GetTileAt(x, z));
                }
            }
        }
    }

    // goes through every Tile in the room , mark it as visited and removes the wall on it
    private void CreateRoom()
    {
        for(int x = this.LowerTile.X - 1; x <= this.UpperTile.X + 1; x++)
        {
            for(int z = this.LowerTile.Z - 1; z <= this.UpperTile.Z + 1; z++)
            {
                Tile t = World.GetTileAt(x, z);
                if(x == this.LowerTile.X - 1 || x == this.UpperTile.X + 1 || z == this.LowerTile.Z - 1 ||
                    z == this.UpperTile.Z + 1)
                {
                    // This are the walls of the room
                    t.IsRoomWall = true;
                }
                else
                {
                    t.IsWall = false;
                }
            }
        }
    }

    // adds a certain number of entrances to the room by deleting a section of the wall that leads into the room
    private void AddEntrances(int entrancesNumber)
    {
        List<Tile> possibleEntranceTiles = new List<Tile>();
        // create a list with all the possible entrance tiles
        foreach(Tile tile in _edgeTiles)
        {
            if((tile.X % 2 != 0) && (tile.Z % 2 != 0) && tile.X != 1 && tile.X != World.Width - 2 && tile.Z != 1 &&
                tile.Z != World.Width - 2)
            {
                possibleEntranceTiles.Add(tile);
            }
        }

        for(int i = 0; i < entrancesNumber; i++)
        {
            // check if there are possible entrance Tiles left over
            if(possibleEntranceTiles.Count == 0)
            {
                return;
            }
            // chose a random tile for the entrance and create the entrance, aka mark it as "not in the room", so the maze Generator can go out of the room again.
            int temp = Random.Range(0, possibleEntranceTiles.Count);
            // 8 possibilities:
            //  - 4 corners
            //  - 4 edges

            // 1 corner, left  lower
            if(possibleEntranceTiles[temp].X == this.LowerTile.X && possibleEntranceTiles[temp].Z == this.LowerTile.Z)
            {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                switch(rand)
                {
                    case 0:
                        World.GetTileAt(possibleEntranceTiles[temp].X - 1, possibleEntranceTiles[temp].Z).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X - 1, possibleEntranceTiles[temp].Z).IsWall = false;
                        break;
                    case 1:
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z - 1).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z - 1).IsWall = false;
                        break;
                }
                continue;
            }
            // 2 corner, left  upper
            if(possibleEntranceTiles[temp].X == this.LowerTile.X && possibleEntranceTiles[temp].Z == this.UpperTile.Z)
            {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                switch(rand)
                {
                    case 0:
                        World.GetTileAt(possibleEntranceTiles[temp].X - 1, possibleEntranceTiles[temp].Z).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X - 1, possibleEntranceTiles[temp].Z).IsWall = false;
                        break;
                    case 1:
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z + 1).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z + 1).IsWall = false;
                        break;
                }
                continue;
            }
            // 3 corner, right lower
            if(possibleEntranceTiles[temp].X == this.UpperTile.X && possibleEntranceTiles[temp].Z == this.LowerTile.Z)
            {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                switch(rand)
                {
                    case 0:
                        World.GetTileAt(possibleEntranceTiles[temp].X + 1, possibleEntranceTiles[temp].Z).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X + 1, possibleEntranceTiles[temp].Z).IsWall = false;
                        break;
                    case 1:
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z - 1).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z - 1).IsWall = false;
                        break;
                }
                continue;
            }
            // 4 corner, right upper
            if(possibleEntranceTiles[temp].X == this.UpperTile.X && possibleEntranceTiles[temp].Z == this.UpperTile.Z)
            {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                switch(rand)
                {
                    case 0:
                        World.GetTileAt(possibleEntranceTiles[temp].X + 1, possibleEntranceTiles[temp].Z).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X + 1, possibleEntranceTiles[temp].Z).IsWall = false;
                        break;
                    case 1:
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z + 1).IsRoomWall =
                            false;
                        World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z + 1).IsWall = false;
                        break;
                }
                continue;
            }
            // 5 edge, left
            if(possibleEntranceTiles[temp].X == this.LowerTile.X)
            {
                // we are on an edge, so there is 1 possible entrances
                World.GetTileAt(possibleEntranceTiles[temp].X - 1, possibleEntranceTiles[temp].Z).IsRoomWall = false;
                World.GetTileAt(possibleEntranceTiles[temp].X - 1, possibleEntranceTiles[temp].Z).IsWall = false;
                continue;
            }
            // 6 edge, upper
            if(possibleEntranceTiles[temp].Z == this.UpperTile.Z)
            {
                World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z + 1).IsRoomWall = false;
                World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z + 1).IsWall = false;
                continue;
            }
            // 7 edge, right
            if(possibleEntranceTiles[temp].X == this.UpperTile.X)
            {
                // we are on a corner, so there is 1 possible entrances
                World.GetTileAt(possibleEntranceTiles[temp].X + 1, possibleEntranceTiles[temp].Z).IsRoomWall = false;
                World.GetTileAt(possibleEntranceTiles[temp].X + 1, possibleEntranceTiles[temp].Z).IsWall = false;
                continue;
            }
            // 8 edge, lower
            if(possibleEntranceTiles[temp].Z == this.LowerTile.Z)
            {
                // we are on a corner, so there are 2 possible entrances
                World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z - 1).IsRoomWall = false;
                World.GetTileAt(possibleEntranceTiles[temp].X, possibleEntranceTiles[temp].Z - 1).IsWall = false;
                continue;
            }
            possibleEntranceTiles.RemoveAt(temp);
        }
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("LeftUnderTile");
        writer.WriteAttributeString("X", this.LowerTile.X.ToString());
        writer.WriteAttributeString("Z", this.LowerTile.Z.ToString());
        writer.WriteEndElement();
        writer.WriteStartElement("RightUpperTile");
        writer.WriteAttributeString("X", this.UpperTile.X.ToString());
        writer.WriteAttributeString("Z", this.UpperTile.Z.ToString());
        writer.WriteEndElement();
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadToDescendant("LeftUnderTile");
        reader.MoveToAttribute("X");
        int x = reader.ReadContentAsInt();
        reader.MoveToAttribute("Z");
        int z = reader.ReadContentAsInt();
        this.LowerTile = this.World.GetTileAt(x, z);
        reader.ReadToNextSibling("RightUpperTile");
        reader.MoveToAttribute("X");
        x = reader.ReadContentAsInt();
        reader.MoveToAttribute("Z");
        z = reader.ReadContentAsInt();
        this.UpperTile = this.World.GetTileAt(x, z);
    }
}
