using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using SEABOURNE.SEABOURNECode.Cards;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Relics;

namespace SEABOURNE.SEABOURNECode.Character;

public sealed class TheSeabourne : CustomCharacterModel
{
    public const string CharacterId = "THE_SEABOURNE";
    public static readonly Color Color = new("1b7f9c");

    public override Color NameColor => Color;
    public override Color MapDrawingColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeCard>(),
        ModelDb.Card<StrikeCard>(),
        ModelDb.Card<StrikeCard>(),
        ModelDb.Card<StrikeCard>(),
        ModelDb.Card<DefendCard>(),
        ModelDb.Card<DefendCard>(),
        ModelDb.Card<DefendCard>(),
        ModelDb.Card<DefendCard>(),
        ModelDb.Card<FireCannonCard>(),
        ModelDb.Card<FishCard>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<SeabourneStarterRelic>(),
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<SEABOURNECardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<SEABOURNERelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<SEABOURNEPotionPool>();

    public override List<string> GetArchitectAttackVfx() =>
    [
        "vfx/vfx_attack_slash",
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_bloody_impact",
    ];

    public override string CustomVisualPath => "res://SEABOURNE/Characters/SEABOURNE/seaborne_character.tscn";
    public override string CustomTrailPath => "res://scenes/vfx/card_trail_ironclad.tscn";
    public override string CustomIconPath => "res://SEABOURNE/Characters/SEABOURNE/seaborne_character_icon.tscn";
    public override string CustomIconTexturePath => "seaborne_placeholder.png".ImagePath();
    public override string CustomRestSiteAnimPath => "res://SEABOURNE/Characters/SEABOURNE/seaborne_character_rest_site.tscn";
    public override string CustomMerchantAnimPath => "res://SEABOURNE/Characters/SEABOURNE/seaborne_character_merchant.tscn";
    public override string CustomCharacterSelectBg => "res://SEABOURNE/Characters/SEABOURNE/char_select_bg_seaborne_character.tscn";
    public override string CustomCharacterSelectIconPath => "seaborne_placeholder.png".ImagePath();
    public override string CustomCharacterSelectLockedIconPath => "seaborne_placeholder.png".ImagePath();
    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
    public override string CustomMapMarkerPath => "seaborne_placeholder.png".ImagePath();
    public override string? CustomEnergyCounterPath => "res://scenes/combat/energy_counters/ironclad_energy_counter.tscn";
}
