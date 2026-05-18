using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class EntangleCard : SeabourneCard
{
    public EntangleCard() : base(1, CardType.Skill, CardRarity.Common, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _weakAmount = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Cast(choiceContext, play, 2);
        await ApplyAll<WeakPower>(choiceContext, play, _weakAmount);
    }

    protected override void OnUpgrade()
    {
        _weakAmount = 2;
    }
}
