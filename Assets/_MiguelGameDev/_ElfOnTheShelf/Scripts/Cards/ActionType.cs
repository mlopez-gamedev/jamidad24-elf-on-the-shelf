using I2.Loc;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "ActionType", menuName = "ElfOnTheShelf/Action Type")]
    public class ActionType : ScriptableObject
    {
        [SerializeField] private EActionType _id;
        [SerializeField, TermsPopup("Action/")] private string _nameTerm;
        [SerializeField] private Sprite _icon;

        public EActionType Id => _id;
        public string NameTerm => _nameTerm;
        public Sprite Icon => _icon;
    }
}