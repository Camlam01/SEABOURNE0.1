using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class EmeraldEbbCard : SeabourneCard
{
    public EmeraldEbbCard() : base(2, CardType.Skill, CardRarity.Common, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardTag> Tags => CardTags("Exhaust");

    private int _waterwall = 7;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        Acquire(play, SeabourneGemType.Emerald);
        await ApplySelf<WaterwallPower>(choiceContext, play, _waterwall);
    }

    protected override void OnUpgrade()
    {
        _waterwall = 9;
    }
}
