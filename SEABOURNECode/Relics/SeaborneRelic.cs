using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SEABOURNE.SEABOURNECode.Character;
using SEABOURNE.SEABOURNECode.Extensions;

namespace SEABOURNE.SEABOURNECode.Relics;

[Pool(typeof(SEABOURNERelicPool))]
public abstract class SeabourneRelic : CustomRelicModel
{
    protected string BaseFileName => $"{GetType().Name.Replace("Relic", string.Empty).ToLowerInvariant()}.png";

    public override string PackedIconPath => BaseFileName.RelicImagePath();
    protected override string PackedIconOutlinePath => BaseFileName.RelicImagePath();
    protected override string BigIconPath => BaseFileName.BigRelicImagePath();

    public virtual Task OnCast(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, int amount) => Task.CompletedTask;
    public virtual Task OnReel(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, int reeledCardCount) => Task.CompletedTask;
    public virtual void ModifyCannonDamage(ref decimal multiplier) { }
}