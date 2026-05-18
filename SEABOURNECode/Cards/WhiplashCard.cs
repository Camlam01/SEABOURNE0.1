using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class WhiplashCard : SeabourneCard
{
    public WhiplashCard() : base(1, CardType.Attack, CardRarity.Common, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(8m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Reel(choiceContext, play);
        await Attack(choiceContext, play, PrimaryDamage);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(3m);
    }
}
