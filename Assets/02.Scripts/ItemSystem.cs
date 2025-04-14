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


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteCenter = _spriteRenderer.size/2;
        _lastMousePos = Input.mousePosition;

    }
    
    public void OnMouseDown()
    {
        _startPos = Pos;
    }

    public void OnMouseDrag()
    {
        Vector3 centerPos = GameManager.Instance.MousePos - (Vector3)_spriteCenter;
        centerPos.z = Pos.z;
        transform.position = centerPos;

        if (GetMouseSpeed() < 10f)
        {

            bool isAllowed = true;

            foreach (Vector2 childPos in itemSO.childSlots)
            {

                if (GameManager.Instance.grid.CheckGridBounds(Pos+(Vector3)childPos) == false)
                    isAllowed = false;
            }

            foreach (Vector2 childPos in itemSO.childSlots)
            {
                GameManager.Instance.grid.FadeSlot(Pos+(Vector3)childPos,isAllowed);

            }
        }
    }

    public void OnMouseUp()
    {
        if (GameManager.Instance.grid.TrySetSlot(Pos,itemSO.childSlots, out Vector3 output))
        {
            transform.position = output;
        }
        else transform.position = _startPos;
    }

    private float GetMouseSpeed()
    {
        Vector3 currentMousePos = Input.mousePosition;
        float distance = Vector3.Distance(currentMousePos, _lastMousePos);

        _lastMousePos = currentMousePos;
        return distance / Time.deltaTime;
    }



}
