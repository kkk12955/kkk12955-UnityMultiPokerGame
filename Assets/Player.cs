
using System;
using System.Collections;
using System.Collections.Generic;
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

    GM gm;
    public Text txt_play1;
    public Text txt_play2;
    public Text txt_play3;
    public Text txt_play4;
    public Button btn_lead;
    public Button btn_pass;
    public Text txtSysMsg;
    public Text txtTipMsg;
    public ArrayList card_selectlead;
    public ArrayList Mycard;
    public SyncListCardItem HaveCards = new SyncListCardItem(); //手牌
    public GameObject[] image_cards = new GameObject[13];


    [SyncVar]
    public bool LeadOver = false;

    [SyncVar]
    public Process process = Process.start;

    [SyncVar]
    public string SysMsg;

    [SyncVar]
    public string TipMsg;

    [SyncVar]
    public bool IsGameStart = false;





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
            Mycard = new ArrayList();
            btn_lead = GameObject.Find("btn_lead").GetComponent<Button>();
            btn_pass = GameObject.Find("btn_pass").GetComponent<Button>();
            btn_lead.onClick.AddListener(() => Cmdplayerlead());
            btn_pass.onClick.AddListener(() => Cmdplayerpass());
            txtSysMsg = GameObject.Find("txtSysMsg").GetComponent<Text>();
            txtTipMsg = GameObject.Find("txtTipMsg").GetComponent<Text>();


        }
    }

    void Update()
    {
        if (isServer)
        {

        }
           
        if (isLocalPlayer)
        {

            txtSysMsg.text = SysMsg;
            txtTipMsg.text = TipMsg;

            if (IsGameStart)
            {
                for (int i = 0; i < HaveCards.Count; i++)
                {
                    Mycard.Add(HaveCards.GetItem(i).UID);

                }

                Mycard.Sort();

                //更新UI
                Update_UI();



                IsGameStart = false;
            }

            if (LeadOver)
            {


                //更新UI
                Update_UI();

                LeadOver = false;
            }

                

            }
    }


    [Server]
    public void Server_AddCard(CardStruct cards)
    {
        //摸牌
        HaveCards.Add(cards);

    }


    [Command]
    public void Cmdplayerlead()
    {
        UnityEngine.Debug.Log("按下lead");

        if(this.process == Process.action)
        {

            int Card_lead_Count = 0;

            for (int i = 0; i < 13; i++)
            {
                if (image_cards[i].GetComponent<Click_Card>().onClick)
                {
                    Card_lead_Count++;
                    card_selectlead.Add(Mycard[i]);
                    UnityEngine.Debug.Log("選取數量為:"+Card_lead_Count);
                }
            }
            gm.CheckCard(card_selectlead,Card_lead_Count);
            card_selectlead.Clear();
            

        }
            
    }

    public void Cmdplayerpass()
    {

    }

    public void SetProcess(Process process)
    {
        this.process = process;
    }


    //更新UI
    public void Update_UI()
    {
        //排序




            for (int i = 0; i < Mycard.Count; i++)
        {

            image_cards[i] = GameObject.Find(Convert.ToString(i + 1));
            image_cards[i].SetActive(true);
            image_cards[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Convert.ToString(Mycard[i]));
            image_cards[i].AddComponent<Click_Card>();

        }

        for(int i = Mycard.Count; i < 13; i++)
        {
            image_cards[i].SetActive(false);
        }
    }



}