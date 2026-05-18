using BaseLib.Abstracts;
using Godot;
using SEABOURNE.SEABOURNECode.Extensions;

namespace SEABOURNE.SEABOURNECode.Character;

public sealed class SEABOURNECardPool : CustomCardPoolModel
{
    public override string Title => TheSeabourne.CharacterId;
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    public override float H => 0.58f;
    public override float S => 0.75f;
    public override float V => 0.95f;
    public override Color DeckEntryCardColor => new("4FB8FF");
    public override Color EnergyOutlineColor => new("1b7f9c");
    public override bool IsColorless => false;
}
