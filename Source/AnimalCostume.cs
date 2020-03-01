using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace AnimalCostumes
{
    public class AnimalCostume : Apparel
    {
        private bool initialized = false;
        private bool isCostumeRequiresMoreFood = false;
        public CompAnimalCostumeThingGenerator CompThingGenerator = null;

        public AnimalCostume() { }

        public void AddToInspectionString(ref string inspection)
        {
            if (this.CompThingGenerator != null && base.Wearer != null && !base.Wearer.Dead)
            {
                StringBuilder sb = new StringBuilder(inspection);
                string text = CompThingGenerator.CompInspectStringExtra();
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

        public void Notify_ApparelAdded()
        {
            this.Initialize();
            if (this.CompThingGenerator != null)
                CompThingGenerator.parent = base.Wearer;
        }

        public void Notify_ApparelRemoved()
        {
            this.Initialize();
            if (this.CompThingGenerator != null)
                CompThingGenerator.parent = null;
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
            this.Initialize();
            if (base.Wearer != null && !base.Wearer.Dead && base.Wearer.IsHashIntervalTick(400))
            {
                this.CompThingGenerator?.CompTick();

                if (this.isCostumeRequiresMoreFood)
                {
                    base.Wearer.needs.food.NeedInterval();
                }
            }
        }

        private void Initialize()
        {
            if (!this.initialized)
            {
                this.initialized = true;
                var comps = base.AllComps;
                if (comps != null)
                {
                    foreach (ThingComp c in comps)
                    {
                        if (c is CompAnimalCostumeThingGenerator acg)
                        {
                            CompThingGenerator = acg;
                            acg.AnimalCostume = this;
                            break;
                        }
                    }
                }

                var def = base.def as AnimalCostumeDef;
                if (def != null)
                    this.isCostumeRequiresMoreFood = def.CostumeRequiresMoreFood;
            }
        }
    }
}
