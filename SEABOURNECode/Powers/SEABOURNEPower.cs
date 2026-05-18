using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using SEABOURNE.SEABOURNECode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SEABOURNE.SEABOURNECode.Powers;

public abstract class SEABOURNEPower : CustomPowerModel
{
    public override string? CustomPackedIconPath
    {
        get
        {
            var path = $"{GetType().Name.Replace("Power", string.Empty).ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string? CustomBigIconPath
    {
        get
        {
            var path = $"{GetType().Name.Replace("Power", string.Empty).ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }

    public override string? CustomBigBetaIconPath => CustomBigIconPath;
}

public sealed class TemporaryStrengthPower : SEABOURNEPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, MegaCrit.Sts2.Core.Combat.CombatSide side)
    {
        if (Owner?.Side != side)
            return;

        await PowerCmd.Remove(this);
    }
}

public sealed class VulnerablePower : SEABOURNEPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}

public sealed class WeakPower : SEABOURNEPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}

public sealed class StrengthPower : SEABOURNEPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}