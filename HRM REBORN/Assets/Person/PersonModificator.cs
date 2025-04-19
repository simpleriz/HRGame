using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public abstract class PersonModificator
{
    //const
    protected const float baseCoupleChance = 20;
    protected const float baseDialogDuration = TimeManager.minuteDuration * 30;
    protected const float baseEffencity = 100;
    protected const int baseMaxEnergy = 3;



    public PersonIdentety identety;
    public virtual bool IsCapacity()
    {
        return true;
    }

    public virtual float CalculateConflictChance(PersonIdentety person)
    {
        return 0;
    }

    public virtual string DebugInfo()
    {
        return "=====" + this.ToString() + "=====";
    }

    public virtual float CalculateCoupleChance(PersonIdentety person)
    {
        return 0;
    }

    public virtual float CalculateDialogDuration(PersonIdentety person)
    {
        return 0;
    }

    public virtual float CalculateWorkEffencity()
    {
        return 0;
    }

    public virtual int CalculateMaxEnergy()
    {
        return 0;
    }

    public virtual void OnFire() { }
    public virtual void OnNight() { }

    public virtual void OnDeath() { }

    public virtual float ConflictWinOutcomeChance(PersonIdentety person)
    {
        return 0;
    }

    public virtual float ConflictNoOutcomeChance(PersonIdentety person)
    {
        return 0;
    }

    public virtual void OnCoflictEnd(PersonIdentety person, bool isWin)
    {
    }

    public virtual PersonIdentety GetDialogCompanion(PersonIdentety _person) 
    { 
        return _person; 
    }
}

public enum PersonSex
{
    None,
    Man,
    Woman,
}

public enum PersonStatus
{
    hidden,
}

public enum PersonEducation
{
    InternetCourses,          
    IncompliteSecondarySchool,
    SecondarySchool,          
    Bachelor,                 
    Master,
    ScienceDoctor,            
}

public enum PersonOrientation
{
    Hetero,
    Gay,
    Bi,
    A,
}

public class BasicStats : PersonModificator
{
    public string name = "-";
    public PersonSex sex;
    public PersonStatus status;
    public PersonEducation education;
    public PersonOrientation orientation;
    public float height;
    public int age;
    public int weight;

    public override float CalculateCoupleChance(PersonIdentety person)
    {
        var _sex = person.GetModificator<BasicStats>().sex;
        if (orientation == PersonOrientation.Hetero & _sex != sex)
        {
            return baseCoupleChance;
        }
        if (orientation == PersonOrientation.Gay & _sex == sex)
        {
            return baseCoupleChance;
        }
        if (orientation == PersonOrientation.Bi)
        {
            return baseCoupleChance;
        }
        
        return -1000;
    }

    public override int CalculateMaxEnergy()
    {
        return baseMaxEnergy;
    }
    public override float CalculateDialogDuration(PersonIdentety person)
    {
        return baseDialogDuration;
    }
    public override string DebugInfo()
    {
        string _ret = base.DebugInfo();

        _ret += "\n";
        _ret += "name:" + name.ToString() + "\n";
        _ret += "sex:" + sex.ToString() + "\n";
        _ret += "age:" + age.ToString() + "\n";
        _ret += "education:" + education.ToString() + "\n";
        _ret += "orientation:" + orientation.ToString() + "\n";
        _ret += "height:" + height.ToString() + "\n";
        _ret += "weight:" + weight.ToString();

        return _ret;
    }
}

public class TeamMember : PersonModificator
{
    public List<PersonIdentety> companions = new List<PersonIdentety>();

    public bool isActive = false;
    public int energy = 3;

    public TeamMember()
    {
        StartWorkDayEvent.OnStartDay.AddListener(OnStartDay);
    }

    public void OnStartDay()
    {
        if (isActive)
        {
            identety.personTransform.WorkTask();
            energy--;

            companions = PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).ToList().OrderBy(i => Random.Range(0f,1f)).ToList();
        }
        else
        {
            energy++;
            var maxEnergy = identety.CalculateMaxEnergy();
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
        }
    }

    public override PersonIdentety GetDialogCompanion(PersonIdentety _person)
    {
        if (_person == null & companions.Count > 0)
        {
            var person = companions[0];

            companions.RemoveAt(0);

            if(person == null)
            {
                return GetDialogCompanion(null);
            }
            return person;
        }
        return _person;
    }
}

public class CoupleMod : PersonModificator
{
    public PersonIdentety person;

    public override void OnFire()
    {
        person.Fire();
    }

