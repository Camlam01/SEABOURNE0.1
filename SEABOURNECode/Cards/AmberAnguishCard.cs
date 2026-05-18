using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class AmberAnguishCard : SeabourneCard
{
    public AmberAnguishCard() : base(2, CardType.Attack, CardRarity.Common, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(11m) ];
    public override IEnumerable<CardTag> Tags => CardTags("Exhaust");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        Acquire(play, SeabourneGemType.Amber);
        await Attack(choiceContext, play, PrimaryDamage);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(3m);
    }
}
