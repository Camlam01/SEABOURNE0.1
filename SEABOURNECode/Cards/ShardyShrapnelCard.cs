using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class ShardyShrapnelCard : SeabourneCard
{
    public ShardyShrapnelCard() : base(1, CardType.Power, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _amount = 6;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplySelf<ShardyShrapnelPower>(choiceContext, play, _amount);
    }

    protected override void OnUpgrade()
    {
        _amount = 8;
    }
}