    public override float CalculateCoupleChance(PersonIdentety person)
    {
        return -1000;
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + $"\nCouple with: {person.gameObject.name}";
    }
}

public class NightSuicideMod : PersonModificator
{
    public override void OnNight()
    {
        identety.Death();
    }
}

public class EffencityDebuffMod : PersonModificator
{
    float value;
    public EffencityDebuffMod(float value)
    {
        this.value = value;
    }

    public override float CalculateWorkEffencity()
    {
        return value;
    }

    public override void OnNight()
    {
        identety.modificators.Remove(this);
    }
}

public class BingeMod : PersonModificator
{
    const float stopChance = 50;
    int days = 0;
    string debug = "";
    public override void OnNight()
    {
        days++;
        if(days == 3)
        {
            identety.Death();
        }
        else
        {
            var dice = Random.Range(0, 101);
            var result = dice <= stopChance;

            debug += $"\nstop binge: chance={stopChance}  dice={dice}  result={result}";

            if (result)
            {
                identety.modificators.Remove(this);
            }
        }
    }

    public override bool IsCapacity()
    {
        return false;
    }

    public override string DebugInfo()
    {
        return base.DebugInfo()+$"days={days}"+debug;
    }
}

public class AlcoholicMod : PersonModificator
{
    const float startBingeChance = 10;
    string debug = "";
    public override void OnNight()
    {
        var dice = Random.Range(1, 101);
        var result = dice <= startBingeChance;

        debug += $"\nbinge: chance={startBingeChance}  dice:{dice}  result:{result}";

        if (result)
        {
            identety.modificators.Add(new BingeMod());
        }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + debug;
    }
}

public class WhinerMod : PersonModificator
{
    const float effencityDebuff = -10;

    public override float CalculateDialogDuration(PersonIdentety person)
    {
        person.AddModificator(new EffencityDebuffMod(effencityDebuff));
        return base.CalculateDialogDuration(person);
    }

    public override float CalculateWorkEffencity()
    {
        return effencityDebuff;
    }

}

public class JoillerMod : PersonModificator
{

}

public class BodybuilderMod : PersonModificator
{
    const float winOutcomeChance = 25;
    const float conflictChance = 25;
    const float additionalEffencity = 20;
    int winCount = 0;
    string names = "";
    public override float ConflictWinOutcomeChance(PersonIdentety person)
    {
        return winOutcomeChance;
    }

    public override float ConflictNoOutcomeChance(PersonIdentety person)
    {
        return -winOutcomeChance;
    }

    public override float CalculateConflictChance(PersonIdentety person)
    {
        return conflictChance;
    }

    public override void OnCoflictEnd(PersonIdentety person,bool isWin)
    {
        if (isWin)
        {
            winCount++;
            names += person.gameObject.name + "\n";
        }
    }

    public override float CalculateWorkEffencity()
    {
        return additionalEffencity * winCount;
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() +"\n"+ names + $"wins today: {winCount}(effencity={additionalEffencity*winCount})";
    }
    public override void OnNight()
    {
        winCount = 0;
        names = "";
    }
}

public class SchizophrenicMod : PersonModificator
{
    const float conflicChance = 25;
    bool selfDialog = false;
    public override void OnNight()
    {
        selfDialog = true;
    }

    public override PersonIdentety GetDialogCompanion(PersonIdentety _person)
    {
        if (selfDialog)
        {
            selfDialog = false;
            return identety;
        }
        return _person;
    }

    public override float CalculateConflictChance(PersonIdentety person)
    {
        return conflicChance;
    }
}

public class WarhammerMod : PersonModificator
{
    const float womanSuicideChance = 25;

    string debug = "";
    public override float CalculateDialogDuration(PersonIdentety person)
    {
        if(person.GetModificator<BasicStats>().sex == PersonSex.Woman)
        {
            var dice = Random.Range(1, 101);
            var result = dice <= womanSuicideChance;

            debug += $"\n{person.gameObject.name}: chance={womanSuicideChance}  dice={dice}  result={result}";

            if (result)
            {
                person.AddModificator(new NightSuicideMod());
            }
        }
        return baseDialogDuration * 1.5f;
    }
}

public class MisogynistMod : PersonModificator
{
    const float efficiencyDebuffFemale = 5;
    const float conflicChance = 25;
    public override float CalculateWorkEffencity()
    {
        return WomanCount() * efficiencyDebuffFemale;
    }

