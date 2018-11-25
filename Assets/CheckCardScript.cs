using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckCardScipt : NetworkBehaviour{

    public bool IsPair(int a , int b)
    {
        if(a/4 == b/4)
        {
            return true;
        }

        return false;
        
    }

    public bool IsTriple(int a, int b , int c)
    {
        if (a / 4 == b / 4 && b / 4 == c / 4)
        {
            return true;
        }

        return false;


    }

    public bool IsQuad(int a, int b, int c , int d, int e)
    {
        if (a / 4 == b / 4 && b / 4 == c / 4 && c / 4 == d / 4)
        {
            return true;
        }else if (b / 4 == c / 4 && c / 4 == d / 4 && d / 4 == e / 4)
        {
            return true;
        }

        return false;
    }

    public bool IsFullhouse(int a, int b , int c , int d , int e)
    {


        if (IsPair(a, b))  //驗證前兩張是否是twoPair
        {
            if (IsTriple(c, d, e)) //驗證後三張是否是Triple
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (IsTriple(a, b, c)) //驗證前三張是否是Triple
        {
            if (IsPair(d, e)) //驗證後兩張是否是twoPair
            {
                return true;
            }
            else
            {
                return false;
            }
        }

         return false;
       
    }

    public bool IsStraight(int a, int b, int c, int d, int e)
    {
        if (e/4 - d/4 == 1 && d/4 - c/4 == 1 && c/4 - b/4 == 1 && b/4 - a/4 == 1) //驗證相鄰順子
        {
            return true;
        }
        else if (e / 4 - d / 4 == 1 && d / 4 - c / 4 == 1 && c / 4 - b / 4 == 1 && a / 4 == 0) //驗證跨越一張牌 例如 A 10 J Q K 
        {
            if(e/4 - a/4 == 12) //驗證跨越
            {
                return true; 
            }
            return false;
        }
        else if (e / 4 - d / 4 == 1 && d / 4 - c / 4 == 1 && a / 4 == 0 && b/4 ==1) //驗證跨越二張牌 例如 A 2 J Q K 
        {
            if(e/4 - a/4 == 12) //驗證跨越
            {
                return true;
            }
            return false;
        }
        else if (e / 4 - d / 4 == 1 && a / 4 == 0 && b / 4 == 1 && c / 4 == 2) //驗證跨越三張牌 例如 A 2 3 Q K 
        {
            if (e / 4 - a / 4 == 12) //驗證跨越
            {
                return true;
            }
            return false;
        }else if (e / 4 == 12 && a / 4 == 0 && b / 4 == 1 && c / 4 == 2 && d / 4 == 3) //驗證跨越四張牌 例如 A 2 3 4 K
        {
            if (e / 4 - a / 4 == 12) //驗證跨越
            {
                return true;
            }
            return false;
        }

            return false;
    }
    public bool IsStraightFlush(int a, int b, int c, int d, int e)
    {
        if(a%4 == b%4 && b%4 == c%4 && c%4 == d%4 && d%4 == e % 4)
        {
            if (IsStraight(a,b,c,d,e))
            {
                return true;
            }

            return false;
        }

        return false;
        
    }

    public String Check5(int a , int b , int c , int d , int e) 
    {
        if (IsStraightFlush(a, b, c, d, e)) return "同花順";
        if (IsQuad(a, b, c, d, e)) return "鐵支";
        if (IsStraight(a, b, c, d, e)) return "順子";
        if (IsFullhouse(a, b, c, d, e)) return "胡蘆";





        return null;
    }






















}
