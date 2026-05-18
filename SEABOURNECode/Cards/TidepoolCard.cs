using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class TidepoolCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m) ];
    public TidepoolCard() : base(3, CardType.Attack, CardRarity.Rare, AllEnemiesTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await AttackAll(choiceContext, play, PrimaryDamage);
        await ApplySelf<WaterwallPower>(choiceContext, play, 5);
        await ApplyAll<WeakPower>(choiceContext, play, 1);
        await ApplyAll<VulnerablePower>(choiceContext, play, 1);
        await AddCardCopyToDiscard<TidepoolCard>(choiceContext, play);
    }

    protected override void OnUpgrade()
    {

    }
}
