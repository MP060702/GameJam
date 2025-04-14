using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public int tag; //임시
    public List<Vector2Int> childSlots = new List<Vector2Int>();

}
