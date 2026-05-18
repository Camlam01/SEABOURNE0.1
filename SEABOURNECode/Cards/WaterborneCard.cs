using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class WaterborneCard : SeabourneCard
{
    public WaterborneCard() : base(2, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _waterwall = 8;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        AddWet(1);
        await ApplySelf<WaterwallPower>(choiceContext, play, _waterwall);
    }

    protected override void OnUpgrade()
    {
        _waterwall = 12;
    }
}
