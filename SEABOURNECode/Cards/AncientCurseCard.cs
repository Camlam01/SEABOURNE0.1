using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class AncientCurseCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public AncientCurseCard() : base(1, CardType.Skill, CardRarity.Uncommon, AllEnemiesTarget)
    {
        AddImbued(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplyAll<VulnerablePower>(choiceContext, play, 1);
        await ApplyAll<WeakPower>(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {
        AddImbued(1);
    }
}
