using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{
    public Item itemSO;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteCenter;
    private Vector3 _startPos;
    private Vector3 Pos => transform.position;
    
    
    //로직 선택 미스로 인한 하드코딩
    private List<Vector2Int> TestRotateChildSlot => itemSO.childSlots.Select(x => RotatePoints(x, RotateState)).ToList();
    private int RotateState => (int)transform.rotation.eulerAngles.z;
    
    
    private Vector3 _lastMousePos;
    private bool _onGrid = false;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteCenter = _spriteRenderer.size/2;
        _lastMousePos = Input.mousePosition;

    }
    
    public void OnMouseDown()
    {
        _startPos = Pos;
        //레이어(z축) 변경 => 이동 중에 다른 모든 아이템보다 위에 렌더링
        transform.position -= Vector3.forward/10;
        
        if (_onGrid)  GameManager.Instance.grid.UnSetSlot(Pos, TestRotateChildSlot);
    }

    public void OnMouseDrag()
    {
        GameManager.Instance.grid.CheckBoundAndFade(Pos, TestRotateChildSlot);

        if (Input.mouseScrollDelta.y > 0)
        {
            transform.Rotate(0,0,90);
        }else if (Input.mouseScrollDelta.y < 0)
        {
            transform.Rotate(0,0,-90);
            
        }

        
        #region 마우스 위치로 아이템 이동
        Vector3 centerPos = GameManager.Instance.MousePos - (Vector3)RotatePoints(_spriteCenter,RotateState);
        centerPos.z = Pos.z;
        transform.position = centerPos;
        #endregion


    }
    
    //로직 선택 미스로 인한 하드코딩2
    public Vector3 RotatePoints(Vector3 point, int angle)
    {
        // 0 ~ 360 정규화

        Vector3 rotatedDir = angle switch
            {
                0 => new Vector3(point.x, point.y),
                90 => new Vector3(-point.y, point.x),
                180 => new Vector3(-point.x, -point.y),
                270 => new Vector3(point.y, -point.x)
            };
    
        return rotatedDir;
    }
    public Vector2Int RotatePoints(Vector2Int point, int angle)
    {
        // 0 ~ 360 정규화

        Vector2Int rotatedDir = angle switch
        {
            0 => new Vector2Int(point.x, point.y),
            90 => new Vector2Int(-point.y-1, point.x),
            180 => new Vector2Int(-point.x-1, -point.y-1),
            270 => new Vector2Int(point.y, -point.x-1)
        };
    
        return rotatedDir;
    }

    public void OnMouseUp()
    {
        //레이어 원상복구
        transform.position += Vector3.forward/10;
        if (GameManager.Instance.grid.CheckBoundAndFade(Pos, TestRotateChildSlot))
        {
            _onGrid = true;
            transform.position = GameManager.Instance.grid.SetSlot(Pos, TestRotateChildSlot,itemSO);
        }
        else
        {
            transform.position = _startPos;
            GameManager.Instance.grid.SetSlotColor(SlotColor.None);
            if(_onGrid) transform.position = GameManager.Instance.grid.SetSlot(Pos, TestRotateChildSlot,itemSO);
        }
    }

    
    //현재 사용되지 않음
    // private float GetMouseSpeed()
    // {
    //     Vector3 currentMousePos = Input.mousePosition;
    //     float distance = Vector3.Distance(currentMousePos, _lastMousePos);
    //
    //     _lastMousePos = currentMousePos;
    //     return distance / Time.deltaTime;
    // }



}
