using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class RubyRuseCard : SeabourneCard
{
    public RubyRuseCard() : base(1, CardType.Skill, CardRarity.Common, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ BlockVar(8m) ];
    public override IEnumerable<CardTag> Tags => CardTags("Exhaust");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        Acquire(play, SeabourneGemType.Ruby);
        await Block(choiceContext, play, PrimaryBlock);
    }

    protected override void OnUpgrade()
    {
        UpgradeBlock(2m);
    }
}
