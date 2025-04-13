using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteCenter;
    private Vector3 _startPos;
    private Vector3 Pos => transform.position;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteCenter = _spriteRenderer.size/2;
    }

    public void OnMouseDown()
    {
        _startPos = Pos;
    }

    public void OnMouseDrag()
    {
        Vector3 a = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector3)_spriteCenter;
        a.z = Pos.z;
        transform.position = a;
    }

    public void OnMouseUp()
    {
        if (GameManager.Instance.grid.CheckGridRange(Vector3Int.RoundToInt(Pos)))
        {
            transform.position = Vector3Int.RoundToInt(Pos);

        }
        else transform.position = _startPos;
    }
}
