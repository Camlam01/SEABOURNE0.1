using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class TrawlCard : SeabourneCard
{
    public TrawlCard() : base(0, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var x = Math.Max(1, Math.Max(play.Resources.EnergySpent, play.Resources.StarsSpent));
        await Cast(choiceContext, play, 2 * x);
        await Reel(choiceContext, play);
    }

    protected override void OnUpgrade()
    {

    }
}
