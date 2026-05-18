using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class MucusMaskCard : SeabourneCard
{
    public MucusMaskCard() : base(1, CardType.Skill, CardRarity.Common, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _slippery = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplySelf<SlipperyPower>(choiceContext, play, _slippery);
    }

    protected override void OnUpgrade()
    {
        _slippery = 2;
    }
}
