using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class DeepDiamondCard : SeabourneCard
{
    public DeepDiamondCard() : base(2, CardType.Skill, CardRarity.Common, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardTag> Tags => CardTags("Exhaust");

    private int _castAmount = 3;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        Acquire(play, SeabourneGemType.Diamond);
        await Cast(choiceContext, play, _castAmount);
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(1);
    }
}
