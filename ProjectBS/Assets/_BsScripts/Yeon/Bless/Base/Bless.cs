using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon2
{
    public class Bless : MonoBehaviour
    {
        public BlessData Data => _data;
        [SerializeField] private BlessData _data;

        [SerializeField] protected PlayerComponent myPlayer;

        //레벨업시 변경될 스테이터스를 저장하는 딕셔너리
        protected Dictionary<string, float> myStatus = new Dictionary<string, float>();
        private void OnEnable()
        {
            Init(Data);
        }

        public void Init(BlessData data)
        {
            _data = data;

            foreach (var lvData in data.LvDataList)
            {
                myStatus.Add(lvData.name, lvData.defaultValue);
            }
        }

        public void LevelUp(int level)
        {
            if (level <= 0 || level > 7)
                return;

            foreach (var lvData in _data.LvDataList)
            {
                myStatus[lvData.name] = lvData[level];
            }
        }

        protected void SetFowardPlayerLook()
        {
            transform.SetParent(GameManager.Instance.Player.RotatingBody);
        }
    }
}
