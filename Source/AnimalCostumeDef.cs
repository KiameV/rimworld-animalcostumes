using System;
using Verse;

namespace AnimalCostumes
{
    [Flags]
    public enum CostumeType
    {
        None = 0,
        Ears = 1,
        Tail = 2,
        Head = 4,
        Body = 8,
    }

    class AnimalCostumeDef : ThingDef
    {
        public string CostumeBreed = "";
        public CostumeType CostumeType = CostumeType.None;
        public bool CostumeRequiresMoreFood = false;

        public AnimalCostumeDef() { }
    }
}