using System;
using System.Collections.Generic;
using UnityEngine;


public class Slot
{
    public GameObject SlotObj;
    
    public Slot ParentSlot;
    public List<Slot> ChildSlot = new List<Slot>();
    public Fade Fade;//임시
    
    public bool IsEmpty => ParentSlot == null || ChildSlot.Count == 0;
}


public class GridSystem : MonoBehaviour
{
    public Vector3 OffSet => transform.position;
    
    public RectInt gridRange;

    public Slot[,] Grids;

    public GameObject slotObj;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        gridRange = new RectInt(0, 0, (int)_spriteRenderer.size.x, (int)_spriteRenderer.size.y);
        
        Grids = new Slot[gridRange.xMax, gridRange.yMax];
        for (int x = 0; x < gridRange.xMax; x++)
        {
            for (int y = 0; y < gridRange.yMax; y++)
            {
                Slot instance = Grids[x, y] = new Slot();
                instance.SlotObj = Instantiate(slotObj, transform);
                instance.SlotObj.transform.position = new Vector3(x, y, -0.1f);
                instance.Fade = instance.SlotObj.GetComponent<Fade>();
            }
        }
    }

    public bool TrySetSlot(Vector3 target, List<Vector2Int> childSlots, out Vector3 outputPos)
    {
        Vector3Int temp = Vector3Int.RoundToInt(target-OffSet);
        outputPos = temp + OffSet;

        bool isMatching = gridRange.Contains((Vector2Int)temp);

        if (isMatching)
        {
            foreach (Vector2Int childSlot in childSlots)
            {
                Grids[temp.x + childSlots[0].x, temp.y + childSlots[0].y].ChildSlot.Add(Grids[temp.x + childSlot.x, temp.y + childSlot.y]);
            
                Grids[temp.x + childSlot.x, temp.y + childSlot.y].ParentSlot =
                    Grids[temp.x + childSlots[0].x, temp.y + childSlots[0].y];
            }
        }
        
        return isMatching;
    }

    public void FadeSlot(Vector3 target,bool isAllowed)
    {
        Debug.Log("FadeSlot");
        Vector3Int temp = Vector3Int.RoundToInt(target-OffSet);
        if(CheckGridBounds(temp))
        {
            Grids[temp.x, temp.y].Fade.SetAbleColor(isAllowed);
            Grids[temp.x, temp.y].Fade.CheakAndPlayFading();
        }

    }

    public bool CheckGridBounds(Vector3Int target)
    {
        return 0 <= target.x && target.x < Grids.GetLength(0) && 0 <= target.y && target.y < Grids.GetLength(1);
    }
    public bool CheckGridBounds(Vector3 target)
    {        
        Vector3Int temp = Vector3Int.RoundToInt(target-OffSet);
        bool isInRange = 0 <= temp.x && temp.x < Grids.GetLength(0) && 0 <= temp.y && temp.y < Grids.GetLength(1);

        return  isInRange && Grids[temp.x, temp.y].ParentSlot == null;
    }
}
