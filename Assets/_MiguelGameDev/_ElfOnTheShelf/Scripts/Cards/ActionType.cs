using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "ActionType", menuName = "ElfOnTheShelf/Action Type")]
    public class ActionType : ScriptableObject
    {
        [SerializeField] private EActionType _id;
        [SerializeField] private Sprite _icon;

        public EActionType Id => _id;
        public Sprite Icon => _icon;
    }
}