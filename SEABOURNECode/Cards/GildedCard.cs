using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class GildedCard : SeabourneCard
{
    public GildedCard() : base(1, CardType.Power, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _amount = 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplySelf<GildedPower>(choiceContext, play, _amount);
    }

    protected override void OnUpgrade()
    {
        _amount = 3;
    }
}
