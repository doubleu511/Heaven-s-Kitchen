using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MinigameStarter
{
    public DeleteDragableUI trashCanUI;

    private void Start()
    {
        trashCanUI.onDelete += () =>
        {

        };
    }
}