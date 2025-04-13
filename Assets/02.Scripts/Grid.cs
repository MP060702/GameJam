using System.Collections.Generic;
using UnityEngine;


public class Slot
{
    public Slot ParentSlot;
    public List<Slot> ChildSlot = new List<Slot>();
    
    public bool IsEmpty => ParentSlot == null || ChildSlot.Count == 0;
}


public class Grid : MonoBehaviour
{
    [HideInInspector] public Vector2 offSet;
    
    public Rect gridRange;

    public Slot[,] Grids;

    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        offSet = transform.position;
        gridRange = new Rect(offSet.x, offSet.y, _spriteRenderer.size.x, _spriteRenderer.size.y);
    }

    public bool CheckGridRange(Vector3Int target)
    {
        return gridRange.Contains(target);
    }
}
