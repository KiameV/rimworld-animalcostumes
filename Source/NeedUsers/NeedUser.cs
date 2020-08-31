using RimWorld;
using System;
using Verse;

namespace NeedUser
{
    public abstract class NeedUser : Def
    {
        public float cost = 0.05f;
        public float minimumLevel = 0.14f;

        public bool CanUse(Pawn pawn)
        {
            return this.GetNeed(pawn)?.CurInstantLevelPercentage - this.minimumLevel > this.cost;
        }

        public int GetRemainingCount(Pawn pawn)
        {
            return (int)((this.GetNeed(pawn)?.CurInstantLevelPercentage - this.minimumLevel) / this.cost);
        }

        public int GetMaxCount(Pawn pawn)
        {
            return (int)(this.GetNeed(pawn)?.MaxLevel / this.cost);
        }

        public void UseOnce(Pawn pawn)
        {
            Need n = this.GetNeed(pawn);
            if (n != null)
                n.CurLevelPercentage = Math.Max(0f, n.CurLevelPercentage - this.cost);
        }

        protected abstract Need GetNeed(Pawn pawn);
    }

    public class NeedUser_Mood : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.mood;
        }
    }

    public class NeedUser_Food : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.food;
        }
    }

    public class NeedUser_Rest : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.rest;
        }
    }

    public class NeedUser_Joy : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.joy;
        }
    }

    public class NeedUser_Beauty : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.beauty;
        }
    }

    public class NeedUser_RoomSize : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.roomsize;
        }
    }

    public class NeedUser_Outdoors : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.outdoors;
        }
    }

    public class NeedUser_DrugsDesire : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.drugsDesire;
        }
    }

    public class NeedUser_Comfort : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.comfort;
        }
    }

    public class NeedUser_Authority : NeedUser
    {
        protected override Need GetNeed(Pawn pawn)
        {
            return pawn.needs.authority;
        }
    }
}
