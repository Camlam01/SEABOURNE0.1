using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class StrikeCard : SeabourneCard
{
    public StrikeCard() : base(1, CardType.Attack, CardRarity.Basic, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(6m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Attack(choiceContext, play, PrimaryDamage);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(3m);
    }
}
