using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class PersonModificator
{
    public PersonIdentety identety;
    public virtual bool IsCapacity()
    {
        return true;
    }

    public virtual string DebugInfo()
    {
        return "=====" + this.ToString() + "=====";
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

public class BasicStats : PersonModificator
{
    public string name = "-";
    public PersonSex sex;
    public PersonStatus status;
    public PersonEducation education;
    public float height;
    public int age;
    public int weight;

    public override string DebugInfo()
    {
        string _ret = base.DebugInfo();

        _ret += "\n";
        _ret += "name:" + name.ToString() + "\n";
        _ret += "sex:" + sex.ToString() + "\n";
        _ret += "age:" + age.ToString() + "\n";
        _ret += "education:" + education.ToString() + "\n";
        _ret += "height:" + height.ToString() + "\n";
        _ret += "weight:" + weight.ToString();

        return _ret;
    }
}

public class AlcoholicMod : PersonModificator
{

}

public class WhinerMod : PersonModificator
{

}

public class JoillerMod : PersonModificator
{

}

public class BodybuilderMod : PersonModificator
{

}

public class SchizophrenicMod : PersonModificator
{

}

public class WarhammerMod : PersonModificator
{

}

public class MisogynistMod : PersonModificator
{

}

public class IncelMod : PersonModificator
{

}

public class WomanizerMod : PersonModificator
{

}

public class FeministMod : PersonModificator
{

}

public class JewishMod : PersonModificator
{

}

public class ArabMod : PersonModificator
{

}

public class BlackMod : PersonModificator
{

}

public class AsianMod : PersonModificator
{

}
