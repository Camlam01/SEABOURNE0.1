using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class RodRamCard : SeabourneCard
{
    public RodRamCard() : base(0, CardType.Attack, CardRarity.Common, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Cast(choiceContext, play, 2);
        await Attack(choiceContext, play, PrimaryDamage);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(2m);
    }
}
