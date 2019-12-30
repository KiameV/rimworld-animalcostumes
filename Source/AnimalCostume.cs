using RimWorld;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace AnimalCostumes
{
    public class AnimalCostume : Apparel
    {
        private bool initialized = false;
        private bool isThingGenerator = false;
        public ThingComp CompGatherable = null;

        public AnimalCostume() { }

        public void AddToInspectionString(ref string inspection)
        {
            if (this.CompGatherable != null && base.Wearer != null && !base.Wearer.Dead)
            {
                StringBuilder sb = new StringBuilder(inspection);
                string text = CompGatherable.CompInspectStringExtra();
                if (text != null && text != "")
                {
                    if (sb.Length != 0)
                    {
                        sb.AppendLine();
                    }
                    sb.Append(text);
                }
                inspection = sb.ToString();
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            WorldComp.Remove(this);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            WorldComp.Add(this);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                WorldComp.Add(this);
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (!this.initialized)
            {
                this.initialized = true;
                var comps = base.AllComps;
                if (comps != null)
                {
                    foreach (ThingComp c in comps)
                    {
                        if (c is CompHasGatherableBodyResource)
                        {
                            CompGatherable = c;
                            CompGatherable.parent = base.Wearer;
                            break;
                        }
                    }
                }

                this.isThingGenerator = (base.def as AnimalCostumeDef).CostumeRequiresMoreFood;
            }

            if (base.Wearer != null && !base.Wearer.Dead && base.Wearer.IsHashIntervalTick(400))
            {
                this.CompGatherable?.CompTick();

                if (this.isThingGenerator)
                {
                    base.Wearer.needs.food.NeedInterval();
                }
            }
        }
    }
}
