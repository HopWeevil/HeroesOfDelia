using System;
using UnityEngine;

[Serializable]
public class LevelTransferData
{
    public string TransferTo;
    public Vector3 Position;

    public LevelTransferData(string transferTo, Vector3 position)
    {
        TransferTo = transferTo;
        Position = position;
    }
}