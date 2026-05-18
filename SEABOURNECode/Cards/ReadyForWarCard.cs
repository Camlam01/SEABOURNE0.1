using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class ReadyForWarCard : SeabourneCard
{
    public ReadyForWarCard() : base(0, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _count = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Cast(choiceContext, play, 3);
        await AddCardCopiesToHand<SpinyCannonballCard>(choiceContext, play, _count);
    }

    protected override void OnUpgrade()
    {
        _count = 2;
    }
}
