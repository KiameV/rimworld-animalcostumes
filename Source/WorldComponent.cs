using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace AnimalCostumes
{
    public class WorldComp : WorldComponent
    {
        private static Dictionary<int, AnimalCostume> WornAnimalCostumes = new Dictionary<int, AnimalCostume>();

        static WorldComp() { }

        public WorldComp(World world) : base(world)
        {
            if (WornAnimalCostumes != null)
                WornAnimalCostumes.Clear();
            else
                WornAnimalCostumes = new Dictionary<int, AnimalCostume>();
        }

        public static void Add(AnimalCostume ac)
        {
            if (ac != null)
            {
                WornAnimalCostumes[ac.thingIDNumber] = ac;
            }
        }

        public static void Remove(AnimalCostume ac)
        {
            if (ac != null)
            {
                WornAnimalCostumes.Remove(ac.thingIDNumber);
            }
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
            foreach (AnimalCostume ac in WornAnimalCostumes.Values)
            {
                ac.Tick();
            }
        }

        private List<AnimalCostume> tempWorn = null;
        public override void ExposeData()
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                this.tempWorn = new List<AnimalCostume>(WornAnimalCostumes.Count);
                foreach (AnimalCostume ac in WornAnimalCostumes.Values)
                {
                    if (ac != null)
                        this.tempWorn.Add(ac);
                }
            }

            Scribe_Collections.Look(ref this.tempWorn, "worn", LookMode.Reference, new object[0]);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                WornAnimalCostumes.Clear();
                if (this.tempWorn != null)
                {
                    foreach (AnimalCostume ac in this.tempWorn)
                    {
                        Add(ac);
                    }
                }
            }
        }
    }
}
