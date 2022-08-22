using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MinigameStarter
{
    public DeleteDragableUI trashCanUI;
    public GameObject trashPrefab;

    private void Start()
    {
        trashCanUI.onDelete += (ingredient) =>
        {
            ThrowTrash(ingredient);
        };
    }

    private void ThrowTrash(IngredientSO ingredient)
    {

    }
}
