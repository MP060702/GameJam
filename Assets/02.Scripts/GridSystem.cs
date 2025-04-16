using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SlotColor
{
    Able,
    Unable,
    None
}
public class Slot
{
    public GameObject SlotObj;

    public Item InItemObj;
    
    public Slot ParentSlot;
    public List<Slot> ChildSlot = new List<Slot>();
    public SpriteRenderer SpriteRenderer;
    public Color Color
    {
        get => SpriteRenderer.color;
        set => SpriteRenderer.color = value;
    }
    
    public void Clear()
    {
        ChildSlot.Clear();
        ParentSlot = null;
    }

    public bool IsEmpty => ParentSlot == null;
    public bool Disable = false;
}


public class GridSystem : MonoBehaviour
{
    public Vector3 RectOffSet => transform.position;


    public Slot[,] Grids;
    public Rect storageBounds;

    public GameObject slotObj;
    
    private RectInt _gridBounds;
    private SpriteRenderer _spriteRenderer;
    private List<Vector2Int> _lastSelectSlots = new();
    private bool _isSetAble;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gridBounds = new RectInt(0, 0, (int)_spriteRenderer.size.x, (int)_spriteRenderer.size.y);

        Grids = new Slot[_gridBounds.xMax, _gridBounds.yMax];
        for (int x = 0; x < _gridBounds.xMax; x++)
        {
            for (int y = 0; y < _gridBounds.yMax; y++)
            {
                Slot instance = Grids[x, y] = new Slot();
                instance.SlotObj = Instantiate(slotObj, transform);
                instance.SlotObj.transform.localPosition = new Vector3(x, y, -0.1f);
                instance.SpriteRenderer = instance.SlotObj.GetComponent<SpriteRenderer>();
            }
        }
    }

    public Vector3 SetSlot(Vector3 target, List<Vector2Int> childSlots, Item itemInfo)
    {
        SetSlotColor(SlotColor.None);

        Vector3Int temp = Vector3Int.RoundToInt(target - RectOffSet);

        Grids[temp.x + childSlots[0].x, temp.y + childSlots[0].y].InItemObj = itemInfo;

        foreach (Vector2Int childSlot in childSlots)
        {
            Grids[temp.x + childSlots[0].x, temp.y + childSlots[0].y].ChildSlot
                .Add(Grids[temp.x + childSlot.x, temp.y + childSlot.y]);
            
            Grids[temp.x + childSlot.x, temp.y + childSlot.y].ParentSlot =
                Grids[temp.x + childSlots[0].x, temp.y + childSlots[0].y];
            
        }
        return temp + RectOffSet;
    }

    public void UnSetSlot(Vector3 target, List<Vector2Int> childSlots)
    {
        Vector3Int temp = Vector3Int.RoundToInt(target - RectOffSet);

        foreach (Vector2Int childSlot in childSlots)
        {
            Grids[temp.x + childSlots[0].x, temp.y + childSlots[0].y].Clear();

            Grids[temp.x + childSlot.x, temp.y + childSlot.y].ParentSlot = null;
        }

    }

    public bool CheckGridBound(Vector3 target, List<Vector2Int> childSlots)
    {
        SetSlotColor(SlotColor.None);
            
        Vector3Int offsetTarget = Vector3Int.RoundToInt(target - RectOffSet);

        childSlots = childSlots.Select(x => x + (Vector2Int)offsetTarget).ToList();

        _isSetAble = childSlots.All(x => _gridBounds.Contains(x) && Grids[x.x, x.y].IsEmpty);

        _lastSelectSlots = childSlots.Where(x=>_gridBounds.Contains(x)).ToList();

        SetSlotColor(_isSetAble ? SlotColor.Able : SlotColor.Unable);
        return _isSetAble;
    }

    public bool CheckStorageBound(Vector3 target, List<Vector2Int> childSlots)
    {
        List<Vector2> normalizeList = childSlots.Select(v => v+(Vector2)target).ToList();

        return normalizeList.All(x => storageBounds.Contains(x));
    }

    public void SetSlotColor(SlotColor slotColor)
    {
        _lastSelectSlots.ForEach(x =>
        {
            Grids[x.x, x.y].Color = slotColor switch
            {
                SlotColor.Able => Color.green,
                SlotColor.Unable => Color.red,
                SlotColor.None => Color.clear
            };
        });
    }
}