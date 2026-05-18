using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class LineWhipCard : SeabourneCard
{
    public LineWhipCard() : base(2, CardType.Attack, CardRarity.Common, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m) ];
    private int _vulnerable = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Reel(choiceContext, play);
        await AttackAll(choiceContext, play, PrimaryDamage);
        await ApplyAll<VulnerablePower>(choiceContext, play, _vulnerable);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(2m);
        _vulnerable = 2;
    }
}
