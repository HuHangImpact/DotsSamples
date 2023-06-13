using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniFramework.Window;
using UniFramework.Event;
using Unity.Entities;

[WindowAttribute(100, false)]
public class UIBattleWindow : UIWindow
{
    private EventGroup _eventGroup = new EventGroup();
    private Text _scoreLabel;
    private GameObject _overView;

    private EntityManager _entityManager;

    private Entity _playerEntity;

    public override void OnCreate()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        _scoreLabel = this.transform.Find("ScoreView/Score").GetComponent<Text>();
        _scoreLabel.text = "Score : 0";

        var restartBtn = this.transform.Find("OverView/Restart").GetComponent<Button>();
        restartBtn.onClick.AddListener(OnClickRestartBtn);

        var homeBtn = this.transform.Find("OverView/Home").GetComponent<Button>();
        homeBtn.onClick.AddListener(OnClickHomeBtn);

        _overView = this.transform.Find("OverView").gameObject;

        _eventGroup.AddListener<BattleEventDefine.ScoreChange>(OnHandleEventMessage);
        _eventGroup.AddListener<BattleEventDefine.GameOver>(OnHandleEventMessage);
    }

    public override void OnDestroy()
    {
        _eventGroup.RemoveAllListener();
    }

    public override void OnRefresh()
    {
        _overView.SetActive(false);
    }

    public override void OnUpdate()
    {
        if (_playerEntity == Entity.Null)
        {
            if (_entityManager.CreateEntityQuery(typeof(PlayerTag)).IsEmpty)
            {
                return;
            }
            _playerEntity = _entityManager.CreateEntityQuery(typeof(PlayerTag)).GetSingletonEntity();
            Debug.Log(_playerEntity.Index);
        }
        else
        {
            var curPlayerExperience = _entityManager.GetComponentData<PlayerSource>(_playerEntity).Source;
            _scoreLabel.text = $"Score: {curPlayerExperience}";
            Debug.Log( $"Score: {curPlayerExperience}");
        }
    }

    private void OnClickRestartBtn()
    {
        SceneEventDefine.ChangeToBattleScene.SendEventMessage();
    }

    private void OnClickHomeBtn()
    {
        SceneEventDefine.ChangeToHomeScene.SendEventMessage();
    }

    private void OnHandleEventMessage(IEventMessage message)
    {
        if (message is BattleEventDefine.ScoreChange)
        {
            var msg = message as BattleEventDefine.ScoreChange;
            _scoreLabel.text = $"Score : {msg.CurrentScores}";
        }
        else if (message is BattleEventDefine.GameOver)
        {
            _overView.SetActive(true);
        }
    }
}