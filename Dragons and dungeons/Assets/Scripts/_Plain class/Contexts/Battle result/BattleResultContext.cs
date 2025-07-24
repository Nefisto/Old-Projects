using System;
using System.Collections.Generic;

public class BattleResultContext
{
    public BattleResult battleResult;

    public int BeautyPoints => battleResult.beautyPoints;
    public int MadnessPoints => battleResult.madnessPoints;
    public int ExperiencePoints => battleResult.experiencePoints;
}