    public override int CalculateMaxEnergy()
    {
        return Mathf.RoundToInt(Mathf.Floor(WomanCount()/3)) * -1;
    }

    public override float CalculateConflictChance(PersonIdentety person)
    {
        if (person.GetModificator<BasicStats>().sex == PersonSex.Woman)
        {
            return conflicChance;
        }
        return 0;
    }

    int WomanCount()
    {
        return PersonManager.Instance.persons.Where(i => i.GetModificator<BasicStats>().sex == PersonSex.Woman).Count();
    }
}

public class IncelMod : PersonModificator
{
    public override void OnNight()
    {
        if (identety.GetModificator<CoupleMod>() != null)
        {
            identety.Death();
        }
    }
}

public class WomanizerMod : PersonModificator
{
    public override float CalculateCoupleChance(PersonIdentety _person)
    {
        return (baseCoupleChance * 0.5f) + (identety.GetAllModificator<CoupleMod>().Count * 1000);
    }
}

public class FeministMod : PersonModificator
{
    const float efficiencyDebuffMale = 5;
    const float conflicChance = 25;
    public override float CalculateWorkEffencity()
    {
        return ManCount() * efficiencyDebuffMale;
    }

    public override int CalculateMaxEnergy()
    {
        return Mathf.RoundToInt(Mathf.Floor(ManCount() / 3)) * -1;
    }

    public override float CalculateConflictChance(PersonIdentety person)
    {
        if (person.GetModificator<BasicStats>().sex == PersonSex.Man)
        {
            return conflicChance;
        }
        return 0;
    }

    int ManCount()
    {
        return PersonManager.Instance.persons.Where(i => i.GetModificator<BasicStats>().sex == PersonSex.Man).Count();
    }
}

public class JewishMod : PersonModificator
{
    public override float CalculateConflictChance(PersonIdentety person)
    {
        if(person.GetModificator<ArabMod>() != null)
        {
            return 100;
        }

        return 0;
    }
}

public class ArabMod : PersonModificator
{
    public override float CalculateConflictChance(PersonIdentety person)
    {
        if (person.GetModificator<JewishMod>() != null)
        {
            return 100;
        }

        return 0;
    }
}

public class BlackMod : PersonModificator
{
    const float tornAnusChance = 10;
    const float chanceBasketballDefeat = 10;
    const float suicideChance = 10;
    string debug = "";

    bool isBasketaballDefeat = false;
    public override void OnNight()
    {
        debug = "\n";

        int dice;
        bool result;
        
        if (identety.GetModificator<BasicStats>().sex == PersonSex.Man)
        {
            debug += "=torn anuses=\n";
            var couples = identety.GetAllModificator<CoupleMod>();
            if (couples.Count == 0)
            {
                debug += "-empty-";
            }
            else
            {
                debug += $"chance to tear anus: {tornAnusChance}";
                foreach (var couple in couples)
                {
                    dice = Random.Range(1, 101);
                    result = dice <= tornAnusChance;
                    debug += $"{couple.person.name}: dice={dice} reslt:{result}\n";
                    if (result)
                    {
                        couple.person.Death();
                    }
                }
            }
        }

        dice = Random.Range(1, 101);
        result = dice <= chanceBasketballDefeat;

        debug += $"\nbasketball team: looseChance={chanceBasketballDefeat}  dice={dice}  result:{result}\n";
        isBasketaballDefeat = result;
        if (isBasketaballDefeat)
        {
            dice = Random.Range(1, 101);
            result = dice <= suicideChance;
            debug += $"Suicide: suicideChance={suicideChance}  dice={dice}  result:{result}\n";

            if (result)
            {
                identety.Death();
            }
        }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + debug;
    }

    public override float CalculateWorkEffencity()
    {
        if (isBasketaballDefeat)
        {
            return baseEffencity * -0.5f;
        }
        return 0;
    }
    public override float CalculateCoupleChance(PersonIdentety _person)
    {
        return baseCoupleChance * 0.5f;
    }
}

public class AsianMod : PersonModificator
{
    const float suicideChance = 25;

    string debug = "";
    public override int CalculateMaxEnergy()
    {
        return 1;
    }

    public override float CalculateWorkEffencity()
    {
        return baseEffencity;
    }

    public override void OnNight()
    {
        int dice;
        bool result;

        dice = Random.Range(1, 101);
        result = dice <= suicideChance;
        debug += $"Suicide: suicideChance={suicideChance}  dice={dice}  result:{result}\n";

        if (result)
        {
            identety.Death();
        }
    }
}
