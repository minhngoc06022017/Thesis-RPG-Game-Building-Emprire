using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class XPManager
{
    public static int CalculateXP(Enemy e)
    {
        
        int baseXP = (Player.Instance.MyLevel * 5) + 45;

        int grayLevel = CalculateGrayLevel();

        int totalXP = 0;

        if(e.MyLevel >= Player.Instance.MyLevel)
        {
            totalXP = (int)((baseXP) * (1 + 0.05 * (e.MyLevel - Player.Instance.MyLevel)));
        }else if(e.MyLevel > grayLevel)
        {
            totalXP = (baseXP) * (1 - (Player.Instance.MyLevel - e.MyLevel) / ZeroDifference());
        }

        return totalXP;
    }

    private static int ZeroDifference()
    {
        if(Player.Instance.MyLevel <= 7)
        {
            return 5;
        }
        if(Player.Instance.MyLevel >=8 && Player.Instance.MyLevel <= 9)
        {
            return 6;
        }
        if(Player.Instance.MyLevel >= 10 && Player.Instance.MyLevel <= 11)
        {
            return 7;
        }
        if(Player.Instance.MyLevel >= 12 && Player.Instance.MyLevel <= 15)
        {
            return 8;
        }
        if(Player.Instance.MyLevel >= 16 && Player.Instance.MyLevel <= 19)
        {
            return 9;
        }
        if (Player.Instance.MyLevel >= 20 && Player.Instance.MyLevel <= 29)
        {
            return 11;
        }
        if (Player.Instance.MyLevel >= 30 && Player.Instance.MyLevel <= 39)
        {
            return 12;
        }
        if (Player.Instance.MyLevel >= 40 && Player.Instance.MyLevel <= 44)
        {
            return 13;
        }
        if (Player.Instance.MyLevel >= 45 && Player.Instance.MyLevel <= 49)
        {
            return 14;
        }
        if (Player.Instance.MyLevel >= 50 && Player.Instance.MyLevel <= 54)
        {
            return 15;
        }
        if (Player.Instance.MyLevel >= 55 && Player.Instance.MyLevel <= 59)
        {
            return 16;
        }

        return 17;

    }

    public static int CalculateGrayLevel()
    {
        if(Player.Instance.MyLevel <= 5)
        {
            return 0;
        }else if(Player.Instance.MyLevel >=6 && Player.Instance.MyLevel <= 49)
        {
            return Player.Instance.MyLevel - (Player.Instance.MyLevel / 10) - 5;
        }else if(Player.Instance.MyLevel == 50)
        {
            return Player.Instance.MyLevel - 10;
        }else if(Player.Instance.MyLevel >=51 && Player.Instance.MyLevel <= 59)
        {
            return Player.Instance.MyLevel - (Player.Instance.MyLevel / 5) - 1;
        }

        return Player.Instance.MyLevel - 9;
    }

    public static int CalculateXP(Quest e)
    {
        if(Player.Instance.MyLevel <= e.MyLevel + 5)
        {
            return e.MyXp;
        }
        if(Player.Instance.MyLevel == e.MyLevel + 6)
        {
            return (int)(e.MyXp * 0.8 / 5) * 5;
        }
        if (Player.Instance.MyLevel == e.MyLevel + 7)
        {
            return (int)(e.MyXp * 0.6 / 5) * 5;
        }
        if (Player.Instance.MyLevel == e.MyLevel + 8)
        {
            return (int)(e.MyXp * 0.4 / 5) * 5;
        }
        if (Player.Instance.MyLevel == e.MyLevel + 9)
        {
            return (int)(e.MyXp * 0.2 / 5) * 5;
        }
        if (Player.Instance.MyLevel >= e.MyLevel + 10)
        {
            return (int)(e.MyXp * 0.1 / 5) * 5;
        }

        return 0;
    }
}