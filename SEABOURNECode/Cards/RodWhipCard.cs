using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class RodWhipCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m) ];
    public RodWhipCard() : base(0, CardType.Attack, CardRarity.Uncommon, AnyEnemyTarget)
    {
        AddImbued(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Attack(choiceContext, play, PrimaryDamage);
        await Cast(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(2m);
    }
}
