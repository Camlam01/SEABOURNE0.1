using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class CannonballCard : SeabourneCard
{
    public CannonballCard() : base(1, CardType.Attack, CardRarity.Common, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(15m) ];
    public override IEnumerable<CardTag> Tags => CardTags("Load");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await LoadCannon(choiceContext, play);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(5m);
    }
}
