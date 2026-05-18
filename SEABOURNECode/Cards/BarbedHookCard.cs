using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class BarbedHookCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public BarbedHookCard() : base(1, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplySelf<BarbedHookPower>(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {

    }
}
