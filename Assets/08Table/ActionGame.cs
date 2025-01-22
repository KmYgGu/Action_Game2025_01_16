using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources")]
//리임포트를 할때 테이플 파일을 해당 경로에 저장해라
public class ActionGame : ScriptableObject
{
	public List<ItemData_Entity> ItemData; // Replace 'EntityType' to an actual type that is serializable.
	public List<TipData_Entity> TipMess; // Replace 'EntityType' to an actual type that is serializable.
	public List<Language_Entity> Language; // Replace 'EntityType' to an actual type that is serializable.
	public List<Monster_Entity> MonsterData; // Replace 'EntityType' to an actual type that is serializable.
	public List<EXP_Entity> LevelEXP; // Replace 'EntityType' to an actual type that is serializable.
}
