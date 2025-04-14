using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSystem : MonoBehaviour
{
    public Item itemSO;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteCenter;
    private Vector3 _startPos;
    private Vector3 Pos => transform.position;
    
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
        if(_onGrid) GameManager.Instance.grid.UnSetSlot(Pos, itemSO.childSlots);
    }

    public void OnMouseDrag()
    {
        #region 마우스 위치로 아이템 이동
        Vector3 centerPos = GameManager.Instance.MousePos - (Vector3)_spriteCenter;
        centerPos.z = Pos.z;
        transform.position = centerPos;
        #endregion


        GameManager.Instance.grid.CheckBoundAndFade(Pos, itemSO.childSlots);
    }

    public void OnMouseUp()
    {
        if (GameManager.Instance.grid.CheckBoundAndFade(Pos, itemSO.childSlots))
        {
            _onGrid = true;
            transform.position = GameManager.Instance.grid.SetSlot(Pos, itemSO.childSlots);
        }
        else
        {
            transform.position = _startPos;
            if(_onGrid) transform.position = GameManager.Instance.grid.SetSlot(Pos, itemSO.childSlots);
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
