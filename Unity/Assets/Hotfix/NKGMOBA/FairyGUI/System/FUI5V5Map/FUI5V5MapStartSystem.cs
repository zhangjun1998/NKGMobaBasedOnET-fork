//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 10:42:59
//------------------------------------------------------------

using System;
using ETHotfix;
using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUI5V5MapStartSystem: StartSystem<FUI5V5Map>
    {
        public override void Start(FUI5V5Map self)
        {
            Unit unit = UnitComponent.Instance.MyUnit;

            UnitAttributesDataComponent unitAttributesDataComponent = unit.GetComponent<UnitAttributesDataComponent>();

            HeroAttributesNodeData heroAttributesNodeData = unitAttributesDataComponent.GetAttributeDataAs<HeroAttributesNodeData>();

            self.SmallMapSprite.onRightClick.Add(this.AnyEventHandler);

            self.Btn_GMController_Enable.self.visible = false;
            self.Btn_GMController_Disable.self.onClick.Add(() =>
            {
                self.Btn_GMController_Disable.Visible = false;
                self.Btn_GMController_Enable.Visible = true;
                self.Par_GMControllerDis.Play();
            });
            self.Btn_GMController_Enable.self.onClick.Add(() =>
            {
                self.Btn_GMController_Disable.Visible = true;
                self.Btn_GMController_Enable.Visible = false;
                self.Part_GMControllerEnable.Play();
            });

            self.Btn_CreateSpiling.self.onClick.Add(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_CreateSpiling()
                {
                    X = unit.Position.x, Y = unit.Position.y, Z = unit.Position.z, ParentUnitId = unit.Id
                });
                ETModel.Log.Info($"发送请求木桩父实体id：{unit.Id}");
            });

            GameObject HeroAvatars =
                    ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadAsset<GameObject>(ABPathUtilities.GetTexturePath("HeroAvatars"));
            GameObject HeroSkillIcons =
                    ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadAsset<GameObject>(ABPathUtilities.GetTexturePath("HeroSkillIcons"));

            self.HeroAvatarLoader.texture = new NTexture(HeroAvatars.GetTargetObjectFromRC<Sprite>(heroAttributesNodeData.UnitAvatar).texture);
            self.SkillTalent_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(heroAttributesNodeData.Talent_SkillSprite).texture);
            self.SkillQ_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(heroAttributesNodeData.Q_SkillSprite).texture);
            self.SkillW_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(heroAttributesNodeData.W_SkillSprite).texture);
            self.SkillE_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(heroAttributesNodeData.E_SkillSprite).texture);
            self.SkillR_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(heroAttributesNodeData.R_SkillSprite).texture);

            self.AttackInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.Attack).ToString();
            self.ExtraAttackInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.AttackAdd).ToString();
            self.MagicInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.MagicStrength).ToString();
            self.ExtraMagicInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.MagicStrengthAdd).ToString();
            self.ArmorInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.Armor).ToString();
            self.ArmorpenetrationInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.ArmorPenetration).ToString();
            self.SpellResistanceInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.MagicResistance).ToString();
            self.MagicpenetrationInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.MagicPenetration).ToString();
            self.AttackSpeedInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.AttackSpeed).ToString();
            self.SkillCDInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.SkillCD).ToString();
            self.CriticalstrikeInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.CriticalStrikeProbability).ToString();
            self.MoveSpeedInfo.text = unitAttributesDataComponent.GetAttribute(NumericType.Speed).ToString();

            self.RedText.text = $"{unitAttributesDataComponent.GetAttribute(NumericType.Hp)}/{unitAttributesDataComponent.GetAttribute(NumericType.MaxHp)}";
            self.BlueText.text = $"{unitAttributesDataComponent.GetAttribute(NumericType.Mp)}/{unitAttributesDataComponent.GetAttribute(NumericType.MaxMp)}";

            self.RedProBar.self.max = unitAttributesDataComponent.GetAttribute(NumericType.MaxHp);
            self.RedProBar.self.value = unitAttributesDataComponent.GetAttribute(NumericType.Hp);

            self.BlueProBar.self.max = unitAttributesDataComponent.GetAttribute(NumericType.MaxMp);
            self.BlueProBar.self.value = unitAttributesDataComponent.GetAttribute(NumericType.Mp);

            self.SkillTalent_CDInfo.visible = false;
            self.SkillTalent_Bar.Visible = false;

            self.SkillQ_CDInfo.visible = false;
            self.SkillQ_Bar.Visible = false;

            self.SkillW_CDInfo.visible = false;
            self.SkillW_Bar.Visible = false;

            self.SkillE_CDInfo.visible = false;
            self.SkillE_Bar.Visible = false;

            self.SkillR_CDInfo.visible = false;
            self.SkillR_Bar.Visible = false;

            self.SkillD_CDInfo.visible = false;
            self.SkillD_Bar.Visible = false;

            self.SkillF_CDInfo.visible = false;
            self.SkillF_Bar.Visible = false;
        }

        void AnyEventHandler(EventContext context)
        {
            Vector2 global2Local = ((GObject) context.sender).GlobalToLocal(context.inputEvent.position);
            Vector2 fgui2Unity = new Vector2(global2Local.x, 200 - global2Local.y);
            Vector3 targetPos = new Vector3(-fgui2Unity.x / (200.0f / 100.0f), 0, -fgui2Unity.y / (200.0f / 100.0f));
            Game.EventSystem.Run(EventIdType.ClickSmallMap, targetPos);
        }
    }
}