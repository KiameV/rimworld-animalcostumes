using RimWorld;
using Verse;

namespace AnimalCostumes
{
    public class CompMilkable_AC : CompMilkable
    {
        public bool WasJustMilked = false;

        protected override bool Active
        {
            get
            {
                if (parent is Pawn pawn)
                {
                    if (pawn == null || pawn.Dead)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        protected override int GatherResourcesIntervalDays
        {
            get
            {
                return this.Props.intervalDays;
            }
        }
        protected override int ResourceAmount
        {
            get
            {
                return this.Props.amount;
            }
        }
        protected override ThingDef ResourceDef
        {
            get
            {
                return this.Props.thingDef;
            }
        }

        public new CompProperties_Milkable_AC Props
        {
            get
            {
                return (CompProperties_Milkable_AC)base.props;
            }
        }

        public override string CompInspectStringExtra()
        {
            if (!Active)
            {
                return null;
            }
            return "MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.WasJustMilked, "wasJustMilked", false);
        }

        public override void CompTick()
        {
            base.CompTick();
            Pawn pawn = parent as Pawn;
            if (fullness > .99f)
            {
                if (pawn.Map != null)
                {
                    fullness = 0;
                    Thing thing = ThingMaker.MakeThing(this.Props.thingDef);
                    thing.stackCount = this.Props.amount;
                    if (this.HasCowHood(pawn))
                    {
                        thing.stackCount = (int)(thing.stackCount * 1.5f);
                    }
                    if (thing.stackCount > 0)
                    {
                        GenPlace.TryPlaceThing(thing, pawn.PositionHeld, pawn.Map, ThingPlaceMode.Near);
                        WasJustMilked = true;
                    }
                }
            }
        }

        public bool HasCowHood(Pawn pawn)
        {
            foreach (Apparel a in pawn.apparel.WornApparel)
            {
                var def = a.def as AnimalCostumeDef;
                if (def != null && def.CostumeType == CostumeType.Head && def.CostumeBreed == "Cow")
                    return true;
            }
            return false;
        }

        public CompMilkable_AC() { }
    }

    public class CompProperties_Milkable_AC : CompProperties
    {
        public int intervalDays;

        public int amount;

        public ThingDef thingDef;

        public CompProperties_Milkable_AC()
        {
            compClass = typeof(CompMilkable_AC);
        }
    }
}