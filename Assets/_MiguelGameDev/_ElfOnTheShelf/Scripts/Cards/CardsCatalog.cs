using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "CardsCatalog", menuName = "ElfOnTheShelf/Cards Catalog")]
    public class CardsCatalog : ScriptableObject
    {
        [SerializeField] ActionCard[] _actionCards;
        [SerializeField] GoalCard[] _goalCards;
        [SerializeField] BustCard[] _bustCards;
        
        public ActionCard[] ActionCards => _actionCards;
        public BustCard[] BustCards => _bustCards;
        public GoalCard[] GoalCards => _goalCards;
    }
}