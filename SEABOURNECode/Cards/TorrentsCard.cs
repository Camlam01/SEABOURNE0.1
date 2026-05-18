using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class TorrentsCard : SeabourneCard
{
    public TorrentsCard() : base(1, CardType.Power, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _waterwall = 6;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplySelf<WaterwallPower>(choiceContext, play, _waterwall);
        await ApplySelf<TorrentsPower>(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {
        _waterwall = 8;
    }
}
