
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

    public enum Process
    {
        start,
        receive,
        action,
        wait,
        end
    }

    public Text txt_play1;
    public Text txt_play2;
    public Text txt_play3;
    public Text txt_play4;
    GM gm;
    public int[] Cards = new int[13]; //這邊應該要修正使用Sync
    public Button btn_lead;
    public Button btn_pass;

    public Text txtSysSmg;

    public ArrayList card_selectlead;

    [SyncVar]
    public Process process = Process.start;

    [SyncVar]
    public string SysMsg;

    [SyncVar]
    public bool IsGameStart = false;
    


    public static GameObject[] image_cards = new GameObject[13];



    void Start()
    {

        if (isServer)
        {
            gm = GameObject.Find("GM").GetComponent<GM>();
            gm.Login(this);
        }

        if (isLocalPlayer)
        {
            card_selectlead = new ArrayList();
            btn_lead = GameObject.Find("btn_lead").GetComponent<Button>();
            btn_pass = GameObject.Find("btn_pass").GetComponent<Button>();
            btn_lead.onClick.AddListener(() => Cmdplayerlead());
            btn_pass.onClick.AddListener(() => Cmdplayerpass());
            txtSysSmg = GameObject.Find("txtSysMsg").GetComponent<Text>();


        }
    }

    void Update()
    {
        if (isServer)
        {

        }
           
        if (isLocalPlayer)
        {

            txtSysSmg.text = SysMsg;

            if (IsGameStart)
            {
                Array.Sort(Cards);

                for (int i = 0; i < 13; i++)
                {
                    image_cards[i] = GameObject.Find(Convert.ToString(i + 1));
                    image_cards[i].SetActive(true);
                    image_cards[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Convert.ToString(Cards[i]));
                    image_cards[i].AddComponent<Click_Card>();


                }
            }

                

            }
    }



    [Command]
    public void Cmdplayerlead()
    {
        int Card_lead_Count = 0;

        for (int i = 0; i < 13; i++)
        {
            if (image_cards[i].GetComponent<Click_Card>().onClick)
            {
                Card_lead_Count++;
                card_selectlead.Add(Cards[i]);
            }
        }

        if (Card_lead_Count == 0 && Card_lead_Count > 5 && Card_lead_Count == 4)
        {
            SysMsg = "出牌不符合";
        }else
        {
            gm.checkCard(card_selectlead);
        }

        
            
    }

    public void Cmdplayerpass()
    {

    }

    public void SetProcess(Process process)
    {
        this.process = process;
    }



}