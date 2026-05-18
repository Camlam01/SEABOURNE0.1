using BaseLib.Abstracts;
using Godot;

namespace SEABOURNE.SEABOURNECode.Character;

public sealed class SEABOURNERelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => TheSeabourne.CharacterId;
    public override Color LabOutlineColor => TheSeabourne.Color;
}
