using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class TideTearCard : SeabourneCard
{
    public TideTearCard() : base(1, CardType.Attack, CardRarity.Uncommon, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(6m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await AttackAll(choiceContext, play, PrimaryDamage);
        var owner = SeabourneReflection.GetOwner(play);
        var draw = owner is null ? null : SeabourneReflection.GetDraw(owner);
        var discard = owner is null ? null : SeabourneReflection.GetDiscard(owner);
        if (draw is not null && discard is not null) { var count = Math.Min(draw.Count, 3); for (var i = 0; i < count; i++) { var idx = draw.Count - 1; var c = draw[idx]; draw.RemoveAt(idx); discard.Add(c); } }
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(2m);
    }
}
