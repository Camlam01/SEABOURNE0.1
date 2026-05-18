using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class BejeweledCard : SeabourneCard
{
    public BejeweledCard() : base(1, CardType.Skill, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        foreach (var gem in Enum.GetValues<SeabourneGemType>()) Acquire(play, gem);
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(0);
    }
}
