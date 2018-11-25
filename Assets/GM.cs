using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class GM : NetworkBehaviour
{


    const int NUMBER_OF_CARDS = 52;
    int[] Cards = new int[NUMBER_OF_CARDS];
    int[] playerlead;
    public List<Player> allPlayer = new List<Player>();

    public Round round = Round.non;
    public Process process = Process.start;
    public bool firstlead = true;
    public bool roundfirstlead = true;
    GameObject CardLeadShow;
    SyncListCardItem CardList = new SyncListCardItem();



    public enum Round
    {
        non,
        player1,
        player2,
        player3,
        player4
    }

    public enum Process
    {
        start,
        waitLogin,
        decidePlayer,
        shuffle,
        choicefirstplayer,
        p1Action,
        p2Action,
        p3Action,
        p4Action,
        checkWin,
        end,
    }

    //玩家登入

    public void Login(Player player)
    {
        allPlayer.Add(player);


        if (allPlayer.Count == 4)//
            process = Process.shuffle;//
    }


    //驗證出牌規則

    public void CheckCard(ArrayList playerCards,int count)
    {

        if (count < 1 || count > 5 || count == 4) LeadError(); //出牌數量不對

        playerlead = new int[playerCards.Count];

        //第一次出牌
        if (roundfirstlead)
        {
            //驗證出牌含有梅花3
            if (!CheckPlum3(playerCards))
            {
                foreach (Player pl in allPlayer)
                {
                    if (pl.process == Player.Process.action) pl.TipMsg = "請先出梅花3";
                }
            }


            //驗證牌組
            if (GmCheck(playerCards, count))
            {


                CardLeadShow.SetActive(true);

                for (int i = 0; i > 5; i++)
                {
                    if (i >= playerCards.Count) GameObject.Find("x" + (i + 1)).GetComponent<SpriteRenderer>().sprite = null;
                    GameObject.Find("x"+(i+1)).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Convert.ToString(playerCards[i]));
                    
                }
                


                if (allPlayer[0].process == Player.Process.action)
                {
                    process = Process.p2Action;
                }
                else if (allPlayer[1].process == Player.Process.action)
                {
                    process = Process.p3Action;
                }
                else if (allPlayer[2].process == Player.Process.action)
                {
                    process = Process.p4Action;
                }
                else if (allPlayer[3].process == Player.Process.action)
                {
                    process = Process.p1Action;
                }
            }
            else
            {
                LeadError();
            }
            
        }


    }


    void Start()
    {
        process = Process.waitLogin;
        Instantiate(CardLeadShow, transform.position, transform.rotation);
        CardLeadShow.SetActive(false);


    }

    void Update()
    {

        switch (process)
        {
            case Process.waitLogin:
                foreach (Player pl in allPlayer)
                    pl.SysMsg = "等待玩家連線...目前人數:" + allPlayer.Count;

                break;

            case Process.shuffle:



                foreach (Player pl in allPlayer)
                    pl.SysMsg = "遊戲開始 開始發牌";



                // 重製卡牌
                for (int i = 0; i < NUMBER_OF_CARDS; i++){ Cards[i] = i; }

                // 進行洗牌
                for (int i = 0; i < NUMBER_OF_CARDS; i++)
                {
                    int swap_index = UnityEngine.Random.Range(0, 51);
                    int t = Cards[i];
                    Cards[i] = Cards[swap_index];
                    Cards[swap_index] = t;
                }

                // 建立牌庫
                
                for (int i = 0; i < NUMBER_OF_CARDS; i++)
                {
                    CardStruct card = new CardStruct();
                    card.UID = Cards[i];
                    CardList.Add(card);

                    
                }

                //建立牌庫並發牌
                for (int i = 0; i < 13; i++)
                {

                    allPlayer[0].Server_AddCard(CardList.GetItem(i));
                    allPlayer[1].Server_AddCard(CardList.GetItem(13 + i));
                    allPlayer[2].Server_AddCard(CardList.GetItem(26 + i));
                    allPlayer[3].Server_AddCard(CardList.GetItem(39 + i));

                }


                allPlayer[0].IsGameStart = true;
                allPlayer[1].IsGameStart = true;
                allPlayer[2].IsGameStart = true;
                allPlayer[3].IsGameStart = true;

                process = Process.decidePlayer;

                break;

            case Process.decidePlayer:

                foreach (Player pl in allPlayer)
                {
                    pl.SysMsg = "等待時間 準備開始 尋找梅花3玩家";
                    
                }

                Load();
                
                for (int i = 0; i < NUMBER_OF_CARDS / 4; i++)
                {

                    if (allPlayer[0].HaveCards.GetItem(i).UID == 8)
                    {
                        process = Process.p1Action;
                    }
                    else if (allPlayer[1].HaveCards.GetItem(i).UID == 8)
                    {
                        process = Process.p2Action;
                    }
                    else if (allPlayer[2].HaveCards.GetItem(i).UID == 8)
                    {
                        process = Process.p3Action;
                    }
                    else if (allPlayer[3].HaveCards.GetItem(i).UID == 8)
                    {
                        process = Process.p4Action;
                    }

                }
                 break;

            case Process.p1Action:
                allPlayer[0].SysMsg = "輪到你了!";
                allPlayer[0].SetProcess(Player.Process.action);
                allPlayer[1].SysMsg = "等待對方...";
                allPlayer[1].SetProcess(Player.Process.wait);
                allPlayer[2].SysMsg = "等待對方...";
                allPlayer[2].SetProcess(Player.Process.wait);
                allPlayer[3].SysMsg = "等待對方...";
                allPlayer[3].SetProcess(Player.Process.wait);
                break;

            case Process.p2Action:
                allPlayer[1].SysMsg = "輪到你了!";
                allPlayer[1].SetProcess(Player.Process.action);
                allPlayer[0].SysMsg = "等待對方...";
                allPlayer[0].SetProcess(Player.Process.wait);
                allPlayer[2].SysMsg = "等待對方...";
                allPlayer[2].SetProcess(Player.Process.wait);
                allPlayer[3].SysMsg = "等待對方...";
                allPlayer[3].SetProcess(Player.Process.wait);
                break;

            case Process.p3Action:
                allPlayer[2].SysMsg = "輪到你了!";
                allPlayer[2].SetProcess(Player.Process.action);
                allPlayer[0].SysMsg = "等待對方...";
                allPlayer[0].SetProcess(Player.Process.wait);
                allPlayer[1].SysMsg = "等待對方...";
                allPlayer[1].SetProcess(Player.Process.wait);
                allPlayer[2].SysMsg = "等待對方...";
                allPlayer[2].SetProcess(Player.Process.wait);
                break;

            case Process.p4Action:
                allPlayer[3].SysMsg = "輪到你了!";
                allPlayer[3].SetProcess(Player.Process.action);
                allPlayer[1].SysMsg = "等待對方...";
                allPlayer[1].SetProcess(Player.Process.wait);
                allPlayer[2].SysMsg = "等待對方...";
                allPlayer[2].SetProcess(Player.Process.wait);
                allPlayer[0].SysMsg = "等待對方...";
                allPlayer[0].SetProcess(Player.Process.wait);
                break;

            case Process.checkWin:
                switch (round)
                {
                    case Round.player1:
                        allPlayer[0].SysMsg = "Winner";
                        allPlayer[1].SysMsg = "Loser";
                        break;
                    case Round.player2:
                        allPlayer[1].SysMsg = "Winner";
                        allPlayer[0].SysMsg = "Loser";
                        break;
                }
                allPlayer[0].SetProcess(Player.Process.end);
                allPlayer[1].SetProcess(Player.Process.end);
                process = Process.end;
                break;
        }
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(3);    //注意等待时间的写法
    }

    //提示玩家出牌不符
    public void LeadError()
    {
        foreach (Player pl in allPlayer)
        {
            if (pl.process == Player.Process.action)
            {
                pl.TipMsg = "出牌不符";
            }
        }
    }


    //驗證梅花3

    public bool CheckPlum3(ArrayList playerCards)
    {
        for(int i = 0; i < playerCards.Count; i++)
        {
            if ((Int32)playerCards[i] == 8)
            {
                return true;
            }
            
        }
        return false;
    }

    //驗證牌組合法

    public bool GmCheck(ArrayList playerCards, int count)
    {

        CheckCardScipt checkCardScipt = new CheckCardScipt();

        for (int i = 0; i < playerlead.Length; i++)
        {
            playerlead[i] = (int)playerCards[i];
        }


        if (count == 5)
        {
            if (checkCardScipt.Check5(playerlead[0], playerlead[1], playerlead[2], playerlead[3], playerlead[4]).Equals("同花順"))
            {
                return true;
            }
            if (checkCardScipt.Check5(playerlead[0], playerlead[1], playerlead[2], playerlead[3], playerlead[4]).Equals("鐵支"))
            {
                return true;
            }
            if (checkCardScipt.Check5(playerlead[0], playerlead[1], playerlead[2], playerlead[3], playerlead[4]).Equals("胡蘆"))
            {
                return true;
            }
            if (checkCardScipt.Check5(playerlead[0], playerlead[1], playerlead[2], playerlead[3], playerlead[4]).Equals("順子"))
            {
                return true;
            }

            return false;
        }else if(count == 2)
        {
            if (checkCardScipt.IsPair(playerlead[0], playerlead[1])) return true;
            return false;
        }else if(count == 1)
        {
            return true;
        }
        return false;
    }



}