using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct CardStruct 
{
    public int UID;         //卡片唯一id
}

public class SyncListCardItem : SyncListStruct<CardStruct> { }

public class Card {

    public CardStruct MyCardStruct = new CardStruct();

    static uint UID_Counter = 0;

    public virtual void Init(CardStruct value)
    {
        MyCardStruct = value;
    }





}
