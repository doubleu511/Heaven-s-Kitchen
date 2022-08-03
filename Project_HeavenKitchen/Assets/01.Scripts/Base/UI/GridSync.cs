using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridSync : MonoBehaviour
{
    private RectTransform rect;
    private GridLayoutGroup grid;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        grid = GetComponentInChildren<GridLayoutGroup>();
    }

    private void OnGUI()
    {
        int constraintCount = grid.constraintCount;
        int gridChildCount = grid.transform.childCount;
        int constraintValue = gridChildCount / constraintCount + gridChildCount % constraintCount;

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, (grid.cellSize.y + grid.spacing.y) * constraintValue + (grid.spacing.y * 2));
    }
}
