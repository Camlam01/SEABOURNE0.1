using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class PowerOfTheSeaCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public PowerOfTheSeaCard() : base(2, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await GainEnergy(play, 2);
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(1);
    }
}
