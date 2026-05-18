using BaseLib.Abstracts;
using BaseLib.Utils;
using SEABOURNE.SEABOURNECode.Character;

namespace SEABOURNE.SEABOURNECode.Potions
{
    [Pool(typeof(SEABOURNEPotionPool))]
    public abstract class SEABOURNEPotion : CustomPotionModel;
}