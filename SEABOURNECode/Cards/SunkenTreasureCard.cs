using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class SunkenTreasureCard : SeabourneCard
{
    public SunkenTreasureCard() : base(1, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => KeywordList(CardKeyword.Innate);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        if (!Acquire(play, SeabourneGemType.Emerald)) Acquire(play, SeabourneGemType.Amber);
        await Cast(choiceContext, play, 2);
    }

    protected override void OnUpgrade()
    {

    }
}
