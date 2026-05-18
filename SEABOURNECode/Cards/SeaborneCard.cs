using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SEABOURNE.SEABOURNECode.Character;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

[Pool(typeof(SEABOURNECardPool))]
public abstract class SeabourneCard(int cost, CardType type, CardRarity rarity, TargetType target) : CustomCardModel(cost, type, rarity, target)
{
    protected static TargetType AnyEnemyTarget => ParseTarget("AnyEnemy", "Enemy");
    protected static TargetType AllEnemiesTarget => ParseTarget("AllEnemies", "AllEnemy");
    protected static TargetType SelfTarget => ParseTarget("Self");
    protected static TargetType NoneTarget => ParseTarget("None", "Self");

    protected static ValueProp MoveProp => ParseValueProp("Move", "Damage", "Block");
    protected static IReadOnlyList<CardTag> CardTags(params string[] names) =>
        names.Select(TryParseTag).Where(t => t.HasValue).Select(t => t!.Value).ToList();
    protected static IReadOnlyList<CardKeyword> KeywordList(params CardKeyword[] keywords) => keywords;

    public override string PortraitPath => "card.png".CardImagePath();
    public override string BetaPortraitPath => PortraitPath;

    public virtual decimal PrimaryDamage => GetVarValue("Damage");
    public virtual decimal PrimaryBlock => GetVarValue("Block");
    public decimal Damage => PrimaryDamage;
    public decimal BlockValue => PrimaryBlock;
    public virtual int EnergyGain => 0;

    protected static DamageVar DamageVar(decimal value) => new(value, MoveProp);
    protected static BlockVar BlockVar(decimal value) => new(value, MoveProp);

    protected decimal GetVarValue(string id)
    {
        if (!DynamicVars.TryGetValue(id, out var dynamicVar))
            return 0m;

        return SeabourneReflection.GetDecimal(dynamicVar, "CurrentValue", "Value", "Amount", "CanonicalValue");
    }

    protected void UpgradeDamage(decimal amount) => DynamicVars["Damage"].UpgradeValueBy(amount);
    protected void UpgradeBlock(decimal amount) => DynamicVars["Block"].UpgradeValueBy(amount);
    protected void UpgradeCost(int newCost) => SeabourneCardRuntime.SetCost(this, newCost);

    protected Task Attack(PlayerChoiceContext choiceContext, CardPlay play, decimal amount, int hitCount = 1) =>
        SeabourneCardRuntime.Attack(choiceContext, this, play, amount, hitCount);

    protected Task AttackAll(PlayerChoiceContext choiceContext, CardPlay play, decimal amount, int hitCount = 1) =>
        SeabourneCardRuntime.AttackAll(choiceContext, this, play, amount, hitCount);

    protected Task Block(PlayerChoiceContext choiceContext, CardPlay play, decimal amount) =>
        SeabourneCardRuntime.GainBlock(choiceContext, this, play, amount);

    protected Task ApplySelf<TPower>(PlayerChoiceContext choiceContext, CardPlay play, int amount)
        where TPower : SEABOURNEPower, new() =>
        SeabourneCardRuntime.ApplySelf<TPower>(choiceContext, this, play, amount);

    protected Task ApplyTarget<TPower>(PlayerChoiceContext choiceContext, CardPlay play, int amount)
        where TPower : SEABOURNEPower, new() =>
        SeabourneCardRuntime.ApplyTarget<TPower>(choiceContext, this, play, amount);

    protected Task ApplyAll<TPower>(PlayerChoiceContext choiceContext, CardPlay play, int amount)
        where TPower : SEABOURNEPower, new() =>
        SeabourneCardRuntime.ApplyAll<TPower>(choiceContext, this, play, amount);

    protected Task Cast(PlayerChoiceContext choiceContext, CardPlay play, int amount) =>
        SeabourneCardRuntime.Cast(choiceContext, this, play, amount);

    protected Task Reel(PlayerChoiceContext choiceContext, CardPlay play) =>
        SeabourneCardRuntime.Reel(choiceContext, this, play);

    protected bool Acquire(CardPlay play, SeabourneGemType gem) =>
        SeabourneCardRuntime.AcquireGem(play, gem);

    protected Task FireCannon(PlayerChoiceContext choiceContext, CardPlay play) =>
        SeabourneCardRuntime.FireCannon(choiceContext, this, play);

    protected Task LoadCannon(PlayerChoiceContext choiceContext, CardPlay play) =>
        SeabourneCardRuntime.LoadCannon(choiceContext, this, play, this);

    protected Task AddCardCopiesToHand<TCard>(PlayerChoiceContext choiceContext, CardPlay play, int count)
        where TCard : CardModel, new() =>
        SeabourneCardRuntime.AddCardCopiesToHand<TCard>(choiceContext, this, play, count);

    protected Task AddCardCopyToDiscard<TCard>(PlayerChoiceContext choiceContext, CardPlay play, int count = 1)
        where TCard : CardModel, new() =>
        SeabourneCardRuntime.AddCardCopyToDiscard<TCard>(choiceContext, this, play, count);

    protected Task GainEnergy(CardPlay play, int amount) => SeabourneCardRuntime.GainEnergy(this, play, amount);
    protected void AddWet(int amount) => SeabourneCardRuntime.AddWet(this, amount);
    protected void AddImbued(int amount) => SeabourneCardRuntime.AddImbued(this, amount);
    protected void SetUnimbued() => SeabourneCardRuntime.SetUnimbued(this);

    public virtual Task OnReeled(PlayerChoiceContext choiceContext, CardPlay play, SeabourneCard source)
    {
        return OnPlay(choiceContext, play);
    }

    private static TargetType ParseTarget(params string[] names)
    {
        foreach (var name in names)
        {
            try
            {
                return (TargetType)Enum.Parse(typeof(TargetType), name);
            }
            catch
            {
            }
        }

        return default;
    }

    private static ValueProp ParseValueProp(params string[] names)
    {
        foreach (var name in names)
        {
            try
            {
                return (ValueProp)Enum.Parse(typeof(ValueProp), name);
            }
            catch
            {
            }
        }

        return default;
    }

    private static CardTag? TryParseTag(string name)
    {
        try
        {
            return (CardTag)Enum.Parse(typeof(CardTag), name);
        }
        catch
        {
            return null;
        }
    }
}
