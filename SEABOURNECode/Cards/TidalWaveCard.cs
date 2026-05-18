using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class TidalWaveCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(2m) ];
    public TidalWaveCard() : base(1, CardType.Attack, CardRarity.Common, AllEnemiesTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        var hand = owner is null ? 0 : SeabourneReflection.GetHand(owner)?.Count ?? 0;
        await AttackAll(choiceContext, play, PrimaryDamage * hand);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(1m);
    }
}
