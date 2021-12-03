using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using APIPlugin;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GBC;
using HarmonyLib;
using Pixelplacement;
using Sirenix;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.PostProcessing;
using UnityEngine.XR;

namespace examplespell
{




    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "kopie.inscryption.examplespell";
        private const string PluginName = "examplespell";
        private const string PluginVersion = "1.0.0";
        internal static ManualLogSource Log;


        
        private void AddExample()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        List<Ability> abilities = new List<Ability>();
	        abilities.Add(NewAbility.abilities.Find(ability => ability.id==AbilityIdentifier.GetAbilityIdentifier("kopie.inscryption.cardsmechanics", "Spell core")).ability);
	        abilities.Add(spellexamplenew.ability);
	        
	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("spellexample.dll",""),"Artwork/spellcard.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);
	        NewCard.Add("spell_fireball", "Fireball", 0, 0, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description:"A unknown type of card?", bloodCost:0, bonesCost:2, abilities:abilities, defaultTex:tex);
        }
        
        
        private NewAbility AddAbility2()
        {
	        AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
	        info.powerLevel = 0;
	        info.rulebookName = "Simple fireball";
	        info.rulebookDescription = "Spell. To target: deal 3 damage!";
	        info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

	        List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
	        DialogueEvent.Line line = new DialogueEvent.Line();
	        line.text = "That was unexpected";
	        lines.Add(line);
	        info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("spellexample.dll",""),"Artwork/examplespell.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        NewAbility spellexample = new NewAbility(info,typeof(spellexamplenew),tex,AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
	        spellexamplenew.ability = spellexample.ability;
	        return spellexample;
        }
        
        
        public class spellexamplenew : AbilityBehaviour
        {
	        public override Ability Ability
	        {
		        get
		        {
			        return ability;
		        }
	        }

	        public static Ability ability;
	        
	        //SlotTargetedForAttack
	        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	        {
		        return true;
	        }

	        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	        {
		        yield return base.PreSuccessfulTriggerSequence();
		        yield return new WaitForSeconds(0.2f);
		        yield return slot.Card.TakeDamage(3, attacker);
		        yield break;
	        }
        }
        
        
        
        
   


        private void Awake()
        {
	        
	        AddAbility2();
	        AddExample();
	        Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
            Plugin.Log = base.Logger;
            
            
        }

    }
}