using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;


public class GM : MonoBehaviour
{


    const int NUMBER_OF_CARDS = 52;
    int[] Cards = new int[NUMBER_OF_CARDS];
    public List<Player> allPlayer = new List<Player>();

    public Round round = Round.non;
    public Process process = Process.start;
    public bool firstlead = true;
    public bool roundfirstlead = true;

    CheckCardScipt cks = new CheckCardScipt();

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

    public void checkCard(ArrayList playerCards)
    {
        if (roundfirstlead)
        {

            if (checkPlum3(playerCards))
            {

            }
            else
            {
                foreach (Player pl in allPlayer)
                {
                    if(pl.process == Player.Process.action)
                    {
                        pl.SysMsg = "請先出梅花3";
                    }
                }
                    
            }





        }
        else if(firstlead)
        {

        }



    }


    void Start()
    {
        process = Process.waitLogin;
        
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
                for (int i = 0; i < NUMBER_OF_CARDS; i++) Cards[i] = i;

                // 進行洗牌
                for (int i = 0; i < NUMBER_OF_CARDS; i++)
                {
                    int swap_index = UnityEngine.Random.Range(0, 51);
                    int t = Cards[i];
                    Cards[i] = Cards[swap_index];
                    Cards[swap_index] = t;
                }

                //發牌
                for (int i = 0; i < 13; i++)
                {
                    allPlayer[0].Cards[i] = Cards[i];
                    allPlayer[1].Cards[i] = Cards[13 + i];
                    allPlayer[2].Cards[i] = Cards[26 + i];
                    allPlayer[3].Cards[i] = Cards[39 + i];

                }

                for (int i = 0; i < NUMBER_OF_CARDS; i++)
                {
                    Debug.Log(Cards[i]);
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

                    if (allPlayer[0].Cards[i] == 8)
                    {
                        process = Process.p1Action;
                    }
                    else if((allPlayer[1].Cards[i] == 8))
                    {
                        process = Process.p2Action;
                    }
                    else if((allPlayer[2].Cards[i] == 8))
                    {
                        process = Process.p3Action;
                    }
                    else if((allPlayer[3].Cards[i] == 8))
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


    //驗證梅花3

    public bool checkPlum3(ArrayList playerCards)
    {
        for(int i = 0; i < playerCards.Count; i++)
        {
            Debug.Log(playerCards[i]);
            
        }
        return false;
    }
}