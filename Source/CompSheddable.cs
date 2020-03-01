using System;
using RimWorld;
using Verse;

namespace AnimalCostumes
{
    public class CompShearable_AC : CompAnimalCostumeThingGenerator
    {
        public bool WasJustShedded = false;

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

        public new CompProperties_Shearable_AC Props
        {
            get
            {
                return (CompProperties_Shearable_AC)base.props;
            }
        }

        public override string CompInspectStringExtra()
        {
            if (!Active)
            {
                return null;
            }
            return "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.WasJustShedded, "wasJustShedded", false);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (fullness > 0.99f)
            {
                var pawn = base.AnimalCostume?.Wearer;
                if (pawn?.Map != null)
                {
                    fullness = 0;
                    Thing thing = ThingMaker.MakeThing(this.Props.thingDef);
                    thing.stackCount = this.Props.amount;
                    if (this.HeadAndBodySameBreed(pawn))
                    {
                        thing.stackCount = (int)(thing.stackCount * 1.5f);
                    }
                    if (thing.stackCount > 0)
                    {
                        GenPlace.TryPlaceThing(thing, pawn.PositionHeld, pawn.Map, ThingPlaceMode.Near);
                        WasJustShedded = true;
                    }
                }
            }
        }

        private bool HeadAndBodySameBreed(Pawn pawn)
        {
            CostumeType found = CostumeType.None;
            string breed = null;
            foreach (Apparel a in pawn.apparel.WornApparel)
            {
                if (a.def is AnimalCostumeDef def)
                {
                    if (breed == null)
                        breed = def.CostumeBreed;
                    else if (breed != def.CostumeBreed)
                        return false;

                    found |= def.CostumeType;
                }
            }
            return (found & CostumeType.Head) != 0 && (found & CostumeType.Body) != 0;
        }

        public CompShearable_AC() { }
    }

    public class CompProperties_Shearable_AC : CompProperties
    {
        public int intervalDays;

        public int amount;

        public ThingDef thingDef;

        public bool milkFemaleOnly = false;

        public CompProperties_Shearable_AC()
        {
            compClass = typeof(CompShearable_AC);
        }
    }
}